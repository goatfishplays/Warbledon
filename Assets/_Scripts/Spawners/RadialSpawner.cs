using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FishNet.Object;
using UnityEngine;

public class RadialSpawner : TimedSpawner
{

    [Header("Radial Stuff")]
    public int numBullets = 1;
    public float currentRotation = 0;
    // public bool rotateThis = false;
    public float spreadAngle = 0f;
    public float rotPerShot = 0;
    public int maxShotsPerRotFlip = -1;
    public int shotsTillFlip = -1;
    public int maxShotsPerRotReset = -1;
    public int shotsTillRotReset = -1;

    void Rotation()
    {
        currentRotation += rotPerShot;

        shotsTillFlip--;
        if (shotsTillFlip == 0)
        {
            rotPerShot *= -1;
            shotsTillFlip = maxShotsPerRotFlip;
        }
        shotsTillRotReset--;
        if (shotsTillRotReset == 0)
        {
            currentRotation -= rotPerShot * maxShotsPerRotReset;
            shotsTillRotReset = maxShotsPerRotReset;
        }
    }

    float temp_rotForSpawn;
    public override void SpawnBullet() // TODO wth is any of this, fix this, why did you do this like this lmao
    {
        if (!IsServerInitialized)
        {
            return;
        }

        PreSpawnLogic();
        // ClientsReceivePreSpawn();

        for (int i = 0; i < numBullets; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (temp_rotForSpawn + rotForShot)), Mathf.Sin(Mathf.Deg2Rad * (temp_rotForSpawn + rotForShot)));
            Vector2 spawnPos = (Vector2)transform.position + (offset.magnitude * dir);
            NetworkObject spawned = Instantiate(bulletPrefab, spawnPos, Quaternion.identity); // TODO idk if this is supposed to be transform.rotation or quat iden

            if (parrentToSpawner)
            {
                spawned.transform.parent = transform;
            }
            if (bulletShotForce != 0)
            {
                Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
                // apply directional velocity
                rb.AddForce(dir * bulletShotForce, ForceMode2D.Impulse); // TODO if we stop using network transform fix this

            }
            // rotate bullets to face forward
            if (rotateBullets)
            {
                // rb.SetRotation(rotI);
                spawned.transform.Rotate(0, 0, temp_rotForSpawn + bulletRotOffset + rotForShot);
                // curBullet.transform.right = 
            }
            if (numBullets != 1) // TODO does this actually work, something feels off
            {
                temp_rotForSpawn += spreadAngle / (numBullets - 1);
            }

            Spawn(spawned);
            if (IsServerOnlyInitialized)
            {
                SpawnLogic(spawned, spawnPos);
            }
            ClientsReceiveSpawn(spawned, spawnPos);
        }

        if (IsServerOnlyInitialized)
        {
            ResetSpawner();
            Rotation();
        }
        ClientsReceivePostSpawn();
    }
    // [ObserversRpc(ExcludeServer = true)]
    // protected void ClientsReceivePreSpawn()
    // {
    //     PreSpawnLogic();
    // }
    protected void PreSpawnLogic()
    {
        temp_rotForSpawn = currentRotation;
        if (numBullets != 1)
        {
            temp_rotForSpawn -= spreadAngle / 2;
        }
    }

    [ObserversRpc]
    protected override void ClientsReceiveSpawn(NetworkObject spawned, Vector2 spawnPos) // Don't need to sync rot and pos and stuff cause will be using network transform I think
    {
        SpawnLogic(spawned, spawnPos);
        // TODO add spawn sound effect or action here that can cause animation to trigger 
    }
    protected override void SpawnLogic(NetworkObject spawned, Vector2 spawnPos) // TODO currently everything is synced with network transform, fix later if perf issue
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
        spawned.GetComponent<Attack>().ownerID = ownerID;

        // curBullet.transform.rotation = Quaternion.Euler(0, 0, bulletRotOffset + rotForShot);
    }

    [ObserversRpc]
    protected void ClientsReceivePostSpawn()
    {
        ResetSpawner();
        Rotation();
    }

}