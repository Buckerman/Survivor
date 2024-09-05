using QuangDM.Common;
using System;
using UnityEngine;
using TMPro;
using System.Collections;

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
        if (this != null)
        {
            startingTime += 5f;
            StartTimer();
        }
    }

    public void StartTimer()
    {
        enabled = true;
        currentTime = startingTime;
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

            StartCoroutine(HandleWaveCompletion());
        }
    }

    private IEnumerator HandleWaveCompletion()
    {
        Observer.Instance.Notify(EventName.PickUpAllLoot);

        yield return new WaitForSeconds(0.5f);

        Observer.Instance.Notify(EventName.DisableAllEnemies);
        Observer.Instance.Notify(EventName.DisableAllBloodSplash);
        Observer.Instance.Notify(EventName.DisableAllDamageText);
        Observer.Instance.Notify(EventName.DisableAllLoot);
        Observer.Instance.Notify(EventName.WaveCompleted, waveLevel);
    }


    public void StopTimer()
    {
        enabled = false;
    }
    void OnDestroy()
    {
        Observer.Instance.RemoveObserver(EventName.CurrentWaveLevel, CurrentWaveLevel);
    }
}
