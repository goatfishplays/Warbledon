using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DirectionalMoveSpawner : TimedSpawner
{
    [Header("Move Stuff")]
    public float spawnerSpeed = .1f;
    public Vector2 mover = Vector2.zero;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (active)
        {
            transform.position += (Vector3)(mover * spawnerSpeed);

        }
    }

    public override void Aim(Vector2 targ)
    {
        if (!IsServerInitialized || !aimable)
        {
            return;
        }


        foreach (TimedSpawner ts in childrenSpawners)
        {
            mover = (targ - (Vector2)ts.transform.position).normalized;
            if (IsServerOnlyInitialized)
            {
                ts.AimLogic(mover);
            }
            ts.ClientsReceiveAim(mover);
        }
    }


    [ObserversRpc]
    public override void ClientsReceiveAim(Vector2 dir)
    {
        AimLogic(dir);
    }

    public override void AimLogic(Vector2 dir)
    {
        mover = dir;

        rotForShot = Mathf.Atan2(mover.y, mover.x) * Mathf.Rad2Deg;

        if (followsCursor)
        {
            transform.right = mover;
        }
    }

}
