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
    PositiveComplex,
    NegativeComplex,
    Env,
    Temp,
    AntiEnv,
    AntiTemp
}

public class Customer : MonoBehaviour
{   
    public static Customer Instance;
    
    public CustomerType currentType;

    public CircleEnvironment targetEnvironment;
    public CircleTemperature targetTemperature;

    [SerializeField] private TextMeshProUGUI customerInfoText;

    [SerializeField] private Image body, currentEyes, currentMouth, currentHorns, currentHat;
    private CustomerAvatar _avatar;
    private FlavorTextManager _flavorTextManager;

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
        _flavorTextManager = FindAnyObjectByType<FlavorTextManager>();
        Randomise();
    }

    private void Randomise()
    {
        Debug.Log("Randomised");
        
        List<Sprite> sprites = _avatar.GetRandomAvatar();
        
        currentEyes.sprite = sprites[0];
        currentMouth.sprite = sprites[1];
        currentHorns.sprite = sprites[2];
        currentHorns.color = _avatar.HornColor;
        
        currentType = (CustomerType)Random.Range(0, Enum.GetNames(typeof(CustomerType)).Length);
        
        AssignSpecifications();
        
        customerInfoText.text = _flavorTextManager.GetFlavorText(this);

        // old system
        /*// 1 -> environment, 2 -> temperature
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

        customerInfoText.text = _flavorTextManager.GetFlavorText(this);

        List<Sprite> sprites = _avatar.GetRandomAvatar();
        
        currentEyes.sprite = sprites[0];
        currentMouth.sprite = sprites[1];
        currentHorns.sprite = sprites[2];
        currentHorns.color = _avatar.HornColor;

        switch (targetTemperature)
        {
            case CircleTemperature.Cold:
                if (currentType is CustomerType.Positive or CustomerType.PositiveComplex)
                {
                    body.color = _avatar.ColdColor;
                    break;
                }
                body.color = _avatar.HotColor;
                break;

            case CircleTemperature.Hot:
                if (currentType is CustomerType.Positive or CustomerType.PositiveComplex)
                {
                    body.color = _avatar.HotColor;
                    break;
                }
                body.color = _avatar.ColdColor;
                break;
        }
        
        if (targetIndex == 1 || currentType == CustomerType.Neutral) body.color = _avatar.NeutralColor;*/
    }

    private void AssignSpecifications()
    {
        if (currentType == CustomerType.Neutral) 
        {
            body.color = _avatar.NeutralColor;
        }

        if (currentType is CustomerType.Temp or CustomerType.AntiTemp)
        {
            targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
            body.color = targetTemperature == CircleTemperature.Cold ? _avatar.ColdColor : _avatar.HotColor;
        }

        if (currentType is CustomerType.Env or CustomerType.AntiEnv)
        {
            body.color = _avatar.NeutralColor;
            targetEnvironment = (CircleEnvironment)Random.Range(0, Enum.GetNames(typeof(CircleEnvironment)).Length);
        }

        if (currentType == CustomerType.PositiveComplex)
        {
            targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
            body.color = targetTemperature == CircleTemperature.Cold ? _avatar.ColdColor : _avatar.HotColor;
            targetEnvironment = (CircleEnvironment)Random.Range(0, Enum.GetNames(typeof(CircleEnvironment)).Length);
        }

        if (currentType == CustomerType.NegativeComplex)
        {
            targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
            body.color = targetTemperature == CircleTemperature.Hot ? _avatar.ColdColor : _avatar.HotColor;
            targetEnvironment = (CircleEnvironment)Random.Range(0, Enum.GetNames(typeof(CircleEnvironment)).Length);
        }
    }

    private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }
}
