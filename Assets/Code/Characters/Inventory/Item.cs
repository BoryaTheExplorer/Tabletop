using UnityEngine;
using WebSocketSharp;

public class Item
{
    public string DisplayName { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ItemType Type { get; private set; } = ItemType.Misc;

    public Item()
    {

    }
    public Item(string name, string description, ItemType type, string displayName) : this()
    {
        Name = name;
        DisplayName = (displayName.IsNullOrEmpty()) ? name : displayName;
        Description = description;
        Type = type;
    }
    public void ChangeDisplayName(string name)
    {
        DisplayName = name;
    }
    public void ChangeDescription(string description)
    {
        Description = description;
    }
    public void ChangeType(ItemType type)
    {
        Type = type;
    }
}
