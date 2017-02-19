using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class RoshanDto
{
    public bool CanDoRoshanEvents { get; set; }
    public float DefeatCount { get; set; }
    public int DurationTillNextSpawn { get; set; }
    public double RoshanHealth { get; set; }
    public double GoldOnStart { get; set; }
    public double TimeRemaining { get; set; }
}
