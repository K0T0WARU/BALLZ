using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] float speed;
    private GameObject focalPoint;
    public bool hasPowerUp = false;
    public GameObject powerUpIndicator;
    private float powerupStrength = 30;

    public PowerUpType currentPowerUp = PowerUpType.None;

    public GameObject bulletPrefab;
    private GameObject tmpBullet;
    private Coroutine powerupCountdown;

    [SerializeField] float hangTime;
    [SerializeField] float smashSpeed;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;

    bool smashing = false;
    float floorY;

    [SerializeField] int timeBuff;
    bool startTimer = false;
    public float timer;
    [SerializeField] int totalBullet;
    int bulletLeft;

    public GameObject ammoEmpty;
    public GameObject Bullet1;
    public GameObject Bullet2;
    public GameObject Bullet3;
    public GameObject Bullet4;
    public GameObject Bullet5;

    public Slider timerSlider;
    public GameObject timerEmpty;

    private GameManager gameManager;

    public AudioSource ballTap;
    public AudioSource buffTaken;
    public AudioSource fireSound;
    public AudioSource smashSound;
    public AudioSource gameOverSound;
    public AudioSource gameOverEmbient;
    public AudioSource defaultEmbient;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
         
        if(currentPowerUp == PowerUpType.Bullets && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBullets();
            fireSound.Play();
            DecreaseAmmo(bulletLeft);
            bulletLeft--;
            if (bulletLeft <= 0)
                timer = 0;
        }

        if(currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            smashSound.Play();
            StartCoroutine(Smash());
        }

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
            gameManager.GameOver();
            defaultEmbient.Stop();
            gameOverSound.Play();
            gameOverEmbient.Play();
        }

        if (startTimer)
        {
            timer -= Time.deltaTime;
            timerSlider.value = timer;
            if (timer < 0)
            {
                startTimer = false;
                PowerDown();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if (currentPowerUp != PowerUpType.None)
                PowerDown();
            hasPowerUp = true;
            buffTaken.Play();
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            switch (currentPowerUp)
            {
                case PowerUpType.Bullets:
                    powerUpIndicator.GetComponent<Renderer>().material.color = new Color(0, 1, 0.3f, 1);
                    bulletLeft = totalBullet;
                    ammoEmpty.SetActive(true);
                    Reload();
                    break;
                case PowerUpType.Pushback:
                    powerUpIndicator.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
                    ballTap.volume = 0.3f;
                    break;
                case PowerUpType.Smash:
                    powerUpIndicator.GetComponent<Renderer>().material.color = new Color(1, 0, 0.3f, 1);
                    break;
            }
            gameManager.UpdateBuff();
            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            /*
            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            */
            timer = timeBuff;
            timerEmpty.SetActive(true);
            startTimer = true;
        }
    }

    void PowerDown()
    {
        hasPowerUp = false;
        switch (currentPowerUp)
        {
            case PowerUpType.Bullets:
                ammoEmpty.SetActive(false);
                break;
            case PowerUpType.Pushback:
                ballTap.volume = 0.05f;
                break;
        }
        currentPowerUp = PowerUpType.None;
        gameManager.UpdateBuff();
        powerUpIndicator.gameObject.SetActive(false);
        timerEmpty.SetActive(false);
    }
    /*
    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(timeBuff);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        gameManager.UpdateBuff();
        powerUpIndicator.gameObject.SetActive(false);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            ballTap.Play();
            if (currentPowerUp == PowerUpType.Pushback)
            {
                Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
        }
    }

    void LaunchBullets()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            tmpBullet = Instantiate(bulletPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpBullet.GetComponent<Bullet>().Fire(enemy.transform);
        }
    }

    void DecreaseAmmo (int bullet)
    {
        switch (bullet)
        {
            case 1:
                Bullet1.SetActive(false);
                break;
            case 2:
                Bullet2.SetActive(false);
                break;
            case 3:
                Bullet3.SetActive(false);
                break;
            case 4:
                Bullet4.SetActive(false);
                break;
            case 5:
                Bullet5.SetActive(false);
                break;
        }
    }
    
    void Reload()
    {
        Bullet1.SetActive(true);
        Bullet2.SetActive(true);
        Bullet3.SetActive(true);
        Bullet4.SetActive(true);
        Bullet5.SetActive(true);
    }
    
    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();

        floorY = transform.position.y;

        float jumpTime = Time.time + hangTime;

        while (Time.time < jumpTime)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        while(transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }

        smashing = false;
    }
}
