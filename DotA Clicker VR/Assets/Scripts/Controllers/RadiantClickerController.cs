using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public int ClickAmount
    {
        get
        {
            if (ClickerMultiplier == 0) return StartClickAmount * 1;
            else return StartClickAmount * ClickerMultiplier;
        }
        set
        {
            StartClickAmount = value;
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
    /// <summary>
    /// Amount of times the Ability has to be pressed to level up at Lvl 1
    /// </summary>
    public int Ability1LvlUpCount;
    /// <summary>
    /// Amount of times the Ability has to be pressed to level up at Lvl 1
    /// </summary>
    public int Ability2LvlUpCount;

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

    //Level Up System
    /// <summary>
    /// Ability 1 Level
    /// </summary>
    public int Ability1Level = 0;
    /// <summary>
    /// Ability 2 Level
    /// </summary>
    public int Ability2Level = 0;
    Slider m_abil1Slider;
    Slider m_abil2Slider;
    List<GameObject> m_abil1Icons = new List<GameObject>();
    List<GameObject> m_abil2Icons = new List<GameObject>();
    /// <summary>
    /// Amount needed to level up from previous to next level - Ability 1
    /// </summary>
    public int m_abil1UseCount = 0;
    /// <summary>
    /// Amount needed to level up from previous to next level - Ability 2
    /// </summary>
    public int m_abil2UseCount = 0;
    Sprite m_notLevelled;
    Sprite m_levelled;

    //Cant Use Ability sound
    AudioClip MagicImmuneSound;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_heroNameText = transform.FindChild("Buttons/StandBack/StandUI/ClickerNameText").GetComponent<Text>();
        m_timeRemainingText = transform.FindChild("Buttons/StandBack/StandUI/ProgressSlider/TimeRemaining").GetComponent<Text>();
        m_amountBoughtText = transform.Find("Buttons/StandBack/StandUI/AmountCanvas/AmountText").GetComponent<Text>();
        m_clickButtonGoldText = transform.Find("Buttons/ClickButtonBack/ClickButton/ClickUI/ClickWorthText").GetComponent<Text>();
        m_upgradeCostText = transform.Find("Buttons/UpgradeCostBack/UpgradeCostCanvas/Cost/UpCostText").GetComponent<Text>(); ;
        m_progressBar = transform.Find("Buttons/StandBack/StandUI/ProgressSlider").GetComponent<Slider>();
        MagicImmuneSound = Resources.Load<AudioClip>("Sounds/UI/magic_immune");
        AbilityLevelUpStart();

        TimeBetweenClicks = new TimeSpan(0, 0, 0, SecondsToCompleteClick);
        //m_clickAmount = ClickAmount;
    }
	
    void AbilityLevelUpStart()
    {
        m_abil1Slider = transform.Find("Buttons/StandBack/UpgradesCanvas/Abil1Progress").GetComponent<Slider>();
        m_abil2Slider = transform.Find("Buttons/StandBack/UpgradesCanvas/Abil2Progress").GetComponent<Slider>();

        Transform abil1Transform = transform.Find("Buttons/StandBack/UpgradesCanvas/Abil1Levels");
        Transform abil2Transform = transform.Find("Buttons/StandBack/UpgradesCanvas/Abil2Levels");
        foreach (Transform trans in abil1Transform)
        {
            m_abil1Icons.Add(trans.gameObject);
        }
        foreach (Transform trans in abil2Transform)
        {
            m_abil2Icons.Add(trans.gameObject);
        }

        m_notLevelled = Resources.Load<Sprite>("Images/UI/Skills/NotLevelled");
        m_levelled = Resources.Load<Sprite>("Images/UI/Skills/Levelled");

        m_abil1Slider.maxValue = Ability1LvlUpCount;
        m_abil1Slider.value = 0;

        m_abil2Slider.maxValue = Ability2LvlUpCount;
        m_abil2Slider.value = 0;
    }

	void Update ()
    {
        UpdateUIText();

        if(IsClicked)
        {
            CurrentClickerTime = DateTime.Now - m_lastClickedTime;
            if(CurrentClickerTime >= TimeBetweenClicks)
            {
                IsClicked = false;
                CompletedClick();
            }
            UpdateCountdownTimer();
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
        var percent = Divide(CurrentClickerTime, TimeBetweenClicks) * 100f;
        m_progressBar.value = percent;

        if(percent <= 100f)
        {
            m_progressBar.value = percent;
        }
        else
        {
            m_progressBar.value = 0;
        }
    }

    public static float Divide(TimeSpan dividend, TimeSpan divisor)
    {
        return (float)dividend.Ticks / (float)divisor.Ticks;
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

        if (CurrentClickerTime <= TimeBetweenClicks)
            m_timeRemainingText.text = CurrentClickerTime.ToString();
        else
            m_timeRemainingText.text = new TimeSpan(0, 0, 0, 0).ToString();
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

        m_sceneController.ClickCount++; //Add to global click count
    }

    public void BuyManager(GameObject obj)
    {
        if(m_sceneController.TotalGold >= ManagerCost)
        {
            HasManager = true;
            obj.SetActive(false);
        }
    }

    public void ActivateAbility(string abilityName, uint index)
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
                {
                    CantUseAbility();
                    return;
                }

                io.ActivateOvercharge();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if(abilityName == "RelocateBtn")
            {
                if (io.RelocateActive)
                {
                    CantUseAbility();
                    return;
                }

                io.ActivateRelocate();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                rubick.ActivateTelekinesis();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if(abilityName == "SpellStealBtn")
            {
                if (rubick.SpellStealActive)
                {
                    CantUseAbility();
                    return;
                }

                rubick.ActivateSpellSteal();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                ogreMagi.ActivateFireblast();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if(abilityName == "BloodlustBtn")
            {
                if (ogreMagi.BloodlustActive)
                {
                    CantUseAbility();
                    return;
                }

                ogreMagi.ActivateBloodlust();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                tusk.ActivateSnowball();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if(abilityName == "WalrusPunchBtn")
            {
                if (tusk.WalrusPunchActive)
                {
                    CantUseAbility();
                    return;
                }

                tusk.ActivateWalrusPunch();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                phoenix.ActivateSunray();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if(abilityName == "SupernovaBtn")
            {
                if (phoenix.SupernovaActive)
                {
                    CantUseAbility();
                    return;
                }

                phoenix.ActivateSupernova();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                sven.ActivateWarCry();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if (abilityName == "GodsStrengthBtn")
            {
                if (sven.GodsStrengthActive)
                {
                    CantUseAbility();
                    return;
                }

                sven.ActivateGodsStrength();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                antiMage.ActivateBlink();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else if (abilityName == "ManaVoid")
            {
                if (antiMage.ManaVoidActive)
                {
                    CantUseAbility();
                    return;
                }

                antiMage.ActivateManaVoid();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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
                {
                    CantUseAbility();
                    return;
                }

                alchemist.ActivateGreevilsGreed();
                HandController.RumbleController(index, 2000);
                Ability1Used();
            }
            else
            {
                if (alchemist.ChemicalRageActive)
                {
                    CantUseAbility();
                    return;
                }

                alchemist.ActivateChemicalRage();
                HandController.RumbleController(index, 2000);
                Ability2Used();
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

    void Ability1Used()
    {
        //Add the use count
        m_abil1UseCount++;

        //If use count matches lvlUp count, level up and reset slider
        if (m_abil1UseCount >= Ability1LvlUpCount)
        {
            //Add a new level if the level isn't lvl 4
            if (Ability1Level < 4)
            {
                Ability1Level++;
            }

            m_abil1Slider.value = 0;
            double rounded = Math.Round(Ability1LvlUpCount * 1.5f, 1, MidpointRounding.AwayFromZero);
            m_abil1Slider.maxValue = (float)rounded;

            ResetLevelIcons("1");
            m_abil1UseCount = 0;
        }
        else
        {
            m_abil1Slider.value = m_abil1UseCount;
        }

    }

    void Ability2Used()
    {
        //Add the use count
        m_abil2UseCount++;

        //If use count matches lvlUp count, level up and reset slider
        if(m_abil2UseCount >= Ability2LvlUpCount)
        {
            //Add a new level if the level isn't lvl 4
            if (Ability2Level < 4)
            {
                Ability2Level++;
            }

            m_abil2Slider.value = 1; //1 as we're adding the current count at start
            double rounded = Math.Round(Ability2LvlUpCount * 1.5f, 1, MidpointRounding.AwayFromZero);
            m_abil2Slider.maxValue = (float)rounded;

            ResetLevelIcons("2");
            m_abil2UseCount = 0;
        }
        else
        {
            //Simply update the value if can't lvl up
            m_abil2Slider.value = m_abil2UseCount;
        }
    }

    public void ResetLevelIcons(string abilityNo)
    {
        if(abilityNo == "1")
        {
            //Reset levels
            for (int j = 0; j > m_abil1Icons.Count; j++)
            {
                m_abil1Icons[j].GetComponent<Image>().sprite = m_notLevelled;
            }
            //Set correct levels by image
            for (int i = 0; i < Ability1Level; i++)
            {
                m_abil1Icons[i].GetComponent<Image>().sprite = m_levelled;
            }
        }
        else
        {
            for (int j = 0; j > m_abil2Icons.Count; j++)
            {
                m_abil2Icons[j].GetComponent<Image>().sprite = m_notLevelled;
            }
            for (int i = 0; i < Ability2Level; i++)
            {
                m_abil2Icons[i].GetComponent<Image>().sprite = m_levelled;
            }
        }
    }

    bool canPlayNopeSound = true;

    void CantUseAbility()
    {
        AudioSource source = this.GetComponentInChildren<AudioSource>();
        //if(!source.isPlaying)]
        if(canPlayNopeSound)
        {
            source.PlayOneShot(MagicImmuneSound);
            canPlayNopeSound = false;
            StartCoroutine(CanPlayNopeSound());
        }

    }

    IEnumerator CanPlayNopeSound()
    {
        yield return new WaitForSeconds(0.2f);
        canPlayNopeSound = true;
    }
}
