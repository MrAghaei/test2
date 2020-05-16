using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float MoveSpeed;
    [SerializeField] float padding = 0.2f;
    [SerializeField] int health = 200;
    [Header("Projectile")]
    [SerializeField] GameObject LaserSprite;
    [SerializeField] float LaserSpeed = 20f;
    [SerializeField] float LaserFireRapid = 0.05f;
    [SerializeField] AudioClip PlayerDeathSFX;
    [SerializeField] AudioClip PlayerLaserSFX;
    [SerializeField] [Range(0, 1)] float PlayerLaserSFXvolume = 0.75f;
    [SerializeField] [Range(0, 1)] float PlayerDeathSFXvolume = 0.75f;
    Coroutine laserFire;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    // Start is called before the first frame update
    void Start()
    {
        LimitMovement();
       
    }

    private void LimitMovement()
    {
        LimitBounderies();

    }

    private void LimitBounderies()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        fire();
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

            Destroy(gameObject);
            AudioSource.PlayClipAtPoint
                (PlayerDeathSFX, Camera.main.transform.position, PlayerDeathSFXvolume);

        }
    }

    private void fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           laserFire=StartCoroutine(FireRappidly());

        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(laserFire);

        }
           
    }
    IEnumerator FireRappidly()
    {
        while (true)
        {
            GameObject laser =
                    Instantiate(LaserSprite, transform.position, Quaternion.identity) 
                    as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, LaserSpeed);
           yield return new WaitForSeconds(LaserFireRapid);
            AudioSource.PlayClipAtPoint
                (PlayerDeathSFX, Camera.main.transform.position, PlayerDeathSFXvolume);
        }
    }

    private void move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        transform.position = new Vector2(newXPos,newYPos);
        
    }
    
}
