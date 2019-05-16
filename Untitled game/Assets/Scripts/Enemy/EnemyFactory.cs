using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract factory

public enum EnemyType
{
    Rat,
    Demon,
    Gnoll,
    Skeleton
}

public abstract class AbstractEnemyFactory
{
    protected GameObject meleePrefab;
    protected GameObject rangedPrefab;
    public abstract GameObject CreateRangedEnemy();
    public abstract GameObject CreateMeleeEnemy();
}

public class RatFactory : AbstractEnemyFactory
{
    float AGRO_RANGE = 7;
    float MELEE_ATTACK_RANGE = 1;
    float RANGED_ATTACK_RANGE = 3;
    int RANGED_HEALTH = 30;
    int MELEE_HEALTH = 20;
    float RANGED_SPEED = 5;
    int MELEE_SPEED = 6;
    int MELEE_DAMAGE = 15;
    int RANGED_DAMAGE = 30;
    public RatFactory()
    {
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        meleePrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Rat.prefab");
        rangedPrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Rat.prefab");
    }
    public override GameObject CreateMeleeEnemy()
    {
        GameObject enemyObject = Object.Instantiate(meleePrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, MELEE_ATTACK_RANGE,
            new EnemyController(new MeleeRatController()), MELEE_HEALTH,
            MELEE_SPEED, MELEE_DAMAGE);
        return enemyObject;
    }

    public override GameObject CreateRangedEnemy()
    {
        GameObject enemyObject = Object.Instantiate(rangedPrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, RANGED_ATTACK_RANGE,
            new EnemyController(new RangedRatController()), RANGED_HEALTH,
            RANGED_SPEED, RANGED_DAMAGE);
        return enemyObject;
    }
}
public class DemonFactory : AbstractEnemyFactory
{
    float AGRO_RANGE = 10;
    float MELEE_ATTACK_RANGE = 5;
    float RANGED_ATTACK_RANGE = 10;
    int RANGED_HEALTH = 40;
    int MELEE_HEALTH = 60;
    float RANGED_SPEED = 4;
    int MELEE_SPEED = 5;
    int MELEE_DAMAGE = 30;
    int RANGED_DAMAGE = 40;
    public DemonFactory()
    {
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        meleePrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Demon.prefab");
        rangedPrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Demon.prefab");
    }

    public override GameObject CreateMeleeEnemy()
    {
        GameObject enemyObject = Object.Instantiate(meleePrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, MELEE_ATTACK_RANGE, 
            new EnemyController(new MeleeDemonController()), MELEE_HEALTH, 
            MELEE_SPEED, MELEE_DAMAGE);
        return enemyObject;
    }
    public override GameObject CreateRangedEnemy()
    {
        GameObject enemyObject = Object.Instantiate(rangedPrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, RANGED_ATTACK_RANGE, 
            new EnemyController(new RangedDemonController()), RANGED_HEALTH,
            RANGED_SPEED, RANGED_DAMAGE);
        return enemyObject;
    }
}
public class GnollFactory : AbstractEnemyFactory
{
    float AGRO_RANGE = 10;
    float MELEE_ATTACK_RANGE = 5;
    float RANGED_ATTACK_RANGE = 10;
    int RANGED_HEALTH = 40;
    int MELEE_HEALTH = 60;
    float RANGED_SPEED = 4;
    int MELEE_SPEED = 5;
    int MELEE_DAMAGE = 30;
    int RANGED_DAMAGE = 40;
    public GnollFactory()
    {
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        meleePrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Gnoll.prefab");
        rangedPrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Gnoll.prefab");
    }

    public override GameObject CreateMeleeEnemy()
    {
        GameObject enemyObject = Object.Instantiate(rangedPrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, MELEE_ATTACK_RANGE,
            new EnemyController(new MeleeGnollController()), MELEE_HEALTH,
            MELEE_SPEED, MELEE_DAMAGE);
        return enemyObject;
    }
    public override GameObject CreateRangedEnemy()
    {
        GameObject enemyObject = Object.Instantiate(rangedPrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();        
        enemyScript.SetParameters(AGRO_RANGE, RANGED_ATTACK_RANGE,
            new EnemyController(new RangedGnollController()), RANGED_HEALTH,
            RANGED_SPEED, RANGED_DAMAGE);
        return enemyObject;
    }
}
public class SkeletonFactory : AbstractEnemyFactory
{
    float AGRO_RANGE = 10;
    float MELEE_ATTACK_RANGE = 5;
    float RANGED_ATTACK_RANGE = 10;
    int RANGED_HEALTH = 40;
    int MELEE_HEALTH = 60;
    float RANGED_SPEED = 4;
    int MELEE_SPEED = 5;
    int MELEE_DAMAGE = 30;
    int RANGED_DAMAGE = 40;
    public SkeletonFactory()
    {
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        meleePrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Skeleton.prefab");
        rangedPrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Skeleton.prefab");
    }

    public override GameObject CreateMeleeEnemy()
    {
        GameObject enemyObject = Object.Instantiate(meleePrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, MELEE_ATTACK_RANGE,
            new EnemyController(new MeleeSkeletonController()), MELEE_HEALTH,
            MELEE_SPEED, MELEE_DAMAGE);
        return enemyObject;
    }
    public override GameObject CreateRangedEnemy()
    {
        GameObject enemyObject = Object.Instantiate(rangedPrefab);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.SetParameters(AGRO_RANGE, RANGED_ATTACK_RANGE,
            new EnemyController(new RangedSkeletonController()), RANGED_HEALTH,
            RANGED_SPEED, RANGED_DAMAGE);
        return enemyObject;
    }
}