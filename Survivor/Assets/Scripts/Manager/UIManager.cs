using Cinemachine;
using QuangDM.Common;
using System.Collections;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public NavMeshSurface _groundSurface;
    public GameTimer _gameTimer;
    private TextMeshProUGUI _defeatGame;
    private TextMeshProUGUI _waveComplete;
    private TextMeshProUGUI _waveLevel;
    private TextMeshProUGUI _surviveTime;
    private TextMeshProUGUI _timeLeft;
    private TextMeshProUGUI _coinAmount;
    private TextMeshProUGUI _playerLevel;
    private Image _experienceBar;
    private VariableJoystick _joystick;

    public void Initialize()
    {
        AssignReferences();

        _waveLevel.text = "1";
        _coinAmount.text = "<sprite=0> 0";
        _playerLevel.text = "0";
    }
    public void StartGame()
    {
        _groundSurface.GetComponent<EnemySpawner>().enabled = true;
        _gameTimer.StartTimer();
    }
    private void AssignReferences()
    {
        if (_groundSurface == null)
            _groundSurface = FindObjectOfType<EnemySpawner>().GetComponent<NavMeshSurface>();

        if (_gameTimer == null)
            _gameTimer = FindObjectOfType<GameTimer>();

        if (_defeatGame == null)
            _defeatGame = GameObject.Find("GUI/DefeatBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_waveComplete == null)
            _waveComplete = GameObject.Find("GUI/WaveCompleteBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_waveLevel == null)
            _waveLevel = GameObject.Find("GUI/HUDContainer/Container/WaveLevelBg/LevelText").GetComponent<TextMeshProUGUI>();

        if (_surviveTime == null)
            _surviveTime = GameObject.Find("GUI/SurviveTimeBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_timeLeft == null)
            _timeLeft = GameObject.Find("GUI/TimeLeftBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_coinAmount == null)
            _coinAmount = GameObject.Find("GUI/HUDContainer/CoinAmountBg").GetComponentInChildren<TextMeshProUGUI>();

        if (_playerLevel == null)
            _playerLevel = GameObject.Find("GUI/HUDContainer/PlayerLevelBg/NumberText").GetComponent<TextMeshProUGUI>();

        if (_experienceBar == null)
            _experienceBar = GameObject.Find("GUI/HUDContainer/PlayerLevelBg/ExpBar").GetComponent<Image>();

        if (_joystick == null)
            _joystick = FindObjectOfType<VariableJoystick>();

        _defeatGame.transform.parent.gameObject.SetActive(false);
        _waveComplete.transform.parent.gameObject.SetActive(false);
        _timeLeft.transform.parent.gameObject.SetActive(false);

        Player.Instance.GetComponent<PlayerLevelSystem>().experienceBar = _experienceBar;
        Player.Instance.GetComponent<PlayerController>().Joystick = _joystick;
    }
    public void PlayerLevelUp(string text)
    {
        _playerLevel.text = text;
    }
    public IEnumerator SurviveTime()
    {
        _surviveTime.text = $"Survive {_gameTimer.StartingTime}s";
        _surviveTime.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        _surviveTime.transform.parent.gameObject.SetActive(false);
    }
    public void TimeLeft()
    {
        _timeLeft.transform.parent.gameObject.SetActive(true);
        _timeLeft.text = $"{(int)_gameTimer.CurrentTime}s";
    }
    public void WaveCompleted(string text)
    {
        _groundSurface.GetComponent<EnemySpawner>().enabled = false;
        StartCoroutine(WaveDelay(2.0f));
        _waveLevel.text = text;
    }
    private IEnumerator WaveDelay(float delay)
    {
        _timeLeft.transform.parent.gameObject.SetActive(false);
        _waveComplete.transform.parent.gameObject.SetActive(true);

        _gameTimer.transform.parent.gameObject.SetActive(false);
        _waveLevel.transform.parent.gameObject.SetActive(false);

        Observer.Instance.Notify(EventName.Joy);

        _joystick.transform.parent.gameObject.SetActive(false);
        _joystick.enabled = false;

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        _waveComplete.transform.parent.gameObject.SetActive(false);

        _groundSurface.GetComponent<EnemySpawner>().enabled = true;
        _gameTimer.transform.parent.gameObject.SetActive(true);
        _waveLevel.transform.parent.gameObject.SetActive(true);
        _joystick.transform.parent.gameObject.SetActive(true);
        _joystick.enabled = true;

        SurviveTime();

        Time.timeScale = 1;
    }
    public void UpdateWalletUI(object data)
    {
        _coinAmount.text = $"<sprite=0> {data}";
    }
    public void EndGame()
    {
        _gameTimer.StopTimer();
        _groundSurface.GetComponent<EnemySpawner>().enabled = false;
        _defeatGame.transform.parent.gameObject.SetActive(true);
        _joystick.transform.parent.gameObject.SetActive(false);
    }
}