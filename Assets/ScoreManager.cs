using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, customerSatisfactionCoefficientText;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image sliderFace; 
    [SerializeField] private Sprite faceNeutral, faceHappy, faceSad;

    private int _customersServed = 0;
    
    private float _customerSatisfactionCoefficient;

    private float _score;

    public float Score
    {
        get => _score;
        private set
        {
            _score = value;
            scoreText.text = $"Score: {value}";
            UpdateScoreValues();
        }
    }

    private void UpdateScoreValues()
    {
        _customerSatisfactionCoefficient = _score / _customersServed * 100;
        customerSatisfactionCoefficientText.text = $"Customer satisfaction: {_customerSatisfactionCoefficient}%";
        UpdateSliderVisual(_customerSatisfactionCoefficient);
        scoreSlider.DOValue(_customerSatisfactionCoefficient, 1f);
    }

    private void UpdateSliderVisual(float csc)
    {
        if (csc < 50)
        {
            sliderFace.sprite = faceSad;
        }
        else if (csc is > 50 and < 75)
        {
            sliderFace.sprite = faceNeutral;
        }
        else if (csc is > 75 and < 100)
        {
            sliderFace.sprite = faceHappy;
        }
    }


    public void AddPoints(Customer customer, Circle circle)
    {
        _customersServed++;
        CustomerType customerType = customer.currentType;

        float scoreToAdd = 0f;
        
        switch (customerType)
        {
            case CustomerType.Positive:
                switch (customer.targetIndex)
                {
                    case 1:
                        scoreToAdd += customer.targetEnvironment == circle.environment ? 1f : 0;
                        break;
                    case 2:
                        scoreToAdd += customer.targetTemperature == circle.temperature ? 1f : 0;
                        break;
                }
                break;
            case CustomerType.Negative:
                switch (customer.targetIndex)
                {
                    case 1:
                        scoreToAdd += customer.targetEnvironment == circle.environment ? 0 : 1f;
                        break;
                    case 2:
                        scoreToAdd += customer.targetTemperature == circle.temperature ? 0 : 1f;
                        break;
                }
                break;
            case CustomerType.Neutral:
                scoreToAdd += 1;
                break;
            case CustomerType.PositiveComplex:
                scoreToAdd += customer.targetEnvironment == circle.environment ? 0.5f : 0;
                scoreToAdd += customer.targetTemperature == circle.temperature ? 0.5f : 0;
                break;
            case CustomerType.NegativeComplex:
                scoreToAdd += customer.targetEnvironment != circle.environment ? 0.5f : 0f;
                scoreToAdd += customer.targetTemperature != circle.temperature ? 0.5f : 0f;
                break;
        }
        
        Debug.Log($"Added {scoreToAdd} points");
        Score += scoreToAdd;
        GameManager.CurrentGameState = GameState.Place; 
    }
}
