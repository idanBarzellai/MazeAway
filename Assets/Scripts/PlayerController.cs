using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float baseSpeed = 0.7f;
    private float speed;
    private bool hasKey = false;
    private bool freezeMovement = true;
    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gameManager;
    private TrailRenderer trailEffect;
    private AudioSource itemAudio;
    public Joystick joystick;
    public GameObject mapObject;

    public GameObject footsteps;
    //private List<GameObject> footstepsActive;
    float lastTimeCreatedFootSteps = 0;
    float creatingFootstepTime = 0.2f;

    void Start()
    {
        speed = baseSpeed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //trailEffect = GetComponentInChildren<TrailRenderer>();
        // Assign the `gameManager` variable by using the static reference
        gameManager = GameManager.Instance; 
        itemAudio = GetComponent<AudioSource>();
        //footstepsActive = new List<GameObject>();
        //mapObject = GameObject.FindGameObjectWithTag("map");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapObject.SetActive(mapObject.activeInHierarchy ? false : true);

        }
    }
    void FixedUpdate()
    {
        if (!freezeMovement)
        {
            float inputX = joystick.Horizontal;
            float inputY = joystick.Vertical;
            float inputXKeyboard = Input.GetAxisRaw("Horizontal"); // when playing on PC
            float inputYKeyboard = Input.GetAxisRaw("Vertical"); // when playing on PC
            if (Mathf.Abs(inputX) > 0 || Mathf.Abs(inputY) > 0)
            {
                Vector2 velocity = new Vector2(inputX * Time.fixedDeltaTime * speed, inputY * Time.fixedDeltaTime * speed);
                rb.MovePosition(rb.position + velocity);
            }
            else
            {
                Vector2 velocityPC = new Vector2(inputXKeyboard * Time.fixedDeltaTime * speed, inputYKeyboard * Time.fixedDeltaTime * speed);
                rb.MovePosition(rb.position + velocityPC);
            }
            changePlayerDirection(inputX);
            changePlayerDirection(inputXKeyboard); // when playing on PC
            anim.SetBool("walking", Mathf.Abs(inputX) + Mathf.Abs(inputY) > 0 || Mathf.Abs(inputXKeyboard) + Mathf.Abs(inputYKeyboard) > 0); // when playing on PC (second cndition)


            // Creating footsteps if moving 
            if((Mathf.Abs(inputX + inputXKeyboard) > 0.01f || Mathf.Abs(inputY + inputYKeyboard) > 0.01f) && Time.time - lastTimeCreatedFootSteps > creatingFootstepTime)
            {
                lastTimeCreatedFootSteps = Time.time;
                GameObject footstepInstace = Instantiate(footsteps, transform.position, transform.rotation);
                RotateFootsteps(inputX + inputXKeyboard, inputY + inputYKeyboard, footstepInstace);
                //footstepsActive.Add(footstepInstace);
                StartCoroutine(DestoryFootstep(footstepInstace));
            }


        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FinishLine")
        {
            if (hasKey)
            {
                gameManager.WinLevel();
                freezeMovement = true;
            }
            else
                StartCoroutine(gameManager.canvas.MessageOut("GET THE KEY!"));
        }
        // Collison for all types of objects
        else if (collision.gameObject.GetComponent<Item>() != null)
        {
            if (collision.gameObject.tag == "maginifing_glass")
                collision.gameObject.GetComponent<MagnifyingGlass>().ItemPower();
            if (collision.gameObject.tag == "clock_item")
                collision.gameObject.GetComponent<ClockItem>().ItemPower();
            if (collision.gameObject.tag == "key_item")
                collision.gameObject.GetComponent<KeyItem>().ItemPower();
            if (collision.gameObject.tag == "coin_item")
                collision.gameObject.GetComponent<CoinItem>().ItemPower();
            itemAudio.Play();
            
        }
    }

    
    public void LostLevel()
    {
        freezeMovement = true;
        anim.SetBool("lost", freezeMovement);
        //footstepsActive.ForEach(x => { Destroy(x); });
    }

    public void StartMovement()
    {
        freezeMovement = false;

        speed = baseSpeed;
        anim.SetBool("lost", freezeMovement);
    }

    void changePlayerDirection(float inputX)
    {
        Vector3 currLocalScale = transform.localScale;
        if (inputX < 0)
        {
            currLocalScale.x = -1;
            transform.localScale = currLocalScale;
        }
        else if (inputX > 0)
        {
            currLocalScale.x = 1;
            transform.localScale = currLocalScale;
        }
    }

    public void GotKey(bool gotit)
    {
        hasKey = gotit;
    }

    private void RotateFootsteps(float inputX, float inputY, GameObject footstep)
    {
        float whereTo = (inputX  > 0 && inputY > 0) ? 315 : 
            (inputX > 0 && inputY == 0) ? 270 :
            (inputX > 0 && inputY < 0) ? 235 :
            (inputX == 0 && inputY < 0) ? 180 :
            (inputX < 0 && inputY < 0) ? 135 :
            (inputX < 0 && inputY == 0) ? 90 :
            (inputX < 0 && inputY > 0) ? 45 : 0;
        footstep.transform.Rotate(0, 0, whereTo);
    }

    IEnumerator DestoryFootstep(GameObject footstep)
    {
        if (footstep != null)
        {
            float alpha = 1;

            while (footstep.GetComponent<SpriteRenderer>().color.a > 0)
            {
                alpha -= 0.007f;
                footstep.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
                yield return new WaitForSecondsRealtime(0.05f);

            }
            if (footstep != null)
                Destroy(footstep);
        }
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public bool GetFreezeMovement()
    {
        return freezeMovement;
    }
}