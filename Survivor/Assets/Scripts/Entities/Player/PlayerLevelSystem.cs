using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelSystem : MonoBehaviour
{
    public Image experienceBar;
    private int _currentExp;
    private int _requiredExp;
    private float maxBarWidth = 245f;

    void Start()
    {
        _currentExp = 0;
        _requiredExp = 20;
        UpdateExperienceBar();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GainExperience(10);
        }
    }

    public void GainExperience(int amount)
    {
        _currentExp += amount;

        while (_currentExp >= _requiredExp)
        {
            LevelUp();
        }

        UpdateExperienceBar();
    }

    private void LevelUp()
    {
        _currentExp -= _requiredExp;
        Player.Instance._level++;

        _requiredExp = Mathf.FloorToInt(_requiredExp * 1.2f);
    }

    private void UpdateExperienceBar()
    {
        float expRatio = (float)_currentExp / _requiredExp;
        float newWidth = expRatio * maxBarWidth;

        RectTransform barRect = experienceBar.GetComponent<RectTransform>();
        barRect.sizeDelta = new Vector2(newWidth, barRect.sizeDelta.y);
    }
}
