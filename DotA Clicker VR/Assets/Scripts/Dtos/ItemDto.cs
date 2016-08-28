using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemDto
{
    public string Name { get; set; }
    public Sprite Image { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public GameObject ItemPrefab { get; set; }

    public ItemDto() { }

    public ItemDto(ItemDto item)
    {
        Name = item.Name;
        Image = item.Image;
        Description = item.Description;
        Cost = item.Cost;
        ItemPrefab = item.ItemPrefab;
    }
}
