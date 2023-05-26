using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFind : MonoBehaviour
{
    public List<GameObject> FoundObjects; // ã�� ������Ʈ ����Ʈ
    public GameObject enemy;
    public string TagName;
    public float shortDis; // ���� ª�� �Ÿ�

    void Start()
    {
        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(TagName)); // �ش� �±��� ������Ʈ���� ����
        shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position); // ù ��°�� �������� ���

        enemy = FoundObjects[0]; // ù ��° ����

        /*foreach (GameObject found in FoundObjects) // ��� ������Ʈ �Ÿ� ����
        {
            // Vector3.Distance(Vector3 a, Vector3 b), a�� b������ �Ÿ��� �����ϰ� ��ȯ�ϴ� �Լ�
            float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

            if (Distance < shortDis) // ������ ���� �������� �Ÿ� ����
            {
                enemy = found;
                shortDis = Distance;
            }
        }
        Debug.Log(enemy.name);*/
    }

    void Update()
    {
        shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position); // ù ��°�� �������� ���

        enemy = FoundObjects[0]; // ù ��° ����

        foreach (GameObject found in FoundObjects) // ��� ������Ʈ �Ÿ� ����
        {
            // Vector3.Distance(Vector3 a, Vector3 b), a�� b������ �Ÿ��� �����ϰ� ��ȯ�ϴ� �Լ�
            float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

            if (Distance < shortDis) // ������ ���� �������� �Ÿ� ����
            {
                enemy = found;
                shortDis = Distance;
            }
        }
        Debug.Log(enemy.name);
    }
}
