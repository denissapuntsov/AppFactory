using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{   
    public static Customer Instance;
    [Range(1, 9)] public int targetCircle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        Randomise();
    }

    private void Randomise()
    {
        targetCircle = Random.Range(1, 9);
        // randomise sprite elements
        // depending on customer type, change some sprite elements and/or assign flavor text
    }

    private enum CustomerType
    {
        Common,
        Deep,
        Precise,
        Avoidant
    }
}
