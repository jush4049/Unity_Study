using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitResult;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                var bullet = ObjectPool.GetObject();
                var direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                bullet.transform.position = direction.normalized;
                bullet.Shoot(direction.normalized);
            }
        }
    }
}
