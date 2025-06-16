using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CustomerType
{
    Common,
     Deep,
     Precise,
     Avoidant
}

public class Customer : MonoBehaviour
{   
    public static Customer Instance;
    
    public CustomerType currentType;
    [Range(0, 8)] public int targetCircle;

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
        targetCircle = currentType == CustomerType.Deep ? 8 : Random.Range(0, 9);
        
        customerDebugInfoText.text = $"Target Circle: {targetCircle}\nCustomer Type: {currentType}";
        GameManager.CurrentGameState = GameState.Idle;

        
        // Visual randomisation
        List<Sprite> sprites = _avatar.GetRandomAvatar();
        
        currentEyes.sprite = sprites[0];
        currentMouth.sprite = sprites[1];
        currentHorns.sprite = sprites[2];

        currentHat.gameObject.SetActive(true);
        
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
            default:
                currentHat.GetComponent<CanvasGroup>().alpha = 0;
                currentHat.sprite = _avatar.noHat.sprite;
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }
}
