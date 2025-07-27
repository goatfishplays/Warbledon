using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomBulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public Transform target;
    public Vector2 spawnYRange;
    public Vector2 spawnXRange;
    public float minSpawnDist;
    public Vector2 spawnCooldownRange;
    public Vector2 veloRange;
    public Vector2 aimMultRange;
    public Vector2 rotationRange;
    public Vector2 scaleRange;
    float curSpawnCooldown = 0f;
    public List<Transform> bullets = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        if (curSpawnCooldown > 0)
        {
            curSpawnCooldown -= Time.deltaTime;
        }
        else
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(spawnXRange.x, spawnXRange.y), Random.Range(spawnYRange.x, spawnYRange.y));
            while ((spawnPos - target.position).magnitude < minSpawnDist)
            {
                spawnPos = transform.position + new Vector3(Random.Range(spawnXRange.x, spawnXRange.y), Random.Range(spawnYRange.x, spawnYRange.y));
            }
            // print((spawnPos - target.position).magnitude);
            // print(target.position); 
            // print(spawnPos); 
            GameObject spawnedBullet = Instantiate(bullet, spawnPos, Quaternion.Euler(0, 0, Random.Range(rotationRange.x, rotationRange.y)), transform);
            bullets.Add(spawnedBullet.transform);
            spawnedBullet.transform.localScale = new Vector3(Random.Range(scaleRange.x, scaleRange.y), Random.Range(scaleRange.x, scaleRange.y), 1);
            Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = ((target.position - spawnedBullet.transform.position).normalized * new Vector2(Random.Range(aimMultRange.x, aimMultRange.y), Random.Range(aimMultRange.x, aimMultRange.y))).normalized * Random.Range(veloRange.x, veloRange.y);

            curSpawnCooldown = Random.Range(spawnCooldownRange.x, spawnCooldownRange.y);
        }

    }
    public void DestroyallBullet()
    {
        foreach (var bullet in bullets)
        {
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
            }
        }
        bullets.Clear();
    }


}
