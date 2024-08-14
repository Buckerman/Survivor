using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using QuangDM.Common;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private NavMeshSurface groundSurface;
    [SerializeField] private string wallTag = "Wall";

    [SerializeField] private Text defeatGameText;
    [SerializeField] private Text winGameText;

    private List<NavMeshSurface> wallSurfaces = new List<NavMeshSurface>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignReferences();
    }

    private void AssignReferences()
    {
        if (countdownTimer == null)
            countdownTimer = FindObjectOfType<CountdownTimer>();

        if (groundSurface == null)
            groundSurface = FindObjectOfType<EnemySpawner>().GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        BakeNavMesh();
        StartGame();
        Observer.Instance.AddObserver("EnemyDisabled", EnemyDisabled);
    }

    private void EnemyDisabled(object data)
    {
        int value = (int)data;
        Debug.Log($"{value} points");
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        groundSurface.GetComponent<EnemySpawner>().enabled = true;
        countdownTimer.StartTimer();

        PlayerData.Instance.Load();
        PlayerData.Instance.ConversationID = 2;
        PlayerData.Instance.Save();
    }

    private void BakeNavMesh()
    {
        groundSurface.BuildNavMesh();

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            foreach (Transform child in building.transform)
            {
                if (child.CompareTag(wallTag))
                {
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
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndWave()
    {
        Time.timeScale = 0;
        winGameText.gameObject.SetActive(true);
        countdownTimer.StopTimer();

        StartCoroutine(ReloadSceneAfterDelay(2.0f));
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        defeatGameText.gameObject.SetActive(true);
        countdownTimer.StopTimer();

        StartCoroutine(ReloadSceneAfterDelay(2.0f));
    }
}
