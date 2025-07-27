using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TimedSpawner : NetworkBehaviour
{
    // TODO if we don't end up implementing anything(animations and suck) that uses client side reading the timings then remove it and just have client method that spawns
    [Header("Owner")]
    public int ownerID = -1;

    [Header("Spawn Info")]

    public NetworkObject bulletPrefab;
    public bool rotateBullets = false;
    public float bulletRotOffset = 0f;
    public float bulletShotForce = 5f;
    public float rotForShot = 0f;
    public bool parrentToSpawner = false;
    public int maxTicksBetweenSpawns = 20;
    public int ticksTillNextSpawn = 0;
    public int numShotsTillDisable = -1;
    public bool destroyAfterDisable = true;
    // public float offset = 0;
    public Vector2 offset = Vector2.right;
    [Header("Spawner Aiming")]
    public bool active = false;
    public bool aimable = true;
    public bool followsCursor = true;
    protected TimedSpawner[] childrenSpawners;

    protected virtual void Awake()
    {
        childrenSpawners = GetComponentsInChildren<TimedSpawner>();
        foreach (TimedSpawner ts in childrenSpawners)
        {
            ts.ownerID = ownerID;
        }
    }

    public virtual void Aim(Vector2 targ)
    {
        if (!IsServerInitialized || !aimable)
        {
            return;
        }


        foreach (TimedSpawner ts in childrenSpawners)
        {
            Vector2 dir = targ - (Vector2)ts.transform.position;
            if (IsServerOnlyInitialized)
            {
                ts.AimLogic(dir);
            }
            ts.ClientsReceiveAim(dir);
        }
    }

    [ObserversRpc]
    public virtual void ClientsReceiveAim(Vector2 dir)
    {
        AimLogic(dir);
    }

    public virtual void AimLogic(Vector2 dir)
    {
        // Debug.Log("Client Receives New Aim");
        rotForShot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (followsCursor)
        {
            transform.right = dir;
        }
    }

    public virtual void Click(int click)
    {
        foreach (TimedSpawner ts in childrenSpawners)
        {
            if (IsServerOnlyInitialized)
            {
                ts.ClickLogic(click);
            }
            ts.ClientsReceiveClick(click);
        }
    }
    [ObserversRpc]
    protected void ClientsReceiveClick(int click)
    {
        ClickLogic(click);
    }
    protected void ClickLogic(int click)
    {
        if (click > 1)
        {
            active = true;
        }
    }

    public virtual void FixedUpdate()
    {
        if (active)
        {
            if (ticksTillNextSpawn-- == 0)
            {
                SpawnBullet();
            }
        }
    }

    public virtual void SpawnBullet()
    {
        if (!IsServerInitialized)
        {
            return;
        }
        // GameObject curBullet = Instantiate(bulletPrefab, transform.position + offset * transform.right, rotateBullets ? Quaternion.Euler(0, 0, rotForShot) : Quaternion.identity);
        Vector2 spawnPos = transform.position + transform.TransformDirection(offset);
        NetworkObject curBullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0, 0, bulletRotOffset));

        // TODO Currently uses a network transform, fix later if bad
        if (rotateBullets)
        {
            curBullet.transform.rotation = Quaternion.Euler(0, 0, bulletRotOffset + rotForShot);
        }
        if (parrentToSpawner)
        {
            curBullet.transform.parent = transform;
        }
        if (bulletShotForce != 0)
        {
            Rigidbody2D rb = curBullet.GetComponent<Rigidbody2D>();
            // apply directional velocity 
            rb.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * rotForShot), Mathf.Sin(Mathf.Deg2Rad * rotForShot)).normalized * bulletShotForce, ForceMode2D.Impulse);
        }


        Spawn(curBullet);
        // Debug.Log($"Spawn occured, {curBullet.name}");
        if (IsServerOnlyInitialized)
        {
            SpawnLogic(curBullet, spawnPos);
        }
        ClientsReceiveSpawn(curBullet, spawnPos);
    }

    [ObserversRpc]
    protected virtual void ClientsReceiveSpawn(NetworkObject spawned, Vector2 spawnPos) // Don't need to sync rot and pos and stuff cause will be using network transform I think
    {
        SpawnLogic(spawned, spawnPos);
        // TODO add spawn sound effect or action here that can cause animation to trigger 
    }
    protected virtual void SpawnLogic(NetworkObject spawned, Vector2 spawnPos)
    {
        spawned.transform.position = spawnPos;
        if (parrentToSpawner)
        {
            spawned.transform.parent = transform;
        }
        else
        {
            spawned.transform.SetParent(AttackManager.instance.transform);
        }

        spawned.GetComponentInChildren<Attack>(true).ownerID = ownerID;


        ResetSpawner();
    }

    public virtual void ResetSpawner()
    {

        #region Destruction
        numShotsTillDisable--;
        if (numShotsTillDisable == 0 && IsServerInitialized)
        {
            if (destroyAfterDisable)
            {
                Despawn(GetComponent<NetworkObject>());
            }
            active = false;
        }
        #endregion

        ticksTillNextSpawn = maxTicksBetweenSpawns;
    }

}
