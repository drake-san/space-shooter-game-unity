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
    private Sprite[] enemySprites;
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
        animator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();

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

        if (enemyHealth < 3 && enemyHealth >= 2)
        {
            Debug.Log(enemySprite.sprite.name);
            enemySprite.sprite = enemySprites[0];
        }


        else if (enemyHealth <= 2 && enemyHealth >= 1)
            enemySprite.sprite = enemySprites[1];

        if (enemyHealth <= 0)
        {
            animator.SetFloat("Health", 0);
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
            healthController.ChangeHealth(-2);
            gameManager.hittedEnemies.Add(gameObject);
            gameManager.hittedEnemiesCounting--;

            if (uiHandler != null)
                uiHandler.UpdateFlow(gameManager.enemyFlow, gameManager.hittedEnemiesCounting);

            Destroy(gameObject);
        }
    }

    public void LaunchProjectile()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
    }
}
