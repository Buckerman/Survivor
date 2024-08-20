using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using QuangDM.Common;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Damage Text Settings")]
    [SerializeField] private DamageText damageTextPrefab;
    [SerializeField] private int damageTextPoolSize = 10;
    private DamageTextPool _damageTextPool;
    public static GameManager Instance { get; private set; }

    private GameTimer _gameTimer;
    private NavMeshSurface _groundSurface;
    private string wallTag = "Wall";
    private Text _defeatGame;
    private Text _waveComplete;
    private Text _waveLevel;
    private Text _surviveTime;
    private Text _timeLeft;
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
        if (_gameTimer == null)
            _gameTimer = FindObjectOfType<GameTimer>();

        if (_groundSurface == null)
            _groundSurface = FindObjectOfType<EnemySpawner>().GetComponent<NavMeshSurface>();

        if (_defeatGame == null)
            _defeatGame = GameObject.Find("GUI/DefeatBg").GetComponentInChildren<Text>();

        if (_waveComplete == null)
            _waveComplete = GameObject.Find("GUI/WaveCompleteBg").GetComponentInChildren<Text>();

        if (_waveLevel == null)
            _waveLevel = GameObject.Find("GUI/WaveLevelBg/LevelText").GetComponentInChildren<Text>();

        if (_surviveTime == null)
            _surviveTime = GameObject.Find("GUI/SurviveTimeBg").GetComponentInChildren<Text>();

        if (_timeLeft == null)
            _timeLeft = GameObject.Find("GUI/TimeLeftBg").GetComponentInChildren<Text>();

        if (_joystick == null)
            _joystick = FindObjectOfType<VariableJoystick>();

        _defeatGame.transform.parent.gameObject.SetActive(false);
        _waveComplete.transform.parent.gameObject.SetActive(false);
        _timeLeft.transform.parent.gameObject.SetActive(false);
    }

    private void InitializeGame()
    {
        Application.targetFrameRate = 60;
        BakeNavMesh();
        StartGame();

        Observer.Instance.AddObserver("WaveCompleted", WaveCompleted);
        Observer.Instance.AddObserver("TimeLeft", TimeLeft);
        Observer.Instance.AddObserver("DamageReceived", DamageReceived);
    }

    public void StartGame()
    {
        _waveLevel.text = "1";
        StartCoroutine(SurviveTime());
        _groundSurface.GetComponent<EnemySpawner>().enabled = true;
        _gameTimer.StartTimer();

        _damageTextPool = new DamageTextPool(damageTextPrefab, damageTextPoolSize);

        //PlayerData.Instance.Load();
        //PlayerData.Instance.ConversationID = 2;
        //PlayerData.Instance.Save();
    }

    private void DamageReceived(object data)
    {
        if (data == null) return;

        Transform targetTransform = null;
        DamageText damageText = null;

        if (data is ValueTuple<PlayerHealth, float> playerData)
        {
            var (playerHealth, damage) = playerData;
            targetTransform = playerHealth.transform;
            damageText = _damageTextPool.GetDamageText();
            damageText.Setup((int)damage, Color.red);
        }
        else if (data is ValueTuple<EnemyHealth, float> enemyData)
        {
            var (enemyHealth, damage) = enemyData;
            targetTransform = enemyHealth.transform;
            damageText = _damageTextPool.GetDamageText();
            damageText.Setup((int)damage, Color.white);
        }

        float randomX = UnityEngine.Random.Range(-1f, 1f);
        Vector3 offset = new Vector3(randomX, 2f, 0f);
        damageText.transform.position = targetTransform.position + offset;
        damageText.Initialize(_damageTextPool);
    }

    private void TimeLeft(object data)
    {
        _timeLeft.transform.parent.gameObject.SetActive(true);
        _timeLeft.text = $"{(int)_gameTimer.CurrentTime}s";
    }

    private void WaveCompleted(object data)
    {
        StartCoroutine(WaveDelay(2.0f));
        _waveLevel.text = data.ToString();
    }

    private IEnumerator WaveDelay(float delay)
    {
        _timeLeft.transform.parent.gameObject.SetActive(false);
        _waveComplete.transform.parent.gameObject.SetActive(true);

        _gameTimer.transform.parent.gameObject.SetActive(false);
        _waveLevel.transform.parent.gameObject.SetActive(false);

        Observer.Instance.Notify("Joy");
        _joystick.transform.parent.gameObject.SetActive(false);
        _joystick.enabled = false;

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);


        _waveComplete.transform.parent.gameObject.SetActive(false);

        _gameTimer.transform.parent.gameObject.SetActive(true);
        _waveLevel.transform.parent.gameObject.SetActive(true);
        _joystick.transform.parent.gameObject.SetActive(true);
        _joystick.enabled = true;

        StartCoroutine(SurviveTime());

        Time.timeScale = 1;
    }

    private IEnumerator SurviveTime()
    {
        _surviveTime.text = $"Survive {_gameTimer.StartingTime}s";
        _surviveTime.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        _surviveTime.transform.parent.gameObject.SetActive(false);
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

    public void EndGame()
    {
        Time.timeScale = 0;
        _gameTimer.StopTimer();
        _groundSurface.GetComponent<EnemySpawner>().enabled = false;
        _defeatGame.transform.parent.gameObject.SetActive(true);
        _joystick.transform.parent.gameObject.SetActive(false);

        StartCoroutine(ReloadSceneAfterDelay(2.0f));
    }
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
