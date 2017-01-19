using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TwitchIRC : MonoBehaviour
{
    public string oauth;
    public string nickName;
    public string channelName;
    private string server = "irc.twitch.tv";
    private int port = 6667;

    //event(buffer).
    public class MsgEvent : UnityEngine.Events.UnityEvent<string> { }
    public MsgEvent messageRecievedEvent = new MsgEvent();

    private string buffer = string.Empty;
    public bool stopThreads = false;
    private Queue<string> commandQueue = new Queue<string>();
    private List<string> recievedMsgs = new List<string>();
    private System.Threading.Thread inProc, outProc;

    RadiantSceneController m_sceneController;
    Text m_currentChannelText;

    public bool StartIRC()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        RadiantSceneController.LoadedConfigFile += LoadedConfigFile;
        m_currentChannelText = transform.Find("CurrentChannelHolder/CurrentChannel").GetComponent<Text>();

        System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
        sock.Connect(server, port);
        if (!sock.Connected)
        {
            Debug.Log("Failed to connect!");
            return false;
        }
        var networkStream = sock.GetStream();
        var input = new System.IO.StreamReader(networkStream);
        var output = new System.IO.StreamWriter(networkStream);

        if(m_sceneController.CurrentConfigFile != null && m_sceneController.CurrentConfigFile.TwitchAuthCode != "" && m_sceneController.CurrentConfigFile.TwitchUsername != "")
        {
            oauth = m_sceneController.CurrentConfigFile.TwitchAuthCode;
            nickName = m_sceneController.CurrentConfigFile.TwitchUsername;

            //Send PASS & NICK.
            output.WriteLine("PASS " + oauth);
            output.WriteLine("NICK " + nickName.ToLower());
            output.Flush();

            //output proc
            outProc = new System.Threading.Thread(() => IRCOutputProcedure(output));
            outProc.Start();
            //input proc
            inProc = new System.Threading.Thread(() => IRCInputProcedure(input, networkStream));
            inProc.Start();
        }
        else
        {
            Debug.Log("No oauth key or Nickname");
            return false;
        }
        return true;
    }

    private void IRCInputProcedure(System.IO.TextReader input, System.Net.Sockets.NetworkStream networkStream)
    {
        while (!stopThreads)
        {
            if (!networkStream.DataAvailable)
                continue;

            buffer = input.ReadLine();

            //was message?
            if (buffer.Contains("PRIVMSG #"))
            {
                lock (recievedMsgs)
                {
                    recievedMsgs.Add(buffer);
                }
            }

            //Send pong reply to any ping messages
            if (buffer.StartsWith("PING "))
            {
                SendCommand(buffer.Replace("PING", "PONG"));
            }

            //After server sends 001 command, we can join a channel
            if (buffer.Split(' ')[1] == "001")
            {
                SendCommand("JOIN #" + channelName);
            }
        }
    }

    private void IRCOutputProcedure(System.IO.TextWriter output)
    {
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        while (!stopThreads)
        {
            lock (commandQueue)
            {
                if (commandQueue.Count > 0) //do we have any commands to send?
                {
                    // https://github.com/justintv/Twitch-API/blob/master/IRC.md#command--message-limit 
                    //have enough time passed since we last sent a message/command?
                    if (stopWatch.ElapsedMilliseconds > 1750)
                    {
                        //send msg.
                        output.WriteLine(commandQueue.Peek());
                        output.Flush();
                        //remove msg from queue.
                        commandQueue.Dequeue();
                        //restart stopwatch.
                        stopWatch.Reset();
                        stopWatch.Start();
                    }
                }
            }
        }
    }

    public void SendCommand(string cmd)
    {
        lock (commandQueue)
        {
            commandQueue.Enqueue(cmd);
        }
    }

    public void SendMsg(string msg)
    {
        lock (commandQueue)
        {
            commandQueue.Enqueue("PRIVMSG #" + channelName + " :" + msg);
        }
    }

    //MonoBehaviour Events.
    void Start()
    {

    }
    void OnEnable()
    {
        stopThreads = false;
        StartIRC();
    }
    void OnDisable()
    {
        stopThreads = true;
        //while (inProc.IsAlive || outProc.IsAlive) ;
        //print("inProc:" + inProc.IsAlive.ToString());
        //print("outProc:" + outProc.IsAlive.ToString());
    }
    void OnDestroy()
    {
        stopThreads = true;
        //while (inProc.IsAlive || outProc.IsAlive) ;
        //print("inProc:" + inProc.IsAlive.ToString());
        //print("outProc:" + outProc.IsAlive.ToString());
    }
    void Update()
    {
        lock (recievedMsgs)
        {
            if (recievedMsgs.Count > 0)
            {
                for (int i = 0; i < recievedMsgs.Count; i++)
                {
                    messageRecievedEvent.Invoke(recievedMsgs[i]);
                }
                recievedMsgs.Clear();
            }
        }

        if(channelName != "")
        {
            m_currentChannelText.text = "Current Channel: '" + channelName + "'";
        }
        else
        {
            m_currentChannelText.text = "Current Channel: 'NO CHANNEL'";
        }
    }

    public void ClearMessageList()
    {
        recievedMsgs.Clear();

        foreach(Transform obj in transform.Find("ChatScrollable/Chat").transform)
        {
            GameObject.Destroy(obj.gameObject);
        }
    }

    void LoadedConfigFile(ConfigDto config)
    {
        oauth = config.TwitchAuthCode;
        nickName = config.TwitchUsername;
        StartIRC();
    }
}
