using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoEnemigo : MonoBehaviour
{
    // La velocidad a la que se mueve el enemigo
    public float xSpeed = 1.5f;
    // Cantidad de danio que hace el enemigo, medido en frames
    public float damage = 5f;
    // La cantidad de knockback cuando se choca con el jugador
    public float knockback = 100f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       // audioSource = GetComponent<AudioSource>();
       // boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = xSpeed < 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null)
        {
            Vector2 newVelocity = rb.velocity;
            newVelocity.x = xSpeed;
            rb.velocity = newVelocity;
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Encontrar el objeto a colisionar usando su Tag
        GameObject colObject = col.gameObject;
        string tag = colObject.tag;
        // Invierte la velocidad si chocamos con un obstaculo o pared
        if (colObject.gameObject.CompareTag("KillingGround") || colObject.gameObject.CompareTag("Wall") || colObject.gameObject.CompareTag("Enemy"))
        {
            xSpeed = -xSpeed;
        }
        
        else if (colObject.CompareTag("Player") && gameObject.activeSelf)
        {
           // CharacterController2D playerController = colObject.GetComponent<CharacterController2D>();
            //playerHealth.remainingLifeTime -= damage;
            // Golpea al jugador hacia atras para que no se quede colisionando infinitamente
            //StartCoroutine(playerController.PerformKnockback(new Vector2(xSpeed > 0 ? knockback : -knockback, knockback)));
            xSpeed = -xSpeed;
        }
    }
}
