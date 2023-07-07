using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb;
    public GameObject slash;

    public float speed = 4f;
    public float jumpSpeed = 7f;
    public float runSpeed = 6f;
    public bool isRunning = false;
    public bool added = true;
    public float availableJumps = 2f;

    private SkillController skillController;

    public Transform RightWallDetector;
    public Transform LeftWallDetector;

    public Vector3 WallDetectorSize;
    public bool canWallJump = false;
    public bool isWallJumping = false;
    public bool isJumpingFromRight = false;
    public bool isJumpingFromLeft = false;
    private bool canClimb = false;
    private bool pause;
    private float initialGravity;
    private Vector3 initialPosition;
    private Vector3 checkpoint;

    public PhysicsMaterial2D WallJumpSlideMaterial;
    private SpriteRenderer spriteRenderer;

    private int health = 3;
    private float slashSpeed = 13f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        skillController = gameObject.GetComponent<SkillController>();
        initialPosition = transform.position;
        initialGravity = rb.gravityScale;
        checkpoint = initialPosition;
        pause = false;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3 && added)
        {
            Debug.Log("ADDING SKILS");
            skillController.obtainSkill(Skills.SLASH);
            skillController.obtainSkill(Skills.WALLJUMP);
            skillController.obtainSkill(Skills.DASH);
            Debug.Log(skillController.availableSkills.Count);
            added = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 4 && added)
        {
            Debug.Log("ADDING FINAL SKILS");
            skillController.obtainSkill(Skills.SLASH);
            skillController.obtainSkill(Skills.WALLJUMP);
            skillController.obtainSkill(Skills.DASH);
            Debug.Log(skillController.availableSkills);
            added = false;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().pauseGame();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().MutePressed();
        }

        if (pause) return;

        Vector2 newVelocity = rb.velocity;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (spriteRenderer != null)
            {
                // flip the sprite
                spriteRenderer.flipX = true;
            }
            newVelocity.x = -speed;
            animator.SetBool("walk", true);
            
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
            newVelocity.x = speed;
            animator.SetBool("walk", true);
        }
        else
        {
            newVelocity.x = 0;
            animator.SetBool("walk", false);
        }

        // Si presiono la barra espaciadora y tiene saltos disponibles
        // se iran removiendo uno a uno mientras su velovidad de subida
        // aumenta
        if (Input.GetKeyDown(KeyCode.Space) && availableJumps > 0)
        {
            newVelocity.y = jumpSpeed;
            removeJump();   
        }

        if(Input.GetKey(KeyCode.UpArrow) && canClimb)
        {
            newVelocity.y = jumpSpeed;
            rb.gravityScale = 0;

            animator.SetBool("isClimbing", true);
        }

        if (Input.GetKey(KeyCode.DownArrow) && canClimb)
        {
            newVelocity.y = -jumpSpeed;
            rb.gravityScale = 0;

            animator.SetBool("isClimbing", true);
        }

        rb.velocity = newVelocity;

        if (skillController.hasSkill(Skills.SLASH) && Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("slash");
            float val = spriteRenderer.flipX == true
                ? transform.position.x - 1
                : transform.position.x + 1;

            GameObject slashInstance = Instantiate(slash, new Vector3(val, transform.position.y, 0), Quaternion.identity);
            SlashController slashcon = slashInstance.GetComponent<SlashController>();

            float selectedSpeed = spriteRenderer.flipX == true
                ? -slashSpeed
                : slashSpeed;
            Debug.Log("DATA: " + val + ' ' + selectedSpeed + ' ' + spriteRenderer.flipX + ' ');
            slashcon.setDirection(spriteRenderer.flipX);
            slashcon.setSpeed(selectedSpeed);

        }

        if (skillController.hasSkill(Skills.DASH) && Input.GetKey(KeyCode.LeftShift))
        {
            skillController.dash();
            isRunning = true;
            animator.SetTrigger("run");
        } else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 4f;
        }


        // Revisar el collider izquierdo para brincar en la pared:
        Collider2D[] collidersLeft =  Physics2D.OverlapBoxAll(LeftWallDetector.position, WallDetectorSize, 0f, LayerMask.GetMask("JumpableWall"));
        
        // Revisar el collider derecho para brincar en la pared:
        Collider2D[] collidersRight = Physics2D.OverlapBoxAll(RightWallDetector.position, WallDetectorSize, 0f, LayerMask.GetMask("JumpableWall"));

        //Set can wall jump:
        if( (collidersLeft.Length > 0 || collidersRight.Length > 0) && (skillController.hasSkill(Skills.WALLJUMP) && Input.GetKey(KeyCode.Space)) )
        {
            canWallJump = true;
        } else
        {
            canWallJump = false;
        }


        if ((collidersLeft.Length > 0 || collidersRight.Length > 0) && (skillController.hasSkill(Skills.WALLJUMP)))
        {
           canWallJump = true;  
        }
        else
        {
            animator.SetBool("wallJump", false);
            canWallJump = false;
        }

        // Animate + Flip on x:
        if(collidersLeft.Length > 0 && (skillController.hasSkill(Skills.WALLJUMP)))
        {
            animator.SetBool("wallJump", true);
            animator.SetBool("walk", false);
            spriteRenderer.flipX = false;
        }

        if (collidersRight.Length > 0 && (skillController.hasSkill(Skills.WALLJUMP)))
        {
            animator.SetBool("wallJump", true);
            animator.SetBool("walk", false);
            spriteRenderer.flipX = true;
        }

        // Activar friccion:
        if (collidersLeft.Length > 0 || collidersRight.Length > 0)
        {
            rb.sharedMaterial = WallJumpSlideMaterial;
        }else
        {
            rb.sharedMaterial = null;
        }

        

        // activar salto en pared si esta disponible:
        if(!isWallJumping && canWallJump && Input.GetKeyDown(KeyCode.Space))
        {
            isWallJumping = true;
            isJumpingFromRight = collidersRight.Length > 0;
            isJumpingFromLeft = collidersLeft.Length > 0;
            Invoke("StopWallJump", 0.15f);
        }

        if(isWallJumping)
        {
            //animator.SetBool("wallJump", true);
            if (isJumpingFromRight)
            {
                rb.velocity = new Vector2(-15, 10);
            } else
            {  
                rb.velocity = new Vector2(15, 10);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            canClimb = true;
        }

        if (collision.CompareTag("Chest"))
        {
            Cofre cofre = collision.gameObject.GetComponent<Cofre>();
            if(!cofre.estaAbierto)
            {
                cofre.estaAbierto = true;
                if(!skillController.hasSkill(cofre.habilidad))
                {
                    skillController.obtainSkill(cofre.habilidad);
                }       
            }
        }

        if (collision.CompareTag("Checkpoint"))
        {
            checkpoint = collision.gameObject.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "KillingGround")
        {
            if(checkpoint != initialPosition)
            {
                // No hagas nada
            }else
            {
                transform.position = initialPosition;
            }

            transform.position = checkpoint != initialPosition 
                ? checkpoint 
                : initialPosition;
            
            // Quitar un punto de vida ya que cayo en las puas
            removeHealth();

        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Quitar un punto de vida si chocamos con el enemigo
            removeHealth();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            canClimb = false;
            animator.SetBool("isClimbing", false);
            rb.gravityScale = initialGravity;
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Para facilitar el desarrollo, vamos a vizualizar
        // el collider izquierdo y derecho
        Gizmos.color = Color.green;
        Gizmos.DrawCube(LeftWallDetector.position, WallDetectorSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(RightWallDetector.position, WallDetectorSize);
    }

    public void StopWallJump()
    {
        isWallJumping = false;
    }

    public void Pause()
    {
        this.pause = !pause;
        rb.velocity = new Vector3(0, 0, 0);
        animator.SetBool("walk", false);
    }

    public void resetJump()
    {
        availableJumps = 2f;
    }

    public void removeJump()
    {
        if(availableJumps > 0)
        {
            availableJumps -= 1f;
        }
    }

    public void removeHealth()
    {
        if(health <= 1)
        {
            //Si pierde todas las vidas, retorna al primer nivel
            SceneManager.LoadScene(1);
            return;
        }
        health -= 1;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().updateLife();
    }

    public int getHealth()
    {
        return this.health;
    }

}
