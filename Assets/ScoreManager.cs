using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject endShiftCanvas;
    
    [SerializeField] private TextMeshProUGUI stateText, bonusesText, bonusesValue, totalScoreText;
    [SerializeField] private HorizontalLayoutGroup scoreMarks;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image sliderFace; 
    [SerializeField] private Sprite faceNeutral, faceHappy, faceSad;

    [SerializeField] private Button nextShiftButton, quitButton;

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
        Sequence scoreSequence = DOTween.Sequence();
        SetTitle();
        
        for (int i = 0; i < CurrentShiftScores.Count; i++)
        {
            scoreMarks.GetComponentsInChildren<Image>(includeInactive:true)[i].gameObject.SetActive(true);
            scoreMarks.GetComponentsInChildren<Image>(includeInactive:true)[i].color = Color.clear;
            
            var i1 = i;
            scoreSequence
                .AppendCallback(() =>
                {
                    Color selectedColor;
                    if (CurrentShiftScores[i1] == 0) selectedColor = Color.red;
                    else if (CurrentShiftScores[i1] < 1f) selectedColor = Color.yellow;
                    else selectedColor = Color.green;
                    scoreMarks.GetComponentsInChildren<Image>()[i1].color = selectedColor;
                })
                .AppendInterval(0.25f);
        }
        
        scoreSequence
            .AppendInterval(1f)
            .AppendCallback(SetScoreTexts);
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
                nextShiftButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
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
