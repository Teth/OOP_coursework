using System.Collections;
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
    SpriteRenderer rend;

    void SetState(ExitTileStateInterface newState)
    {
        state = newState;
        if(newState == closedState)
        {
            rend.sprite = closedStateSprite;
        }
        else
        {
            rend.sprite = openStateSprite;
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
        else
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
        rend = GetComponent<SpriteRenderer>();
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
    bool isPlaying;
    public OpenedState()
    {
        AssetProxy GameObjectLoader = new AssetProxy(typeof(GameObject));
        particles = Object.Instantiate(GameObjectLoader.LoadAsset("Assets/Tiles/ExitTileParticles.prefab"));
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
        SceneManager.LoadScene("SampleScene");
    }
}

public class ClosedState : ExitTileStateInterface
{
    GameObject bossEnemy;

    public ClosedState()
    {
        AssetProxy GameObjectLoader = new AssetProxy(typeof(GameObject));
        bossEnemy = GameObjectLoader.LoadAsset("Assets/fish.prefab");
        // redo to spawner
    }

    public void PlayerInRange(Transform parentTransform)
    {
        GameObject bossEnemy = Object.Instantiate(this.bossEnemy, parentTransform);
        Enemy enemyBossScript = bossEnemy.GetComponent<Enemy>();
        enemyBossScript.SetParameters(7, 5, new EnemyController(new MeleeRatController()), 50, 5, 10);
        bossEnemy.tag = "Boss";
    }

    public void PlayerOnTile()
    {
        Debug.Log("AYYYYY");
    }
}
