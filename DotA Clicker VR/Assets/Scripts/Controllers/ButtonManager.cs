using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Simple script meant to control the clickable buttons behavior (animation)
/// </summary>
public class ButtonManager : MonoBehaviour
{
    public bool CanClick = true;

    string m_buttonName;
    RadiantSceneController m_sceneController;
    RadiantClickerController m_clickerController;
    public Animator m_animator;

    bool m_buyUpgradeCooldown = false;

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
        if ((col.tag == "ViveController" || col.gameObject.layer == 2)/*Ignore raycasat layer used by VRTK*/ && m_buttonName == "BuyButton")
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("BuyButtonIdle")
                || m_sceneController.TotalGold + (decimal)m_clickerController.UpgradeCost < (decimal)m_clickerController.UpgradeCost
                || m_sceneController.TotalGold - (decimal)m_clickerController.UpgradeCost < 0
                || m_buyUpgradeCooldown)
            {
                Debug.Log("Cant upgrade clicker '" + m_clickerController.HeroName + "'");
                return;
            }

            m_clickerController.ClickerMultiplier += 1;
            m_sceneController.RemoveFromTotal((decimal)m_clickerController.UpgradeCost);

            //Invoke event
            m_clickerController.OnBuyClickerButtonPressed();
            m_animator.SetTrigger("isClicked");
            StartCoroutine(PlayButtonPushAnimation(1f));

            m_buyUpgradeCooldown = true;
        }
        //Hero clicker button
        else if((col.tag == "ViveController" || col.gameObject.layer == 2)/*Ignore raycasat layer used by VRTK*/ && m_buttonName == "ClickButtonBack")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClickButtonPush") || m_clickerController.IsClicked || m_clickerController.ClickerMultiplier == 0) //if (is in animation) || (IsClick is progress)
            {
                return;
            }

            if(!m_clickerController.CanBeClicked)
            {
                //if clicker hasnt been bought, show red button and then go back
                //StartCoroutine(DenyClickStage1());
            }

            m_clickerController.OnClickButtonPressed();
            m_animator.SetTrigger("isClicked");
            StartCoroutine(PlayButtonPushAnimation(0.3f));
        }
    }

    IEnumerator PlayButtonPushAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        m_buyUpgradeCooldown = false;
    }

    //IEnumerator CantPushButtonError()
    //{
    //    m_buttonMaterial.SetColor("_Color", new Color(255, 0, 0));
    //    yield return new WaitForSeconds(0.3f);
    //    m_buttonMaterial.SetColor("_Color", new Color(64, 64, 64));
    //}
}
