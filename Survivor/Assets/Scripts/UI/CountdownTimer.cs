using QuangDM.Common;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 15f;

    private Text _countDown;

    void Start()
    {
        currentTime = startingTime;
        _countDown = GetComponent<Text>();
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
            currentTime = 0;
            Time.timeScale = 0;
            StopTimer();
            GameManager.Instance.EndWave();
        }
    }

    public void StopTimer()
    {
        enabled = false;
    }
}
