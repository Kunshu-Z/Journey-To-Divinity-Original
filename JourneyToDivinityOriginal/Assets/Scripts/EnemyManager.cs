//Parth Talwar                                                                                                                                                                                                                  ID: 2220145

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Animator anim;
    public Transform player;
    public int health;
    public int attackDamage = 20;
    public Vector3 attackOffset;
    public BoxCollider2D enemyCollider;
    public float attackRange = 10f;
    public LayerMask attackMask;
    public SpriteRenderer MonsterSprite;
    public float movementSpeed = 2.0f;
    public float movementDuration = 2.0f;
    public float volume = 0.5f;
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;

    private float currentRunningTime = 0.0f;
    private bool isRunning = false;
    private bool isIdle = true;
    private bool moveRight = true;
    private bool controlEnable = true;

    // Start is called before the first frame update

    void Start()
    {

    }

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

    //To visualize the enemy's attack radius within the editor
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    //Task to make enemy recieve damage from player
    public void RecieveDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("EnemyHurt");
        audioSource.PlayOneShot(clip2, volume = 1.0f);

        if (health <= 0)
        {
            Death();
        }
    }

    //Task to show enemy has been defeated
    void Death()
    {
        controlEnable = false;
        enemyCollider.enabled = false;
        anim.SetBool("IsDead", true);
    }

    void Update()
    {
        if (controlEnable == true)
        {
            if (isIdle)
            {
                //Start running after a delay
                StartCoroutine(StartRunningAfterDelay());
            }
            else if (isRunning)
            {
                if (moveRight)
                {
                    transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
                    MonsterSprite.flipX = true;
                }
                else
                {
                    transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
                    MonsterSprite.flipX = false;
                }

                //Update the timer
                currentRunningTime += Time.deltaTime;

                //Check if the enemy should stop running
                if (currentRunningTime >= movementDuration)
                {
                    StopRunning();
                    currentRunningTime = 0f;
                    moveRight = !moveRight;
                }
            }

        }
    }

    IEnumerator StartRunningAfterDelay()
    {
        //Wait for a certain duration before starting to run
        yield return new WaitForSeconds(10);

        //Start running
        isIdle = false;
        isRunning = true;

        //Set the animation parameter for running
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", true);
    }

    void StopRunning()
    {
        //Stop running
        isRunning = false;
        isIdle = true;

        //Set the animation parameter for idle
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", true);

        
        //Start running again after a certain delay if needed
        StartCoroutine(StartRunningAfterDelay());
    }
}
