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

    private List<CustomerType> _currentPool = new List<CustomerType>();

    [SerializeField] private Image body, currentEyes, currentMouth, currentHorns, currentHat;
    private CustomerAvatar _avatar;
    private FlavorTextManager _flavorTextManager;

    private void OnEnable()
    {
        GameManager.OnPlace += () => Randomise(_currentPool);
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
    }

    public void Randomise(List<CustomerType> customerTypePool)
    {
        List<Sprite> sprites = _avatar.GetRandomAvatar();
        
        currentEyes.sprite = sprites[0];
        currentMouth.sprite = sprites[1];
        currentHorns.sprite = sprites[2];
        currentHorns.color = _avatar.HornColor;
        
        _currentPool = customerTypePool;
        currentType = _currentPool[Random.Range(0, customerTypePool.Count)];
        
        AssignSpecifications();
        
        customerInfoText.text = _flavorTextManager.GetFlavorText(this);
        
    }

    private void AssignSpecifications()
    {
        if (currentType == CustomerType.Neutral) 
        {
            body.color = _avatar.NeutralColor;
        }

        if (currentType == CustomerType.Temp)
        {
            targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
            body.color = targetTemperature == CircleTemperature.Cold ? _avatar.ColdColor : _avatar.HotColor;
        }

        if (currentType == CustomerType.AntiTemp)
        {
            targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
            body.color = targetTemperature == CircleTemperature.Cold ? _avatar.HotColor : _avatar.ColdColor;
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

    /*private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }*/
}
