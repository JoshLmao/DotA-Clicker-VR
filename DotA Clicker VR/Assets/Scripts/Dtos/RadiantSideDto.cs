using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class RadiantSideDto
{
    public long TotalGold { get; set; }
    /*Clickers*/
    //public HeroDto CrystalMaiden { get; set; }
    //public HeroDto Rubick { get; set; }
    //public HeroDto OgreMagi { get; set; }
    //public HeroDto Tusk { get; set; }
    //public HeroDto Phoenix { get; set; }
    //public HeroDto Sven { get; set; }
    //public HeroDto Alchemist { get; set; }
    //public HeroDto AntiMage { get; set; }
    public List<HeroDto> Heroes { get; set; }

    //Can do Roshan Events?
    public bool RoshanEvents { get; set; }
}
