using Unity.AI.Navigation;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using QuangDM.Common;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Damage Text Settings")]
    [SerializeField] private DamageText damageTextPrefab;
    [SerializeField] private int damageTextPoolSize = 10;
    private DamageTextPool _damageTextPool;

    [Header("Loot Drop Settings")]
    [SerializeField] private List<Loot> lootPrefabs;
    [SerializeField] private int lootPoolSize = 10;
    private LootPool _lootPool;

    [Header("Blood Splash Settings")]
    [SerializeField] private BloodSplash bloodSplashPrefab;
    [SerializeField] private int bloodSplashPoolSize = 10;
    private BloodSplashPool _bloodSplashPool;

    public static GameManager Instance { get; private set; }

    private GameTimer _gameTimer;
    private NavMeshSurface _groundSurface;
    private TextMeshProUGUI _defeatGame;
    private TextMeshProUGUI _waveComplete;
    private TextMeshProUGUI _waveLevel;
    private TextMeshProUGUI _surviveTime;
    private TextMeshProUGUI _timeLeft;
    private TextMeshProUGUI _coinAmount;
    private VariableJoystick _joystick;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

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
        SetupCameraFollow();
    }
    private void AssignReferences()
    {
        if (_gameTimer == null)
            _gameTimer = FindObjectOfType<GameTimer>();

        if (_groundSurface == null)
            _groundSurface = FindObjectOfType<EnemySpawner>().GetComponent<NavMeshSurface>();

        if (_defeatGame == null)
            _defeatGame = GameObject.Find("GUI/DefeatBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_waveComplete == null)
            _waveComplete = GameObject.Find("GUI/WaveCompleteBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_waveLevel == null)
            _waveLevel = GameObject.Find("GUI/WaveLevelBg/LevelText").GetComponentInChildren<TextMeshProUGUI>();

        if (_surviveTime == null)
            _surviveTime = GameObject.Find("GUI/SurviveTimeBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_timeLeft == null)
            _timeLeft = GameObject.Find("GUI/TimeLeftBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_coinAmount == null)
            _coinAmount = GameObject.Find("GUI/CoinAmountBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_joystick == null)
            _joystick = FindObjectOfType<VariableJoystick>();

        if (_cinemachineVirtualCamera == null)
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        _defeatGame.transform.parent.gameObject.SetActive(false);
        _waveComplete.transform.parent.gameObject.SetActive(false);
        _timeLeft.transform.parent.gameObject.SetActive(false);

        Player.Instance.GetComponent<PlayerController>().Joystick = _joystick;
    }
    private void InitializeGame()
    {
        Application.targetFrameRate = 60;
        //BakeNavMesh();
        StartGame();

        Observer.Instance.AddObserver(EventName.WaveCompleted, WaveCompleted);
        Observer.Instance.AddObserver(EventName.TimeLeft, TimeLeft);
        Observer.Instance.AddObserver(EventName.DamageReceived, DamageReceived);
        Observer.Instance.AddObserver(EventName.ReactivatePlatform, ReactivatePlatform);
        Observer.Instance.AddObserver(EventName.DropLoot, DropLoot);
        Observer.Instance.AddObserver(EventName.BloodSpawn, BloodSpawn);
        Observer.Instance.AddObserver(EventName.UpdateWalletUI, UpdateWalletUI);
    }
    public void StartGame()
    {
        _waveLevel.text = "1";
        _coinAmount.text = "0";

        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
            playerHealth.HealthBar.gameObject.SetActive(true);
        }

        StartCoroutine(SurviveTime());
        _groundSurface.GetComponent<EnemySpawner>().enabled = true;
        _gameTimer.StartTimer();

        _damageTextPool = new DamageTextPool(damageTextPrefab, damageTextPoolSize);
        _lootPool = new LootPool(lootPrefabs, lootPoolSize);
        _bloodSplashPool = new BloodSplashPool(bloodSplashPrefab, bloodSplashPoolSize);

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
    private void BloodSpawn(object data)
    {
        _bloodSplashPool.GetBloodSplash((Transform)data);
    }
    private void DropLoot(object data)
    {
        _lootPool.GetLoot((Vector3)data);
    }
    private IEnumerator SurviveTime()
    {
        _surviveTime.text = $"Survive {_gameTimer.StartingTime}s";
        _surviveTime.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        _surviveTime.transform.parent.gameObject.SetActive(false);
    }
    private void TimeLeft(object data)
    {
        _timeLeft.transform.parent.gameObject.SetActive(true);
        _timeLeft.text = $"{(int)_gameTimer.CurrentTime}s";
    }
    private void WaveCompleted(object data)
    {
        _groundSurface.GetComponent<EnemySpawner>().enabled = false;
        StartCoroutine(WaveDelay(2.0f));
        _waveLevel.text = data.ToString();
    }
    private IEnumerator WaveDelay(float delay)
    {
        _timeLeft.transform.parent.gameObject.SetActive(false);
        _waveComplete.transform.parent.gameObject.SetActive(true);

        _gameTimer.transform.parent.gameObject.SetActive(false);
        _waveLevel.transform.parent.gameObject.SetActive(false);
        _coinAmount.transform.parent.gameObject.SetActive(false);

        Observer.Instance.Notify(EventName.Joy);

        _joystick.transform.parent.gameObject.SetActive(false);
        _joystick.enabled = false;

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        _waveComplete.transform.parent.gameObject.SetActive(false);

        _groundSurface.GetComponent<EnemySpawner>().enabled = true;
        _gameTimer.transform.parent.gameObject.SetActive(true);
        _waveLevel.transform.parent.gameObject.SetActive(true);
        _coinAmount.transform.parent.gameObject.SetActive(true);

        _joystick.transform.parent.gameObject.SetActive(true);
        _joystick.enabled = true;

        StartCoroutine(SurviveTime());

        Time.timeScale = 1;
    }
    private void UpdateWalletUI(object data)
    {
        _coinAmount.text = data.ToString();
    }
    private void SetupCameraFollow()
    {
        if (_cinemachineVirtualCamera != null && Player.Instance != null)
        {
            _cinemachineVirtualCamera.Follow = Player.Instance.transform;
        }
    }
    public void ReactivatePlatform(object data)
    {
        StartCoroutine(ReactivatePlatformCoroutine((GameObject)data));
    }
    private IEnumerator ReactivatePlatformCoroutine(GameObject platform)
    {
        yield return new WaitForSeconds(3f);
        platform.SetActive(true);
    }
    public void EndGame()
    {
        Observer.Instance.RemoveObserver(EventName.WaveCompleted, WaveCompleted);
        Observer.Instance.RemoveObserver(EventName.TimeLeft, TimeLeft);
        Observer.Instance.RemoveObserver(EventName.DamageReceived, DamageReceived);
        Observer.Instance.RemoveObserver(EventName.ReactivatePlatform, ReactivatePlatform);
        Observer.Instance.RemoveObserver(EventName.DropLoot, DropLoot);
        Observer.Instance.RemoveObserver(EventName.BloodSpawn, BloodSpawn);
        Observer.Instance.RemoveObserver(EventName.UpdateWalletUI, UpdateWalletUI);

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

    //private void BakeNavMesh()
    //{
    //    GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
    //    foreach (GameObject building in buildings)
    //    {
    //        foreach (Transform child in building.transform)
    //        {
    //            if (child.CompareTag(wallTag))
    //            {
    //                NavMeshSurface surface = child.gameObject.GetComponent<NavMeshSurface>();
    //                if (surface == null)
    //                {
    //                    surface = child.gameObject.AddComponent<NavMeshSurface>();
    //                }

    //                surface.layerMask = 1 << LayerMask.NameToLayer("Wall");

    //                wallSurfaces.Add(surface);
    //                surface.BuildNavMesh();
    //            }
    //        }
    //    }
    //}

}
