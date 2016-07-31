using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script meant to control the buttons behavior (animation)
/// </summary>
public class ButtonManager : MonoBehaviour
{
    public delegate void OnButtonPressed();
    public static event OnButtonPressed OnPressed;

    ClickerController m_parentController;
    Animator m_animator;

	void Start ()
    {
        m_animator = this.GetComponent<Animator>();
	}
	
	void Update ()
    {
        
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ViveController")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ButtonPush"))
                return;

            //Invoke event
            OnPressed();
            m_parentController.AmountOfClickers += 1;
            m_animator.SetBool("isClicked", true);
            StartCoroutine(PlayButtonPushAnimation());
        }
    }

    IEnumerator PlayButtonPushAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        m_animator.SetBool("isClicked", false);
    }
}
