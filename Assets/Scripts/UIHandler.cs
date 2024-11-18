using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject flow;
    public TextMeshProUGUI flowText;

    public GameObject countdown;
    public TextMeshProUGUI countdownText;
    public static UIHandler instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il y'a plus d'une instance de UI dans la scene");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        flowText = flow.GetComponent<TextMeshProUGUI>();
        countdownText = countdown.GetComponent<TextMeshProUGUI>();

        countdownText.SetText("");

        countdown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateFlow(int flow, int enemies)
    {
        flowText.SetText("Enemies: " + flow + "/" + enemies);
    }

    public void UpdateTimer(int time)
    {
        countdownText.SetText(time.ToString());
    }
}
