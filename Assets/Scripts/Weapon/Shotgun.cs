using System.Collections;
using UnityEngine;

public class Shotgun : WeaponBase, IReloadable
{
    [SerializeField] private int shotgunBulletCount = 5;
    [SerializeField] private float shotgunBulletAngle = 15f;
    [SerializeField] private ShotgunBullet shotgunBullet;

    private Coroutine coroutine;
    private bool isReload = false;

    private void Start()
    {
        SyncAmmoFromManager();
    }

    private void OnEnable()
    {
        SyncAmmoFromManager();
    }

    private void SyncAmmoFromManager()
    {
        if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > 5)
        {
            ItemData myData = EquipmentManager.instance.currentEquipment[5]; // 5번 방 = 손에 든 무기
            if (myData != null)
            {
                currentAmmo = myData.currentAmmo;
                maxAmmo = myData.maxAmmo;

                if (currentAmmo <= 0 && !myData.itemName.Contains("Pistol"))
                {
                    currentAmmo = maxAmmo;
                    myData.currentAmmo = maxAmmo;
                }
            }
        }

        if (HUDController.instance != null)
        {
            HUDController.instance.SetWeaponAmmo(currentAmmo, maxAmmo);
            HUDController.instance.UpdateAmmoUI();
        }
    }

    public override void Shoot()
    {
        if (isReload) return;

        if (currentAmmo <= 0)
        {
            SyncAmmoFromManager();
        }

        if (currentAmmo <= 0)
        {
            Reload(); // 💡 인터페이스 규칙인 Reload() 호출
            return;
        }

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
            if (Managers.Input.isAttackPressed && currentAmmo > 0 && !isReload)
            {
                FireBullet();

                // 💡 샷건도 한 번 쐈으니 즉시 내 코루틴을 스스로 정지시켜 단발로 만듭니다.
                StopShoot();
                yield break;
            }
            else yield return null;
        }
    }

    private void FireBullet()
    {
        currentAmmo--;

        // 💡 금고 데이터 실시간 동기화
        if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > 5)
        {
            if (EquipmentManager.instance.currentEquipment[5] != null)
            {
                EquipmentManager.instance.currentEquipment[5].currentAmmo = currentAmmo;
            }
        }

        // 💡 HUD UI 실시간 동기화
        if (HUDController.instance != null)
        {
            HUDController.instance.SetWeaponAmmo(currentAmmo, maxAmmo);
            HUDController.instance.UpdateAmmoUI();
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < shotgunBulletCount; i++)
        {
            float angleOffset = shotgunBulletAngle * ((float)i / (shotgunBulletCount - 1) - 0.5f);
            float finalAngle = baseAngle + angleOffset;
            Vector2 bulletDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad));

            ShotgunBullet bullet = Managers.Pool.GetPool(shotgunBullet);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, finalAngle);
            bullet.Fire(bulletDir);
        }
    }

    // 💡 [핵심] IReloadable 규격을 충족하는 진짜 Reload() 구현 부분!
    public void Reload()
    {
        if (currentAmmo >= maxAmmo || isReload) return;

        if (Inventory.instance == null) return;

        InventoryItem ammoItem = Inventory.instance.items.Find(x => x.itemData != null && x.itemData.itemName == "샷건 탄창");
        if (ammoItem == null || ammoItem.quantity <= 0)
        {
            Debug.LogWarning("인벤토리에 샷건 탄창(ShotgunMG)이 없어 재장전할 수 없습니다!");
            return;
        }

        StartCoroutine(ReloadCo());
    }

    public IEnumerator ReloadCo()
    {
        isReload = true;
        yield return ReloadDelayWait;

        if (Inventory.instance != null)
        {
            int targetSlotIndex = Inventory.instance.items.FindIndex(x => x.itemData != null && x.itemData.itemName == "샷건 탄창");
            if (targetSlotIndex != -1)
            {
                Inventory.instance.RemoveAt(targetSlotIndex);
                currentAmmo = maxAmmo;

                // 장비 금고도 장전된 탄약으로 최신화
                if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > 5 && EquipmentManager.instance.currentEquipment[5] != null)
                {
                    EquipmentManager.instance.currentEquipment[5].currentAmmo = currentAmmo;
                }

                Debug.Log("샷건 재장전 완료!");
            }
        }

        if (HUDController.instance != null)
        {
            HUDController.instance.SetWeaponAmmo(currentAmmo, maxAmmo);
            HUDController.instance.UpdateAmmoUI();
        }

        isReload = false;
    }
}