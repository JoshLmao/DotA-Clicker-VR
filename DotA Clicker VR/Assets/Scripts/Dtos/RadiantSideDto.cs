using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class RadiantSideDto
{
    public int TotalGold { get; set; }
    /*Clickers*/
    public HeroDto Io { get; set; }
    public HeroDto Rubick { get; set; }
    public HeroDto OgreMagi { get; set; }
    public HeroDto Tusk { get; set; }
    public HeroDto Phoenix { get; set; }
    public HeroDto Sven { get; set; }
    public HeroDto Alchemist { get; set; }
    public HeroDto AntiMage { get; set; }

    //Can do Roshan Events?
    public bool RoshanEvents { get; set; }
}
