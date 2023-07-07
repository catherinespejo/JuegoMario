using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashController : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rb;
    private GameManager gm;
    private PlayerBehavior player;
    public SpriteRenderer sprite;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(player.GetComponent<SpriteRenderer>().flipX == true)
        {
            sprite.flipX = true;
        }
        rb.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Desaparacer el ataque si colisiona con enemigos
        if (collision.CompareTag("Enemy") )
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Debug.Log("Golpeamos al enemigo");
        }

        // Desaparecer el ataque si colisiona con paredes
        if (collision.CompareTag("MG"))
        {
            Destroy(gameObject);
        }
    }

    public void setDirection(bool dir)
    {
        sprite.flipX = dir;
        Debug.Log("Se cambio la direccion" + sprite.flipX);    
    }

    public void setSpeed(float speed)
    {
        this.speed= speed;
        Debug.Log("Se cambio la velocidad" + speed);
    }
}
