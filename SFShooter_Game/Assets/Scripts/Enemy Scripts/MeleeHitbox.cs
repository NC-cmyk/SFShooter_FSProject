using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [Header("--- Charge Stats ---")]
    [Range(1, 3)] [SerializeField] int damage;
    [Range(20, 50)][SerializeField] int knockbackDistance;

    [Header("----- Audio Clips -----")]
    [SerializeField] AudioSource mEnemyAudSource;
    [SerializeField] AudioClip mEnemyAttackSound;
    [Range(0, 1)][SerializeField] float attackSoundVol;

    public bool hit; // the player got hit so the damage should not stack

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hit)
        {
            hit = true;

            // should only play when the enemy actualy hits the player
            mEnemyAudSource.PlayOneShot(mEnemyAttackSound, attackSoundVol);

            other.GetComponent<IDamage>().takeDamage(damage);

            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0;

            other.GetComponent<IPhysics>().takePhysics(direction.normalized * knockbackDistance);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hit = false;
    }
}
