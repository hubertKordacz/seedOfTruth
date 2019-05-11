using UnityEngine;
using System.Collections;
using System;

public class CollectiblePicker : MonoBehaviour
{
    private PlayerWeaponController weaponController;
    private PlayerHealth health;

    private void Awake()
    {
        weaponController = this.GetComponent<PlayerWeaponController>();
        health = this.GetComponent<PlayerHealth>();
    }
    public void PickUpCollectible(CollectibleItem item)
    {
        ApplyEffect(item.itemType);
        item.Hide();
    }

    private void ApplyEffect(CollectibleItem.CollectibleType itemType)
    {

        switch (itemType)
        {

            case CollectibleItem.CollectibleType.HEALTH:
                {
                    if (health)
                        health.Heal(10.0f);
                    break;
                }
            case CollectibleItem.CollectibleType.FLAMETHROWER:
                {
                    if (weaponController)
                        weaponController.SetCurrentWeapon(WeaponBase.WeaponType.FLAMETHROWER);
                    break;
                }
            case CollectibleItem.CollectibleType.MACHINEGUN:
                {
                    if (weaponController)
                        weaponController.SetCurrentWeapon(WeaponBase.WeaponType.MACHINEGUN);
                    break;
                }
            case CollectibleItem.CollectibleType.PISTOL:
                {
                    if (weaponController)
                        weaponController.SetCurrentWeapon(WeaponBase.WeaponType.PISTOL);
                    break;
                }
            case CollectibleItem.CollectibleType.ROCKETLAUNCHER:
                {
                    if (weaponController)
                        weaponController.SetCurrentWeapon(WeaponBase.WeaponType.ROCKETLAUNCHER);
                    break;
                }
            case CollectibleItem.CollectibleType.SHOTGUN:
                {
                    if (weaponController)
                        weaponController.SetCurrentWeapon(WeaponBase.WeaponType.SHOTGUN);
                    break;
                }
            case CollectibleItem.CollectibleType.SNIPERRIFFLE:
                {
                    if (weaponController)
                        weaponController.SetCurrentWeapon(WeaponBase.WeaponType.SNIPERRIFFLE);
                    break;
                }
            default:
                break;
        }

    }
}
