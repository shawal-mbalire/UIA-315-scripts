using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{

    [Header("Rifle")]
    public Camera cam;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 15f;
    public PlayerScript player;
    public Animator animator;

    [Header("Rifle Ammunition and shooting")]
    private float nextTimeToShoot = 0f;
    private int maximumAmmunition = 20;
    private int mag = 15;
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    public GameObject goreEffect;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public Audioclip realodingSound;


    void Shoot()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            Objects objects = hitInfo.transform.GetComponent<Objects>();

            if(objects != null)
            {
                objects.objectHitDamage(giveDamage);
            }
        }
    }
    private void Awake()
    {
        presentAmmunition = maximumAmmunition;
    }
    // Update is called once per frame
    void Update()
    {
        if(setReloading) return;
        if(presentAmmunition <=0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButtonDown("Fire1") && nextTimeToShoot.time >= nextTimeToShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeToShoot = nextTimeToShoot.time + 1f/fireCharge;
            Shoot();
        }
    }

    void Shoot()
    {
        if(mag==0)
        {
            //show out of ammo text
        }

        presentAmmunition --;
        if (presentAmmunition == 0)
        {
            mag --;
        }

        //update ui
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);

        muzzleSpark.Play();
        audioSource.PlayOneShot (shootingSound);
        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.log(hitInfo.transform.name);
            Objects objects = hitInfo.transform.GetComponent<Objects>();

            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

            if (objects!=null)
            {
                objects.objectHitDamage(giveDamage);
                GameObject WoodGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                DEsTroy(WoodGo, 1f);
            }
            else if(enemy != null)
            {
                enemy.enemyHitDamage(giveDamage);
                GameObject GoreGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                DEsTroy(GoreGo, 1f);

            }
        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");

        //animation and audio
        animator.SetBool("Reloading",true);
        audioSource.PlayOneShot (realodingSound);
        yield return new WaitForSeconds(reloadingTime);

        //animations
        animator.SetBool("Reloading",false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;

    }
}
