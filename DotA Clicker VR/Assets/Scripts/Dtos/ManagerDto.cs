using UnityEngine;
using System.Collections;

public class ManagerDto : MonoBehaviour
{ 
    public string Name { get; set; }
    public Sprite Image { get; set; }
    public int Cost { get; set; }

    public ManagerDto() { }

    public ManagerDto(ManagerDto manager)
    {
        Name = manager.Name;
        Image = manager.Image;
        Cost = manager.Cost;
    }

}
