using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject endShiftCanvas;
    
    [SerializeField] private TextMeshProUGUI stateText, scoresText, bonusesText, bonusesValue, totalScoreText;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image sliderFace; 
    [SerializeField] private Sprite faceNeutral, faceHappy, faceSad;

    private int _customersServed = 0;
    
    private float _customerSatisfactionCoefficient;
    private float _lastShiftSatisfaction;
    
    private float _score;
    private float _totalScore;

    private List<float> _currentShiftScores = new List<float>();

    public List<float> CurrentShiftScores
    {
        get => _currentShiftScores; 
        private set => _currentShiftScores = value;
    }

    public float Score
    {
        get => _score;
        private set
        {
            _score = value;
            UpdateGlobalScoreValues();
        }
    }

    private void OnEnable()
    {
        endShiftCanvas.SetActive(false);
        
        GameManager.OnEnter += () => _lastShiftSatisfaction = _customerSatisfactionCoefficient;
        GameManager.OnShiftComplete += DisplayScore;
        GameManager.OnShiftIncomplete += AddMissedScores;
    }
    
    private void UpdateGlobalScoreValues()
    {
        _customerSatisfactionCoefficient = _score / _customersServed;
        UpdateSliderVisual(_customerSatisfactionCoefficient);
        scoreSlider.value = _customerSatisfactionCoefficient;
    }

    private void UpdateSliderVisual(float csc)
    {
        if (csc < 50)
        {
            sliderFace.sprite = faceSad;
        }
        else if (csc is >= 50 and < 75)
        {
            sliderFace.sprite = faceNeutral;
        }
        else if (csc is >= 75 and <= 100)
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
                scoreToAdd += customer.targetEnvironment == circle.environment ? 0.75f : 0;
                scoreToAdd += customer.targetTemperature == circle.temperature ? 0.25f : 0;
                break;
            case CustomerType.NegativeComplex:
                scoreToAdd += customer.targetEnvironment != circle.environment ? 0.75f : 0f;
                scoreToAdd += customer.targetTemperature != circle.temperature ? 0.25f : 0f;
                break;
        }
        
        CurrentShiftScores.Add(scoreToAdd);
        
        Debug.Log($"Added {scoreToAdd} points");
        Score += scoreToAdd * 100f;
        GameManager.CurrentGameState = GameState.Place; 
    }

    private void AddMissedScores()
    {
        for (int i = ShiftManager.Instance.ShiftLength - CurrentShiftScores.Count; i > 0; i--)
        {
            _customersServed++;
            Score += 0;
            CurrentShiftScores.Add(0);
        }
        DisplayScore();
    }

    private void DisplayScore()
    {
        endShiftCanvas.SetActive(true);
        
        SetTitle();

        scoresText.text = String.Empty;
        foreach (float scoreValue in CurrentShiftScores)
        {
            scoresText.text += $"{scoreValue} / ";
        }
        CurrentShiftScores.Clear();

        SetScoreTexts();
    }

    private void SetScoreTexts()
    {
        string scoreString = "Customer satisfaction: \n";
        string numberString = $"{(int)_customerSatisfactionCoefficient} %\n";

        string earlyBonusText = "Early bonus: \n";
        string consistencyBonusText = "Consistency bonus: \n";

        float earlyBonus = 1f;
        float consistencyBonus = 1f;

        float time = FindAnyObjectByType<Timer>().Time;

        if (time <= 15.0f)
        {
            scoreString += earlyBonusText;
            earlyBonus = 1 + (17.0f - time) / 16f;
            numberString += $"{(int)(earlyBonus * 100)}%\n";
        }

        if (_lastShiftSatisfaction >= 75 && _customerSatisfactionCoefficient >= 75)
        {
            scoreString += consistencyBonusText;
            consistencyBonus = _lastShiftSatisfaction + _customerSatisfactionCoefficient / 200f;
            numberString += $"{(int)(100 + consistencyBonus)}%\n";
        }

        bonusesText.text = scoreString;
        bonusesValue.text = numberString;
        _totalScore += Score * earlyBonus * (1 + consistencyBonus / 100f);

        totalScoreText.text = $"SCORE: {(int)_totalScore}";
    }

    private void SetTitle()
    {
        switch (GameManager.CurrentGameState)
        {
            case GameState.ShiftComplete:
                if (_customerSatisfactionCoefficient > 50)
                {
                    stateText.text = "SHIFT COMPLETED!";
                    break;
                }
                stateText.text = "YOU'RE FIRED!";
                break;
            case GameState.ShiftIncomplete:
                if (_customerSatisfactionCoefficient > 50)
                {
                    stateText.text = "YOU RAN OUT OF TIME!";
                    break;
                }
                stateText.text = "YOU'RE FIRED!";
                break;
        }
    }
}
