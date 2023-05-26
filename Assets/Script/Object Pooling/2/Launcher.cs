using UnityEngine;
using UnityEngine.Pool;

public class Launcher : MonoBehaviour
{
    [SerializeField] Ball ballPrefab;

    private IObjectPool<Ball> objectPool;

    private void Awake()
    {
        // �����ϴ� �Լ�, Get, Release, Destroy, Max)
        // Pool�� ũ�⸦ 3���� ���� -> ���������� 3���� Ǯ�� ����
        objectPool = new ObjectPool<Ball>(
            CreateBall,
            OnGet,
            OnRelease,
            OnDestroy,
            maxSize: 3);
    }

    private Ball CreateBall()
    {
        Ball ball = Instantiate(ballPrefab); // ������Ʈ ����
        ball.SetPool(objectPool);
        return ball;
    }

    // Get -> element�� parameter�� ��
    private void OnGet(Ball ball)
    {
        ball.gameObject.SetActive(true);
    }

    private void OnRelease(Ball ball)
    {
        ball.gameObject.SetActive(false);
    }

    private void OnDestroy(Ball ball)
    {
        ball.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            objectPool.Get();
        }
    }
}
