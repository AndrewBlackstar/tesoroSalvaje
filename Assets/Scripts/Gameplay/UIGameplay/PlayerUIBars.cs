using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIBars : MonoBehaviour
{
    [Header("Puntaje")]
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [Header("Barras de UI")]
    public Image energyBar;
    public Image stealthBar;

    [Header("Velocidad de suavizado")]
    public float smoothSpeed = 5f;

    private float targetEnergy = 1f;
    private float currentEnergy = 1f;

    private float targetStealth = 1f;
    private float currentStealth = 1f;

    void Update()
    {
        // Energía
        currentEnergy = Mathf.Lerp(currentEnergy, targetEnergy, Time.deltaTime * smoothSpeed);
        if (energyBar != null)
            energyBar.fillAmount = currentEnergy;

        // Sigilo
        currentStealth = Mathf.Lerp(currentStealth, targetStealth, Time.deltaTime * smoothSpeed);
        if (stealthBar != null)
            stealthBar.fillAmount = currentStealth;
    }

    public void AddScore(int value)
    {
        currentScore += value;
        UpdateScoreUI();
    }

    public void SetScore(int value)
    {
        currentScore = value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Tesoros: " + currentScore.ToString(); // o "Puntaje:"
    }

    // Setters públicos
    public void SetEnergy(float value)
    {
        targetEnergy = Mathf.Clamp01(value);
    }

    public void SetStealth(float value)
    {
        targetStealth = Mathf.Clamp01(value);
    }

}


