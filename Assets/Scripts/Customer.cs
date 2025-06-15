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
    [Range(1, 9)] public int targetCircle;

    [SerializeField] private TextMeshProUGUI customerDebugInfoText;

    [SerializeField] private Image currentEyes, currentMouth, currentHorns, currentHat;
    [SerializeField] private List<Sprite> eyes, mouths, horns, hats;
    [SerializeField] private Sprite deepHat, commonHat, noHat;

    private void OnEnable()
    {
        Randomise();
        GameManager.OnPlace += Randomise;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Randomise()
    {
        Debug.Log("Randomised");
        currentType = (CustomerType)Random.Range(0, Enum.GetNames(typeof(CustomerType)).Length);
        targetCircle = currentType == CustomerType.Deep ? 8 : Random.Range(0, 9);
        
        customerDebugInfoText.text = $"Target Circle: {targetCircle}\nCustomer Type: {currentType}";
        GameManager.CurrentGameState = GameState.Idle;
        
        currentEyes.sprite = eyes[Random.Range(0, eyes.Count)];
        currentMouth.sprite = mouths[Random.Range(0, mouths.Count)];
        currentHorns.sprite = horns[Random.Range(0, horns.Count)];

        currentHat.gameObject.SetActive(true);
        
        switch (currentType)
        {
            case CustomerType.Common:
                currentHat.GetComponent<CanvasGroup>().alpha = 1;
                currentHat.sprite = Random.Range(0, 2) == 1 ? commonHat : noHat;
                break;
            case CustomerType.Deep:
                currentHat.GetComponent<CanvasGroup>().alpha = 1;
                currentHat.sprite = Random.Range(0, 2) == 1 ? deepHat : noHat;
                break;
            default:
                currentHat.GetComponent<CanvasGroup>().alpha = 0;
                currentHat.sprite = noHat;
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }
}
