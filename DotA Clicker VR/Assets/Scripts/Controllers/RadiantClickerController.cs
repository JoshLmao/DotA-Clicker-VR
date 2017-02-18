using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

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
            if(value > 0)
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
    /// <summary>
    /// For stats and saving
    /// </summary>
    public float Ability1UseCount;
    /// <summary>
    /// For stats and saving
    /// </summary>
    public float Ability2UseCount;
    /// <summary>
    /// Current modifier's multiplier amount
    /// </summary>
    public double ItemModifierMultiplier = -1;

    public double AbilityModifierMulitiplier = -1;

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
    public bool Ability1InUse = false;
    /// <summary>
    /// Ability 2 Level
    /// </summary>
    public int Ability2Level = 0;
    public bool Ability2InUse = false;
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

    //ItemModifiers
    bool m_ironBranchModifierActive = false, m_clarityModifierActive = false, m_magicStickModifierActive = false,
        m_quellingBladeModifierActive = false, m_mangoModifierActive = false, m_powerTreadsModifierActive = false,
        m_bottleModifierActive = false, m_blinkDaggerModifierActive = false, m_hyperstoneModifierActive = false,
        m_bloodstoneModifierActive = false, m_reaverModifierActive = false, m_divineRapierModifierActive = false,
        m_recipeModifierActive = false;

    Image m_activeModifier = null;
    Transform m_itemModifierHolder;
    GameObject m_activeItemModifierPrefab;

    //Track Start Times of Coroutines
    public DateTime m_currentModifierRoutineStarted;
    public int m_currentModifierTotalTime = -1;
    public string m_currentModifier = string.Empty;
    public TimeSpan m_currentClickTimePassed;
    public DateTime m_ability1ClickTime = DateTime.MinValue;
    public DateTime m_ability2ClickTime = DateTime.MinValue;

    bool m_modifierCountdownActive = false;
    Text m_modifierCountdownText = null;

    //Calculates end result of amount * multiplier for UI
    double m_clickAmount;
   
    void Awake()
    {
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;

        AbilityLevelUpStart();

        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_heroNameText = transform.FindChild("Buttons/StandBack/StandUI/ClickerNameText").GetComponent<Text>();
        m_timeRemainingText = transform.FindChild("Buttons/StandBack/StandUI/ProgressSlider/TimeRemaining").GetComponent<Text>();
        m_amountBoughtText = transform.Find("Buttons/StandBack/StandUI/AmountCanvas/AmountText").GetComponent<Text>();
        m_clickButtonGoldText = transform.Find("Buttons/ClickButtonBack/ClickButton/ClickUI/ClickWorthText").GetComponent<Text>();
        m_upgradeCostText = transform.Find("Buttons/UpgradeCostBack/UpgradeCostCanvas/Cost/UpCostText").GetComponent<Text>(); ;
        m_progressBar = transform.Find("Buttons/StandBack/StandUI/ProgressSlider").GetComponent<Slider>();
        MagicImmuneSound = Resources.Load<AudioClip>("Sounds/UI/magic_immune");

        m_activeModifier = transform.Find("Buttons/StandBack/StandUI/ActiveModifierUI/ActiveModifier").GetComponent<Image>();
        m_activeModifier.color = new Color(255, 255, 255, 0);
        m_itemModifierHolder = transform.Find("ItemModifierStand/ItemHolderTransform").gameObject.transform;

        m_modifierCountdownText = transform.Find("ItemModifierStand/ModifierTimeCanvas/ModifierTimeRemaining").GetComponent<Text>();
        m_modifierCountdownText.gameObject.SetActive(false);
    }

    void Start ()
    {
        TimeBetweenClicks = new TimeSpan(0, 0, 0, SecondsToCompleteClick);
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

        //I hate myself for this
        HandController.IronBranchModifierAdded += ActivateModifier;
        HandController.ClarityModifierAdded += ActivateModifier;
        HandController.MagicStickModifierAdded += ActivateModifier;
        HandController.QuellingBladeModifierAdded += ActivateModifier;
        HandController.MangoModifierAdded += ActivateModifier;
        HandController.PowerTreadsModifierAdded += ActivateModifier;
        HandController.BottleModifierAdded += ActivateModifier;
        HandController.BlinkDaggerModifierAdded += ActivateModifier;
        HandController.HyperstoneModifierAdded += ActivateModifier;
        HandController.BloodstoneModifierAdded += ActivateModifier;
        HandController.ReaverModifierAdded += ActivateModifier;
        HandController.DivineRapierModifierAdded += ActivateModifier;
        HandController.RecipeModifierAdded += ActivateModifier;
        PickedUpItemController.IronBranchModifierAdded += ActivateModifier;
        PickedUpItemController.ClarityModifierAdded += ActivateModifier;
        PickedUpItemController.MagicStickModifierAdded += ActivateModifier;
        PickedUpItemController.QuellingBladeModifierAdded += ActivateModifier;
        PickedUpItemController.MangoModifierAdded += ActivateModifier;
        PickedUpItemController.PowerTreadsModifierAdded += ActivateModifier;
        PickedUpItemController.BottleModifierAdded += ActivateModifier;
        PickedUpItemController.BlinkDaggerModifierAdded += ActivateModifier;
        PickedUpItemController.HyperstoneModifierAdded += ActivateModifier;
        PickedUpItemController.BloodstoneModifierAdded += ActivateModifier;
        PickedUpItemController.ReaverModifierAdded += ActivateModifier;
        PickedUpItemController.DivineRapierModifierAdded += ActivateModifier;
        PickedUpItemController.RecipeModifierAdded += ActivateModifier;
    }

    void Update ()
    {
        UpdateUIText();

        if(IsClicked)
        {
            CurrentClickerTime = DateTime.Now - m_lastClickedTime;
            m_currentClickTimePassed = CurrentClickerTime; //For saving, how much time has passed on current click
            if (CurrentClickerTime >= TimeBetweenClicks)
            {
                IsClicked = false;
                CompletedClick();
            }
            UpdateCountdownTimer();
        }
        else
        {
            //For loading a save file in
            m_currentClickTimePassed = TimeSpan.MinValue;
        }

        if(HasManager)
        {
            if(!IsClicked)
            {
                OnClickButtonPressed();
            }
        }

        //if (m_modifierCountdownActive)
        //{
        //    var timeRemaining = m_currentModifierRoutineStarted - DateTime.Now;
        //    m_modifierCountdownText.text = timeRemaining.Seconds.ToString();
        //    Console.WriteLine("timeRemaining = " + timeRemaining);
        //}
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

        CalculateClickResultUI();

        m_clickButtonGoldText.text = m_clickAmount + " gold";
        m_amountBoughtText.text = ClickerMultiplier.ToString();
        m_upgradeCostText.text = UpgradeCost.ToString() + " gold";

        if (CurrentClickerTime <= TimeBetweenClicks)
            m_timeRemainingText.text = CurrentClickerTime.ToString();
        else
            m_timeRemainingText.text = new TimeSpan(0, 0, 0, 0).ToString();
    }

    void CalculateClickResultUI()
    {
        double abilityModify = 0;
        double itemModify = 0;

        if (AbilityModifierMulitiplier > 1)
            abilityModify = AbilityModifierMulitiplier;
        if (ItemModifierMultiplier > 1)
            itemModify = ItemModifierMultiplier;

        if (abilityModify > 0 && itemModify <= 0)
            m_clickAmount = ClickAmount * abilityModify;
        else if (abilityModify <= 0 && itemModify > 0)
            m_clickAmount = ClickAmount * itemModify;
        else if (abilityModify > 0 && itemModify > 0)
            m_clickAmount = ClickAmount * (abilityModify + itemModify);
        else if (abilityModify <= 0 && itemModify <= 0)
            m_clickAmount = ClickAmount;
    }

    public void OnClickButtonPressed()
    {
        //On Clicker first pressed
        m_lastClickedTime = DateTime.Now;
        IsClicked = true;
        OnClickedButton.Invoke(name);
    }

    void OnClickWithTimeRemaining(double secondsRemaining)
    {
        m_lastClickedTime = DateTime.Now - TimeSpan.FromSeconds(SecondsToCompleteClick - secondsRemaining);
        IsClicked = true;
        OnClickedButton.Invoke(name);
    }

    void CompletedClick()
    {
        //On Clicker timer complete
        m_sceneController.AddToTotal(ClickAmount, AbilityModifierMulitiplier, ItemModifierMultiplier);

        if(OnClickedFinished != null)
            OnClickedFinished.Invoke(name);

        m_sceneController.AddToClickTotal(1); //Add to global click count
        m_lastClickedTime = DateTime.MinValue;
    }

    public void BuyManager(GameObject obj)
    {
        if(m_sceneController.TotalGold >= ManagerCost)
        {
            ClickerMultiplier += 1;
            HasManager = true;
            obj.SetActive(false);
        }
    }

    IEnumerator Ability1InUseAwait(int duration)
    {
        yield return new WaitForSeconds(duration);
        Ability1InUse = false;
    }

    IEnumerator Ability2InUseAwait(int duration)
    {
        yield return new WaitForSeconds(duration);
        Ability2InUse = false;
    }

    public bool ActivateAbility(string abilityName)
    {
        //Very bad of you, but too lazy
        if (abilityName == "CrystalNovaBtn" || abilityName == "FrostbiteBtn")
        {
            CMController cm = GetComponentInParent<CMController>();

            if (abilityName == "CrystalNovaBtn")
            {
                if (cm.CrystalNovaActive || !cm.CrystalNovaUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                cm.ActivateCrystalNova();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(cm.CrystalNovaActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "FrostbiteBtn")
            {
                if (cm.FrostbiteActive || !cm.FrostbiteUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                cm.ActivateFrostbite();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(cm.FrostbiteActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "TelekinesisBtn" || abilityName == "SpellStealBtn")
        {
            RubickController rubick = GetComponentInParent<RubickController>();

            if (abilityName == "TelekinesisBtn")
            {
                if (rubick.TelekinesisActive || !rubick.TelekinesisUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                rubick.ActivateTelekinesis();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(rubick.TelekinesisActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "SpellStealBtn")
            {
                if (rubick.SpellStealActive || !rubick.SpellStealUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                rubick.ActivateSpellSteal();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(rubick.SpellStealActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "FireblastBtn" || abilityName == "BloodlustBtn")
        {
            OgreMagiController ogreMagi = GetComponentInParent<OgreMagiController>();

            if (abilityName == "FireblastBtn" || !ogreMagi.FireblastUpgrade)
            {
                if (ogreMagi.FireblastActive || !ogreMagi.FireblastUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                ogreMagi.ActivateFireblast();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(ogreMagi.FireblastActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "BloodlustBtn")
            {
                if (ogreMagi.BloodlustActive || !ogreMagi.BloodlustUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                ogreMagi.ActivateBloodlust();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(ogreMagi.BloodlustActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "SnowballBtn" || abilityName == "WalrusPunchBtn")
        {
            TuskController tusk = GetComponentInParent<TuskController>();

            if (abilityName == "SnowballBtn")
            {
                if (tusk.SnowballActive || !tusk.SnowballUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                tusk.ActivateSnowball();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(tusk.SnowballActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "WalrusPunchBtn")
            {
                if (tusk.WalrusPunchActive || !tusk.WalrusPunchUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                tusk.ActivateWalrusPunch();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(tusk.WalrusPunchActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "SunrayBtn" || abilityName == "SupernovaBtn")
        {
            PhoenixController phoenix = GetComponentInParent<PhoenixController>();

            if (abilityName == "SunrayBtn")
            {
                if (phoenix.SunrayActive || !phoenix.SunrayUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                phoenix.ActivateSunray();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(phoenix.SunrayActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "SupernovaBtn")
            {
                if (phoenix.SupernovaActive || !phoenix.SupernovaUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                phoenix.ActivateSupernova();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(phoenix.SupernovaActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "WarCryBtn" || abilityName == "GodsStrengthBtn")
        {
            SvenController sven = GetComponentInParent<SvenController>();

            if (abilityName == "WarCryBtn")
            {
                if (sven.WarCryActive || !sven.WarCryUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                sven.ActivateWarCry();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(sven.WarCryActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "GodsStrengthBtn")
            {
                if (sven.GodsStrengthActive || !sven.GodsStrengthUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                sven.ActivateGodsStrength();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(sven.GodsStrengthActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "BlinkBtn" || abilityName == "ManaVoidBtn")
        {
            AntiMageController antiMage = GetComponentInParent<AntiMageController>();

            if (abilityName == "BlinkBtn")
            {
                if (antiMage.BlinkActive || !antiMage.BlinkUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                antiMage.ActivateBlink();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(antiMage.BlinkActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "ManaVoid")
            {
                if (antiMage.ManaVoidActive || !antiMage.ManaVoidUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                antiMage.ActivateManaVoid();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(antiMage.ManaVoidActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "GreevilsGreedBtn" || abilityName == "ChemicalRageBtn")
        {
            AlchemistController alchemist = GetComponentInParent<AlchemistController>();

            if (abilityName == "GreevilsGreedBtn")
            {
                if (alchemist.GreevilsGreedActive || !alchemist.GreevilsGreedUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                alchemist.ActivateGreevilsGreed();
                Ability1InUse = true;
                StartCoroutine(Ability1InUseAwait(alchemist.GreevilsGreedActiveDuration));

                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else
            {
                if (alchemist.ChemicalRageActive || !alchemist.ChemicalRageUpgrade)
                {
                    CantUseAbility();
                    return false;
                }

                alchemist.ActivateChemicalRage();
                Ability2InUse = true;
                StartCoroutine(Ability2InUseAwait(alchemist.ChemicalRageActiveDuration));

                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        return true;
    }

    public void ActivateAbility(string abilityName, uint index)
    {
        var success = ActivateAbility(abilityName);
        if(success)
            HandController.RumbleController(index, 2000);
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

            switch (Ability1Level)
            {
                //On level up from previous to that case
                case 1:
                    {
                        Ability1LvlUpCount += 25;
                        break;
                    }
                case 2:
                    {
                        Ability1LvlUpCount += 45;
                        break;
                    }
                case 3:
                    {
                        Ability1LvlUpCount += 50;
                        break;
                    }
                case 4:
                    {
                        Ability1LvlUpCount += 75;
                        break;
                    }
            }

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

            switch (Ability2Level)
            {
                //On level up from previous to that case
                case 1:
                    {
                        Ability2LvlUpCount += Constants.Ability0To1Difference;
                        break;
                    }
                case 2:
                    {
                        Ability2LvlUpCount += Constants.Ability1To2Difference;
                        break;
                    }
                case 3:
                    {
                        Ability2LvlUpCount += Constants.Ability2To3Difference;
                        break;
                    }
                case 4:
                    {
                        Ability2LvlUpCount += Constants.Ability3To4Difference;
                        break;
                    }
            }

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

    IEnumerator WaitForItemModifier(float totalActiveDuration, string modifier)
    {
        m_currentModifierRoutineStarted = DateTime.Now;
        m_currentModifier = modifier;

        m_modifierCountdownActive = true;
        m_modifierCountdownText.gameObject.SetActive(true);

        yield return StartCoroutine(Yield(totalActiveDuration)); //WaitForSeconds(totalActiveDuration)

        m_modifierCountdownActive = false;
        m_modifierCountdownText.gameObject.SetActive(false);

        RemoveModifier(modifier);
        m_currentModifierRoutineStarted = DateTime.MinValue;
        m_currentModifier = string.Empty;
        ItemModifierMultiplier = -1;
    }

    IEnumerator Yield(float duration)
    {
        for(int i = 0; i < duration; i++)
        {
            m_modifierCountdownText.text = (duration - i).ToString();
            yield return new WaitForSeconds(1);
            //var timeRemaining = m_currentModifierRoutineStarted - DateTime.Now;
        }
    }

    public void SetAbilityModifierAmount(double amount)
    {
        if (AbilityModifierMulitiplier == -1)
            AbilityModifierMulitiplier = amount;
        else
        {
            AbilityModifierMulitiplier += amount;
        }
    }

    public void RemoveAbilityModifierAmount(double amount)
    {
        //if ((ItemModifierMultiplier - amount) == 0)
        AbilityModifierMulitiplier -= amount;

        if (AbilityModifierMulitiplier == 0)
            AbilityModifierMulitiplier = -1;
    }

    public void SetItemModifierAmount(double amount)
    {
        ItemModifierMultiplier = amount;
    }

    void AddModifier(string hero, int duration, double multiplierAmount, int multiplierDuration, string modifierName, string modifierPrefabName, string modifierIconLocation, out bool modifierBool)
    {
        if (!CheckIfModifierActive() || HeroName != hero)
        {
            modifierBool = false;
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, modifierName));

        SetItemModifierAmount(multiplierAmount);

        m_currentModifierTotalTime = multiplierDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>(modifierIconLocation);
        m_activeModifier.color = new Color(255, 255, 255, 1);
        modifierBool = true;

        if (m_activeItemModifierPrefab == null)
        {
            var prefab = m_sceneController.ItemModifierDisplayPrefabs.FirstOrDefault(x => x.name.Contains(modifierPrefabName));
            m_activeItemModifierPrefab = Instantiate(prefab);
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Returns false if a modifier is active. Return true if you can add a modifier
    /// </summary>
    /// <returns></returns>
    bool CheckIfModifierActive()
    {
        if (m_ironBranchModifierActive || m_clarityModifierActive || m_magicStickModifierActive || m_quellingBladeModifierActive || m_mangoModifierActive || 
            m_powerTreadsModifierActive || m_bottleModifierActive || m_blinkDaggerModifierActive || m_hyperstoneModifierActive || m_bloodstoneModifierActive || 
            m_reaverModifierActive || m_divineRapierModifierActive || m_reaverModifierActive)
        {
            //Theres already a modifier applied to current hero, return false
            return false;
        }
        else
        {
            //No modifier active on hero
            return true;
        }
    }

    void RemoveModifier(string modifier)
    {
        switch(modifier)
        {
            case "ironBranch":
                {
                    m_ironBranchModifierActive = false;
                    break;
                }
            case "clarity":
                {                    
                    m_clarityModifierActive = false;
                    break;
                }
            case "magicStick":
                {
                    m_magicStickModifierActive = false;
                    break;
                }
            case "quellingBlade":
                {
                    m_quellingBladeModifierActive = false;
                    break;
                }
            case "mango":
                {
                    m_mangoModifierActive = false;
                    break;
                }
            case "powerTreads":
                {
                    m_powerTreadsModifierActive = false;
                    break;
                }
            case "bottle":
                {
                    m_bottleModifierActive = false;
                    break;
                }
            case "blinkDagger":
                {
                    m_blinkDaggerModifierActive = false;
                    break;
                }
            case "hyperstone":
                {
                    m_hyperstoneModifierActive = false;
                    break;
                }
            case "bloodstone":
                {
                    m_bloodstoneModifierActive = false;
                    break;
                }
            case "reaver":
                {
                    m_reaverModifierActive = false;
                    break;
                }
            case "divineRapier":
                {
                    m_divineRapierModifierActive = false;
                    break;
                }
            case "recipe":
                {
                    m_recipeModifierActive = false;
                    break;
                }
        }

        //Remove modifier image & set to transparent
        m_activeModifier.sprite = null;
        m_activeModifier.color = new Color(255, 255, 255, 0);
        
        //Delete active item
        Destroy(m_activeItemModifierPrefab);
        m_activeItemModifierPrefab = null;

        m_currentModifierTotalTime = -1;
        ItemModifierMultiplier = -1;
    }   

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        List<HeroDto> heroes = saveFile.RadiantSide.Heroes;
        foreach (HeroDto hero in heroes)
        {
            if (hero.HeroName == HeroName)
            {
                //Apply save to hero
                ClickerMultiplier = hero.ClickersBought;

                Ability1Level = hero.Ability1Level;
                Ability1UseCount = hero.Ability1UseCount;
                Ability1InUse = hero.Ability1InUse;
                //Add to Ability Slider count and update value
                m_abil1UseCount = (int)hero.Ability1UseCount;
                m_abil1Slider.value = m_abil1UseCount;

                Ability2Level = hero.Ability2Level;
                Ability2UseCount = hero.Ability2UseCount;
                Ability2InUse = hero.Ability2InUse;
                //Add to Ability Slider count and update value
                m_abil2UseCount = (int)hero.Ability2UseCount;
                m_abil2Slider.value = m_abil2UseCount;

                if(hero.ClickerTimeRemaining != 0)
                {
                    OnClickWithTimeRemaining(hero.ClickerTimeRemaining);
                }

                //If ability was counting down
                if (hero.Ability1RemainingTime != 0)
                {
                    if (HeroName == "Crystal Maiden")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("CrystalNovaBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("FrostbiteBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                    else if (HeroName == "Rubick")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("TelekinesisBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("SpellStealBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }

                    }
                    else if (HeroName == "Ogre Magi")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("FireblastBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("BloodlustBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                    else if (HeroName == "Tusk")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("SnowballBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("WalrusPunchBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                    else if (HeroName == "Phoenix")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("SunrayBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("SupernovaBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                    else if (HeroName == "Sven")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("WarCryBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("GodsStrengthBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                    else if (HeroName == "Anti Mage")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("BlinkBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("ManaVoidBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                    else if (HeroName == "Alchemist")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("GreevilsGreedBtn", hero.Ability1RemainingTime, hero.Ability1InUse);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("ChemicalRageBtn", hero.Ability2RemainingTime, hero.Ability2InUse);
                        }
                    }
                }

                if (hero.ModifierTimeRemaining != 0 && hero.CurrentModifier != string.Empty)
                {
                    //check hero.CurrentModifier
                    ActivateModifier(hero.HeroName, hero.CurrentModifier, (int)hero.ModifierTimeRemaining);
                }
            }
            else
            {
                continue;
            }
        }
    }

    /// <summary>
    /// For loading from Save File
    /// </summary>
    /// <param name="abilityName"></param>
    /// <param name="secondsRemaining"></param>
    void ActivateAbility(string abilityName, double secondsRemaining, bool abilityInUse)
    {
        if (abilityName == "CrystalNovaBtn" || abilityName == "FrostbiteBtn")
        {
            CMController cm = GetComponentInParent<CMController>();

            if (abilityName == "CrystalNovaBtn" && cm.CrystalNovaUpgrade)
            {
                if (cm.CrystalNovaActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    cm.ActivateCrystalNova(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }
                else
                {
                    //Only do if ability isnt in use but seconds are remaining
                    cm.OnAbilityFinished("CrystalNovaActiveFinish", false, (int)secondsRemaining);
                }
            }
            else if (abilityName == "FrostbiteBtn" && cm.FrostbiteUpgrade)
            {
                if (cm.FrostbiteActive)
                {
                    CantUseAbility();
                    return;
                }

                if (abilityInUse)
                {
                    cm.ActivateFrostbite(secondsRemaining, false);
                    //HandController.RumbleController(index, 2000);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }
                else
                {
                    cm.OnAbilityFinished("FrostbiteActiveFinish", false, (int)secondsRemaining);
                }
            }
        }
        else if (abilityName == "TelekinesisBtn" || abilityName == "SpellStealBtn")
        {
            RubickController rubick = GetComponentInParent<RubickController>();

            if (abilityName == "TelekinesisBtn" && rubick.TelekinesisUpgrade)
            {
                if (rubick.TelekinesisActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    rubick.ActivateTelekinesis(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                rubick.OnAbilityFinished("TelekinesisActiveFinish", false);

            }
            else if (abilityName == "SpellStealBtn" && rubick.SpellStealUpgrade)
            {
                if (rubick.SpellStealActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    rubick.ActivateSpellSteal(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }

                rubick.OnAbilityFinished("SpellStealActiveFinish", false);
            }
        }
        else if (abilityName == "FireblastBtn" || abilityName == "BloodlustBtn")
        {
            OgreMagiController ogreMagi = GetComponentInParent<OgreMagiController>();

            if (abilityName == "FireblastBtn" && ogreMagi.FireblastUpgrade)
            {
                if (ogreMagi.FireblastActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    ogreMagi.ActivateFireblast(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                ogreMagi.OnAbilityFinished("FireblastActiveFinish", false);
            }
            else if (abilityName == "BloodlustBtn" && ogreMagi.BloodlustUpgrade)
            {
                if (ogreMagi.BloodlustActive)
                {
                    CantUseAbility();
                    return;
                }

                if (abilityInUse)
                {
                    ogreMagi.ActivateBloodlust(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }
                ogreMagi.OnAbilityFinished("BloodlustActiveFinish", false);
            }
        }
        else if (abilityName == "SnowballBtn" || abilityName == "WalrusPunchBtn")
        {
            TuskController tusk = GetComponentInParent<TuskController>();

            if (abilityName == "SnowballBtn" && tusk.SnowballUpgrade)
            {
                if (tusk.SnowballActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    tusk.ActivateSnowball(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                tusk.OnAbilityFinished("SnowballActiveFinish", false);
            }
            else if (abilityName == "WalrusPunchBtn" && tusk.WalrusPunchUpgrade)
            {
                if (tusk.WalrusPunchActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    tusk.ActivateWalrusPunch(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }

                tusk.OnAbilityFinished("WalrusPunchActiveFinish", false);
            }
        }
        else if (abilityName == "SunrayBtn" || abilityName == "SupernovaBtn")
        {
            PhoenixController phoenix = GetComponentInParent<PhoenixController>();

            if (abilityName == "SunrayBtn" && phoenix.SunrayUpgrade)
            {
                if (phoenix.SunrayActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    phoenix.ActivateSunray(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                phoenix.OnAbilityFinished("SunrayActiveFinish", false);
            }
            else if (abilityName == "SupernovaBtn" && phoenix.SupernovaUpgrade)
            {
                if (phoenix.SupernovaActive)
                {
                    CantUseAbility();
                    return;
                }

                if (abilityInUse)
                {
                    phoenix.ActivateSupernova(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }

                phoenix.OnAbilityFinished("SupernovaActiveFinish", false);
            }
        }
        else if (abilityName == "WarCryBtn" || abilityName == "GodsStrengthBtn")
        {
            SvenController sven = GetComponentInParent<SvenController>();

            if (abilityName == "WarCryBtn" && sven.WarCryUpgrade)
            {
                if (sven.WarCryActive)
                {
                    CantUseAbility();
                    return;
                }
                if (abilityInUse)
                {
                    sven.ActivateWarCry(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                sven.OnAbilityFinished("WarCryActiveFinish", false);
            }
            else if (abilityName == "GodsStrengthBtn" && sven.GodsStrengthUpgrade)
            {
                if (sven.GodsStrengthActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    sven.ActivateGodsStrength(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }

                sven.OnAbilityFinished("GodsStrengthActiveFinish", false);
            }
        }
        else if (abilityName == "BlinkBtn" || abilityName == "ManaVoidBtn")
        {
            AntiMageController antiMage = GetComponentInParent<AntiMageController>();

            if (abilityName == "BlinkBtn" && antiMage.BlinkUpgrade)
            {
                if (antiMage.BlinkActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    antiMage.ActivateBlink(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                antiMage.OnAbilityFinished("BlinkActiveFinish", false);
            }
            else if (abilityName == "ManaVoid" && antiMage.ManaVoidUpgrade)
            {
                if (antiMage.ManaVoidActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    antiMage.ActivateManaVoid(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }

                antiMage.OnAbilityFinished("ManaVoidActiveFinish", false);
            }
        }
        else if (abilityName == "GreevilsGreedBtn" || abilityName == "ChemicalRageBtn")
        {
            AlchemistController alchemist = GetComponentInParent<AlchemistController>();

            if (abilityName == "GreevilsGreedBtn" && alchemist.GreevilsGreedUpgrade)
            {
                if (alchemist.GreevilsGreedActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    alchemist.ActivateGreevilsGreed(secondsRemaining, false);
                    Ability1Used();
                    Ability1UseCount++;
                    m_ability1ClickTime = DateTime.Now;
                }

                alchemist.OnAbilityFinished("GreevilsGreedActiveFinish", false);
            }
            else if(abilityName == "ChemicalRageBtn" && alchemist.ChemicalRageUpgrade)
            {
                if (alchemist.ChemicalRageActive)
                {
                    CantUseAbility();
                    return;
                }

                if(abilityInUse)
                {
                    alchemist.ActivateChemicalRage(secondsRemaining, false);
                    Ability2Used();
                    Ability2UseCount++;
                    m_ability2ClickTime = DateTime.Now;
                }

                alchemist.OnAbilityFinished("ChemicalRageActiveFinish", false);
            }
        }
    }

    //Used on Loaded Save. Check HandController for actual complete setting
    void ActivateModifier(string toHero, string item, int durationRemaining)
    {
        if(item == "ironBranch")
        {
            //OnIronBranchModifier(modifier, durationRemaining);
            AddModifier(toHero, durationRemaining, Constants.ModifierIronBranchModifyAmount, Constants.ModifierIronBranchDuration,
                "ironBranch", "iron_branchPrefab", "Images/UI/ModifierIcons/iron_BranchModifier", out m_ironBranchModifierActive);
        }
        else if(item == "clarity")
        {
            //OnClarityModifier(modifier, durationRemaining);
            AddModifier(toHero, durationRemaining, Constants.ModifierClarityModifyAmount, Constants.ModifierClarityDuration,
                "clarity", "clarity", "Images/UI/ModifierIcons/clarityModifier", out m_clarityModifierActive);
        }
        else if(item == "magicStick")
        {
            //OnMagicStickModifier(modifier, durationRemaining);
            AddModifier(toHero, durationRemaining, Constants.ModifierMagicStickModifyAmount, Constants.ModifierMagicStickDuration,
                "magicStick", "magic_stickPrefab", "Images/UI/ModifierIcons/magic_StickModifier", out m_magicStickModifierActive);
        }
        else if(item == "quellingBlade")
        {
            //OnQuellingBladeModifier(modifier, durationRemaining);
            AddModifier(toHero, durationRemaining, Constants.ModifierQuellingBladeModifyAmount, Constants.ModifierQuellingBladeDuration,
                "quellingBlade", "quelling_bladePrefab", "Images/UI/ModifierIcons/quelling_BladeModifier", out m_quellingBladeModifierActive);
        }
        else if(item == "mango")
        {
            //OnMangoModifier(modifier, durationRemaining);
            AddModifier(toHero, durationRemaining, Constants.ModifierMangoModifyAmount, Constants.ModifierMangoDuration,
                "mango", "mangoPrefab", "Images/UI/ModifierIcons/mangoModifier", out m_mangoModifierActive);
        }
        else if(item == "powerTreads")
        {
            //OnPowerTreadsModifier(modifier, durationRemaining);
            AddModifier(toHero, durationRemaining, Constants.ModifierPowerTreadsModifyAmount, Constants.ModifierPowerTreadsDuration,
                "power_Treads", "power_treadsPrefab", "Images/UI/ModifierIcons/power_treadsModifier", out m_powerTreadsModifierActive);
        }
        else if(item == "bottle")
        {
            //OnBottleModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierBottleModifyAmount, Constants.ModifierBottleDuration,
                "bottle", "bottlePrefab", "Images/UI/ModifierIcons/bottleModifier", out m_bottleModifierActive);
        }
        else if(item == "blinkDagger")
        {
            //OnBlinkDaggerModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierBlinkDaggerModifyAmount, Constants.ModifierBlinkDaggerDuration,
                "blinkDagger", "blink_daggerPrefab", "Images/UI/ModifierIcons/blink_DaggerModifier", out m_blinkDaggerModifierActive);
        }
        else if(item == "hyperstone")
        {
            //OnHyperstoneModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierHyperstoneModifyAmount, Constants.ModifierHyperstoneDuration,
                "hyperstone", "hyperstonePrefab", "Images/UI/ModifierIcons/hyperstoneModifier", out m_hyperstoneModifierActive);
        }
        else if(item == "bloodstone")
        {
            //OnBloodstoneModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierBloodstoneModifyAmount, Constants.ModifierBloodstoneDuration,
                "bloodstone", "bloodstonePrefab", "Images/UI/ModifierIcons/bloodstoneModifier", out m_bloodstoneModifierActive);
        }
        else if(item == "reaver")
        {
            //OnReaverModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierBloodstoneModifyAmount, Constants.ModifierBloodstoneDuration,
                "reaver", "reaver", "Images/UI/ModifierIcons/bloodstoneModifier", out m_reaverModifierActive);

        }
        else if(item == "divineRapier")
        {
            //OnDivineRapierModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierDivineRapierModifyAmount, Constants.ModifierDivineRapierDuration,
                "divinerapier", "divine_rapierPrefab", "Images/UI/ModifierIcons/divine_RapierModifier", out m_divineRapierModifierActive);
        }
        else if(item == "recipe")
        {
            //OnRecipeModifier(modifier, duration);
            AddModifier(toHero, durationRemaining, Constants.ModifierRecipeModifyAmount, Constants.ModifierRecipeDuration,
                "recipe", "recipePrefab", "Images/UI/ModifierIcons/recipeModifier", out m_recipeModifierActive);
        }
    }
}
