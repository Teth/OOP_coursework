﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ExitTile : MonoBehaviour
{
    GameObject bossLink;

    [SerializeField]
    Sprite openStateSprite;
    [SerializeField]
    Sprite closedStateSprite;
    GameObject player;
    [SerializeField]
    float range;
    private ExitTileStateInterface state;
    private ClosedState closedState;
    private OpenedState openedState;
    private bool bossAlive;
    bool isPlayerExists = false;
    SpriteRenderer spRenderer;

    void SetState(ExitTileStateInterface newState)
    {
        state = newState;
        if(newState == closedState)
        {
            spRenderer.sprite = closedStateSprite;
        }
        else
        {
            spRenderer.sprite = openStateSprite;
        }
    }

    void Update()
    {        
        if (!isPlayerExists)
        {
            player = GameObject.FindWithTag("Player");            
            if (player)
                isPlayerExists = true;
        }
        else if (player != null)
        {
            float distance = (player.transform.position - transform.position).magnitude;
            if (distance < range)
            {
                if (!bossAlive)
                {
                    //spawn boss
                    bossAlive = true;

                    state.PlayerInRange(transform);
                }
                if (distance < 1)
                {
                    state.PlayerOnTile();
                }
            }

            bossLink = GameObject.FindWithTag("Boss");

            if (bossAlive && bossLink == null)
            {
                SetState(openedState);
                bossAlive = false;
            }
        }        
    }
    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        bossAlive = false;
        player = GameObject.FindWithTag("Player");
        closedState = new ClosedState();
        openedState = new OpenedState();
        SetState(closedState);
    }
}


public interface ExitTileStateInterface
{
    void PlayerOnTile();
    void PlayerInRange(Transform parentTransform);
}

public class OpenedState : ExitTileStateInterface
{
    ParticleSystem particleSystem;
    GameObject particles;
    GameData gameData;
    bool isPlaying;
    public OpenedState()
    {
        gameData = new AssetProxy(typeof(GameData)).LoadAsset("Objects/Data.asset");
        AssetProxy GameObjectLoader = new AssetProxy(typeof(GameObject));
        particles = Object.Instantiate(GameObjectLoader.LoadAsset("Objects/Tiles/ExitTileParticles.prefab"));
        particleSystem = particles.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    public void PlayerInRange(Transform parentTransform)
    {
        if (!isPlaying)
        {
            particles.transform.position = parentTransform.position;
            particleSystem.Play();
            isPlaying = true;
        }
    }

    public void PlayerOnTile()
    {
        Debug.Log("EXIT");
        PlayerPrefs.SetInt("LevelCleared", PlayerPrefs.GetInt("LevelCleared") + 1);
        SceneManager.LoadScene("LoadingScene");
    }
}

public class ClosedState : ExitTileStateInterface
{
    GameObject bossEnemy;
    GameObject hpbar;

    public ClosedState()
    {
        AssetProxy GameObjectLoader = new AssetProxy(typeof(GameObject));
        hpbar = GameObjectLoader.LoadAsset("Objects/HpBar/healthBar.prefab");
        bossEnemy = GameObjectLoader.LoadAsset("Objects/Enemy/Skeleton.prefab");
        // redo to spawner
    }

    public void PlayerInRange(Transform parentTransform)
    {
        GameObject bossEnemy = Object.Instantiate(this.bossEnemy, parentTransform);
        Enemy enemyBossScript = bossEnemy.GetComponent<Enemy>();
        enemyBossScript.SetParameters(7, 5, new EnemyController(new RangedSkeletonController()), 50, 5, 10);
        GameObject hpBar = Object.Instantiate(hpbar);
        hpBar.GetComponent<HealthBar>().setTarget(bossEnemy);
        bossEnemy.GetComponent<Enemy>().healthBar = hpBar.GetComponent<HealthBar>();
        bossEnemy.transform.localScale = new Vector3(2, 2, 2);
        bossEnemy.tag = "Boss";
    }

    public void PlayerOnTile()
    {
        Debug.Log("AYYYYY");
    }
}
