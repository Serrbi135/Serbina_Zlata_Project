using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerScriptPlatformer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public GameObject pauseMenu;
    public GameObject deathMenu;
    [SerializeField] private CanvasGroup deathCV;

    [SerializeField] private Image[] Hearts;

    [SerializeField] private Sprite Heart_Full;
    [SerializeField] private Sprite Heart_Null;

    private bool isGround;
    private bool isLeft = true;

    private bool isPause = false;
    private bool isHurt = true;

    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    [SerializeField] private float rayDistance = 0.5f;
    private bool doubleJump = false;

    [Header("Sounds")]
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip collect;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip ui_click;
    [SerializeField] private AudioClip heart_break;
    //private AudioSource audioSource;


    [Header("Характеристики")]
    public int HP = 3;
    public float Speed = 1f;
    public float jumpForce = 4f;

    [Header("Shooting")]
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public float bulletSpeed = 10f;
    Vector3 shootDir;

    void Start()
    {
        //audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        Time.timeScale = 1;
        isPause = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        deathMenu.SetActive(false);
    }


    public void OnExit()
    {
        Loader.LoadScene(0);
    }

    public void OnClickMainMenu()
    {
        AudioManager.Instance.PlaySFX(ui_click);
        Loader.LoadScene(0);
    }

    public void OnContinueClick()
    {
        AudioManager.Instance.PlaySFX(ui_click);
        isPause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void OnClickReplay()
    {
        
        AudioManager.Instance.PlaySFX(ui_click);
        Loader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

    void Update()
    {
        isGround = Physics2D.OverlapCircle(rb.position, rayDistance, LayerMask.GetMask("Ground"));
        if(isGround )
        {
            anim.SetBool("Jump", false);
            doubleJump = false;
        }

        

        if (HP == 3)
        {
            Hearts[0].sprite = Heart_Full;
            Hearts[1].sprite = Heart_Full;
            Hearts[2].sprite = Heart_Full;
        }
        else if(HP == 2)
        {
            Hearts[0].sprite = Heart_Full;
            Hearts[1].sprite = Heart_Full;
            Hearts[2].sprite = Heart_Null;
            
        }
        else if(HP == 1)
        {
            Hearts[0].sprite = Heart_Full;
            Hearts[1].sprite = Heart_Null;
            Hearts[2].sprite = Heart_Null;
            //Hearts[1].GetComponent<Animator>().Play("heart_anim");
        }
        else if(HP <= 0)
        {
            
            isHurt = false;
            Hearts[0].sprite = Heart_Null;
            Hearts[1].sprite = Heart_Null;
            Hearts[2].sprite = Heart_Null;
            //Hearts[0].GetComponent<Animator>().Play("heart_anim");
            StartCoroutine(waitDeath());
        }
        if(isPause == false && HP > 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                if (isGround)
                {
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    anim.SetBool("Jump", true);
                }
                else if (!doubleJump && rb.velocity.y < 0)
                {
                    doubleJump = true;
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("Run", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                shootDir = Vector3.left;
                rb.AddForce(Vector3.left * Speed);
                isLeft = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("Run", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                shootDir = Vector3.right;
                rb.AddForce(Vector3.right * Speed);
                isLeft = false;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("Run", false);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        AudioManager.Instance.PlaySFX(hit);
        bulletScript.SetDirection(shootDir);
    }

    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Spikes")
        {
            AudioManager.Instance.PlaySFX(hit);
            isHurt = true;
            HP -= 1;
            StartCoroutine(waitSpikes());
        }
        else if(collision.tag == "Enemy")
        {
            AudioManager.Instance.PlaySFX(hit);
            HP -= 1;
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Spikes")
        {
            isHurt = false;
            AudioManager.Instance.StopSFX();
            StopCoroutine(waitSpikes());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "ExitCollider")
        {
            OnExit();
        }
        else if(collision.gameObject.name == "DoorCollider")
        {
            OnExit();
        }
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        AudioManager.Instance.PlaySFX(heart_break);
    }

    public IEnumerator waitSpikes()
    {
        yield return new WaitForSeconds(1f);
        if(isHurt == true)
        {
            AudioManager.Instance.PlaySFX(hit);
            HP -= 1;
            StartCoroutine(waitSpikes());
        }
    }

    public IEnumerator waitDeath()
    {
        yield return new WaitForSeconds(0.1f);
        deathMenu.SetActive(true);
        
        //AudioManager.Instance.SetSFXVolume(0);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rayDistance);

    }
}
