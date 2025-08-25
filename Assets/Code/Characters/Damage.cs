using UnityEngine;

public class Damage
{
    public DamageType Type {  get; private set; }
    public int Points { get; private set; } = 0;

    public Damage(DamageType type, int points)
    {
        this.Type = type;
        this.Points = points;
    }
}
