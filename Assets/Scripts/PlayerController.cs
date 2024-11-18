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
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }

    void Awake()
    {
        health = 5.0f;
        speed = 5.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction.Enable();
        projectileAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {

        if (projectileAction.activeControl != null)
        {
            if (Input.GetKeyDown(projectileAction.activeControl.name))
            {
                Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 1.0f), Quaternion.identity);
            }
        }

        moveValue = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        position = rb.position + moveValue * speed * Time.deltaTime;

        rb.MovePosition(position);
    }
}
