using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtEnemy : MonoBehaviour
{
    [SerializeField] Transform target = null;
    public bool trackTargetX = false;
    public bool trackTargetY = false;


    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            int ownerID = GetComponent<TimedSpawner>().ownerID;

            Collider2D[] targCheck = Physics2D.OverlapCircleAll(transform.position, 50, LayerMask.GetMask("Entities"));
            float minDist = 50;

            // for (int i = 0; i < targCheck.GetContacts(cols); i++)
            // print(targCheck.Length);
            foreach (Collider2D col in targCheck)
            {
                Transform cur = col.transform;
                // Debug.Log(cur.gameObject.name);
                // Debug.Log(cur.GetComponent<Entity>().id);
                if (cur.GetComponent<Entity>().gid != ownerID)
                {
                    if (target == null || (cur.position - transform.position).magnitude < minDist)
                    {
                        minDist = (cur.position - transform.position).magnitude;
                        target = cur;
                    }
                }
            }
            if (target != null)
            {
                transform.position = target.position;
            }

            if ((!trackTargetX && !trackTargetY) || target == null)
            {
                Destroy(this);
            }
        }
        else
        {
            Vector2 pos = transform.position;
            if (trackTargetX)
            {
                pos.x = target.position.x;
            }
            if (trackTargetY)
            {
                pos.y = target.position.y;
            }
            transform.position = pos;
        }
    }
}
