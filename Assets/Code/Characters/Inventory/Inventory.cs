using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Item> Items {  get; private set; } = new List<Item>();
    
    public Inventory()
    {

    }
    public Inventory(List<Item> items) : this()
    {
        Items = items;
    }

    public void AddItem(Item item)
    {

    }
    public void AddItem(string itemName)
    {

    }
    public void RemoveItem(string itemName)
    {

    }
}
