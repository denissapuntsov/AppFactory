using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
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

    [SerializeField] private TextMeshProUGUI customerDebugInfoText;

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
        if (currentType == CustomerType.Negative || currentType == CustomerType.Positive)
        {
            targetIndex = Random.Range(0, 2);
        }
        
        // randomize preferred temperature, environment
        
        targetTemperature = (CircleTemperature)Random.Range(0, Enum.GetNames(typeof(CircleTemperature)).Length);
        targetEnvironment = (CircleEnvironment)Random.Range(0, Enum.GetNames(typeof(CircleEnvironment)).Length);
        
        //targetCircle = currentType == CustomerType.Deep ? 8 : Random.Range(0, 9);
        
        customerDebugInfoText.text = $"Targets: {targetEnvironment}, {targetTemperature}\nCustomer Type: {currentType}";
        
        // Visual randomisation
        List<Sprite> sprites = _avatar.GetRandomAvatar();
        
        currentEyes.sprite = sprites[0];
        currentMouth.sprite = sprites[1];
        currentHorns.sprite = sprites[2];

        /*currentHat.gameObject.SetActive(true);
        
        switch (currentType)
        {
            case CustomerType.Common:
                currentHat.GetComponent<CanvasGroup>().alpha = 1;
                currentHat.sprite = Random.Range(0, 2) == 1 ? _avatar.commonHat.sprite : _avatar.noHat.sprite;
                break;
            case CustomerType.Deep:
                currentHat.GetComponent<CanvasGroup>().alpha = 1;
                currentHat.sprite = Random.Range(0, 2) == 1 ? _avatar.deepHat.sprite : _avatar.noHat.sprite;
                break;
        }*/
    }

    private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }
}
