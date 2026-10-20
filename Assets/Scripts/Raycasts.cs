using System.Collections;
using UnityEngine;

public class Raycasts : MonoBehaviour
{
    private WeaponGraphics weapon_graphics;
    private OsobineOružja osobine_oružja;
    private Animator animator;
    private SnajperScope snajper_scope;
    private CircleSpin circle_spin;

    private bool is_aiming_gun;
    public bool is_aiming;
    public float damage = 10f;
    public float range = 100f;

    public int max_ammo = 10;
    public int current_ammo;
    public int all_ammo;
    public float reload_time = 1f;
    public bool is_reloading;

    public Camera fps_cam;
    private float normal_fov = 60f;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        osobine_oružja = GetComponentInParent<OsobineOružja>();
        weapon_graphics = GetComponentInChildren<WeaponGraphics>(true);
        snajper_scope = GetComponentInChildren<SnajperScope>(true);
        circle_spin = GetComponentInParent<CircleSpin>();

        KontrolaOružja controller = GetComponentInParent<KontrolaOružja>();
        int index = controller != null ? controller.GetWeaponIndex(osobine_oružja) : -1;
        bool isAutomatic = index == 2 || index == 4;
        if (isAutomatic)
        {
            if (max_ammo == 10)
                max_ammo = 30;
            if (all_ammo == 0)
                all_ammo = 90;
        }

        if (fps_cam == null)
            fps_cam = Camera.main;
        if (fps_cam != null)
            normal_fov = fps_cam.fieldOfView;
    }

    private void Start()
    {
        current_ammo = max_ammo;
    }

    private void Update()
    {
        Aim();

        if (is_reloading || !Input.GetKeyDown(KeyCode.R))
            return;

        if (osobine_oružja == null || osobine_oružja.bullet_type == WeaponBulletType.None)
            return;
        if (max_ammo <= 0 || current_ammo >= max_ammo || all_ammo <= 0)
            return;

        StartCoroutine(Reload());
    }

    public void Shoot(bool consumeAmmo = true, float maxDistance = -1f)
    {
        if (consumeAmmo)
        {
            if (current_ammo <= 0)
                return;
            current_ammo--;
        }

        if (fps_cam == null)
            return;

        float distance = maxDistance > 0f ? maxDistance : range;
        RaycastHit hit;
        if (!Physics.Raycast(fps_cam.transform.position, fps_cam.transform.forward, out hit, distance))
            return;

        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
            target.TakeDamge(damage);

        if (weapon_graphics != null && weapon_graphics.hit_effect != null)
        {
            GameObject hitEffect = Instantiate(weapon_graphics.hit_effect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, 1f);
        }
    }

    private void Aim()
    {
        if (osobine_oružja == null || fps_cam == null)
            return;

        if (!is_aiming_gun && !is_reloading &&
            Input.GetMouseButtonDown(1) && osobine_oružja.weapon_aim == WeaponAim.Aim)
        {
            is_aiming_gun = true;
            if (animator != null)
                animator.Play("Aim");
            fps_cam.fieldOfView = 50f;

            if (osobine_oružja.tag == "Sniper")
            {
                if (snajper_scope != null && snajper_scope.crosshair != null)
                    snajper_scope.crosshair.SetActive(false);
                is_aiming = true;
            }
        }
        else if (is_aiming_gun && (Input.GetMouseButtonUp(1) || is_reloading))
        {
            ResetAim(true);
        }
    }

    private void ResetAim(bool playAnimation)
    {
        if (is_aiming_gun && playAnimation && animator != null)
            animator.Play("Aim Down");

        is_aiming_gun = false;
        is_aiming = false;

        if (fps_cam != null)
            fps_cam.fieldOfView = normal_fov;
        if (snajper_scope != null && snajper_scope.crosshair != null)
            snajper_scope.crosshair.SetActive(true);
    }

    private IEnumerator Reload()
    {
        is_reloading = true;
        ResetAim(true);

        if (animator != null)
            animator.SetBool("Reloading", true);
        if (osobine_oružja != null)
            osobine_oružja.PlayReloadSound();
        if (circle_spin != null && circle_spin.circle_animator != null)
            circle_spin.circle_animator.Play("Circle shoot");

        yield return new WaitForSeconds(reload_time + 0.25f);

        int needed = max_ammo - current_ammo;
        int loaded = Mathf.Min(needed, Mathf.Max(0, all_ammo));
        current_ammo += loaded;
        all_ammo -= loaded;

        is_reloading = false;
        if (animator != null)
            animator.SetBool("Reloading", false);
    }

    private void OnDisable()
    {
        bool wasReloading = is_reloading;
        StopAllCoroutines();
        is_reloading = false;
        if (wasReloading && animator != null)
            animator.SetBool("Reloading", false);
        ResetAim(false);
    }
}
