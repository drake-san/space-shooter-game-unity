using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthController : MonoBehaviour
{
    public PlayerController player;
    public float playerHealth;
    private int maxHealth = 5;
    private UIDocument uIDocument;
    private VisualElement healthBar;
    public static HealthController healthInstance;
    public bool isInvincible;
    public float invincibleTimer;

    void Awake()
    {
        if (healthInstance != null)
        {
            Debug.Log("Il y'a plus d'une instance de HealthController dans la scene");

            return;
        }

        else
        {
            healthInstance = this;
        }


    }

    // Start is called before the first frame update
    void Start()
    {

        uIDocument = GetComponent<UIDocument>();

        healthBar = uIDocument.rootVisualElement.Q<VisualElement>("Health");


        playerHealth = player.Health;

        isInvincible = false;
        invincibleTimer = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
        }

        if (invincibleTimer <= 0f)
        {
            isInvincible = false;
            invincibleTimer = 5.0f;
        }
    }

    public void ChangeHealth(float amount)
    {
        if (!isInvincible)
        {
            isInvincible = true;

            playerHealth = Math.Clamp(playerHealth + amount, 0, maxHealth);
            healthBar.style.width = Length.Percent(playerHealth / maxHealth * 100.0f);


        }

    }
}
