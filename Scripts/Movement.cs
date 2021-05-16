using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    public Vector3 moveLeft = new Vector3(-1f, 0f, 0f);
    public Vector3 moveRight = new Vector3(1f, 0f, 0f);
    public Vector3 jumpVector = new Vector3(0f, 1f, 0f);
    public float jumpControl = 0.75f;
    public float deathHeight = -40f;
    public float fallHeight = -8f;
    public Vector3 ladderClimbingVector = new Vector3(0f, 1f, 0f);
    public float climbingSpeed = 3f;

    public Transform groundCheckTransform;
    public LayerMask groundCheckLayer;
    public Animator animator;
    public Transform cameraTransform;
    public Image potionIndicator1;
    public Image potionIndicator2;
    public Image potionIndicator3;
    public Sprite potionIndicationSpeed;
    public Sprite potionIndicationJumpBoost;
    public Sprite potionIndicationRegeneration;

    private bool facingRight = true; // player is facing right at the start
    private bool facingLeft = false;
    private bool onGround = false; // player spawn in the air
    private bool walkAnimationRunning = false; // player isn't walking at the start of the game
    private GameManager gameManager;
    private bool jumpForceAdded = false;
    private bool fallAnimationRunning = false;
    private bool itemTaken = false;
    private bool onLadder = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (onGround)
            {
                transform.position += moveLeft * Time.deltaTime;
            } else
            {
                transform.position += moveLeft * Time.deltaTime * jumpControl;
            }
            if (facingRight)
            {
                Flip();
                facingRight = false;
                facingLeft = true;
            }
            if (!walkAnimationRunning && onGround)
            {
                walkAnimationRunning = true;
                animator.SetTrigger("WalkStart");
            }
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (onGround)
            {
                transform.position += moveRight * Time.deltaTime;
            }
            else
            {
                transform.position += moveRight * Time.deltaTime * jumpControl;
            }
            if (facingLeft)
            {
                Flip();
                facingLeft = false;
                facingRight = true;
            }
            if (!walkAnimationRunning && onGround)
            {
                walkAnimationRunning = true;
                animator.SetTrigger("WalkStart");
            }
        } else
        {
            if (walkAnimationRunning)
            {
                walkAnimationRunning = false;
                animator.SetTrigger("WalkEnd");
            }
        }
        cameraTransform.position = transform.position;
    }

    private void FixedUpdate()
    {
        CheckHeight();
        CheckGround();
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && onGround && !jumpForceAdded && !onLadder)
        {
            transform.GetComponent<Rigidbody2D>().AddForce(jumpVector);
            onGround = false;
            jumpForceAdded = true;
            Invoke("ClearJumpForceAdded", 0.5f);
        }
    }
    private void ClearJumpForceAdded()
    {
        jumpForceAdded = false;
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void CheckGround()
    {
        onGround = false;
        Physics2D.OverlapCircleAll(groundCheckTransform.position, 0.0005f, groundCheckLayer).ToList().ForEach(collider =>
        {
            if (collider.gameObject != gameObject)
                onGround = true;
        });
    }
    private void CheckHeight()
    {
        if (transform.position.y < fallHeight)
        {
            if (!fallAnimationRunning)
            {
                fallAnimationRunning = true;
                animator.SetTrigger("WalkEnd");
                animator.SetTrigger("FallStart");
            }
        }
        if (transform.position.y < deathHeight)
            gameManager.RequestRespawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (itemTaken)
        {
            return;
        }
        itemTaken = true;
        Invoke("ClearItemTaken", 0.1f);
        switch (collision.tag)
        {
            case "LevelTrigger":
                gameManager.RequestNextLevel();
                break;
            case "Potion_Speed":
                moveLeft *= PotionManager.MULTIPLIER_SPEED;
                moveRight *= PotionManager.MULTIPLIER_SPEED;
                Destroy(collision.gameObject);
                Invoke("ClearSpeedEffect", PotionManager.TIME_SPEED);
                ApplyPotionIndicator(potionIndicationSpeed, PotionManager.TIME_SPEED);
                break;
            case "Potion_JumpBoost":
                jumpVector *= PotionManager.MULTIPLIER_JUMPBOOST;
                Destroy(collision.gameObject);
                Invoke("ClearJumpBoostEffect", PotionManager.TIME_JUMPBOOST);
                ApplyPotionIndicator(potionIndicationJumpBoost, PotionManager.TIME_JUMPBOOST);
                break;
            case "Potion_Regeneration":
                Destroy(collision.gameObject);
                Invoke("ClearRegenerationEffect", PotionManager.TIME_REGENERATION);
                ApplyPotionIndicator(potionIndicationRegeneration, PotionManager.TIME_REGENERATION);
                break;
            default:
                break;
        }
    }
    private void ClearSpeedEffect()
    {
        moveLeft /= PotionManager.MULTIPLIER_SPEED;
        moveRight /= PotionManager.MULTIPLIER_SPEED;
    }
    private void ClearJumpBoostEffect()
    {
        jumpVector /= PotionManager.MULTIPLIER_JUMPBOOST;
    }
    private void ClearPotionIndicator1()
    {
        potionIndicator1.sprite = null;
        potionIndicator1.color = new Color(0, 0, 0, 0);
    }
    private void ClearPotionIndicator2()
    {
        potionIndicator2.sprite = null;
        potionIndicator2.color = new Color(0, 0, 0, 0);
    }
    private void ClearPotionIndicator3()
    {
        potionIndicator3.sprite = null;
        potionIndicator3.color = new Color(0, 0, 0, 0);
    }
    private void ApplyPotionIndicator(Sprite potionSprite, float time)
    {
        if (potionIndicator1.sprite == null)
        {
            potionIndicator1.sprite = potionSprite;
            potionIndicator1.color = new Color(255, 255, 255, 200);
            Invoke("ClearPotionIndicator1", time);
        } else if (potionIndicator2.sprite == null)
        {
            potionIndicator2.sprite = potionSprite;
            potionIndicator2.color = new Color(255, 255, 255, 200);
            Invoke("ClearPotionIndicator2", time);
        } else
        {
            potionIndicator3.sprite = potionSprite;
            potionIndicator3.color = new Color(255, 255, 255, 200);
            Invoke("ClearPotionIndicator3", time);
        }
    }
    private void ClearItemTaken()
    {
        itemTaken = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Ladder"))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, climbingSpeed);
            } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -climbingSpeed);
            }
            onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Ladder"))
        {
            onLadder = false;
        }
    }

}
