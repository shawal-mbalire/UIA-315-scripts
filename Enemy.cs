using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Health Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float enemySpeed;

    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform LookPoint;
    public GameObject ShootingRayCastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public Transform Spawn;
    public Transform EnemyCharacter;

    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previuoslyShoot;

    [Header("Enemy animation and spark effect")]
    public Animator anim;

    [Header("Enemy States")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool isPlayer = false;
    private void Awake() {
        enemyAgent = GetComponent<NavMeshAgent>();
        presentHealth = enemyHealth;
    }

    private void Update() {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        if(playerInvisionRadius && !playerInshootingRadius) PursuePlayer();
        if(playerInvisionRadius && playerInshootingRadius) ShootPlayer();
    }

    private void PursuePlayer()
    {
        if(enemyAgent.SetDestination(transform.position))
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

    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        if(!previuoslyShoot)
        {
            RaycastHit hit;
            if(Physics.Raycast(ShootingRayCastArea.transform.position, ShootingRayCastArea.transform.forward, out hit, shootindRadius))
            {
                Debug.log("Shooting" + hit.transform.name);

                if (isPlayer == true)
                {
                    PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();
                    if (playerBody != null)
                    {
                        playerBody.playerHitDamage(giveDamage);
                    }
                }
                else
                {
                    PlayerAI playerBody = hit.transform.GetComponent<PlayerAI>();
                    if (playerBody != null)
                    {
                        playerBody.PlayerAIHitDamage(giveDamage);
                    }
                }

            }
            anim.SetBool("Running", false);
            anim.SetBool("Shooting", true);
        }
        previuoslyShoot = true;
        Invoke(nameof(ActiveShooting),timebtwShoot);

    }
    private void ActiveShooting()
    {
        previuoslyShoot = false;
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if (presentHealth<=0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius= 0f;
        visionRadius = 0f;
        playerInshootingRadius =false;
        playerInshootingRadius = false;
        anim.SetBool("Die", true);
        anim.SetBool("Shooting", false);
        anim.SetBool("Running", false);

        //animations

        Debug.Log("Dead");
        yield return new WaitForSeconds(5f);
        Debug.Log("Spawn");
        presentHealth=120f;
        enemySpeed = 1f;
        shootingRadius = 10f;
        visionRadius = 100f;
        playerInvisionRadius = true;
        playerInshootingRadius = false;

        //animations
        anim.SetBool("Running", true);
        anim.SetBool("Die", false);

        //spawn point
        EnemyCharacter.transform.position = Spawn.transform.position;
        PursuePlayer();
    }


}
