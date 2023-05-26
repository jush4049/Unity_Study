using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab; // 오브젝트 풀링으로 관리할 오브젝트

    Queue<Bullet> poolingObjectQueue = new Queue<Bullet>(); // Queue를 생성하여 오브젝트 관리

    private void Awake() // 싱글톤
    {
        Instance = this;

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); // 미리 생성된 오브젝트를 큐에 추가
        }
    }

    private Bullet CreateNewObject() // poolingObjectPrefab으로부터 새 오브젝트르 만들고 비활성화 후 반환
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Bullet GetObject() // 오브젝트 풀이 갖고 있는 오브젝트를 꺼내줌
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnObject(Bullet obj) // 오브젝트를 돌려받음
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
