using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Image { get; set; }
    public bool IsUnlocked { get; set; }
}
