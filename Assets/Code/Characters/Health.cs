using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public int MaxPoints { get; private set; } = 1;
    public int CurrentPoints { get; private set; }
    public List<DamageType> Immunities { get; private set; } = new List<DamageType>();
    public List<DamageType> Resistances { get; private set; } = new List<DamageType>();
    public List<DamageType> Vulnerabilities { get; private set; } = new List<DamageType>();

    public Health()
    {

    }
    public Health(int maxPoints, int currentPoints, List<DamageType> immunities = null, List<DamageType> resistances = null, List<DamageType> vulnerabilities = null)
    {
        this.MaxPoints = maxPoints;
        this.CurrentPoints = currentPoints;

        if (immunities != null) 
            this.Immunities = immunities;

        if (resistances != null)
            this.Resistances = resistances;

        if (vulnerabilities != null)
            this.Vulnerabilities = vulnerabilities;
    }
    public Health(int points, List<DamageType> immunities = null, List<DamageType> resistances = null, List<DamageType> vulnerabilities = null) : this(points, points, immunities, resistances, vulnerabilities)
    {

    }

    public bool Damage(DamageType damageType, int damage)
    {
        if (damage < 1)
            return false;

        if (Immunities.Contains(damageType)) 
            return false;

        if (Resistances.Contains(damageType))
            damage /= 2;

        if (Vulnerabilities.Contains(damageType))
            damage *= 2;

        MaxPoints -= damage;
        return true;
    }

    public void AddImmunity(DamageType damageType)
    {
        if (!Immunities.Contains(damageType))
            Immunities.Add(damageType);
    }
    public void AddResistance(DamageType damageType)
    {
        if (!Resistances.Contains(damageType))
            Resistances.Add(damageType);
    }
    public void AddVulnerability(DamageType damageType)
    {
        if (!Vulnerabilities.Contains(damageType))
            Vulnerabilities.Add(damageType);
    }

    public void RemoveImmunity(DamageType damageType)
    {
       Immunities.Remove(damageType);
    }
    public void RemoveResistance(DamageType damageType)
    {
        Resistances.Remove(damageType);
    }
    public void RemoveVulnerability(DamageType damageType)
    {
        Vulnerabilities.Remove(damageType);
    }
}
