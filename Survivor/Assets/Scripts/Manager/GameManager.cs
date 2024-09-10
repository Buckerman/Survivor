using Unity.AI.Navigation;
using UnityEngine;
using TMPro;
using QuangDM.Common;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private LootDropManager _lootDropManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private BuffManager _buffManager;

    [Header("Damage Text Settings")]
    [SerializeField] private DamageText damageTextPrefab;
    [SerializeField] private int damageTextPoolSize = 10;
    private DamageTextPool _damageTextPool;

    [Header("Blood Splash Settings")]
    [SerializeField] private BloodSplash bloodSplashPrefab;
    [SerializeField] private int bloodSplashPoolSize = 10;
    private BloodSplashPool _bloodSplashPool;

    public static GameManager Instance { get; private set; }

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
        SetupCameraFollow();
        InitializeGame();
    }
    private void AssignReferences()
    {
        if (_cinemachineVirtualCamera == null)
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    private void InitializeGame()
    {
        _lootDropManager.Initialize();
        _uiManager.Initialize();

        Application.targetFrameRate = 60;
        StartGame();

        AddObsevers();
    }
    public void StartGame()
    {
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
            playerHealth.HealthBar.gameObject.SetActive(true);
        }

        SurviveTime();
        _uiManager.StartGame();

        _damageTextPool = new DamageTextPool(damageTextPrefab, damageTextPoolSize);
        _bloodSplashPool = new BloodSplashPool(bloodSplashPrefab, bloodSplashPoolSize);

        //PlayerData.Instance.Load();
        //PlayerData.Instance.ConversationID = 2;
        //PlayerData.Instance.Save();
    }
    private void PlayerLevelUp(object data)
    {
        _uiManager.PlayerLevelUp(data.ToString());
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
        _lootDropManager.DropLoot((Vector3)data);
    }
    private void ActiveteBuffUI(object data)
    {
        _uiManager.ActiveteBuffUI();
    }
    private void RemoveBuffUI(object data)
    {
        _uiManager.RemoveBuffUI();
    }
    private void UpdateBuffUI(object data)
    {
        _uiManager.UpdateBuffUI(data);
    }
    private void SurviveTime()
    {
        StartCoroutine(_uiManager.SurviveTime());
    }
    private void TimeLeft(object data)
    {
        _uiManager.TimeLeft();
    }
    private void WaveCompleted(object data)
    {
        _uiManager.WaveCompleted(data.ToString());
    }
    private void UpdateWalletUI(object data)
    {
        _uiManager.UpdateWalletUI(data);
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
        RemoveObservers();

        Time.timeScale = 0;
        _uiManager.EndGame();
        StartCoroutine(ReloadSceneAfterDelay(2.0f));
    }
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void AddObsevers()
    {
        Observer.Instance.AddObserver(EventName.WaveCompleted, WaveCompleted);
        Observer.Instance.AddObserver(EventName.TimeLeft, TimeLeft);
        Observer.Instance.AddObserver(EventName.DamageReceived, DamageReceived);
        Observer.Instance.AddObserver(EventName.ReactivatePlatform, ReactivatePlatform);
        Observer.Instance.AddObserver(EventName.DropLoot, DropLoot);
        Observer.Instance.AddObserver(EventName.BloodSpawn, BloodSpawn);
        Observer.Instance.AddObserver(EventName.UpdateWalletUI, UpdateWalletUI);
        Observer.Instance.AddObserver(EventName.PlayerLevelUp, PlayerLevelUp);
        Observer.Instance.AddObserver(EventName.ActiveteBuffUI, ActiveteBuffUI);
        Observer.Instance.AddObserver(EventName.RemoveBuffUI, RemoveBuffUI);
        Observer.Instance.AddObserver(EventName.UpdateBuffUI, UpdateBuffUI);
    }
    private void RemoveObservers()
    {
        Observer.Instance.RemoveObserver(EventName.WaveCompleted, WaveCompleted);
        Observer.Instance.RemoveObserver(EventName.TimeLeft, TimeLeft);
        Observer.Instance.RemoveObserver(EventName.DamageReceived, DamageReceived);
        Observer.Instance.RemoveObserver(EventName.ReactivatePlatform, ReactivatePlatform);
        Observer.Instance.RemoveObserver(EventName.DropLoot, DropLoot);
        Observer.Instance.RemoveObserver(EventName.BloodSpawn, BloodSpawn);
        Observer.Instance.RemoveObserver(EventName.UpdateWalletUI, UpdateWalletUI);
        Observer.Instance.RemoveObserver(EventName.PlayerLevelUp, PlayerLevelUp);
        Observer.Instance.RemoveObserver(EventName.ActiveteBuffUI, ActiveteBuffUI);
        Observer.Instance.RemoveObserver(EventName.RemoveBuffUI, RemoveBuffUI);
        Observer.Instance.RemoveObserver(EventName.UpdateBuffUI, UpdateBuffUI);
    }
}
