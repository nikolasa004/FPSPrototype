using UnityEngine;
using EZCameraShake;

public enum WeaponAim
{
    None,
    Aim
}

public enum WeaponFireType
{
    Single,
    Multiple
}

public enum WeaponBulletType
{
    None,
    Bullet,
    Arrow
}

public class OsobineOružja : MonoBehaviour
{
    private Animator animator;
    public WeaponAim weapon_aim;

    [SerializeField]
    private AudioSource shoot_sound, reload_sound, dry_shoot;

    public WeaponFireType fire_type;
    public WeaponBulletType bullet_type;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShootAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Shoot");
    }

    public void AttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger(Random.Range(1, 3) == 1 ? "Attack1" : "Attack2");

        if (CameraShaker.Instance != null)
            CameraShaker.Instance.ShakeOnce(1f, 1f, 0.2f, 0.3f);
    }

    public void PlayShootSound()
    {
        if (shoot_sound == null || shoot_sound.clip == null)
            return;

        if (fire_type == WeaponFireType.Multiple)
        {
            shoot_sound.PlayOneShot(shoot_sound.clip);
        }
        else
        {
            shoot_sound.Play();
        }
    }

    public void PlayReloadSound()
    {
        if (reload_sound != null)
            reload_sound.Play();
    }

    public void PlayDryShoot()
    {
        if (dry_shoot != null)
            dry_shoot.Play();
    }

    public bool IsPlaying()
    {
        if (animator == null)
            return false;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        return (info.IsName("Attack1") || info.IsName("Attack2")) && info.normalizedTime < 1f;
    }

    public bool IsPlayingGun()
    {
        if (animator == null)
            return false;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName("Shoot") && info.normalizedTime < 1f;
    }
}
