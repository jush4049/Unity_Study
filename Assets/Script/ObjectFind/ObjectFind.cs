using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFind : MonoBehaviour
{
    public List<GameObject> FoundObjects; // 찾을 오브젝트 리스트
    public GameObject enemy;
    public string TagName;
    public float shortDis; // 가장 짧은 거리

    void Start()
    {
        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(TagName)); // 해당 태그인 오브젝트들을 나열
        shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position); // 첫 번째를 기준으로 잡기
    }

    void Update()
    {
        shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position); // 첫 번째를 기준으로 잡기

        enemy = FoundObjects[0]; // 첫 번째 먼저

        foreach (GameObject found in FoundObjects) // 모든 오브젝트 거리 측정
        {
            // Vector3.Distance(Vector3 a, Vector3 b), a와 b사이의 거리를 측정하고 반환하는 함수
            float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

            if (Distance < shortDis) // 위에서 잡은 기준으로 거리 측정
            {
                enemy = found;
                shortDis = Distance;
            }
        }
        Debug.Log(enemy.name);
    }
}
