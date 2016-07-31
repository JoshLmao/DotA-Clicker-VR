using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script meant to control the buttons behavior (animation)
/// </summary>
public class ButtonManager : MonoBehaviour
{
    public delegate void OnButtonPressed();
    public static event OnButtonPressed OnPressed;

    RadiantSceneController m_sceneController;
    RadiantClickerController m_parentController;
    Animator m_animator;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_parentController = GetComponentInParent<RadiantClickerController>();
        m_animator = this.GetComponent<Animator>();
    }
	
	void Update ()
    {
        
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ViveController" && this.gameObject.name == "StandBack")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ButtonPush"))
                return;

            //Invoke event
            OnPressed();
            m_animator.SetBool("isClicked", true);
            StartCoroutine(PlayButtonPushAnimation());
        }
        else if(col.tag == "ViveController" && this.gameObject.name == "ClickButtonBack")
        {
            m_sceneController.TotalGold += m_parentController.ClickAmount;
        }
    }

    IEnumerator PlayButtonPushAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        m_animator.SetBool("isClicked", false);
    }
}
