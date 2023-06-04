using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAk47 : MonoBehaviour
{
    [SerializeField]
    private float currentAmmo = 30f;

    [SerializeField]
    private float ammunition = 180f;

    [SerializeField]
    private float maxAmmo = 600f;

    [SerializeField]
    private float bulletSpeed = 200f;

    [SerializeField]
    private string weaponName = "BK74";

    [SerializeField]
    private GameObject owner;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform muzzleLocation;

    [SerializeField]
    private AudioSource fireSound;

    [SerializeField]
    private AudioSource reloadSound;

    [SerializeField]
    private AudioClip fireSoundClip;

    [SerializeField]
    private GameObject fbxMesh;

    [SerializeField]
    bool canFire = true;

    private float lastShootTime;

    private float fireRate = 0.1f;

    private float timer;

    private float reloadDelayTime = 1.6f;

    private float timeRate = 5f;



    Animator animation;

    // Start is called before the first frame update
    void Start()
    {
        fireSound = GetComponent<AudioSource>();

        PlayEqpAnim();
    }

    // Update is called once per frame
    void Update()
    {
        animation = fbxMesh.GetComponent<Animator>();

        Timer();
    }

    public void Firegun()
    {
        if (bullet)
        {
            if (currentAmmo > 0 && canFire)
            {
                currentAmmo = currentAmmo - 1;

                GameObject bulletGameObject = Instantiate(bullet, muzzleLocation.transform.position, transform.rotation);

                BulletScript BulletScriptComponent = bulletGameObject.GetComponent<BulletScript>();

                if (BulletScriptComponent)
                {
                    BulletScriptComponent.SetOwner(this.gameObject);
                }

                bulletGameObject.GetComponent<Rigidbody>().AddForce(muzzleLocation.transform.forward * bulletSpeed);

                fireSound.PlayOneShot(fireSoundClip, 0.7f);

                animation.Play("Fire");

                if (currentAmmo <= 0)
                {
                    canFire = false;
                }
            }
        }
    }

    public void AutoFire()
    {
        if (Time.time > lastShootTime + fireRate)
        {
            lastShootTime = Time.time;

            Firegun();

            if (currentAmmo == 0)
            {
                ReloadGun();
            }

        }
    }

    public void ReloadGun()
    {
        if (currentAmmo != 30 && ammunition > 0)
        {
            canFire = false;
            if (currentAmmo == 0)
            {

                if (ammunition >= 30)
                {
                    currentAmmo += 30;
                    ammunition = ammunition - 30;
                }

                else
                {
                    currentAmmo += ammunition;
                    ammunition -= ammunition;
                }

            }

            else
            {
                float ammoCalculator = 30 - currentAmmo;
                currentAmmo = currentAmmo + ammoCalculator;
                ammunition = ammunition - ammoCalculator;
            }
            animation.Play("Reload");
            reloadSound.Play();
        }


    }

    //Animations
    public void PlayWalkAnim(bool EnableWalkAnim)
    {
        if (EnableWalkAnim)
        {
            if (animation)
                animation.SetBool("IsWalk", EnableWalkAnim);
        }

        if (EnableWalkAnim == false)
        {
            if (animation)
                animation.SetBool("IsWalk", false);
        }

    }


    public void PlayRunAnim(bool EnableAnim)
    {
        if (EnableAnim)
        {
            animation.SetBool("IsRun", EnableAnim);
        }

        if (EnableAnim == false)
        {
            animation.SetBool("IsRun", false);
        }

    }


    public void PlayEqpAnim()
    {
        if (animation)
            animation.Play("EQP");
    }

    void EnableFire()
    {
        canFire = true;
    }

    void Timer()
    {
        if (canFire == false)
        {
            timer += Time.deltaTime;

            if (timer > reloadDelayTime)
            {
                EnableFire();

                timer = 0f;
            }
        }

    }

    public float SetAmmo(float AmmoValue)
    {
        if (ammunition != maxAmmo)
        {
            ammunition += AmmoValue;

            if (ammunition > maxAmmo)
            {
                ammunition = maxAmmo;
            }
        }

        return ammunition;
    }

    public float GetAmmo()
    {
        return currentAmmo;
    }

    public float GetHave()
    {
        return ammunition;
    }
}
