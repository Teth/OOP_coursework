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
        SceneManager.LoadSceneAsync("SampleScene");
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
        GameObject be = Object.Instantiate(bossEnemy, parentTransform);
        be.tag = "Boss";
    }

    public void PlayerOnTile()
    {
        Debug.Log("AYYYYY");
    }
}
