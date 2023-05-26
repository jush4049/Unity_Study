using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        Invoke("DestroyBullet", 5f);
    }

    public void DestroyBullet()
    {
        ObjectPool.ReturnObject(this);
    }
    void Update()
    {
        transform.Translate(direction);
    }
}
