using Unity.AI.Navigation;
using UnityEngine;
using TMPro;
using QuangDM.Common;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private LootDropManager _lootDropManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PowerUpManager _powerUpManager;
    [SerializeField] private AbilityManager _abilityManager;

    [Header("Damage Text Settings")]
    [SerializeField] private GameObject damageTextPrefab;

    [Header("Blood Splash Settings")]
    [SerializeField] private GameObject bloodSplashPrefab;

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
        _abilityManager.Initialize();

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

        GameObject damageTextObject = ObjectPooling.Instance.GetObject(damageTextPrefab);
        DamageText damageText = damageTextObject.GetComponent<DamageText>();

        if (data is ValueTuple<PlayerHealth, float> playerData)
        {
            var (playerHealth, damage) = playerData;
            targetTransform = playerHealth.transform;
            damageText.Setup((int)damage, Color.red);
        }
        else if (data is ValueTuple<EnemyHealth, float> enemyData)
        {
            var (enemyHealth, damage) = enemyData;
            targetTransform = enemyHealth.transform;
            damageText.Setup((int)damage, Color.yellow);
        }

        damageText.Initialize(targetTransform.position);
    }
    private void BloodSpawn(object data)
    {
        GameObject bloodSplashObject = ObjectPooling.Instance.GetObject(bloodSplashPrefab);
        BloodSplash bloodSplash = bloodSplashObject.GetComponent<BloodSplash>();

        bloodSplash.Initialize((Transform)data);
    }
    private void DropLoot(object data)
    {
        _lootDropManager.DropLoot((Vector3)data);
    }
    private void ActivatePowerUpfUI(object data)
    {
        _uiManager.ActivateBuffUI(data.ToString());
    }
    private void RemovePowerUpUI(object data)
    {
        _uiManager.RemoveBuffUI(data.ToString());
    }
    private void UpdatePowerUpUI(object data)
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
        ObjectPooling.Instance.ClearPool();
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
        Observer.Instance.AddObserver(EventName.ActivatePowerUpfUI, ActivatePowerUpfUI);
        Observer.Instance.AddObserver(EventName.RemovePowerUpUI, RemovePowerUpUI);
        Observer.Instance.AddObserver(EventName.UpdatePowerUpUI, UpdatePowerUpUI);
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
        Observer.Instance.RemoveObserver(EventName.ActivatePowerUpfUI, ActivatePowerUpfUI);
        Observer.Instance.RemoveObserver(EventName.RemovePowerUpUI, RemovePowerUpUI);
        Observer.Instance.RemoveObserver(EventName.UpdatePowerUpUI, UpdatePowerUpUI);
    }
}
