using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
    private Stamp[] _stamps;

    [SerializeField] private Button nextShiftButton, quitButton;
    
    [SerializeField] private AudioSource ringingAudioSource;
    [SerializeField] private AudioSource stampAudioSource;
    [SerializeField] private AudioClip pointSound, winSting, loseSting, timerGoingOff;
    
    private AudioSource _stingerAudioSource;

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
            if (value < 0) value = 0;
            _score = value;
            UpdateGlobalScoreValues();
        }
    }

    private void OnEnable()
    {
        endShiftCanvas.SetActive(false);
        
        GameManager.OnEnter += () =>
        {
            _lastShiftSatisfaction = _customerSatisfactionCoefficient;
            CurrentShiftScores = new List<float>();
        };
        GameManager.OnShiftComplete += DisplayScore;
        GameManager.OnShiftIncomplete += AddMissedScores;
    }

    private void OnDisable()
    {
        GameManager.OnShiftComplete -= DisplayScore;
        GameManager.OnShiftIncomplete -= AddMissedScores;
    }

    private void Start()
    {
        _stingerAudioSource = FindAnyObjectByType<AudioManager>().stingerAudioSource;
        _stamps = scoreMarks.GetComponentsInChildren<Stamp>(true);
        foreach (var stamp in _stamps) stamp.gameObject.SetActive(false);
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
            case CustomerType.Neutral:
                scoreToAdd += 1;
                break;
            case CustomerType.Temp:
                scoreToAdd += customer.targetTemperature == circle.temperature ? 1 : -0.5f;
                break;
            case CustomerType.AntiTemp:
                scoreToAdd +=  customer.targetTemperature == circle.temperature ? -0.5f : 1;
                break;
            case CustomerType.Env:
                scoreToAdd += customer.targetEnvironment == circle.environment ? 1 : -0.5f;
                break;
            case CustomerType.AntiEnv:
                scoreToAdd += customer.targetEnvironment == circle.environment ? -0.5f : 1;
                break;
            case CustomerType.PositiveComplex:
                scoreToAdd += customer.targetTemperature == circle.temperature ? 1f : -0.5f;
                scoreToAdd += customer.targetEnvironment == circle.environment ? 1f : -0.5f;
                break;
            case CustomerType.NegativeComplex:
                scoreToAdd += customer.targetTemperature == circle.temperature ? -0.5f : 1f;
                scoreToAdd += customer.targetEnvironment == circle.environment ? -0.5f : 1f;
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
        FindAnyObjectByType<AudioManager>().globalMusicAudioSource.mute = true;
        PlayScoreStinger(_customerSatisfactionCoefficient > 50);
        foreach (var stamp in _stamps) stamp.gameObject.SetActive(false);
        endShiftCanvas.SetActive(true);
        
        // celar bonuses text boxes
        bonusesText.text = string.Empty;
        bonusesValue.text = string.Empty;
        
        // clear buttons
        nextShiftButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        
        SetTitle();
        Sequence scoreSequence = DOTween.Sequence();
        scoreSequence.SetLink(gameObject);
        
        Sequence pointSequence = DOTween.Sequence();
        pointSequence.SetLink(gameObject);
        
        pointSequence.AppendInterval(0.5f);
        // display row of empty stamps
        for (int i = 0; i < CurrentShiftScores.Count; i++)
        {
            _stamps[i].gameObject.SetActive(true);
        }
        // reset pitch of audio source just in case
        stampAudioSource.pitch = 1;
        
        // fill stamps one by one
        for (int i = 0; i < CurrentShiftScores.Count; i++)
        {
            var i1 = i;
            pointSequence
                .AppendCallback(() =>
                {
                    _stamps[i1].DisplayStamp(CurrentShiftScores[i1]);
                    stampAudioSource.pitch = 1 + 0.1f * i1;
                    stampAudioSource.PlayOneShot(pointSound);
                })
                .AppendInterval(0.25f);
        }
        
        Sequence syncedSequence = DOTween.Sequence();
        syncedSequence.SetLink(gameObject);

        syncedSequence
            .AppendInterval(_customerSatisfactionCoefficient >= 50 ? 3.45f : 2f)
            .AppendCallback(SetScoreTexts)
            .AppendInterval(_customerSatisfactionCoefficient >= 50 ? 0.85f : 1.6f)
            .AppendCallback(() => SetButtons(_customerSatisfactionCoefficient > 50));
        
        scoreSequence
            .Join(pointSequence)
            .Join(syncedSequence);
    }

    private void SetButtons(bool isAboveThreshold)
    {
        nextShiftButton.gameObject.SetActive(isAboveThreshold);
        quitButton.gameObject.SetActive(!isAboveThreshold);
    }

    private void SetScoreTexts()
    {
        string scoreString = "Customer satisfaction: \n";
        string numberString = $"{(int)_customerSatisfactionCoefficient} %\n";

        string earlyBonusText = "Early bonus: \n";
        string consistencyBonusText = "Consistency bonus: \n";

        float earlyBonus = 1f;
        float consistencyBonus = 1f;

        float time = FindAnyObjectByType<Timer>().GameTime;

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
                ringingAudioSource.PlayOneShot(timerGoingOff);
                if (_customerSatisfactionCoefficient > 50)
                {
                    stateText.text = "YOU RAN OUT OF TIME!";
                    break;
                }
                stateText.text = "YOU'RE FIRED!";
                break;
        }
    }

    private void PlayScoreStinger(bool isWin = true)
    {
        _stingerAudioSource.PlayOneShot(isWin ? winSting : loseSting);
    }
}
