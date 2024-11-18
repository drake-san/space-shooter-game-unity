using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    [SerializeField]
    private float speed = 3.0f;
    private GameManager gameManager;
    private UIHandler uiHandler;
    private HealthController healthController;

    void Start()
    {
        gameManager = GameManager.gameManagerInstance;
        uiHandler = UIHandler.instance;
        healthController = HealthController.healthInstance;
    }

    void Update()
    {
        if (gameObject.name.Contains("Enemy"))
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.name.Contains("Enemy"))
        {
            if (collision.gameObject.tag == "Player")
            {
                healthController.ChangeHealth(-1);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                GameObject enemy = collision.gameObject;

                if (enemy.GetComponent<EnemyController>().EnemyHealth <= 0f)
                {
                    gameManager.hittedEnemies.Add(enemy);
                    gameManager.hittedEnemiesCounting--;

                    if (uiHandler != null)
                        uiHandler.UpdateFlow(gameManager.enemyFlow, gameManager.hittedEnemiesCounting);

                    Destroy(enemy);
                }
                else
                    enemy.GetComponent<EnemyController>().EnemyHealth--;

            }
        }

        Destroy(gameObject);
    }


}
