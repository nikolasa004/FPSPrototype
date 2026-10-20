using System.Collections;
using UnityEngine;
using EZCameraShake;

public class Pucanje : MonoBehaviour
{
    private KontrolaOružja kontrola_oružja;
    private CameraShake camera_shake;
    private AmmoDisplay[] ammoDisplays;

    public float fire_rate = 15f;
    private float next_time_to_fire;
    private OsobineOružja lastWeapon;

    private void Awake()
    {
        kontrola_oružja = GetComponent<KontrolaOružja>();
        camera_shake = GetComponentInChildren<CameraShake>(true);
        FindAmmoDisplays();
    }

    private void FindAmmoDisplays()
    {
        ammoDisplays = FindObjectsByType<AmmoDisplay>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (kontrola_oružja == null)
            return;

        OsobineOružja weapon = kontrola_oružja.GetCurrentSelectedWeapon();
        if (weapon == null)
            return;

        Raycasts raycasts = weapon.GetComponentInChildren<Raycasts>(true);
        WeaponGraphics graphics = weapon.GetComponentInChildren<WeaponGraphics>(true);

        if (weapon != lastWeapon)
        {
            lastWeapon = weapon;
            next_time_to_fire = 0f;
        }

        bool isKnife = weapon.tag == "Nož" || weapon.bullet_type == WeaponBulletType.None;
        if (isKnife)
        {
            if (Input.GetMouseButtonDown(0) && !weapon.IsPlaying())
            {
                weapon.AttackAnimation();
                StartCoroutine(KnifeHit(weapon, raycasts));
            }
            return;
        }

        if (raycasts == null || raycasts.is_reloading)
            return;

        bool wantsToFire = weapon.fire_type == WeaponFireType.Multiple
            ? Input.GetMouseButton(0) && Time.time >= next_time_to_fire
            : Input.GetMouseButtonDown(0) && !weapon.IsPlayingGun();

        if (!wantsToFire)
            return;

        if (raycasts.current_ammo <= 0)
        {
            // No exception if the optional empty-magazine sound is unassigned.
            if (Input.GetMouseButtonDown(0))
                weapon.PlayDryShoot();
            return;
        }

        if (weapon.fire_type == WeaponFireType.Multiple)
            next_time_to_fire = Time.time + 1f / Mathf.Max(0.01f, fire_rate);

        weapon.ShootAnimation();

        if (CameraShaker.Instance != null)
        {
            if (weapon.tag == "Shotgun" || weapon.tag == "Sniper")
                CameraShaker.Instance.ShakeOnce(5f, 5f, 0.2f, 0.5f);
            else if (weapon.fire_type == WeaponFireType.Multiple)
                CameraShaker.Instance.ShakeOnce(2f, 1f, 0.2f, 0.2f);
            else
                CameraShaker.Instance.ShakeOnce(3f, 3f, 0.2f, 0.2f);
        }

        if (weapon.tag == "Sniper" && camera_shake != null)
            camera_shake.Shake();

        if (graphics != null && graphics.muzzle_flash != null)
            graphics.muzzle_flash.Play();

        raycasts.Shoot();
        weapon.PlayShootSound();
    }

    private void LateUpdate()
    {
        if (kontrola_oružja == null)
            return;

        OsobineOružja weapon = kontrola_oružja.GetCurrentSelectedWeapon();
        if (weapon == null)
            return;

        if (ammoDisplays == null || ammoDisplays.Length == 0)
            FindAmmoDisplays();

        Raycasts raycasts = weapon.GetComponentInChildren<Raycasts>(true);
        bool isKnife = weapon.tag == "Nož" || weapon.bullet_type == WeaponBulletType.None;
        foreach (AmmoDisplay display in ammoDisplays)
        {
            if (display != null)
                display.ShowSelectedWeapon(raycasts, isKnife);
        }
    }

    private IEnumerator KnifeHit(OsobineOružja weapon, Raycasts raycasts)
    {
        yield return new WaitForSeconds(0.1f);
        if (weapon == null || kontrola_oružja.GetCurrentSelectedWeapon() != weapon)
            yield break;

        weapon.PlayShootSound();

        yield return new WaitForSeconds(0.2f);
        if (weapon == null || kontrola_oružja.GetCurrentSelectedWeapon() != weapon)
            yield break;

        if (raycasts != null)
            raycasts.Shoot(false, 3f);
    }
}
