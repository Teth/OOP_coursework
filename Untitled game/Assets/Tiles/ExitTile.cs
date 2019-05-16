using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitTile : MonoBehaviour
{
    

    [SerializeField]
    Sprite openStateSprite;
    [SerializeField]
    Sprite closedStateSprite;
    GameObject bossEnemy;
    GameObject player;
    [SerializeField]
    float range;
    private ExitTileStateInterface state;
    private ClosedState closedState;
    private OpenedState openedState;
    private bool bossSpawned;

    void SetState(ExitTileStateInterface newState)
    {
        state = newState;
    }

    void Update()
    {
        bossEnemy = GameObject.FindWithTag("Boss");

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance < range)
        {
            if (!bossSpawned)
            {
                //spawn boss
                state.PlayerInRange();
                bossSpawned = true;
            }
            if (distance < 1)
            {
                state.PlayerOnTile();
            }
        }

        if(bossSpawned && bossEnemy == null)
        {
            //SetState(openedState);
        }

    }
    void Start()
    {
        bossSpawned = false;
        player = GameObject.FindWithTag("Player");
        closedState = new ClosedState();
        openedState = new OpenedState();
        state = closedState;
    }
}


public interface ExitTileStateInterface
{
    void PlayerOnTile();
    void PlayerInRange();
}

public class OpenedState : ExitTileStateInterface
{
    public void PlayerInRange()
    {
        // Spawn particles
    }

    public void PlayerOnTile()
    {
        // load next level
    }
}

public class ClosedState : ExitTileStateInterface
{
    public void PlayerInRange()
    {
        Debug.Log("BOSS SPAWNING");
    }

    public void PlayerOnTile()
    {
        Debug.Log("AYYYYY");
    }
}
