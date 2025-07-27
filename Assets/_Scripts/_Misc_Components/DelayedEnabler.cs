using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DelayedEnabler : NetworkBehaviour
{
    public float time = 1f;
    public GameObject thing;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsServerInitialized)
        {
            return;
        }
        StartCoroutine(DelayEnable());
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(time);

        // thing.SetActive(true);
        Spawn(thing);
        Destroy(this);
    }


}
