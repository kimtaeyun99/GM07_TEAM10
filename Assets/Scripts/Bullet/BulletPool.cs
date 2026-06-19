using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab; // 생성할 총알 프리팹
    [SerializeField] private int initialPoolSize = 30; // 처음 생성해둘 총알 개수

    private Queue<Bullet> bulletPool = new Queue<Bullet>(); // 사용하지 않는 총알을 보관하는 큐

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateBullet();
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform); // 총알 프리팹 생성

        bullet.SetPool(this); // 총알이 자신을 관리하는 풀을 알 수 있도록 설정
        bullet.gameObject.SetActive(false); // 생성 후 비활성화

        bulletPool.Enqueue(bullet); // 사용하지 않는 총알 큐에 추가

        return bullet;
    }

    public Bullet GetBullet(Vector3 position, Quaternion rotation)
    {
        if (bulletPool.Count <= 0) // 사용할 수 있는 총알이 없으면 추가 생성
        {
            CreateBullet();
        }

        Bullet bullet = bulletPool.Dequeue(); // 큐에서 총알 하나 꺼내기

        bullet.transform.SetPositionAndRotation(position, rotation); // 총알 위치와 회전 설정
        bullet.gameObject.SetActive(true); // 총알 활성화

        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false); // 총알 비활성화
        bullet.transform.SetParent(transform); // 총알을 풀 오브젝트의 자식으로 설정

        bulletPool.Enqueue(bullet); // 다시 사용할 수 있도록 큐에 추가
    }
}