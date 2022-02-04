using UnityEngine;

public class Item : MonoBehaviour
{
    // Class for all items to inherite
    protected Camera mainCamera;
    protected Collider2D collider;
    protected SpriteRenderer sprite;
    protected GameManager gm;
    protected BlockContorller parentBlock;
    protected Vector3 pos;

    protected float view;
    float speed = 2f;
    float height = 0.05f;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        mainCamera = gm.mainCamera;
        pos = transform.position;

    }

    protected virtual void Update()
    {
        
        view = mainCamera.orthographicSize;
        float newY = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(pos.x , pos.y + (newY * height), 0) ;
    }
    public virtual void ItemPower() {
        Invoke("Destroy", 2f);
        this.collider.enabled = false;
        this.sprite.enabled = false;
    }

    protected void SendMessageToCanvas(string s)
    {
        StartCoroutine(gm.canvas.ItemMessageOut(s));
    }

    protected virtual void Destroy()
    {
        Destroy(this);
    }
}
