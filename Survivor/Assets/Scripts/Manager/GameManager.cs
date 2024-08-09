using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using QuangDM.Common;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private Text defeatGameText;
    [SerializeField] private Text winGameText;
    [SerializeField] private NavMeshSurface groundSurface;
    [SerializeField] private string wallTag = "Wall";

    private List<NavMeshSurface> wallSurfaces = new List<NavMeshSurface>();

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
        Application.targetFrameRate = 60;
        BakeNavMesh();
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        defeatGameText.gameObject.SetActive(false);
        winGameText.gameObject.SetActive(false);
        groundSurface.GetComponent<EnemySpawner>().enabled = true;
        countdownTimer.StartTimer();

        PlayerData.Instance.Load();
        PlayerData.Instance.ConversationID = 2;
        PlayerData.Instance.Save();
    }

    private void BakeNavMesh()
    {
        groundSurface.BuildNavMesh();

        //Find all buildings
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            // Find wall objects that are children of the building
            foreach (Transform child in building.transform)
            {
                if (child.CompareTag(wallTag))
                {
                    // Add NavMeshSurface to the wall if it doesn't have one already
                    NavMeshSurface surface = child.gameObject.GetComponent<NavMeshSurface>();
                    if (surface == null)
                    {
                        surface = child.gameObject.AddComponent<NavMeshSurface>();
                    }

                    wallSurfaces.Add(surface);
                    surface.BuildNavMesh();
                }
            }
        }
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
