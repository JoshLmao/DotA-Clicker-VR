using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {

    public string Input = "";

    public delegate void OnEnterPressed();
    public static event OnEnterPressed EnterPressed;
    bool m_canType = true;

	void Start ()
    {
	    
	}
	
	void Update ()
    {
	    if(Input.Length > 25)
        {
            m_canType = false;
        }
        else
        {
            m_canType = true;
        }
	}

    public void ClearStream()
    {
        Input = "";
    }

    public void Backspace()
    {
        if(Input.Length != 0)
            Input = Input.Remove(Input.Length - 1);
    }
    public void Enter()
    {
        if(EnterPressed != null)
            EnterPressed.Invoke();
    }
    public void Zero()
    {
        if (m_canType)
            Input += "0";
    }
    public void One()
    {
        if (m_canType)
            Input += "1";
    }
    public void Two()
    {
        if (m_canType)
            Input += "2";
    }
    public void Three()
    {
        if (m_canType)
            Input += "3";
    }
    public void Four()
    {
        if (m_canType)
            Input += "4";
    }
    public void Five()
    {
        if (m_canType)
            Input += "5";
    }
    public void Six()
    {
        if (m_canType)
            Input += "6";
    }
    public void Severn()
    {
        if (m_canType)
            Input += "7";
    }
    public void Eight()
    {
        if (m_canType)
            Input += "8";
    }
    public void Nine()
    {
        if (m_canType)
            Input += "9";
    }
    public void Q()
    {
        if (m_canType)
            Input += "q";
    }
    public void W()
    {
        if (m_canType)
            Input += "w";
    }
    public void E()
    {
        if (m_canType)
            Input += "e";
    }
    public void R()
    {
        if (m_canType)
            Input += "r";
    }
    public void T()
    {
        if (m_canType)
            Input += "t";
    }
    public void Y()
    {
        if (m_canType)
            Input += "y";
    }
    public void U()
    {
        if (m_canType)
            Input += "u";
    }
    public void I()
    {
        if (m_canType)
            Input += "i";
    }
    public void O()
    {
        if (m_canType)
            Input += "o";
    }
    public void P()
    {
        if (m_canType)
            Input += "p";
    }
    public void A()
    {
        if (m_canType)
            Input += "a";
    }
    public void S()
    {
        if (m_canType)
            Input += "s";
    }
    public void D()
    {
        if (m_canType)
            Input += "d";
    }
    public void F()
    {
        if (m_canType)
            Input += "f";
    }
    public void G()
    {
        if (m_canType)
            Input += "g";
    }
    public void H()
    {
        if (m_canType)
            Input += "h";
    }
    public void J()
    {
        if (m_canType)
            Input += "j";
    }
    public void K()
    {
        if (m_canType)
            Input += "k";
    }
    public void L()
    {
        if (m_canType)
            Input += "l";
    }
    public void Z()
    {
        if (m_canType)
            Input += "z";
    }
    public void X()
    {
        if (m_canType)
            Input += "x";
    }
    public void C()
    {
        if (m_canType)
            Input += "c";
    }
    public void V()
    {
        if (m_canType)
            Input += "v";
    }
    public void B()
    {
        if (m_canType)
            Input += "b";
    }
    public void N()
    {
        if (m_canType)
            Input += "n";
    }
    public void M()
    {
        if (m_canType)
            Input += "m";
    }
    public void Dash()
    {
        if (m_canType)
            Input += "-";
    }
    public void Underscore()
    {
        if (m_canType)
            Input += "_";
    }
}
