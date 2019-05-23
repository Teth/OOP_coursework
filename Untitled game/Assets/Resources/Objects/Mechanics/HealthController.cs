using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController
{
    public int maxHealth;
    public bool IsAlive { get; set; }
    public int healthAmount;
    public HealthBar hpbar;
    public HealthController(int health = 100)
    {
        healthAmount = health;
        maxHealth = healthAmount;
        IsAlive = true;
    }

    public void ReceiveDamage(int damage)
    {
        healthAmount -= damage;
        if(hpbar)
            hpbar.setSize((float)healthAmount / maxHealth);
        if (healthAmount <= 0)
        {
            IsAlive = false;
            healthAmount = 0;            
        }
        else if (healthAmount > maxHealth)
        {
            healthAmount = maxHealth;
        }
    }

    public int UpgradeMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        return maxHealth;
    }
}