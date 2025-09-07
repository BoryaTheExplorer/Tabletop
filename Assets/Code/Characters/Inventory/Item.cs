using UnityEngine;

public class Item
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ItemType Type { get; private set; } = ItemType.Misc;

    public Item()
    {

    }
    public Item(string name, string description, ItemType type) : this()
    {
        Name = name;
        Description = description;
        Type = type;
    }
    public void ChangeName(string name)
    {
        Name = name;
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
