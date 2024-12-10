using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer enemySprite;
    [SerializeField]
    private float enemySpeed;
    public float EnemySpeed
    {
        get
        {
            return enemySpeed;
        }

        set
        {
            enemySpeed = value;
        }
    }
    [SerializeField]
    private float enemyHealth;
    public float EnemyHealth
    {
        get
        {
            return enemyHealth;
        }

        set
        {
            enemyHealth = value;
        }
    }
    [SerializeField]
    private float patrolTimer;
    private int direction = 1;
    private Vector2 position;
    private bool goDown = false;
    private HealthController healthController;
    public GameObject projectile;
    private GameManager gameManager;
    private UIHandler uiHandler;
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        patrolTimer = 3.0f;

        healthController = HealthController.healthInstance;
        gameManager = GameManager.gameManagerInstance;
        uiHandler = UIHandler.instance;

        InvokeRepeating("LaunchProjectile", Random.Range(0, 5), Random.Range(0, 5));
    }

    // Update is called once per frame
    void Update()
    {
        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0)
        {
            patrolTimer = Random.Range(patrolTimer, 8);
            direction = -direction;

            goDown = true;
        }

        if (enemyHealth < 3f && enemyHealth >= 2f)
            animator.SetInteger("Health", 2);

        else if (enemyHealth <= 2f && enemyHealth >= 1f)
            animator.SetInteger("Health", 1);

        else if (enemyHealth <= 0f)
        {
            animator.SetInteger("Health", 0);

            GetComponent<CircleCollider2D>().enabled = false;
        }



    }

    void FixedUpdate()
    {
        position = rb.position;

        position.x += enemySpeed * direction * Time.deltaTime;

        if (goDown)
        {
            position.y -= 0.5f;

            goDown = false;
        }

        if (rb.bodyType == RigidbodyType2D.Dynamic)
            rb.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bound")
        {
            patrolTimer = 0;

            if (collision.gameObject.name == "Down Bound")
            {
                Destroy(gameObject);
                gameManager.Die();
            }

        }

        else if (collision.gameObject.tag == "Player")
        {
            if (!healthController.isInvincible)
            {
                healthController.ChangeHealth(-2);
                gameManager.hittedEnemies.Add(gameObject);
                gameManager.hittedEnemiesCounting--;

                if (uiHandler != null)
                    uiHandler.UpdateFlow(gameManager.enemyFlow, gameManager.hittedEnemiesCounting);

                Destroy(gameObject);
            }
        }
    }

    public void LaunchProjectile()
    {
        if (enemyHealth < 3f && enemyHealth >= 2f)
            Instantiate(projectile, transform.position, Quaternion.identity);
    }
}
