using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScriptSujet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public static PlayerScriptSujet Instance;

    private GameObject pauseMenu;
    private GameObject deathMenu;
    //[SerializeField] private GameObject jumpAnim;

    [SerializeField] private Image[] Hearts;

    [SerializeField] private Sprite Heart_Full;
    [SerializeField] private Sprite Heart_Null;

    private bool isGround;
    private bool isLeft = false;

    private bool isPause = false;
    private bool isHurt = true;

    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    [SerializeField] private float rayDistance = 0.6f;
    private bool doubleJump = false;

    [Header("Sounds")]
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip collect;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip ui_click;
    private AudioSource audioSource;

    [Header("Характеристики")]
    public float Speed = 1f;
    public float jumpForce = 4f;
    public int Energy = 0;
    private bool canMove = true;

    public GameObject currentPanel;


    void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        Time.timeScale = 1;
        isPause = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();


    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void OnExit()
    {
        

        SceneManager.LoadScene(0);
    }

    public void OnClickMainMenu()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(0);
    }

    private void OnContinueClick()
    {
        audioSource.PlayOneShot(ui_click);
        isPause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void OnClickReplay()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*public void OnClickSound()
    {
        audioSource.PlayOneShot(ui_click);
        if (audioSource.volume == 1)
        {
            GameObject.Find("Sound_Button").GetComponent<Image>().sprite = soundOff;
            audioSource.volume = 0;
        }
        else if(audioSource.volume == 0)
        {
            GameObject.Find("Sound_Button").GetComponent<Image>().sprite = soundOn;
            audioSource.volume = 1;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

        if(hit.collider != null)
        {
            isGround = true;
            doubleJump = false;
        }
        else
        {
            isGround = false;
        }

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
            {
                isPause = true;
            }
            else
            {
                isPause = false;
            }
        }*/

        
        //if(!isPause)
        //{
            if(canMove)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (isGround)
                    {
                        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        /*jumpAnim.SetActive(true);
                        jumpAnim.transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
                        jumpAnim.GetComponent<Animator>().SetTrigger("jump");*/
                    }
                    else if (!doubleJump && rb.velocity.y < 0)
                    {
                        doubleJump = true;
                        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        /*jumpAnim.SetActive(true);
                        jumpAnim.transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
                        jumpAnim.GetComponent<Animator>().SetTrigger("jump");*/
                    }
                }

                if (Input.GetKey(KeyCode.A))
                {
                    //var rot = GameObject.Find("mushroom_rot");
                    //rot.transform.localPosition = new Vector3(-0.1f, rot.transform.localPosition.y, rot.transform.localPosition.z);
                    anim.SetBool("Run", true);
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    rb.AddForce(Vector3.left * Speed);
                    isLeft = true;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    //var rot = GameObject.Find("mushroom_rot");
                    //rot.transform.localPosition = new Vector3(0.1f, rot.transform.localPosition.y, rot.transform.localPosition.z);
                    anim.SetBool("Run", true);
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    rb.AddForce(Vector3.right * Speed);
                    isLeft = false;
                }
                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
                {
                    anim.SetBool("Run", false);
                }
            }
            
        }
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.down) * 2;
        Gizmos.DrawRay(transform.position, direction);
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "ExitCollider")
        {
            OnExit();
        }
        else if(collision.gameObject.name == "SecretRoom")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.SetActive(false);
            GameObject.Find("Star_3_Light").GetComponent<Light>().enabled = true;
        }
        else if(collision.gameObject.name == "DoorCollider")
        {
            OnExit();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "nextScene")
        {
            bool isActive = !currentPanel.activeSelf;
            currentPanel.SetActive(isActive);

            Time.timeScale = isActive ? 0f : 1f;
        }
    }

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void DontLoadNext()
    {
        bool isActive = !currentPanel.activeSelf;
        currentPanel.SetActive(isActive);

        Time.timeScale = isActive ? 0f : 1f;
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            rb.velocity = Vector2.zero;
        }
    }

}
