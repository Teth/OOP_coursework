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
    public RatFactory()
    {
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        meleePrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Rat.prefab");
        rangedPrefab = prefabLoader.LoadAsset("Assets/Objects/Enemy/Textures/Rat.prefab");
    }

    public override GameObject CreateMeleeEnemy()
    {        
        return Object.Instantiate(meleePrefab);    
    }

    public override GameObject CreateRangedEnemy()
    {
        throw new System.NotImplementedException();
    }
}
public class DemonFactory : AbstractEnemyFactory
{
    public override GameObject CreateMeleeEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject CreateRangedEnemy()
    {
        throw new System.NotImplementedException();
    }
}
public class GnollFactory : AbstractEnemyFactory
{
    public override GameObject CreateMeleeEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject CreateRangedEnemy()
    {
        throw new System.NotImplementedException();
    }
}
public class SkeletonFactory : AbstractEnemyFactory
{
    public override GameObject CreateMeleeEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject CreateRangedEnemy()
    {
        throw new System.NotImplementedException();
    }
}