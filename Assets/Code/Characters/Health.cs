using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public int Points { get; private set; } = 1;
    public List<DamageType> Immunities { get; private set; } = new List<DamageType>();
    public List<DamageType> Resistances { get; private set; } = new List<DamageType>();
    public List<DamageType> Vulnerabilities { get; private set; } = new List<DamageType>();

    public Health()
    {

    }
    public Health(int points, List<DamageType> immunities = default, List<DamageType> resistances = default, List<DamageType> vulnerabilities = default)
    {
        this.Points = points;

        if (immunities != default) 
            this.Immunities = immunities;

        if (resistances != default)
            this.Resistances = resistances;

        if (vulnerabilities != default)
            this.Vulnerabilities = vulnerabilities;
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

        Points -= damage;
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
