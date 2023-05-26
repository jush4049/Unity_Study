using UnityEngine;
using UnityEngine.Pool;

public class Launcher : MonoBehaviour
{
    [SerializeField] Ball ballPrefab;

    private IObjectPool<Ball> objectPool;

    private void Awake()
    {
        // 생성하는 함수, Get, Release, Destroy, Max)
        // Pool의 크기를 3으로 유지 -> 최종적으로 3개만 풀에 저장
        objectPool = new ObjectPool<Ball>(
            CreateBall,
            OnGet,
            OnRelease,
            OnDestroy,
            maxSize: 3);
    }

    private Ball CreateBall()
    {
        Ball ball = Instantiate(ballPrefab); // 오브젝트 생성
        ball.SetPool(objectPool);
        return ball;
    }

    // Get -> element가 parameter가 됨
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
