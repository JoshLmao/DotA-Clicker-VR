using UnityEngine;
using System.Collections;

public class HeroClicker : MonoBehaviour
{
    public string Name { get; set; }
    public double GoldPerClick { get; set; }
    public Time TimeToComplete { get; set; }
    public int Amount { get; set; }

    public HeroClicker() { }

    public HeroClicker(HeroClicker hero)
    {
        Name = hero.name;
        GoldPerClick = hero.GoldPerClick;
        TimeToComplete = hero.TimeToComplete;
        Amount = hero.Amount;
    }
}
