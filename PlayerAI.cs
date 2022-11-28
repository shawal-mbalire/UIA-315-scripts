using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAI : MonoBehaviour
{

    [Header("Player Health Damage")]
    private float PlayerHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float PlayerSpeed;

    [Header("Player Things")]
    public NavMeshAgent PlayerAgent;
    public Transform LookPoint;
    public GameObject ShootingRayCastArea;
    public Transform enemyBody;
    public LayerMask enemyLayer;
    public Transform Spawn;
    public Transform PlayerCharacter;

    [Header("Player Shooting Var")]
    public float timebtwShoot;
    bool previuoslyShoot;

    [Header("Player animation and spark effect")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Player States")]
    public float visionRadius;
    public float shootingRadius;
    public bool enemyInvisionRadius;
    public bool enemyInshootingRadius;
    public bool isPlayer = false;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;

    public ScoreManager scoreManager;

    private void Awake() {
        PlayerAgent = GetComponent<NavMeshAgent>();
        presentHealth = PlayerHealth;
    }

    private void Update() {
        enemyInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, enemyLayer);
        enemyInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, enemyLayer);

        if(enemyInvisionRadius && !enemyInshootingRadius) PursueEnemy();
        if(enemyInvisionRadius && enemyInshootingRadius) ShootEnemy();
    }

    private void PursueEnemy()
    {
        if(PlayerAgent.SetDestination(transform.position))
        {
            //animations
            anim.SetBool("Running", true);
            anim.SetBool("Shooting", false);
        }
        else
        {
            anim.SetBool("Running", false);
            anim.SetBool("Shooting", false);
        }
    }

    private void ShootEnemy()
    {
        PlayerAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        if(!previuoslyShoot)
        {

            muzzleSpark.Play();
            audioSource.PlayOneShot (shootingSound);

            RaycastHit hit;
            if(Physics.Raycast(ShootingRayCastArea.transform.position, ShootingRayCastArea.transform.forward, out hit, shootindRadius))
            {
                Debug.log("Shooting" + hit.transform.name);
                Enemy enemy = hit.transform.GetComponent<Enemy>();

                if(enemy != null)
                {
                    enemy.enemyHitDamage(giveDamage);
                }
                anim.SetBool("Running", false);
                anim.SetBool("Shooting", true);
            }
            previuoslyShoot = true;
            Invoke(nameof(ActiveShooting),timebtwShoot);
        }
    }
    private void ActiveShooting()
    {
        previuoslyShoot = false;
    }

    public void PlayerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if (presentHealth<=0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        PlayerAgent.SetDestination(transform.position);
        PlayerSpeed = 0f;
        shootingRadius= 0f;
        visionRadius = 0f;
        enemyInshootingRadius =false;
        enemyInshootingRadius = false;
        anim.SetBool("Die", true);
        anim.SetBool("Shooting", false);
        anim.SetBool("Running", false);

        //animations

        Debug.Log("Dead");
        scoreManager.enemykills += 1;

        yield return new WaitForSeconds(5f);
        Debug.Log("Spawn");
        presentHealth=120f;
        PlayerSpeed = 1f;
        shootingRadius = 10f;
        visionRadius = 100f;
        enemyInvisionRadius = true;
        enemyInshootingRadius = false;

        //animations
        anim.SetBool("Running", true);
        anim.SetBool("Die", false);

        //spawn point
        PlayerCharacter.transform.position = Spawn.transform.position;
        PursueEnemy();
    }


}
