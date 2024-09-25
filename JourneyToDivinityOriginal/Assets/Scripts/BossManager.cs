//Parth Talwar                                                                                                                                                                                                                  ID: 2220145

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    //Public fields
    public Transform player;
    public Animator anim;
    public BoxCollider2D bossCollider;
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;
    public GameObject CrimsonCrystal;
    public bool isFlipped = false;
    public int health = 2;
    public int attackDamage = 20;
    public Vector3 attackOffset;
    public float attackRange = 10f;
    public float volume = 0.5f;
    public float invulnerabilityDuration = 2.0f; 
    public LayerMask attackMask;

    //Private fields
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0.0f;

    void Start()
    {
        CrimsonCrystal.gameObject.SetActive(false);
    }

    //Task to make boss look at the player (To determine when to flip sprites)
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    //Task to make boss attack player
    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            audioSource.PlayOneShot(clip1, volume = 2.0f);
            colInfo.GetComponent<CharacterManager>().RecieveDamage(attackDamage);
        }
    }

    //To visualise the enemy's attack radius within the editor
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    //Task to manage boss taking damage from player
    public void RecieveDamage(int damage)
    {

        if (!isInvulnerable)
        {
            health -= damage;
            audioSource.PlayOneShot(clip2, volume = 1.0f);
            anim.Play("BossHit");

            if (health <= 0)
            {
                Death();
            }

            //Set the boss as invulnerable and start the invulnerability timer (to prevent player from spamming to defeat the boss)
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }
    }

    //Task to manage boss death
    void Death()
    {
        bossCollider.enabled = false;
        anim.SetBool("isDead", true);
        CrimsonCrystal.gameObject.SetActive(true);
    }

    void Update()
    {
        //Update the invulnerability timer
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
            }
        }
    }
}

