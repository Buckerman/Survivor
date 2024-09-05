using QuangDM.Common;
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
        ResetExp();
    }

    public void ResetExp()
    {
        Player.Instance._level = 0;
        _currentExp = 0;
        _requiredExp = 20;
        UpdateExperienceBar();
    }

    public void GainExperience()
    {
        //do zmiany w celu skalowalnosci
        _currentExp += 1;

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
        Observer.Instance.Notify(EventName.PlayerLevelUp, Player.Instance._level);

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
