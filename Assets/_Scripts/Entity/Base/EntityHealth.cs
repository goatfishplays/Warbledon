using UnityEngine;
using System.Collections;
using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class EntityHealth : NetworkBehaviour
{

    // Health Variables
    [SerializeField] private bool changingHealth = true;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    // [SerializeField] private float healthChangeRate = 0f;

    // State 
    public bool dead = false;
    public bool hasIFrames => co_iFrames != null;

    // Actions/Events
    public event Action OnDie;
    public event Action OnHealthChange;
    public event Action OnHit;

    // Misc
    public AudioClip hitSound = null;
    public AudioClip deathSound = null;

    // Coroutines
    public Coroutine co_iFrames = null;

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }


    private void Update()
    {
        // // Passive Health Change
        // if (changingHealth)
        // {
        //     ChangeHealth(healthChangeRate * Time.deltaTime, 0f, true);
        // }
    }

    /// <summary>
    /// For health changes, Runs on server only
    /// Triggers OnHealthChange Action
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="iFramesAddTime"></param>
    /// <param name="ignoresIframes"></param>
    public void ChangeHealth(bool isHit, float delta, float iFramesAddTime = 0.2f, bool ignoresIframes = false)
    {
        if (!IsServerInitialized || !changingHealth || dead)
        {
            return;
        }

        if (delta > 0 || !hasIFrames || ignoresIframes)
        {
            health += delta;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            if (health <= 0)
            {
                if (IsServerOnlyInitialized)
                {
                    TriggerDie();
                }
                ClientsReceiveDie();
            }

            if (iFramesAddTime > 0f)
            {
                SetIFrames(iFramesAddTime, overridesCurrent: false);
            }

            if (IsServerOnlyInitialized)
            {
                TriggerHealthChange(isHit);
            }
            ClientsReceiveHealth(health, isHit);
        }
    }

    [ObserversRpc]
    private void ClientsReceiveHealth(float newHealth, bool isHit)
    {
        health = newHealth;
        TriggerHealthChange(isHit);
        if (isHit && hitSound != null)
        {
            AudioManager.instance.PlaySFXAtTracker(hitSound, transform);
        }
    }

    private void TriggerHealthChange(bool isHit)
    {
        OnHealthChange?.Invoke();
        if (isHit)
        {
            OnHit?.Invoke();
        }
    }

    public Coroutine SetIFrames(float iFramesSetTime, bool overridesCurrent = false)
    {
        if (hasIFrames)
        {
            if (!overridesCurrent)
            {
                return co_iFrames;
            }
            StopCoroutine(co_iFrames);
        }
        co_iFrames = StartCoroutine(IFrameSet(iFramesSetTime));
        return co_iFrames;
    }

    private IEnumerator IFrameSet(float iFrameAddTime)
    {
        yield return new WaitForSeconds(iFrameAddTime);
        co_iFrames = null;
    }


    [ObserversRpc]
    private void ClientsReceiveDie()
    {
        TriggerDie();
        if (deathSound != null)
        {
            AudioManager.instance.PlaySFXAtTracker(deathSound, transform);
        }
    }

    private void TriggerDie()
    {
        dead = true;
        OnDie?.Invoke();
    }
}
