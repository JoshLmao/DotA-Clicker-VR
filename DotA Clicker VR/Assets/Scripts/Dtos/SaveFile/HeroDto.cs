using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HeroDto
{
    public string HeroName { get; set; }
    public int ClickersBought{ get; set; }
    public double ClickerTimeRemaining { get; set; }

    public int Ability1Level { get; set; }
    public float Ability1UseCount { get; set; }
    public double Ability1RemainingTime { get; set; }
    public int Ability2Level { get; set; }
    public float Ability2UseCount { get; set; }
    public double Ability2RemainingTime { get; set; }

    public bool ModifierActive { get; set; }
    public string CurrentModifier { get; set; }
    public double ModifierTimeRemaining { get; set; }
}
