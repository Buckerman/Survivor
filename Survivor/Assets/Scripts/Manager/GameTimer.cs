using QuangDM.Common;
using System;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    float currentTime = 0f;
    [SerializeField] float startingTime = 30f;
    public float StartingTime => startingTime;
    public float CurrentTime => currentTime;

    private int waveLevel = 1;
    private TextMeshProUGUI _countDown;

    void Start()
    {
        currentTime = startingTime;
        _countDown = GetComponent<TextMeshProUGUI>();

        Observer.Instance.AddObserver(EventName.CurrentWaveLevel, CurrentWaveLevel);
    }

    private void CurrentWaveLevel(object data)
    {
        startingTime += 5f;
        StartTimer();
    }

    public void StartTimer()
    {
        currentTime = startingTime;
        enabled = true;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        _countDown.text = ((int)currentTime).ToString();
        if (currentTime < 6f)
        {
            Observer.Instance.Notify(EventName.TimeLeft);   
        }
        if (currentTime <= 0)
        {
            waveLevel++;
            StopTimer();

            Observer.Instance.Notify(EventName.DisableAllEnemies);
            Observer.Instance.Notify(EventName.DisableAllDamageText);
            Observer.Instance.Notify(EventName.DisableAllLoot);
            Observer.Instance.Notify(EventName.WaveCompleted, waveLevel);
        }
    }

    public void StopTimer()
    {
        enabled = false;
    }
}
