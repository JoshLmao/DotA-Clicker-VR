using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Generic class aimed at being used by each clicker. Controls times and multipliers
/// </summary>
public class RadiantClickerController : MonoBehaviour
{
    /**  Clicker Details & Start Values**/
    public string HeroName;
    public int StartClickAmount = 1;
    public float UpgradeCost = 10;
    public float UpgradeMultiplier = 1.15f;
    /****************************/
    /// <summary>
    /// Current amount the clicker will give if clicked
    /// </summary>
    public int ClickAmount {
        get {
            if (ClickerMultiplier == 0) return StartClickAmount * 1;
            else return StartClickAmount * ClickerMultiplier;
        }
    }
    /// <summary>
    /// Amount of times the clicker has been bought
    /// </summary>
    public int ClickerMultiplier = 0;
    /// <summary>
    /// Public variable for TimeSpan
    /// </summary>
    public int SecondsToCompleteClick;
    /// <summary>
    /// Start time it takes to complete one click. 
    /// </summary>
    public TimeSpan TimeBetweenClicks; //5 seconds
    /// <summary>
    /// Current time the timer is at. Used in displays
    /// </summary>
    public TimeSpan CurrentClickerTime;
    /// <summary>
    /// Bool to determine if Click button has been clicked
    /// </summary>
    public bool IsClicked = false;
    /// <summary>
    /// Cost to buy Manager upgrade
    /// </summary>
    public int ManagerCost;
    /// <summary>
    /// Is the hero automated?
    /// </summary>
    public bool HasManager = false;
    /// <summary>
    /// Can the hero be clicked
    /// </summary>
    public bool CanBeClicked = false;

    public delegate void OnClickButton(string clickerName);
    public event OnClickButton OnClickedButton;
    public delegate void OnClickFinished(string clickerName);
    public event OnClickFinished OnClickedFinished;

    private RadiantSceneController m_sceneController;
    private DateTime m_lastClickedTime;

    private Slider m_progressBar;
    private Text m_heroNameText;
    private Text m_timeRemainingText;
    private Text m_amountBoughtText;
    private Text m_clickButtonGoldText;
    private Text m_upgradeCostText;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_heroNameText = transform.FindChild("Buttons/StandBack/StandUI/ClickerNameText").GetComponent<Text>();
        m_timeRemainingText = transform.FindChild("Buttons/StandBack/StandUI/TimeRemaining").GetComponent<Text>();
        m_amountBoughtText = transform.Find("Buttons/StandBack/StandUI/AmountCanvas/AmountText").GetComponent<Text>();
        m_clickButtonGoldText = transform.Find("Buttons/ClickButtonBack/ClickButton/ClickUI/ClickWorthText").GetComponent<Text>();
        m_upgradeCostText = transform.Find("Buttons/UpgradeCostBack/UpgradeCostCanvas/Cost/UpCostText").GetComponent<Text>(); ;
        m_progressBar = transform.Find("Buttons/StandBack/StandUI/ProgressSlider").GetComponent<Slider>();

