using System.Collections;
using UnityEngine;

public class Pistol : WeaponBase
{
    [SerializeField] private PistolBullet pistolBullet; // 사용하시는 권총 총알 프리팹

    private Coroutine coroutine;

    private void Start()
    {
        SyncAmmoFromManager();
    }

    private void OnEnable()
    {
        SyncAmmoFromManager();
    }

    /// <summary>
    /// 💡 장비 데이터와 HUD를 권총(무제한) 규칙에 맞게 세팅하는 함수
    /// </summary>
    private void SyncAmmoFromManager()
    {
        if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > 5)
        {
            ItemData myData = EquipmentManager.instance.currentEquipment[5]; // 5번 방 = 손에 든 무기
            if (myData != null)
            {
                // 권총은 내부 수치도 항상 maxAmmo로 가득 찬 상태를 유지시킵니다.
                currentAmmo = myData.maxAmmo;
                maxAmmo = myData.maxAmmo;
                myData.currentAmmo = myData.maxAmmo;
            }
        }

        // HUD UI 강제 새로고침 명령
        if (HUDController.instance != null)
        {
            HUDController.instance.SetWeaponAmmo(currentAmmo, maxAmmo);
            HUDController.instance.UpdateAmmoUI(); // 👈 여기서 "권총" 이름을 판별해 ∞ 기호로 그려줍니다.
        }
    }

    public override void Shoot()
    {
        // 💡 권총은 탄약이 줄어들지 않으므로 currentAmmo <= 0 체크나 Reload() 호출이 아예 필요 없습니다!
        if (coroutine == null)
        {
            coroutine = StartCoroutine(ShootCo());
        }
    }

    public override void StopShoot()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator ShootCo()
    {
        while (true)
        {
            if (Managers.Input.isAttackPressed)
            {
                FireBullet();

                // 💡 권총은 한 번 쐈으니 즉시 내 코루틴을 스스로 정지시켜 단발로 만듭니다.
                StopShoot();
                yield break;
            }
            else yield return null;
        }
    }

    private void FireBullet()
    {
        // ❌ currentAmmo--;  <-- 권총은 탄약 감소 코드를 절대 넣지 않습니다!

        // 💡 1. 두뇌 데이터(EquipmentManager) 내부 수치가 꼬이지 않게 계속 만땅으로 유지
        if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > 5)
        {
            if (EquipmentManager.instance.currentEquipment[5] != null)
            {
                EquipmentManager.instance.currentEquipment[5].currentAmmo = maxAmmo;
            }
        }

        // 💡 2. HUDController에 알리고 즉시 UI를 새로고침 (∞ 기호 표시용)
        if (HUDController.instance != null)
        {
            HUDController.instance.SetWeaponAmmo(currentAmmo, maxAmmo);
            HUDController.instance.UpdateAmmoUI();
        }

        // 💡 3. 실제 총알 생성 및 발사 처리 (기존에 작성해두신 총알 코드 구현)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - firePoint.position).normalized;

        PistolBullet bullet = Managers.Pool.GetPool(pistolBullet);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        bullet.Fire(dir);
    }
}