using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//proxy

public abstract class AbstractHealthController
{        
    public abstract void ReceiveDamage(int damage);   
}

public class HealthController : AbstractHealthController
{
    public int maxHealth;
    public bool IsAlive { get; set; }
    public int healthAmount;
    public HealthController(int health = 100)
    {
        healthAmount = health;
        maxHealth = healthAmount;
        IsAlive = true;
    }
    public override void ReceiveDamage(int damage)
    {
        healthAmount -= damage;        
    }
    public int UpgradeMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        return maxHealth;
    }
}

public class ProxyHealthController : AbstractHealthController
{
    public HealthController healthController { get; set; }

    public ProxyHealthController(int health)
    {
        healthController = new HealthController(health);
    }

    public override void ReceiveDamage(int damage)
    {
        healthController.ReceiveDamage(damage);
        if (healthController.healthAmount <= 0)
        {
            healthController.IsAlive = false;
            healthController.healthAmount = 0;
        }
        else if (healthController.healthAmount > healthController.maxHealth)
        {
            healthController.healthAmount = healthController.maxHealth;
        }
    }
}