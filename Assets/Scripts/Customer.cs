using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CustomerType
{
    Neutral,
    Positive,
    Negative,
    PositiveComplex,
    NegativeComplex
}

public class Customer : MonoBehaviour
{   
    public static Customer Instance;
    
    public CustomerType currentType;

    public CircleEnvironment targetEnvironment;
    public CircleTemperature targetTemperature;
    public int targetIndex; // 1 = environment, 2 = temperature

    [SerializeField] private TextMeshProUGUI customerInfoText;

    [SerializeField] private Image currentEyes, currentMouth, currentHorns, currentHat;
    private CustomerAvatar _avatar;

    private void OnEnable()
    {
        GameManager.OnPlace += Randomise;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _avatar = GetComponent<CustomerAvatar>();
        Randomise();
    }

    private void Randomise()
    {
        Debug.Log("Randomised");
        currentType = (CustomerType)Random.Range(0, Enum.GetNames(typeof(CustomerType)).Length);
        
        // 1 -> environment, 2 -> temperature
        if (currentType is CustomerType.Negative or CustomerType.Positive)
        {
            targetIndex = Random.Range(1, 3);
        }
        else targetIndex = 0;
        
        // randomize preferred temperature, environment
        
        targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
        targetEnvironment = (CircleEnvironment)Random.Range(0, Enum.GetNames(typeof(CircleEnvironment)).Length);


        if (currentType == CustomerType.Neutral)
        {
            customerInfoText.text = "Target is any circle";
        }

        switch (currentType)
        {
            case CustomerType.Neutral:
                customerInfoText.text = "Wants: anything";
                break;
            case CustomerType.Positive:
                customerInfoText.text = "Wants: ";
                customerInfoText.text += targetIndex == 1 ? $"{targetEnvironment}" : $"{targetTemperature}";
                break;
            case CustomerType.PositiveComplex:
                customerInfoText.text = $"Wants: {targetTemperature} and {targetEnvironment}";
                break;
            case CustomerType.Negative:
                customerInfoText.text = "Doesn't want: ";
                customerInfoText.text += targetIndex == 1 ? $"{targetEnvironment}" : $"{targetTemperature}";
                break;
            case CustomerType.NegativeComplex:
                customerInfoText.text = $"Doesn't want: ";
                customerInfoText.text += $"{targetEnvironment} or {targetTemperature}";
                break;
        }

        List<Sprite> sprites = _avatar.GetRandomAvatar();
        
        currentEyes.sprite = sprites[0];
        currentMouth.sprite = sprites[1];
        currentHorns.sprite = sprites[2];
    }

    private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }
}
