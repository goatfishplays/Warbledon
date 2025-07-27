using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField] private float dashCost = 5;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCooldownTime = 1f;
    [SerializeField] private float iFrameTime = .2f;
    [SerializeField] private float dashForce = 15f;
    public const string DASH_MOVEMENT_MULT_ID = "dash";
    [SerializeField] private float dashMovementMult = 0.25f;
    // private Coroutine co_dashing = null;  
    private Coroutine co_dashcooldown = null;
    // public bool isDashing => co_dashing != null;
    public bool isOnCooldown => co_dashcooldown != null;
    public Entity entity;

    public void AttemptDash(Vector2 dir)
    {
        if (isOnCooldown || dir.magnitude < 0.1f)
        {
            return;
        }
        co_dashcooldown = StartCoroutine(Dash(dir));
    }

    private IEnumerator Dash(Vector2 dir)
    {
        // if (entity.rb.linearVelocity.magnitude < 0.1f)  
        // {
        //     co_dashcooldown = null;   
        //     yield break;
        // }
        entity.entityHealth.ChangeHealth(false, -dashCost, 0f, true);
        entity.rb.AddRelativeForce(new Vector3(dir.x, 0.0f, dir.y).normalized * dashForce, ForceMode2D.Impulse);
        entity.entityHealth.SetIFrames(iFrameTime, true);
        // entity.entityMovement.SetLockMovement(dashTime);
        entity.entityMovement.AddTimedCompiledSpeedMult(dashTime, DASH_MOVEMENT_MULT_ID, dashMovementMult);
        yield return new WaitForSeconds(dashCooldownTime);
        co_dashcooldown = null;
    }


}
