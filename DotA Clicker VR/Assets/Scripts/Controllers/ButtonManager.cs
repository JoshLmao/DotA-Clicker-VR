using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Simple script meant to control the buttons behavior (animation)
/// </summary>
public class ButtonManager : MonoBehaviour
{
    public delegate void OnBuyClickerButtonPressed();
    public static event OnBuyClickerButtonPressed OnBuyClickerPressed;

    public delegate void OnClickerButtonPressed();
    public static event OnClickerButtonPressed OnHeroClickButtonPress;

    public bool CanClick = true;

    string m_buttonName;
    RadiantSceneController m_sceneController;
    RadiantClickerController m_clickerController;
    public Animator m_animator;

	void Start ()
    {
        m_buttonName = this.gameObject.name;
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_clickerController = GetComponentInParent<RadiantClickerController>();
        
        m_animator = this.GetComponentInChildren<Animator>();
    }
	
	void Update ()
    {
        
	}

    void OnTriggerEnter(Collider col)
    {
        //Buy a multiplier clicker
        if(col.tag == "ViveController" && m_buttonName == "StandBack")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("BuyButtonPush") 
                || m_sceneController.TotalGold + m_clickerController.BuyNextLevel < m_clickerController.BuyNextLevel 
                || m_sceneController.TotalGold - m_clickerController.BuyNextLevel < 0)
            {
                Debug.Log("Can upgrade clicker '" + m_clickerController.HeroName + "'");
                return;
            }

            m_clickerController.ClickerMultiplier += 1;
            m_sceneController.TotalGold -= m_clickerController.BuyNextLevel;

            //Invoke event
            OnBuyClickerPressed();
            m_animator.SetBool("isClicked", true);
            StartCoroutine(PlayButtonPushAnimation(0.5f));
        }
        //Hero clicker button
        else if(col.tag == "ViveController" && m_buttonName == "ClickButtonBack")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClickButtonPush") || m_clickerController.IsClicked) //if (is in animation) || (IsClick is progress)
            {
                Debug.Log("Can't click");
                return;
            }

            m_clickerController.AddGold(); //Do click
            
            OnHeroClickButtonPress();
            m_animator.SetBool("isClicked", true);
            StartCoroutine(PlayButtonPushAnimation(0.3f));
        }
    }

    IEnumerator PlayButtonPushAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        m_animator.SetBool("isClicked", false);
    }
}
