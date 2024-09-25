//Parth Talwar                                                                                                                                                                                                                  ID: 2220145

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Security.Authentication.ExtendedProtection;
using System.Globalization;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    //Public Fields
    public GameObject Character;
    public GameObject CharacterMirror;
    public GameObject BlackImage;
    public GameObject GamePausedText;
    public GameObject LevelDefeatText;
    public GameObject LevelCompleteText;
    public GameObject Healthbar;
    public GameObject PauseBtn;
    public GameObject MuteBtn;
    public GameObject PlayBtn;
    public GameObject RestartBtn;
    public GameObject ExitBtn;
    public GameObject RespawnBtn;
    public GameObject ProceedBtn;
    public GameObject HealthIcon;
    public GameObject HealthIcon2;
    public GameObject HealthIcon3;
    public SpriteRenderer CharacterSprite;
    public Animator anim;
    public BoxCollider2D topCollider;
    public CircleCollider2D bottomCollider;
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip4;
    public AudioClip clip5;
    public CharacterController2D controller;
    public Slider healthSlider;
    public float moveSpeed = 40f;
    public float volume = 0.5f;
    public Transform attackingPoint;
    public Transform plungingAttackingPoint;
    public float attackingRange = 1.5f;
    public float plungingAttackingRange = 1.5f;
    public int attackingDamage;
    public float attackingRate = 2f;
    public float durationBeforeIdle = 5f; 
    public int maxHealth = 100;
    public int currentHealth;
    public bool healthFull;
    public LayerMask enemyLayers;
    public Button PauseButton;
    public Button PlayButton;
    public Button MuteButton;
    public Button RestartButton;
    public Button ExitButton;
    public Button RespawnButton;
    public Button ProceedButton;
    public string StartPoint;
    public string RestartGame;
    public string StartScene;
    public string NextLevel;

    //Private Fields
    private Vector2 currentPosition;
    private Vector2 cameraPosition;
    private Rigidbody2D rb2;
    private float horizontalMovement = 0f;
    private float nextAttackingTime = 0;
    private int attackStage = 1;
    private int blockStage = 1;
    private int initialHealth;
    private float timeSinceLastMovement = 0f;
    private float attackCooldown = 0.5f;
    private float previousAttackTime = 0f;
    private bool jump = false;
    private bool isIdle = false;
    private bool isAttacking = false;
    private bool crouch = false;
    private bool gamePaused = false;
    private bool musicPaused = false;
    private bool controlEnable = true;
    private bool isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        CharacterSprite = Character.GetComponent<SpriteRenderer>();
        anim = Character.GetComponent<Animator>();
        rb2 = Character.GetComponent<Rigidbody2D>();
        //To prevent the player from modifying the health slider
        healthSlider.interactable = false;
        timeSinceLastMovement = 0f;
        UpdateHealth();


        //Pause button code
        Button Pause = PauseButton.GetComponent<Button>();
        Pause.onClick.AddListener(TaskOnClickPause);
        //Play button code
        Button Play = PlayButton.GetComponent<Button>();
        Play.onClick.AddListener(TaskOnClickPlay);
        //Mute button code
        Button Mute = MuteButton.GetComponent<Button>();
        Mute.onClick.AddListener(TaskOnClickMute);
        //Restart button code
        Button Restart = RestartButton.GetComponent<Button>();
        Restart.onClick.AddListener(TaskOnClickRestart);
        //Exit to Menu button code
        Button Exit = ExitButton.GetComponent<Button>();
        Exit.onClick.AddListener(TaskOnClickExit);
		//Respawn button code
		Button Respawn = RespawnButton.GetComponent<Button>();
		Respawn.onClick.AddListener(TaskOnClickRespawn);
		//Proceed to next level code
		Button Proceed = ProceedButton.GetComponent<Button>();
        Proceed.onClick.AddListener(TaskOnClickProceed);

        BlackImage.gameObject.SetActive(false);
		LevelDefeatText.gameObject.SetActive(false);
		LevelCompleteText.gameObject.SetActive(false);
        ProceedBtn.gameObject.SetActive(false);
        GamePausedText.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(false);
        RestartBtn.gameObject.SetActive(false);
        ExitBtn.gameObject.SetActive(false);
		RespawnBtn.gameObject.SetActive(false);
	}

    //Task to pause game
    void TaskOnClickPause()
    {
        Debug.Log("Game Paused");
        if (!gamePaused)
        {
            Time.timeScale = 0;
            gamePaused = true;
            musicPaused = true;
            controlEnable = false;
            BlackImage.gameObject.SetActive(true);
            GamePausedText.gameObject.SetActive(true);
            PlayBtn.gameObject.SetActive(true);
            RestartBtn.gameObject.SetActive(true);
            ExitBtn.gameObject.SetActive(true);
        }
        //Mute audio
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            musicPaused = true;
        }

    }
    //Task to play game
    void TaskOnClickPlay()
    {
        Debug.Log("Resuming Game");
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
            controlEnable = true;
            BlackImage.gameObject.SetActive(false);
            GamePausedText.gameObject.SetActive(false);
            PlayBtn.gameObject.SetActive(false);
            RestartBtn.gameObject.SetActive(false);
            ExitBtn.gameObject.SetActive(false);
        }
        //Unmute audio
        if (musicPaused)
        {
            audioSource.UnPause();
            musicPaused = false;
        }
    }
    //Task to mute game
    void TaskOnClickMute()
    {
        //if music is paused, set the volume variable to 0.2f 
        if (musicPaused)
        {
            Debug.Log("Unmuting Audio...");
            audioSource.volume = 0.5f;
            musicPaused = false;
        }

        //else condition for muting the audio by decreasing volume variable to 0f
        else
        {
            Debug.Log("Muting Audio...");
            audioSource.volume = 0f;
            musicPaused = true;
        }
    }

    //Task for selecting restart button
    void TaskOnClickRestart()
    {
        Debug.Log("Restarting Game...");
        SceneManager.LoadScene(RestartGame);
        Time.timeScale = 1;
        gamePaused = false;
        controlEnable = true;
        BlackImage.gameObject.SetActive(false);
        GamePausedText.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(false);
        RestartBtn.gameObject.SetActive(false);
        ExitBtn.gameObject.SetActive(false);
    }

    //Task for selecting exit button
    void TaskOnClickExit()
    {
        Debug.Log("Exiting back to menu...");
        SceneManager.LoadScene(StartScene);
    }

    //Task for selection respawn button
    void TaskOnClickRespawn()
    {
        Debug.Log("Respawning character...");
		SceneManager.LoadScene(StartPoint);
	}

    //Task for selecting proceed button
	void TaskOnClickProceed()
    {
        Debug.Log("Going to next level...");
        SceneManager.LoadScene(NextLevel);
        Time.timeScale = 1;
    }

    //Task to update the player's health
    public void UpdateHealth()
    {
        float healthValue = (float)currentHealth / maxHealth;
        healthSlider.value = healthValue;
    }

    //Task to make objects with collision colliders function
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Heart")
        {
            audioSource.PlayOneShot(clip4, volume = 3f);
            anim.Play("CharacterHeal");
            HealthIcon.gameObject.SetActive(false);

            if (healthFull == false)
            {
                currentHealth = currentHealth + 20;
                UpdateHealth();
            }

            if (currentHealth >= 100)
            {
                healthFull = true;
                currentHealth = 100;
            }
        }

        if (hit.gameObject.tag == "Heart2")
        {
            audioSource.PlayOneShot(clip4, volume = 3f);
            anim.Play("CharacterHeal");
            HealthIcon2.gameObject.SetActive(false);

            if (healthFull == false)
            {
                currentHealth = currentHealth + 20;
                UpdateHealth();
            }

            if (currentHealth >= 100)
            {
                healthFull = true;
                currentHealth = 100;
            }
        }

        if (hit.gameObject.tag == "Heart3")
        {
            audioSource.PlayOneShot(clip4, volume = 3f);
            anim.Play("CharacterHeal");
            HealthIcon3.gameObject.SetActive(false);

            if (healthFull == false)
            {
                currentHealth = currentHealth + 20;
                UpdateHealth();
            }

            if (currentHealth >= 100)
            {
                healthFull = true;
                currentHealth = 100;
            }
        }

        if (hit.gameObject.tag == "Spike")
        {
            audioSource.PlayOneShot(clip5, volume = 10.0f);
			currentHealth = currentHealth - 20;
            UpdateHealth();
            anim.Play("CharacterHurt");
		}
    }

    //Task to make objects with trigger colliders function
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Water")
        {
            Debug.Log("Water Hit");
            horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed / 2;
        }

        if (hit.gameObject.tag == "ExitDoor") 
        {
            Time.timeScale = 0;
            controlEnable = false;
            BlackImage.gameObject.SetActive(true);
			PauseBtn.gameObject.SetActive(false);
			LevelCompleteText.gameObject.SetActive(true);
            ProceedBtn.gameObject.SetActive(true);
        }

        if (hit.gameObject.tag == "CrimsonCrystal")
        {
            anim.Play("CharacterPray");
            horizontalMovement = Input.GetAxisRaw("Horizontal") * 0;
            controlEnable = false;
            BlackImage.gameObject.SetActive(true);
            PauseBtn.gameObject.SetActive(false);
            LevelCompleteText.gameObject.SetActive(true);
            ProceedBtn.gameObject.SetActive(true);
        }
    }

    //Task for the player to recieve damage from the enemy
    public void RecieveDamage(int damage)
    {
        //If the player is crouching, deflect the enemy's attack and deplete no health
        if (crouch)
        {
            if (blockStage == 2)
            {
                blockStage = 1;
            }
            string blockAnimationName = "CharacterBlock" + blockStage;
            anim.Play(blockAnimationName);
            audioSource.PlayOneShot(clip2, volume = 1.2f);
            blockStage++;
            return;
        }

        //Else, make the player take damage
        else
        {
            currentHealth -= damage;
            UpdateHealth();
            healthFull = false;
            audioSource.PlayOneShot(clip5, volume = 10.0f);
            anim.Play("CharacterHurt");
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (controlEnable == true)
        {
            isAttacking = false;
            if (!isAttacking)
            {
                //If player is not crouching, allow for player movement
                if(!crouch && !isAttacking)
                {
                    horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
                }
				

				anim.SetFloat("Speed", Mathf.Abs(horizontalMovement));

                //If the player selects up or w key
				if (Input.GetButtonDown("Jump"))
				{
					jump = true;
					anim.SetBool("isJumping", true);

                    if (!isJumping)
                    {
                        audioSource.PlayOneShot(clip1, volume = 0.5f);
                        isJumping = true;
                    }
                }

                //If the player selects down or s key
                if (Input.GetButtonDown("Crouch") && !isJumping)
				{
					Debug.Log("Character Crouching...");
					crouch = true;
                    horizontalMovement = 0f;
                    OnCrouching(true);
				}

                //If the player releases down or s key
				else if (Input.GetButtonUp("Crouch"))
				{
					Debug.Log("Character Standing...");
					crouch = false;
                    
					OnCrouching(false);
				}

                //If the player loses all health
                if (currentHealth <= 0)
                {
                    topCollider.enabled = false;     
					BlackImage.gameObject.SetActive(true);
					PauseBtn.gameObject.SetActive(false);
                    MuteBtn.gameObject.SetActive(false);
					Healthbar.gameObject.SetActive(false);
					LevelDefeatText.gameObject.SetActive(true);
					RespawnBtn.gameObject.SetActive(true);
					controlEnable = false;
                    horizontalMovement = 0f;
                    anim.SetBool("isDead", true);
                }

				if (Time.time >= nextAttackingTime)
				{
                    //Make player attack if pressing z
					if (Input.GetKeyDown("z"))
					{
						if (Time.time - previousAttackTime >= attackCooldown)
						{

							bool Jumping = !controller.m_Grounded;

                            //If the player is jumping when pressing z, make player perform plunging attack
							if (Jumping)
							{
								// Play the plunging attack animation
								anim.Play("CharacterPlungingAttack");
                                audioSource.PlayOneShot(clip3, volume = 1.6f);
                                AttackInAir();
							}

                            //Else, make player attack normally
							else
							{
                                //Function to make player use multiple attack animations
								if (attackStage == 5)
								{
									attackStage = 1;
								}
                                isAttacking = true;
								string attackAnimationName = "CharacterAttack" + attackStage;
								anim.Play(attackAnimationName);
                                audioSource.PlayOneShot(clip2, volume = 1.2f);

								attackStage++;
								previousAttackTime = Time.time;

								Attack();
								nextAttackingTime = Time.time + 1f / attackingRate;
							}
						}
					}
				}

                //Task to make player attack 
                void Attack()
                {
                    Collider2D[] hittingEnemies = Physics2D.OverlapCircleAll(attackingPoint.position, attackingRange, enemyLayers);

                    foreach (Collider2D enemyCollider in hittingEnemies)
                    {
                        // Check if the enemy is a general enemy
                        EnemyManager enemy = enemyCollider.GetComponent<EnemyManager>();
                        if (enemy != null)
                        {
                            enemy.RecieveDamage(attackingDamage);
                        }

                        // Check if the enemy is the boss
                        BossManager boss = enemyCollider.GetComponent<BossManager>();
                        if (boss != null)
                        {
                            boss.RecieveDamage(attackingDamage);
                        }
                    }
                }

                //Task to make player perform plunging attack
                void AttackInAir()
                {
                    Collider2D[] hittingEnemies = Physics2D.OverlapCircleAll(plungingAttackingPoint.position, attackingRange, enemyLayers);

                    foreach (Collider2D enemyCollider in hittingEnemies)
                    {
                        // Check if the enemy is a general enemy
                        EnemyManager enemy = enemyCollider.GetComponent<EnemyManager>();
                        if (enemy != null)
                        {
                            enemy.RecieveDamage(attackingDamage);
                        }

                        // Check if the enemy is the boss
                        BossManager boss = enemyCollider.GetComponent<BossManager>();
                        if (boss != null)
                        {
                            boss.RecieveDamage(attackingDamage);
                        }
                    }
                }

                if (Mathf.Abs(horizontalMovement) > 0.01f || Input.GetButtonDown("Jump"))
				{
					//Player moved, reset the idle timer
					timeSinceLastMovement = 0f;
					isIdle = false;
				}
				else
				{
					// Player is not moving, increment the idle timer
					timeSinceLastMovement += Time.deltaTime;

					// Check if the idle duration has been reached
					if (timeSinceLastMovement >= durationBeforeIdle && !isIdle)
					{
						// Play the "CharacterPray" animation
						anim.Play("CharacterPray");
						isIdle = true; // Set to true to prevent playing the animation repeatedly.
					}
				}
			}

            else 
            {
                horizontalMovement = 0f;    
            }
        }
    }

    //Task to draw gizmos for both the attacking and plunging attacking points
    void OnDrawGizmosSelected()
    {
        if (attackingPoint == null)
            return;

        Gizmos.DrawWireSphere(attackingPoint.position, attackingRange);

        Gizmos.DrawWireSphere(plungingAttackingPoint.position, attackingRange);
    }

    //Task to determine whether player is on ground or not
    public void OnLanding ()
    {
        anim.SetBool("isJumping", false);
        isJumping = false;
    }

    //Task to set animation to crouch 
    public void OnCrouching(bool isCrouching)
    {
		anim.SetBool("isCrouching", isCrouching);
    }

    //Fixed update method to set jump to false everytime the player hits the ground
    void FixedUpdate()
    {
        controller.Move(horizontalMovement * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
