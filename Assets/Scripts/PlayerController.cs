using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int speed = 1;
    private bool hasKey = false;
    bool freezeMovement = false;
    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gameManager;
    private TrailRenderer trailEffect;
    private AudioSource itemAudio;
    public Joystick joystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trailEffect = GetComponentInChildren<TrailRenderer>();
        // Assign the `gameManager` variable by using the static reference
        gameManager = GameManager.Instance; 
        itemAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!freezeMovement)
        {
            float inputX = joystick.Horizontal;
            float inputY = joystick.Vertical;
            Vector2 velocity = new Vector2(inputX * Time.fixedDeltaTime * speed, inputY * Time.fixedDeltaTime * speed);
            rb.MovePosition(rb.position + velocity);

            changePlayerDirection(inputX);
            anim.SetBool("walking", Mathf.Abs(inputX) + Mathf.Abs(inputY) > 0);
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
        trailEffect.Clear();
        trailEffect.emitting = false;
    }

    public void StartMovement()
    {
        freezeMovement = false;
        trailEffect.Clear();
        trailEffect.emitting = true;
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
}