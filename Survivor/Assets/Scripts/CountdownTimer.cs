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

    void Update()
    {
        currentTime -= Time.deltaTime;
        countDown.text = ((int)currentTime).ToString();
        if (currentTime <= 0)
        {
            currentTime = 0;
            Time.timeScale = 0;
        }
    }
}