        TimeBetweenClicks = new TimeSpan(0, 0, 0, SecondsToCompleteClick);
    }
	
	void Update ()
    {
        UpdateCountdownTimer();
        UpdateUIText();

        if(IsClicked)
        {
            CurrentClickerTime = DateTime.Now - m_lastClickedTime;
            m_progressBar.value = CurrentClickerTime.Milliseconds;
            if(CurrentClickerTime >= TimeBetweenClicks)
            {
                Debug.Log("Completed Click");
                IsClicked = false;
                CompletedClick();
            }
        }

        if(HasManager)
        {
            if(!IsClicked)
            {
                OnClickButtonPressed();
            }
        }
	}

    public void UpdateCountdownTimer()
    {

    }

    public void OnBuyClickerButtonPressed()
    {
        UpgradeCost = Mathf.Round(UpgradeCost * UpgradeMultiplier);
    }

    void UpdateUIText()
    {
        m_heroNameText.text = HeroName;
        m_clickButtonGoldText.text = ClickAmount + " gold";
        m_amountBoughtText.text = ClickerMultiplier.ToString();
        m_upgradeCostText.text = UpgradeCost.ToString() + " gold";
        m_timeRemainingText.text = CurrentClickerTime.ToString();
    }

    public void OnClickButtonPressed()
    {
        //On Clicker first pressed
        m_lastClickedTime = DateTime.Now;
        IsClicked = true;
        OnClickedButton.Invoke(name);
    }

    void CompletedClick()
    {
        //On Clicker timer complete
        m_sceneController.TotalGold += ClickAmount; //Add gold
        OnClickedFinished.Invoke(name);
    }

    public void BuyManager(GameObject obj)
    {
        if(m_sceneController.TotalGold >= ManagerCost)
        {
            HasManager = true;
            obj.SetActive(false);
        }
    }

    public void ActivateAbility(string abilityName)
    {
        //Very bad of you, but too lazy
        if(abilityName == "OverchargeBtn" || abilityName == "RelocateBtn")
        {
            IoController io = GetComponentInParent<IoController>();
            if (!io.OverchargeUpgrade || !io.RelocateUpgrade)
                return;

            if (abilityName == "OverchargeBtn")
            {
                if (io.OverchargeActive)
                    return;

                io.ActivateOvercharge();
            }
            else if(abilityName == "RelocateBtn")
            {
                if (io.RelocateActive)
                    return;

                io.ActivateRelocate();
            }
        }
        else if (abilityName == "TelekinesisBtn" || abilityName == "SpellStealBtn")
        {
            RubickController rubick = GetComponentInParent<RubickController>();
            if (!rubick.TelekinesisUpgrade || !rubick.SpellStealUpgrade)
                return;

            if (abilityName == "TelekinesisBtn")
            {
                if (rubick.TelekinesisActive)
                    return;

                rubick.ActivateTelekinesis();
            }
            else if(abilityName == "SpellStealBtn")
            {
                if (rubick.SpellStealActive)
                    return;

                rubick.ActivateSpellSteal();
            }
        }
        else if (abilityName == "FireblastBtn" || abilityName == "BloodlustBtn")
        {
            OgreMagiController ogreMagi = GetComponentInParent<OgreMagiController>();
            if (!ogreMagi.FireblastUpgrade || !ogreMagi.BloodlustUpgrade)
                return;

            if (abilityName == "FireblastBtn")
            {
                if (ogreMagi.FireblastActive)
                    return;

                ogreMagi.ActivateFireblast();
            }
            else if(abilityName == "BloodlustBtn")
            {
                if (ogreMagi.BloodlustActive)
                    return;

                ogreMagi.ActivateBloodlust();
            }
        }
        else if (abilityName == "SnowballBtn" || abilityName == "WalrusPunchBtn")
        {
            TuskController tusk = GetComponentInParent<TuskController>();
            if (!tusk.SnowballUpgrade || !tusk.WalrusPunchUpgrade)
                return;

            if (abilityName == "SnowballBtn")
            {
                if (tusk.SnowballActive)
                    return;

                tusk.ActivateSnowball();
            }
            else if(abilityName == "WalrusPunchBtn")
            {
                if (tusk.WalrusPunchActive)
                    return;

                tusk.ActivateWalrusPunch();
            }
        }
        else if (abilityName == "SunrayBtn" || abilityName == "SupernovaBtn")
        {
            PhoenixController phoenix = GetComponentInParent<PhoenixController>();
            if (!phoenix.SunrayUpgrade || !phoenix.SupernovaUpgrade)
                return;

            if (abilityName == "SunrayBtn")
            {
                if (phoenix.SunrayActive)
                    return;

                phoenix.ActivateSunray();
            }
            else if(abilityName == "SupernovaBtn")
            {
                if (phoenix.SupernovaActive)
                    return;

                phoenix.ActivateSupernova();
            }
        }
        else if (abilityName == "WarCryBtn" || abilityName == "GodsStrengthBtn")
        {
            SvenController sven = GetComponentInParent<SvenController>();
            if (!sven.WarCryUpgrade || !sven.GodsStrengthUpgrade)
                return;

            if (abilityName == "WarCryBtn")
            {
                if (sven.WarCryActive)
                    return;

                sven.ActivateWarCry();
            }
            else if (abilityName == "GodsStrengthBtn")
            {
                if (sven.GodsStrengthActive)
                    return;

                sven.ActivateGodsStrength();
            }
        }
        else if (abilityName == "BlinkBtn" || abilityName == "ManaVoid")
        {
            AntiMageController antiMage = GetComponentInParent<AntiMageController>();
            if (!antiMage.BlinkUpgrade || !antiMage.ManaVoidUpgrade)
                return;

            if (abilityName == "BlinkBtn")
            {
                if (antiMage.BlinkActive)
                    return;

                antiMage.ActivateBlink();
            }
            else if (abilityName == "ManaVoid")
            {
                if (antiMage.ManaVoidActive)
                    return;

                antiMage.ActivateManaVoid();
            }
        }
        else if (abilityName == "GreevilsGreedBtn" || abilityName == "ChemicalRageBtn")
        {
            AlchemistController alchemist = GetComponentInParent<AlchemistController>();
            if (!alchemist.GreevilsGreedUpgrade || !alchemist.ChemicalRageUpgrade)
                return;

            if (abilityName == "GreevilsGreedBtn")
            {
                if (alchemist.GreevilsGreedActive)
                    return;

                alchemist.ActivateGreevilsGreed();
            }
            else
            {
                if (alchemist.ChemicalRageActive)
                    return;

                alchemist.ActivateChemicalRage();
            }
        }   
    }

    public static void PlayRandomClip(AudioSource audioSource, AudioClip[] clips)
    {
        int pick = UnityEngine.Random.Range(0, clips.Length);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clips[pick]);
        }
    }
}
