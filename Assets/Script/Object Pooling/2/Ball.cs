using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Ball : MonoBehaviour
{
    [SerializeField] Vector3 speed;

    // 오브젝트 자체에서 어떤 Pool에 들어가야 하는지 알고 있어야함
    private IObjectPool<Ball> objectPool;

    // Pool의 정보는 Bullet이 반환될 때 같이 불러준다.
    public void SetPool(IObjectPool<Ball> pool)
    {
        objectPool = pool;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime;
    }

    // 화면에서 사라지면 Pool로 다시 집어 넣는다.
    private void OnBecameInvisible()
    {
        objectPool.Release(this);
    }
}
