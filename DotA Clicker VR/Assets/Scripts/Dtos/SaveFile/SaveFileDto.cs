using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SaveFileDto
{ 
    public string PlayerName { get; set; }
    public RadiantSideDto RadiantSide { get; set; }
    public StatsDto SessionStats { get; set; }
    public AchievementsDto Achievements { get; set; }
    public RoshanDto Roshan { get; set; }
}
