using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HeroDto
{
    public int ClickersBought{ get; set; }
    public int Ability1Level { get; set; }
    public float Ability1UseCount { get; set; }
    public int Ability2Level { get; set; }
    public float Ability2UseCount { get; set; }
}
