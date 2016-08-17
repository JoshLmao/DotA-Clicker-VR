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
        SteamVR_TrackedController controller = col.GetComponent<SteamVR_TrackedController>();

        //Buy a multiplier clicker
        if (col.tag == "ViveController" && m_buttonName == "StandBack")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("BuyButtonPush") 
                || m_sceneController.TotalGold + m_clickerController.UpgradeCost < m_clickerController.UpgradeCost
                || m_sceneController.TotalGold - m_clickerController.UpgradeCost < 0)
            {
                Debug.Log("Cant upgrade clicker '" + m_clickerController.HeroName + "'");
                return;
            }

            m_clickerController.ClickerMultiplier += 1;
            m_sceneController.TotalGold -= (int)m_clickerController.UpgradeCost;

            //Invoke event
            m_clickerController.OnBuyClickerButtonPressed();
            m_animator.SetBool("isClicked", true);
            StartCoroutine(PlayButtonPushAnimation(0.5f));

            HandController.RumbleController(controller.controllerIndex, 2000);
        }
        //Hero clicker button
        else if(col.tag == "ViveController" && m_buttonName == "ClickButtonBack")
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClickButtonPush") || m_clickerController.IsClicked || m_clickerController.ClickerMultiplier == 0) //if (is in animation) || (IsClick is progress)
            {
                return;
            }

            m_clickerController.OnClickButtonPressed();
            m_animator.SetBool("isClicked", true);
            StartCoroutine(PlayButtonPushAnimation(0.3f));

            HandController.RumbleController(controller.controllerIndex, 2000);
        }
    }

    IEnumerator PlayButtonPushAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        m_animator.SetBool("isClicked", false);
    }

    //IEnumerator CantPushButtonError()
    //{
    //    m_buttonMaterial.SetColor("_Color", new Color(255, 0, 0));
    //    yield return new WaitForSeconds(0.3f);
    //    m_buttonMaterial.SetColor("_Color", new Color(64, 64, 64));
    //}
}
