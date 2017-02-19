using UnityEngine;
using System.Collections;

public class UpgradeDto : MonoBehaviour
{
    public string Name { get; set; }
    public string Description { get; set; }
    /// <summary>
    /// Hero that the upgrade will be applied to
    /// </summary>
    public string HeroUpgrade { get; set; }
    public Sprite Image { get; set; }
    public int Cost { get; set; }

    public UpgradeDto() { }

    public UpgradeDto(UpgradeDto upgrade)
    {
        Name = upgrade.Name;
        Description = upgrade.Description;
        HeroUpgrade = upgrade.HeroUpgrade;
        Image = upgrade.Image;
        Cost = upgrade.Cost;
    }
}
