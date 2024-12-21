using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private float[] cameraWidthBounds;
    public GameObject enemy;
    private List<GameObject> enemies;
    public List<GameObject> hittedEnemies;
    private int enemiesCount;
    public static GameManager gameManagerInstance;
    public PlayerController player;
    private HealthController healthController;
    [SerializeField]
    private Sprite[] shipSprites;
    public int enemyFlow;
    public int hittedEnemiesCounting;
    private bool gotoNextFlow;
    private float respawnFlowTime = 6.0f;
    private UIHandler uIHandler;
    public GameObject powerUp;

    void Awake()
    {
        if (gameManagerInstance != null)
        {
            Debug.Log("Il y'a plus d'une instance de GameManager dans la scene");
            return;
        }
        else
        {
            gameManagerInstance = this;
        }
    }

    void Start()
    {

        enemies = new List<GameObject>();

        cameraWidthBounds = new float[] { -10.7f, 10.7f };

        InvokeRepeating("SpawnEnemy", 3.0f, 1.0f);

        for (int i = 0; i < 10; i++)
        {
            enemies.Add(enemy);
        }

        enemiesCount = enemies.Count;
        hittedEnemiesCounting = enemies.Count;

        healthController = HealthController.healthInstance;

        uIHandler = UIHandler.instance;

        uIHandler.flowText.text += enemyFlow + "/" + hittedEnemiesCounting;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyFlow <= 0)
            if (uIHandler != null)
                uIHandler.flowText.SetText("You won");

        if (player != null)
        {
            if (healthController.playerHealth <= 0f)
            {
                Die();
            }

            if (Input.GetKeyDown(KeyCode.F))
                healthController.ChangeHealth(-1);

            if (healthController.playerHealth > 0 && healthController.playerHealth <= 1)
            {
                player.gameObject.GetComponent<SpriteRenderer>().sprite = shipSprites[2];

                player.Speed = 2.0f;
            }


            else if (healthController.playerHealth > 1 && healthController.playerHealth < 3)
            {
                player.gameObject.GetComponent<SpriteRenderer>().sprite = shipSprites[1];

                player.Speed = 3.0f;
            }


            if (healthController.playerHealth >= 3 && healthController.playerHealth < 5)
            {
                player.gameObject.GetComponent<SpriteRenderer>().sprite = shipSprites[0];
            }
        }

        if (gotoNextFlow)
        {
            respawnFlowTime -= Time.deltaTime;

            uIHandler.UpdateTimer((int)respawnFlowTime);
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }


    void OnApplicationQuit()
    {
        enemy.GetComponent<EnemyController>().EnemySpeed = 3.0f;
        enemy.GetComponent<EnemyController>().EnemyHealth = 3.0f;

    }

    public void SpawnEnemy()
    {
        if (enemyFlow > 0)
        {
            if (enemies.Count > 0)
            {
                Instantiate(enemy, new Vector2(cameraWidthBounds[Random.Range(0, 2)], transform.position.y), Quaternion.identity);

                enemies.Remove(enemy);
            }
            else
            {
                if (hittedEnemies.Count == enemiesCount)
                {
                    enemyFlow--;

                    if (enemyFlow > 0)
                    {
                        hittedEnemies.Clear();

                        enemiesCount += 5;
                        hittedEnemiesCounting = enemiesCount;

                        enemy.GetComponent<EnemyController>().EnemySpeed++;
                        enemy.GetComponent<EnemyController>().EnemyHealth++;

                        uIHandler.countdown.SetActive(true);

                        gotoNextFlow = true;
                    }
                    else
                    {
                        return;
                    }
                }

                if (respawnFlowTime <= 0f)
                {

                    if (enemyFlow > 0)
                    {
                        for (int i = 0; i < enemiesCount; i++)
                        {
                            enemies.Add(enemy);
                        }

                        if (uIHandler != null)
                            uIHandler.UpdateFlow(enemyFlow, hittedEnemiesCounting);

                    }


                    respawnFlowTime = 6.0f;

                    player.SpawnRocketInterval = Mathf.Clamp(player.SpawnRocketInterval - 0.1f, 0f, 0.5f);

                    gotoNextFlow = false;

                    Instantiate(powerUp);

                    uIHandler.countdown.SetActive(false);
                }
            }
        }

    }

    public void Die()
    {
        uIHandler.flowText.SetText("Game Over");
        Destroy(player.gameObject);
        gameObject.SetActive(false);
    }
}
