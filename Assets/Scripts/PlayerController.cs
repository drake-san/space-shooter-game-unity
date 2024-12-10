using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField]
    private float speed;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }
    public InputAction moveAction;
    private Vector2 moveValue;
    private Vector2 position;
    public GameObject projectile;
    public InputAction projectileAction;
    [SerializeField]
    private bool hasPowerUp;
    public bool PowerUp
    {
        get
        {
            return hasPowerUp;
        }

        set
        {
            hasPowerUp = value;
        }
    }

    [SerializeField]
    private float powerUpTime;
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }

    [SerializeField]
    private PowerUpController powerUpController;
    private HealthController healthController;
    [SerializeField]
    private GameObject shieldPrefab;
    private GameObject shield;
    private bool isShieldInstantiated;
    [SerializeField]
    private float rocketLaunchInterval;

    void Awake()
    {
        health = 5.0f;
        speed = 5.0f;

        healthController = HealthController.healthInstance;
    }

    // Start is called before the first frame update
    void Start()
    {

        hasPowerUp = false;
        isShieldInstantiated = false;
        powerUpController = null;

        rb = GetComponent<Rigidbody2D>();

        moveAction.Enable();
        projectileAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (rocketLaunchInterval > 0f)
            rocketLaunchInterval -= Time.deltaTime;

        if (projectileAction.activeControl != null)
        {
            if (Input.GetKeyDown(projectileAction.activeControl.name))
            {
                if (rocketLaunchInterval <= 0f)
                {
                    Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 1.0f), Quaternion.identity);
                    rocketLaunchInterval = 0.5f;
                }

                if (hasPowerUp)
                {
                    if (powerUpController.powerUp.type == PowerUpObject.Type.ROCKET)
                    {
                        Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 1.8f), Quaternion.identity);
                    }


                    else if (powerUpController.powerUp.type == PowerUpObject.Type.LASER)
                    {
                        Debug.Log("Laser picked");
                    }

                }
            }
        }

        moveValue = moveAction.ReadValue<Vector2>();

        if (hasPowerUp)
        {

            powerUpTime -= Time.deltaTime;

            if (powerUpController.powerUp.type == PowerUpObject.Type.SHIELD)
            {
                if (!isShieldInstantiated)
                {
                    shield = Instantiate(shieldPrefab);

                    shield.transform.SetParent(gameObject.transform);
                    shield.transform.localPosition = Vector2.zero;


                    isShieldInstantiated = true;
                    healthController.isInvincible = true;
                }

            }

        }

        if (powerUpTime <= 0f)
        {
            hasPowerUp = false;

            if (isShieldInstantiated)
            {
                isShieldInstantiated = false;
                healthController.isInvincible = false;

                Destroy(shield);
            }
        }
    }

    void FixedUpdate()
    {
        position = rb.position + moveValue * speed * Time.deltaTime;

        rb.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        powerUpController = other.gameObject.GetComponent<PowerUpController>();
        powerUpTime = powerUpController.powerUp.duration;
    }
}
