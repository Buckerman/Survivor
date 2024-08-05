using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private Text defeatGameText;
    [SerializeField] private Text winGameText;
    [SerializeField] private NavMeshSurface navMeshSurface;

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
        BakeNavMesh();
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        defeatGameText.gameObject.SetActive(false);
        winGameText.gameObject.SetActive(false);
        countdownTimer.StartTimer();
    }

    private void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void EndWave()
    {
        Time.timeScale = 0;
        winGameText.gameObject.SetActive(true);
        countdownTimer.StopTimer();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        defeatGameText.gameObject.SetActive(true);
        countdownTimer.StopTimer();
    }
}
