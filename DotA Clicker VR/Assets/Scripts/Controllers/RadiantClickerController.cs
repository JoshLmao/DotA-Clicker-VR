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
    /// <summary>
    /// For stats and saving
    /// </summary>
    public float Ability1UseCount;
    /// <summary>
    /// For stats and saving
    /// </summary>
    public float Ability2UseCount;
    /// <summary>
    /// 
    /// </summary>
    public int ItemModifierMultiplier = 1;

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

    //ItemModifiers
    bool m_ironBranchModifierActive = false, m_clarityModifierActive = false, m_magicStickModifierActive = false,
        m_quellingBladeModifierActive = false, m_mangoModifierActive = false, m_powerTreadsModifierActive = false,
        m_bottleModifierActive = false, m_blinkDaggerModifierActive = false, m_hyperstoneModifierActive = false,
        m_bloodstoneModifierActive = false, m_reaverModifierActive = false, m_divineRapierModifierActive = false,
        m_recipeModifierActive = false;

    int m_ironBranchModify, m_clarityModify, m_magicStickModify, m_quellingBladeModify, m_mangoModify, m_powerTreadsModify,
        m_bottleModify, m_blinkDaggerModify, m_hyperstoneModify, m_bloodstoneModify, m_reaverModify, m_divineRapierModify, m_recipeModify;

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

        HandController.IronBranchModifierAdded += OnIronBranchModifier;
        HandController.ClarityModifierAdded += OnClarityModifier;
        HandController.MagicStickModifierAdded += OnMagicStickModifier;
        HandController.QuellingBladeModifierAdded += OnQuellingBladeModifier;
        HandController.MangoModifierAdded += OnMangoModifier;
        HandController.PowerTreadsModifierAdded += OnPowerTreadsModifier;
        HandController.BottleModifierAdded += OnBottleModifier;
        HandController.BlinkDaggerModifierAdded += OnBlinkDaggerModifier;
        HandController.HyperstoneModifierAdded += OnHyperstoneModifier;
        HandController.BloodstoneModifierAdded += OnBloodstoneModifier;
        HandController.ReaverModifierAdded += OnReaverModifier;
        HandController.DivineRapierModifierAdded += OnDivineRapierModifier;
        HandController.RecipeModifierAdded += OnRecipeModifier;
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

    void OnClickWithTimeRemaining(double secondsRemaining)
    {
        m_lastClickedTime = DateTime.Now - TimeSpan.FromSeconds(SecondsToCompleteClick - secondsRemaining);
        IsClicked = true;
        OnClickedButton.Invoke(name);
    }

    void CompletedClick()
    {
        //On Clicker timer complete
        m_sceneController.AddToTotal(ClickAmount);
        OnClickedFinished.Invoke(name);

        m_sceneController.ClickCount++; //Add to global click count
        m_lastClickedTime = DateTime.MinValue;
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
        if(abilityName == "CrystalNovaBtn" || abilityName == "FrostbiteBtn")
        {
            CMController cm = GetComponentInParent<CMController>();
            if (!cm.CrystalNovaUpgrade || !cm.FrostbiteUpgrade)
                return;

            if (abilityName == "CrystalNovaBtn")
            {
                if (cm.CrystalNovaActive)
                {
                    CantUseAbility();
                    return;
                }

                cm.ActivateCrystalNova();
                HandController.RumbleController(index, 2000);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if(abilityName == "FrostbiteBtn")
            {
                if (cm.FrostbiteActive)
                {
                    CantUseAbility();
                    return;
                }

                cm.ActivateFrostbite();
                HandController.RumbleController(index, 2000);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "BlinkBtn" || abilityName == "ManaVoidBtn")
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
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
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

    void OnIronBranchModifier(string hero, int duration)
    {
        if(!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "ironBranch"));

        m_ironBranchModify = ClickAmount * ItemModifierMultiplier;
        ClickAmount = m_ironBranchModify;
        m_currentModifierTotalTime = Constants.ModifierIronBranchDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/iron_BranchModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_ironBranchModifierActive = true;

        if(m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[0]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnClarityModifier(string hero, int duration)
    {
        //If a modifier is active or the clicker GameObject name isnt the same as the hero string
        //The hero string is the parent name of the Item Prefab, which is the clicker itself.
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "clarity"));

        m_clarityModify = ClickAmount * (ItemModifierMultiplier * (int)1.5);
        ClickAmount = m_clarityModify;
        m_currentModifierTotalTime = Constants.ModifierClarityDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/clarityModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_clarityModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[1]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnMagicStickModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "magicStick"));

        m_magicStickModify = ClickAmount * (ItemModifierMultiplier * (int)2);
        ClickAmount = m_magicStickModify;
        m_currentModifierTotalTime = Constants.ModifierMagicStickDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/magic_StickModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_magicStickModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[2]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnQuellingBladeModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "quellingBlade"));

        m_quellingBladeModify = ClickAmount * (ItemModifierMultiplier * (int)2.5);
        ClickAmount = m_quellingBladeModify;
        m_currentModifierTotalTime = Constants.ModifierQuellingBladeDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/quelling_BladeModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_quellingBladeModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[3]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnMangoModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "mango"));

        m_mangoModify = ClickAmount * (ItemModifierMultiplier * (int)3);
        ClickAmount = m_mangoModify;
        m_currentModifierTotalTime = Constants.ModifierMangoDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/mangoModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_mangoModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[4]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnPowerTreadsModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "powerTreads"));

        m_powerTreadsModify = ClickAmount * (ItemModifierMultiplier * (int)3.5);
        ClickAmount = m_powerTreadsModify;
        m_currentModifierTotalTime = Constants.ModifierPowerTreadsDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/power_TreadsModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_powerTreadsModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[5]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnBottleModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "bottle"));

        m_bottleModify = ClickAmount * (ItemModifierMultiplier * (int)4);
        ClickAmount = m_bottleModify;
        m_currentModifierTotalTime = Constants.ModifierBottleDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/bottleModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_bottleModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[6]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnBlinkDaggerModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "blinkDagger"));

        m_blinkDaggerModify = ClickAmount * (ItemModifierMultiplier * (int)4.5);
        ClickAmount = m_blinkDaggerModify;
        m_currentModifierTotalTime = Constants.ModifierBlinkDaggerDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/blink_DaggerModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_blinkDaggerModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[7]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnHyperstoneModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "hyperstone"));

        m_hyperstoneModify = ClickAmount * (ItemModifierMultiplier * (int)5);
        ClickAmount = m_hyperstoneModify;
        m_currentModifierTotalTime = Constants.ModifierHyperstoneDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/hyperstoneModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_hyperstoneModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[8]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnBloodstoneModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "bloodstone"));

        m_bloodstoneModify = ClickAmount * (ItemModifierMultiplier * (int)5.5);
        ClickAmount = m_bloodstoneModify;
        m_currentModifierTotalTime = Constants.ModifierBloodstoneDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/bloodstoneModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_bloodstoneModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[9]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnReaverModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "reaver"));

        m_reaverModify = ClickAmount * (ItemModifierMultiplier * (int)6);
        ClickAmount = m_reaverModify;
        m_currentModifierTotalTime = Constants.ModifierReaverDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/reaverModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_reaverModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[10]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnDivineRapierModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "divineRapier"));

        m_divineRapierModify = ClickAmount * (ItemModifierMultiplier * (int)6.5);
        ClickAmount = m_divineRapierModify;
        m_currentModifierTotalTime = Constants.ModifierDivineRapierDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/divine_RapierModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_divineRapierModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[11]);
            //Make changes to rigidbody & position and parent it
            m_activeItemModifierPrefab.transform.parent = m_itemModifierHolder.transform;
            m_activeItemModifierPrefab.transform.position = Vector3.zero;
            m_activeItemModifierPrefab.transform.rotation = Quaternion.identity;
        }
    }

    void OnRecipeModifier(string hero, int duration)
    {
        if (!CheckIfModifierActive() || name != hero)
        {
            return;
        }

        StartCoroutine(WaitForItemModifier(duration, "recipe"));

        m_recipeModify = ClickAmount * (ItemModifierMultiplier * (int)10);
        ClickAmount = m_recipeModify;
        m_currentModifierTotalTime = Constants.ModifierRecipeDuration;

        m_activeModifier.sprite = Resources.Load<Sprite>("Images/UI/ModifierIcons/recipeModifier");
        m_activeModifier.color = new Color(255, 255, 255, 1);
        m_recipeModifierActive = true;

        if (m_activeItemModifierPrefab == null)
        {
            m_activeItemModifierPrefab = Instantiate(m_sceneController.ItemModifierDisplayPrefabs[12]);
            //Make changes to rigidbody & position and parent it
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
                    ClickAmount -= m_ironBranchModify;
                    m_ironBranchModifierActive = false;
                    break;
                }
            case "clarity":
                {
                    ClickAmount -= m_clarityModify;
                    m_clarityModifierActive = false;
                    break;
                }
            case "magicStick":
                {
                    ClickAmount -= m_magicStickModify;
                    m_magicStickModifierActive = false;
                    break;
                }
            case "quellingBlade":
                {
                    ClickAmount -= m_quellingBladeModify;
                    m_quellingBladeModifierActive = false;
                    break;
                }
            case "mango":
                {
                    ClickAmount -= m_mangoModify;
                    m_mangoModifierActive = false;
                    break;
                }
            case "powerTreads":
                {
                    ClickAmount -= m_powerTreadsModify;
                    m_powerTreadsModifierActive = false;
                    break;
                }
            case "bottle":
                {
                    ClickAmount -= m_bottleModify;
                    m_bottleModifierActive = false;
                    break;
                }
            case "blinkDagger":
                {
                    ClickAmount -= m_blinkDaggerModify;
                    m_blinkDaggerModifierActive = false;
                    break;
                }
            case "hyperstone":
                {
                    ClickAmount -= m_hyperstoneModify;
                    m_hyperstoneModifierActive = false;
                    break;
                }
            case "bloodstone":
                {
                    ClickAmount -= m_bloodstoneModify;
                    m_bloodstoneModifierActive = false;
                    break;
                }
            case "reaver":
                {
                    ClickAmount -= m_reaverModify;
                    m_reaverModifierActive = false;
                    break;
                }
            case "divineRapier":
                {
                    ClickAmount -= m_divineRapierModify;
                    m_divineRapierModifierActive = false;
                    break;
                }
            case "recipe":
                {
                    ClickAmount -= m_recipeModify;
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
                //Add to Ability Slider count and update value
                m_abil1UseCount = (int)hero.Ability1UseCount;
                m_abil1Slider.value = m_abil1UseCount;

                Ability2Level = hero.Ability2Level;
                Ability2UseCount = hero.Ability2UseCount;

                //Add to Ability Slider count and update value
                m_abil2UseCount = (int)hero.Ability2UseCount;
                m_abil2Slider.value = m_abil2UseCount;

                if(hero.ClickerTimeRemaining != 0)
                {
                    OnClickWithTimeRemaining(hero.ClickerTimeRemaining);
                }

                if (hero.Ability1RemainingTime != 0)
                {
                    if (HeroName == "Crystal Maiden")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("CrystalNovaBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("FrostbiteBtn", hero.Ability2RemainingTime);
                        }
                    }
                    else if (HeroName == "Rubick")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("TelekinesisBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("SpellStealBtn", hero.Ability2RemainingTime);
                        }

                    }
                    else if (HeroName == "Ogre Magi")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("FireblastBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("BloodlustBtn", hero.Ability2RemainingTime);
                        }
                    }
                    else if (HeroName == "Tusk")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("SnowballBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("WalrusPunchBtn", hero.Ability2RemainingTime);
                        }
                    }
                    else if (HeroName == "Phoenix")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("SunrayBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("SupernovaBtn", hero.Ability2RemainingTime);
                        }
                    }
                    else if (HeroName == "Sven")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("WarCryBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("GodsStrengthBtn", hero.Ability2RemainingTime);
                        }
                    }
                    else if (HeroName == "Anti Mage")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("BlinkBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("ManaVoidBtn", hero.Ability2RemainingTime);
                        }
                    }
                    else if (HeroName == "Alchemist")
                    {
                        if (hero.Ability1RemainingTime != 0)
                        {
                            ActivateAbility("GreevilsGreedBtn", hero.Ability1RemainingTime);
                        }
                        if (hero.Ability2RemainingTime != 0)
                        {
                            ActivateAbility("ChemicalRageBtn", hero.Ability2RemainingTime);
                        }
                    }
                }

                if (hero.ModifierTimeRemaining != 0 && hero.CurrentModifier != string.Empty)
                {
                    ActivateModifier(hero.CurrentModifier, (int)hero.ModifierTimeRemaining);
                }
            }
            else
            {
                continue;
            }
        }
    }

    /// <summary>
    /// For loading form Save File
    /// </summary>
    /// <param name="abilityName"></param>
    /// <param name="secondsRemaining"></param>
    void ActivateAbility(string abilityName, double secondsRemaining)
    {
        if (abilityName == "CrystalNovaBtn" || abilityName == "FrostbiteBtn")
        {
            CMController cm = GetComponentInParent<CMController>();
            if (!cm.CrystalNovaUpgrade || !cm.FrostbiteUpgrade)
                return;

            if (abilityName == "CrystalNovaBtn")
            {
                if (cm.CrystalNovaActive)
                {
                    CantUseAbility();
                    return;
                }

                cm.ActivateCrystalNova(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "FrostbiteBtn")
            {
                if (cm.FrostbiteActive)
                {
                    CantUseAbility();
                    return;
                }

                cm.ActivateFrostbite(secondsRemaining);
                //HandController.RumbleController(index, 2000);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

                rubick.ActivateTelekinesis(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "SpellStealBtn")
            {
                if (rubick.SpellStealActive)
                {
                    CantUseAbility();
                    return;
                }

                rubick.ActivateSpellSteal(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

                ogreMagi.ActivateFireblast(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "BloodlustBtn")
            {
                if (ogreMagi.BloodlustActive)
                {
                    CantUseAbility();
                    return;
                }

                ogreMagi.ActivateBloodlust(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

                tusk.ActivateSnowball(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "WalrusPunchBtn")
            {
                if (tusk.WalrusPunchActive)
                {
                    CantUseAbility();
                    return;
                }

                tusk.ActivateWalrusPunch(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

                phoenix.ActivateSunray(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "SupernovaBtn")
            {
                if (phoenix.SupernovaActive)
                {
                    CantUseAbility();
                    return;
                }

                phoenix.ActivateSupernova(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

                sven.ActivateWarCry(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "GodsStrengthBtn")
            {
                if (sven.GodsStrengthActive)
                {
                    CantUseAbility();
                    return;
                }

                sven.ActivateGodsStrength(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
        else if (abilityName == "BlinkBtn" || abilityName == "ManaVoidBtn")
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

                antiMage.ActivateBlink(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else if (abilityName == "ManaVoid")
            {
                if (antiMage.ManaVoidActive)
                {
                    CantUseAbility();
                    return;
                }

                antiMage.ActivateManaVoid(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
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

                alchemist.ActivateGreevilsGreed(secondsRemaining);
                Ability1Used();
                Ability1UseCount++;
                m_ability1ClickTime = DateTime.Now;
            }
            else
            {
                if (alchemist.ChemicalRageActive)
                {
                    CantUseAbility();
                    return;
                }

                alchemist.ActivateChemicalRage(secondsRemaining);
                Ability2Used();
                Ability2UseCount++;
                m_ability2ClickTime = DateTime.Now;
            }
        }
    }

    void ActivateModifier(string modifier, int duration)
    {
        if(modifier == "ironBranch")
        {
            OnIronBranchModifier(modifier, duration);
        }   
        else if(modifier == "clarity")
        {
            OnClarityModifier(modifier, duration);
        }
        else if(modifier == "magicStick")
        {
            OnMagicStickModifier(modifier, duration);
        }
        else if(modifier == "quellingBlade")
        {
            OnQuellingBladeModifier(modifier, duration);
        }
        else if(modifier == "mango")
        {
            OnMangoModifier(modifier, duration);
        }
        else if(modifier == "powerTreads")
        {
            OnPowerTreadsModifier(modifier, duration);
        }
        else if(modifier == "bottle")
        {
            OnBottleModifier(modifier, duration);
        }
        else if(modifier == "blinkDagger")
        {
            OnBlinkDaggerModifier(modifier, duration);
        }
        else if(modifier == "hyperstone")
        {
            OnHyperstoneModifier(modifier, duration);
        }
        else if(modifier == "bloodstone")
        {
            OnBloodstoneModifier(modifier, duration);
        }
        else if(modifier == "reaver")
        {
            OnReaverModifier(modifier, duration);
        }
        else if(modifier == "divineRapier")
        {
            OnDivineRapierModifier(modifier, duration);
        }
        else if(modifier == "recipe")
        {
            OnRecipeModifier(modifier, duration);
        }
    }
}
