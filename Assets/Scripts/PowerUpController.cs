using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField]
    private PowerUpObject[] powerUpObjects;

    public PowerUpObject powerUp;
    [SerializeField]
    private int speed = 3;
    private Vector2 moveDirection;
    private PlayerController playerController;

    void Start()
    {
        int i = Random.Range(0, powerUpObjects.Length);

        powerUp = powerUpObjects[i];

        GetComponent<SpriteRenderer>().sprite = powerUp.sprite;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        ChangeDirection();

    }

    void Update()
    {

        transform.Translate(moveDirection * speed * Time.deltaTime);


        Vector2 position = Camera.main.WorldToViewportPoint(transform.position);

        if (position.x < 0 || position.x > 1)
            moveDirection.x = -moveDirection.x;

        if (position.y < 0 || position.y > 1)
            moveDirection.y = -moveDirection.y;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerController.PowerUp = true;
            Destroy(gameObject);
        }

    }

    public void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);

        moveDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
    }

}
