using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float MinTimesBetweenShots = 0.2f;
    [SerializeField] float MaxTimesBetweenShots = 3f;
    [SerializeField] GameObject EnemyLaser;
    [SerializeField] float EnemyLaserSpeed = 10f;
    [SerializeField] GameObject ExplosionVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip EnemyLaserSFX;
    [SerializeField] [Range(0, 1)] float EnemyLaserSFXvolume = 0.75f;
    [SerializeField] [Range(0, 1)] float deathSFXvolume = 0.75f;
    private void Start()
    {
        shotCounter = Random.Range(MinTimesBetweenShots, MaxTimesBetweenShots);
    }
    private void Update()
    {
        CountDownAndShoot();
    }
    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            fire();
            shotCounter = Random.Range(MinTimesBetweenShots, MaxTimesBetweenShots);
        }
    }

    private void fire()
    {
        GameObject enemyLaser =
            Instantiate(EnemyLaser,transform.position,Quaternion.identity)
            as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -EnemyLaserSpeed);
        AudioSource.PlayClipAtPoint
            (EnemyLaserSFX, Camera.main.transform.position, EnemyLaserSFXvolume);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProccessHit(damageDealer);
        
        
    }
    private void ProccessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject ExVfx =
                        Instantiate(ExplosionVFX, transform.position, transform.rotation);
        Destroy(ExVfx, 1f);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXvolume);
    }
}
