using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    public Image potionIndicator1;
    public Image potionIndicator2;
    public Image potionIndicator3;
    public Sprite potionIndicationSpeed;
    public Sprite potionIndicationJumpBoost;
    public Sprite potionIndicationRegeneration;
    public Transform cosmeticHat;

    private bool facingRight = true;
    private bool facingLeft = false;
    private bool onGround = false;
    private bool walkAnimationRunning = false;
    private GameManager gameManager;
    private bool jumpForceAdded = false;
    private bool fallAnimationRunning = false;
    private bool itemTaken = false;
    private bool onLadder = false;
    private DiscordManagement discordManagement;
    private bool nextLevelTriggered = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        discordManagement = FindObjectOfType<DiscordManagement>();

        StartCoroutine(CheckGround());
        StartCoroutine(CheckHeight());
    }

    private void Update()
    {
        if (PauseController.paused || nextLevelTriggered)
            return;

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
    }

    private void FixedUpdate()
    {
        if (PauseController.paused || nextLevelTriggered)
            return;

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

    private IEnumerator CheckGround()
    {
        while (true)
        {
            onGround = false;
            Physics2D.OverlapCircleAll(groundCheckTransform.position, 0.0005f, groundCheckLayer).ToList().ForEach(collider =>
            {
                if (collider.gameObject != gameObject)
                    onGround = true;
            });
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator CheckHeight()
    {
        while (true)
        {
            if (transform.position.y < fallHeight)
            {
                if (!fallAnimationRunning)
                {
                    fallAnimationRunning = true;
                    animator.SetTrigger("WalkEnd");
                    animator.SetTrigger("FallStart");
                    cosmeticHat.parent = null;
                }
            }
            if (transform.position.y < deathHeight)
                StartCoroutine(gameManager.RequestRespawn());
            yield return new WaitForSeconds(0.2f);
        }
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
                nextLevelTriggered = true;
                animator.SetTrigger("WalkEnd");
                StartCoroutine(gameManager.RequestNextLevel());
                break;
            case "Potion_Speed":
                moveLeft *= PotionManager.MULTIPLIER_SPEED;
                moveRight *= PotionManager.MULTIPLIER_SPEED;
                Destroy(collision.gameObject);
                Invoke("ClearSpeedEffect", PotionManager.TIME_SPEED);
                ApplyPotionIndicator(potionIndicationSpeed, PotionManager.TIME_SPEED);
                discordManagement.ApplyPresence(PotionManager.TEXTURE_SPEED, PotionManager.TEXT_SPEED);
                StartCoroutine(ClearDiscordPresence(PotionManager.TIME_SPEED));
                break;
            case "Potion_JumpBoost":
                jumpVector *= PotionManager.MULTIPLIER_JUMPBOOST;
                Destroy(collision.gameObject);
                Invoke("ClearJumpBoostEffect", PotionManager.TIME_JUMPBOOST);
                ApplyPotionIndicator(potionIndicationJumpBoost, PotionManager.TIME_JUMPBOOST);
                discordManagement.ApplyPresence(PotionManager.TEXTURE_JUMPBOOST, PotionManager.TEXT_JUMPBOOST);
                StartCoroutine(ClearDiscordPresence(PotionManager.TIME_JUMPBOOST));
                break;
            case "Potion_Regeneration":
                Destroy(collision.gameObject);
                Invoke("ClearRegenerationEffect", PotionManager.TIME_REGENERATION);
                ApplyPotionIndicator(potionIndicationRegeneration, PotionManager.TIME_REGENERATION);
                discordManagement.ApplyPresence(PotionManager.TEXTURE_REGENERATION, PotionManager.TEXT_REGENERATION);
                StartCoroutine(ClearDiscordPresence(PotionManager.TIME_REGENERATION));
                break;
            case "Item_Coin":
                Destroy(collision.gameObject);
                CoinManagement.AddCoins(1);
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
    private IEnumerator ClearPotionIndicator(int indicatorIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        switch (indicatorIndex)
        {
            case 1:
                potionIndicator1.sprite = null;
                potionIndicator1.color = new Color(0, 0, 0, 0);
                break;
            case 2:
                potionIndicator2.sprite = null;
                potionIndicator2.color = new Color(0, 0, 0, 0);
                break;
            case 3:
                potionIndicator3.sprite = null;
                potionIndicator3.color = new Color(0, 0, 0, 0);
                break;
        }
    }
    private void ApplyPotionIndicator(Sprite potionSprite, float time)
    {
        if (potionIndicator1.sprite == null)
        {
            potionIndicator1.sprite = potionSprite;
            potionIndicator1.color = new Color(255, 255, 255, 200);
            StartCoroutine(ClearPotionIndicator(1, time));
        } else if (potionIndicator2.sprite == null)
        {
            potionIndicator2.sprite = potionSprite;
            potionIndicator2.color = new Color(255, 255, 255, 200);
            StartCoroutine(ClearPotionIndicator(2, time));
        } else
        {
            potionIndicator3.sprite = potionSprite;
            potionIndicator3.color = new Color(255, 255, 255, 200);
            StartCoroutine(ClearPotionIndicator(3, time));
        }
    }
    private void ClearItemTaken()
    {
        itemTaken = false;
    }
    private IEnumerator ClearDiscordPresence(float delay)
    {
        yield return new WaitForSeconds(delay);
        discordManagement.ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
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
