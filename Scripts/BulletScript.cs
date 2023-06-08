using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private GameObject owner;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Zombie")
        {
            ZombieScript zombie = collision.gameObject.GetComponent<ZombieScript>();

            if (zombie)
            {
                zombie.TakeDamage(25);
            }
        }

        if (collision.transform.tag == "Player")
        {
            Character playerCharacter = collision.gameObject.GetComponent<Character>();

            if (playerCharacter)
            {
                playerCharacter.TakeDamage(5f, owner);
            }
        }

        if (collision.transform.tag == "Boss")
        {
            BossScript bossEnemy = collision.gameObject.GetComponent<BossScript>();

            if (bossEnemy)
            {
                bossEnemy.TakeDamage(25);
            }
        }

        Destroy(gameObject);
    }

    public void SetOwner(GameObject Object)
    {
        owner = Object;
    }
}
