using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    [SerializeField]
    private Animator animation;

    [SerializeField]
    private GameObject fbxMesh;

    [SerializeField]
    private Transform playerLoc;

    [SerializeField]
    private Transform fireLocation;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private AudioSource explosionSfx;

    [SerializeField]
    private AudioClip explosionClip;

    [SerializeField]
    private GameObject explosionFX;

    [SerializeField]
    private float health = 100.0f;

    private float bulletSpeed = 300f;

    private float attackDistance = 70f;

    private float fireDistance = 16f;

    private float fireRate = 0.5f;

    private float lastShootTime;

    private bool spawn = false;

    static string tagCharacter = "Zombie";

    protected UnityEngine.AI.NavMeshAgent navMeshAgentObject;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = tagCharacter;

        navMeshAgentObject = GetComponent<UnityEngine.AI.NavMeshAgent>();

        explosionSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Killer();

        animation = fbxMesh.GetComponent<Animator>();

        float distance = Vector3.Distance(playerLoc.position, transform.position);

        if (distance < attackDistance)
        {
            navMeshAgentObject.SetDestination(playerLoc.position);

            new Vector3(playerLoc.transform.position.x, 0f, 0f);

            this.gameObject.transform.LookAt(playerLoc);
        }

        AutoFire();
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
    }


    void Killer()
    {
        if (health <= 0)
        {
            if (spawn == false)
            {
                GameObject ExplosionAnim = Instantiate(explosionFX, fireLocation.transform.position, transform.rotation);

                spawn = true;

                explosionSfx.PlayOneShot(explosionClip, 0.01f);

                Destroy(gameObject, 0.1f);

                Destroy(ExplosionAnim, 1.5f);
            }
        }
    }


    void FireGun()
    {
        GameObject bulletGameObject = Instantiate(bullet, fireLocation.transform.position, transform.rotation);

        BulletScript bulletScriptComponent = bulletGameObject.GetComponent<BulletScript>();

        bulletScriptComponent.SetOwner(this.gameObject);

        bulletGameObject.GetComponent<Rigidbody>().AddForce(fireLocation.transform.forward * bulletSpeed);
    }

    void AutoFire()
    {
        float distance = Vector3.Distance(playerLoc.position, transform.position);

        if (distance < fireDistance)
        {
            if (Time.time > lastShootTime + fireRate)
            {
                lastShootTime = Time.time;

                FireGun();
            }
        }
    }
}
