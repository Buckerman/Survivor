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

    private CountdownTimer _countdownTimer;
    private NavMeshSurface _groundSurface;
    private string wallTag = "Wall";
    private Text _defeatGameText;
    private Text _winGameText;
    private VariableJoystick _joystick;

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
        InitializeGame();
    }

    private void AssignReferences()
    {
        if (_countdownTimer == null)
            _countdownTimer = FindObjectOfType<CountdownTimer>();

        if (_groundSurface == null)
            _groundSurface = FindObjectOfType<EnemySpawner>().GetComponent<NavMeshSurface>();

        if (_defeatGameText == null)
            _defeatGameText = GameObject.Find("GUI/DefeatBg").GetComponentInChildren<Text>();

        if (_winGameText == null)
            _winGameText = GameObject.Find("GUI/WaveCompleteBg").GetComponentInChildren<Text>();

        if (_joystick == null)
            _joystick = FindObjectOfType<VariableJoystick>();

        _joystick.enabled = true;
        _defeatGameText.transform.parent.gameObject.SetActive(false);
        _winGameText.transform.parent.gameObject.SetActive(false);
    }

    private void InitializeGame()
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
        _groundSurface.GetComponent<EnemySpawner>().enabled = true;
        _countdownTimer.StartTimer();

        PlayerData.Instance.Load();
        PlayerData.Instance.ConversationID = 2;
        PlayerData.Instance.Save();
    }

    private void BakeNavMesh()
    {
        _groundSurface.BuildNavMesh();

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
        _winGameText.transform.parent.gameObject.SetActive(true);
        _countdownTimer.StopTimer();
        _joystick.enabled = false;

        StartCoroutine(ReloadSceneAfterDelay(2.0f));
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        _defeatGameText.transform.parent.gameObject.SetActive(true);
        _countdownTimer.StopTimer();
        _joystick.enabled = false;

        StartCoroutine(ReloadSceneAfterDelay(2.0f));
    }
}
