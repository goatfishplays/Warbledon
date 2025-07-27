using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class Attack : NetworkBehaviour
{
    public Rigidbody2D rb;
    public AttackSO attackBase;
    public int ownerID = -1;

    // public LayerMask targetLayer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!IsServerInitialized)
        {
            return;
        }
        // Debug.Log("wake up");
        StartCoroutine(AutoBreakTimer());
    }
    protected virtual void Update() { }

    private IEnumerator AutoBreakTimer()
    {
        yield return new WaitForSeconds(attackBase.liveTime);
        // Debug.Log("autobreak");
        Break();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServerInitialized)
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("World") && attackBase.breaksOnWall)
        {
            Break();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Entities"))
        {
            Entity otherEntity = other.gameObject.GetComponent<Entity>();
            // if (owner != null && other.gameObject.layer == owner.gameObject.layer && ownerImmune)// don't hit owner/other teammates
            if (ownerID == otherEntity.gid && attackBase.ownerImmune)// don't hit owner/other teammates
            {
                return;
            }

            // otherEntity.ApplyKnockback(rb.velocity.normalized * kb);
            otherEntity.entityMovement.ApplyKnockback((other.transform.position - transform.position).normalized * attackBase.kb);
            // otherEntity.ApplyKnockback(other. * kb);
            otherEntity.entityHealth.ChangeHealth(true, -attackBase.damage);
            if (attackBase.breaksOnHit)
            {
                Break();
            }
        }
    }

    public virtual void Break()
    {
        Despawn();
    }
}
