using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private Text endGameText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        endGameText.gameObject.SetActive(false);
        countdownTimer.StartTimer();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        endGameText.gameObject.SetActive(true);
        countdownTimer.StopTimer();
    }
}
