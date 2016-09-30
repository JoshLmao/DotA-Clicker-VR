using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StatsDto
{
    public float TotalPlayTime { get; set; }
    public float ClickCount { get; set; }

    public int IronBranchCount { get; set; }
    public int ClarityCount { get; set; }
    public int MagicStickCount { get; set; }
    public int QuellingBladeCount { get; set; }
    public int MangoCount { get; set; }
    public int PowerTreadsCount { get; set; }
    public int BottleCount { get; set; }
    public int BlinkDaggerCount { get; set; }
    public int HyperstoneCount { get; set; }
    public int BloodstoneCount { get; set; }
    public int ReaverCount { get; set; }
    public int DivineRapierCount { get; set; }
    public int RecipeCount { get; set; }
}
