using System.Collections;
using UnityEngine;

public class AR : WeaponBase, IReloadable
{
    [SerializeField] private ARBullet arBullet;

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
            ItemData myData = EquipmentManager.instance.currentEquipment[5];
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

    public IEnumerator ShootCo()
    {
        while (true)
        {
            if (Managers.Input.isAttackPressed && currentAmmo > 0 && !isReload)
            {
                FireBullet();
                yield return ShootDelayWait;
            }
            else yield return null;
        }
    }

    public void FireBullet()
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

        ARBullet bullet = Managers.Pool.GetPool(arBullet);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        bullet.Fire(dir);
    }

    // 💡 [핵심] IReloadable 규격을 충족하는 진짜 Reload() 구현 부분!
    public void Reload()
    {
        if (currentAmmo >= maxAmmo || isReload) return;

        if (Inventory.instance == null) return;

        InventoryItem ammoItem = Inventory.instance.items.Find(x => x.itemData != null && x.itemData.itemName == "AR탄창");
        if (ammoItem == null || ammoItem.quantity <= 0)
        {
            Debug.LogWarning("인벤토리에 AR 탄창(AR_MG)이 없어 재장전할 수 없습니다!");
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
            int targetSlotIndex = Inventory.instance.items.FindIndex(x => x.itemData != null && x.itemData.itemName == "AR탄창");
            if (targetSlotIndex != -1)
            {
                Inventory.instance.RemoveAt(targetSlotIndex);
                currentAmmo = maxAmmo;

                // 장비 금고도 장전된 탄약으로 최신화
                if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > 5 && EquipmentManager.instance.currentEquipment[5] != null)
                {
                    EquipmentManager.instance.currentEquipment[5].currentAmmo = currentAmmo;
                }

                Debug.Log("AR 재장전 완료!");
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