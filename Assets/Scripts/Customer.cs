using System;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Serialization;
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

    public void Randomise()
    {
        Debug.Log("Randomised");
        currentType = (CustomerType)Random.Range(0, Enum.GetNames(typeof(CustomerType)).Length);
        targetCircle = currentType == CustomerType.Deep ? 8 : Random.Range(0, 9);
        
        customerDebugInfoText.text = $"Target Circle: {targetCircle}\nCustomer Type: {currentType}";
        GameManager.CurrentGameState = GameState.Idle;
    }

    private void OnDisable()
    {
        GameManager.OnPlace -= Randomise;
    }
}
