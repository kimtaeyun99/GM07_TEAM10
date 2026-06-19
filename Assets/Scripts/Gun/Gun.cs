using System.Collections;
using UnityEngine.UIElements;
using UnityEditor.Search;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunData data;    // ScriptableObejct GunData 
    [SerializeField] protected Transform firePoint; // 사격위치
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] protected BulletData bulletData; // ScriptableObject BulletData
    [SerializeField] protected BulletPool bulletPool;

    protected int currentAmmo;   //현재 탄약
    protected float nextFireTime; // 다음 발사 가능 시간
    protected int currentMagazineCount;
    protected bool isReloading; // 재장전 유무
    private TextMeshProUGUI textMeshPro;

    public SpriteRenderer GunSprite => weaponRenderer;
    public int CurrentAmmo => currentAmmo;   // 현재 탄약 외부에서 꺼내쓸수있도록 빼놈
    public int MagazineSize => data.magazineSize; // 최대 잔탄수 외부에서 꺼내쓸수있도록 빼놈

    public bool IsSwap => Time.time < (nextFireTime - data.fireInterval) + 0.25f;      // 사격 후 무기변경 가능시간
    protected virtual void Awake()  // 초기화
    {
        bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        currentAmmo = data.magazineSize; // 현재 잔탄수를 GunData의 magzineSzie 만큼 설정
        currentMagazineCount = data.magazineCount;
        if (weaponRenderer != null)
        {
            weaponRenderer.sprite = data.weaponSprite;  // 스프라이트를 GunData의 weaponSprite로 설정
        }
        textMeshPro = GameObject.Find("GunUi").GetComponent<TextMeshProUGUI>();
        ShowUi();
    }

    private void OnEnable()
    {
        ShowUi();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isReloading = false;
    }

    public void ShowUi(string text = "")
    {
        if (text == "")
        {
            textMeshPro.text = $"{CurrentAmmo} / {data.magazineSize}  Magazin : {currentMagazineCount}";

        }
        else
        {
            textMeshPro.text = text;
        }
    }


    public virtual void TryFire(Vector2 direction)   // 사격이 가능한지 부터 확인하는 함수
    {

        if (isReloading)   // 재장전 중이면 리턴
            return;

        if (Time.time < nextFireTime) // 발사 가능시간 충족안될시 리턴
            return;

        if (currentAmmo <= 0)   // 현재 잔탄수가 0이면 자동 재장전후 리턴
        {
            TryReload();
            return;
        }

        nextFireTime = Time.time + data.fireInterval; // 사격 딜레이 설정
        currentAmmo--; // 잔탄수 -1 

        Fire(direction);
        ShowUi();
    }

    protected virtual void Fire(Vector2 direction) // 사격
    {
                 // 여기에 무기별 사격 사운드 코드 추가
        int count = Mathf.Max(1, data.bulletCount); // GunData의 발사총알수를 비교함, 예) 산탄총일시 Mathf.Max(1,8)

        for (int i = 0; i < count; i++)  // 카운트만큼 반복하면서 총알 생성
        {
            float spread = Random.Range(-data.spreadAngle * 0.5f, data.spreadAngle * 0.5f);  // 총알이 나가는 방향의 랜덤성을 부여함 ( 정확도 )

            Vector2 shotDirection = Quaternion.Euler(0f, 0f, spread) * direction;  // 위 수치적용하여 방향 계산

            SpawnBullet(shotDirection);  //해당 방향으로 총알 생성
        }
    }


    protected void SpawnBullet(Vector2 direction)
    {
        Bullet bullet = bulletPool.GetBullet(firePoint.position,Quaternion.identity); // 오브젝트 풀에서 총알 가져오기

        bullet.Initialize(
            bulletData, // ScriptableObject BulletData 
            direction,  //총알이 나아갈 방향
            gameObject  //Initialize 함수를 호출한 gameObject (총알이 누가 발사했는지 구분여부)
                        //1f   <-- 총알 대미지배율 1배만큼 설정  나중에 총알공격력이 증가되는 아이템을 획득시 여기에 배율 추가해주면 됨
        );
    }

    public void TryReload()  // 재장전 함수
    {
        if (currentMagazineCount <= 0)  // 남은 탄창이 없으면 리턴
        {
            return;
        }

        if (isReloading)  // 재장전 중이면 리턴
            return;

        if (currentAmmo >= data.magazineSize) // 현재 잔탄수가 최대 탄약수보다 크거나 같으면 리턴
            return;


        StartCoroutine(ReloadCoroutine());   // 코루틴으로 딜레이
    }

    private IEnumerator ReloadCoroutine()
    {
        ShowUi("Reloading...");
        isReloading = true; // 재장전 상태 변경
        yield return new WaitForSeconds(data.reloadTime); // GunData의 realoadTime 만큼 대기

        currentAmmo = data.magazineSize;
        currentMagazineCount--;
        isReloading = false;  // 재장전 상태 변경
        ShowUi();
    }
}