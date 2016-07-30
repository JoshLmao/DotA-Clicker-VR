using UnityEngine;
using System.Collections;

public class HeroClicker : MonoBehaviour
{
    public class Hero
    {
        public string Name { get; set; }
        public double GoldPerClick { get; set; }
        public Time TimeToComplete { get; set; }
        public int BoughtAmount { get; set; }
    }
}
