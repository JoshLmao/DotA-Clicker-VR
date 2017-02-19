using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    bool m_canClick = true;
    Material m_buttonMaterial;

    void Awake()
    {
        if (this.gameObject.name == "ClickButtonBack")
        {
            var obj = transform.Find("ClickButton");
            m_buttonMaterial = new Material(obj.GetComponent<Renderer>().material);
            obj.GetComponent<Renderer>().material = m_buttonMaterial;
        }
        else
        {
            m_buttonMaterial = new Material(this.GetComponent<Renderer>().material);
            gameObject.GetComponent<Renderer>().material = m_buttonMaterial;
        }

        m_buttonMaterial.name = "test";
    }

    void Start()
    {
        m_buttonName = this.gameObject.name;
        if (SceneManager.GetActiveScene().name == "RadiantSide")
            m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_clickerController = GetComponentInParent<RadiantClickerController>();

        m_animator = this.GetComponentInChildren<Animator>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        //Buy a multiplier clicker
        if ((col.tag == "ViveController" || col.gameObject.layer == 2)/*Ignore raycasat layer used by VRTK*/ && m_buttonName == "BuyButton" && m_canClick)
        {
            if(!OnBuyMultiplier())
            {
                m_canClick = false;
                StartCoroutine(ClickDelay(1));
            }
        }
        //Hero clicker button
        else if ((col.tag == "ViveController" || col.gameObject.layer == 2)/*Ignore raycasat layer used by VRTK*/ && m_buttonName == "ClickButtonBack" && m_canClick)
        {
            if(!OnClickButton())
            {
                m_canClick = false;
                StartCoroutine(ClickDelay(1));
            }
        }
    }

    IEnumerator ClickDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        m_canClick = true;
    }

    IEnumerator PlayButtonPushAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        m_buyUpgradeCooldown = false;
    }

    IEnumerator CantPushButtonError()
    {
        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/magic_immune"));
        m_buttonMaterial.SetColor("_Color", ConvertColor(130f, 58f, 58f, 255f));
        yield return new WaitForSeconds(0.3f);
        m_buttonMaterial.SetColor("_Color", ConvertColor(64f, 64f, 64f, 255f));
    }

    Color ConvertColor(float r, float g, float b, float a)
    {
        return new Color(r / 255, g / 255, b / 255, a / 255);
    }

    public bool OnBuyMultiplier()
    {
        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("BuyButtonIdle")
                || m_sceneController.TotalGold + (decimal)m_clickerController.UpgradeCost < (decimal)m_clickerController.UpgradeCost
                || m_sceneController.TotalGold - (decimal)m_clickerController.UpgradeCost < 0
                || m_buyUpgradeCooldown)
        {
            StartCoroutine(CantPushButtonError());
            return false;
        }

        m_clickerController.ClickerMultiplier += 1;
        m_sceneController.RemoveFromTotal((decimal)m_clickerController.UpgradeCost);

        //Invoke event
        m_clickerController.OnBuyClickerButtonPressed();
        m_animator.SetTrigger("isClicked");
        StartCoroutine(PlayButtonPushAnimation(1f));

        m_buyUpgradeCooldown = true;
        return true;
    }

    public bool OnClickButton()
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClickButtonPush") || m_clickerController.IsClicked || m_clickerController.ClickerMultiplier == 0) //if (is in animation) || (IsClick is progress)
        {
            StartCoroutine(CantPushButtonError());
            return false;
        }

        if (!m_clickerController.CanBeClicked)
        {
            //if clicker hasnt been bought, show red button and then go back
            //StartCoroutine(DenyClickStage1());
        }
        m_clickerController.OnClickButtonPressed();
        m_animator.SetTrigger("isClicked");
        StartCoroutine(PlayButtonPushAnimation(0.3f));
        return true;
    }
}
