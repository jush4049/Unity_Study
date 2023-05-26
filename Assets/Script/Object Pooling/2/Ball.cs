using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Ball : MonoBehaviour
{
    [SerializeField] Vector3 speed;

    // ������Ʈ ��ü���� � Pool�� ���� �ϴ��� �˰� �־����
    private IObjectPool<Ball> objectPool;

    // Pool�� ������ Bullet�� ��ȯ�� �� ���� �ҷ��ش�.
    public void SetPool(IObjectPool<Ball> pool)
    {
        objectPool = pool;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime;
    }

    // ȭ�鿡�� ������� Pool�� �ٽ� ���� �ִ´�.
    private void OnBecameInvisible()
    {
        objectPool.Release(this);
    }
}
