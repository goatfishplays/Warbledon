using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class AttackManager : NetworkBehaviour
{
    public static AttackManager instance;
    // public float tempVar = 10;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two AttackManagers detected, deleting second");
            Destroy(gameObject);
        }
    }

    public void DestroyCurrent()
    {
        if (!IsServerInitialized)
        {
            return;
        }
        foreach (Transform t in instance.transform)
        {
            // Destroy(t.gameObject);
            Despawn(t.gameObject);
        }
    }

    // public void Update()
    // {
    //     tempVar -= Time.deltaTime;
    //     if (tempVar < 0)
    //     {
    //         DestroyCurrent();  
    //     }
    // }
}
