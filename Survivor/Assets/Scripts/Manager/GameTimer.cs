using QuangDM.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    float currentTime = 0f;
    [SerializeField] float startingTime = 30f;
    public float StartingTime => startingTime;

    private int waveLevel = 1;
    private Text _countDown;

    void Start()
    {
        currentTime = startingTime;
        _countDown = GetComponent<Text>();

        Observer.Instance.AddObserver("CurrentWaveLevel", CurrentWaveLevel);
    }

    private void CurrentWaveLevel(object data)
    {
        startingTime += 15f;
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
        if (currentTime <= 0)
        {
            waveLevel++;
            StopTimer();

            Observer.Instance.Notify("DisableAllEnemies");
            Observer.Instance.Notify("WaveCompleted", waveLevel);
        }
    }

    public void StopTimer()
    {
        enabled = false;
    }
}
