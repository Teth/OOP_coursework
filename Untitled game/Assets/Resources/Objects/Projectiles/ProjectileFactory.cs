using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//flyweight

//flyweight factory
public class ProjectileFactory
{
    Dictionary<EnemyType, AbstractProjectile> projectiles = new Dictionary<EnemyType, AbstractProjectile>();
    public ProjectileFactory()
    {
        projectiles.Add(EnemyType.Rat, new RatProjectile(15));
        projectiles.Add(EnemyType.Gnoll, new GnollProjectile(17));
        projectiles.Add(EnemyType.Demon, new DemonProjectile(13));
        projectiles.Add(EnemyType.Skeleton, new SkeletonProjectile(20));
    }
    public AbstractProjectile GetProjectile(EnemyType key)
    {
        if (projectiles.ContainsKey(key))
            return projectiles[key];
        else
            return null;
    }
}

// abstract flyweight
public abstract class AbstractProjectile
{    
    public int damage { get; set; }
    protected GameObject projectilePrefab;
    public GameObject GetProjectilePrefab() => projectilePrefab;
}
//concrete flyweight
public class RatProjectile : AbstractProjectile
{   
    public RatProjectile(float speed)
    {        
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        projectilePrefab = prefabLoader.LoadAsset("Objects/Projectiles/Goo.prefab");        
        projectilePrefab.GetComponent<Projectile>().speed = speed;
    }    
}

public class GnollProjectile : AbstractProjectile
{
    public GnollProjectile(float speed)
    {        
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        projectilePrefab = prefabLoader.LoadAsset("Objects/Projectiles/Bullet.prefab");        
        projectilePrefab.GetComponent<Projectile>().speed = speed;
    }
}
public class DemonProjectile : AbstractProjectile
{
    public DemonProjectile(float speed)
    {      
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        projectilePrefab = prefabLoader.LoadAsset("Objects/Projectiles/Fireball.prefab");
        projectilePrefab.GetComponent<Projectile>().speed = speed;
    }
}
public class SkeletonProjectile : AbstractProjectile
{
    public SkeletonProjectile(float speed)
    {        
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        projectilePrefab = prefabLoader.LoadAsset("Objects/Projectiles/Bone.prefab");
        projectilePrefab.GetComponent<Projectile>().speed = speed;
    }
}