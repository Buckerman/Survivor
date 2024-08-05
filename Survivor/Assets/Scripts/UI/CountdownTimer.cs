using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 60f;

    [SerializeField] Text countDown;

    void Start()
    {
        currentTime = startingTime;
    }

    public void StartTimer()
    {
        currentTime = startingTime;
        enabled = true;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        countDown.text = ((int)currentTime).ToString();
        if (currentTime <= 0)
        {
            currentTime = 0;
            Time.timeScale = 0;
            GameManager.Instance.EndGame();
        }
    }

    public void StopTimer()
    {
        enabled = false;
    }
}
