using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleManager : MonoBehaviour
{
    private List<Circle> _circles = new List<Circle>();
    public static Circle CurrentCircle;
    
    private void Awake()
    {
        _circles = FindObjectsByType<Circle>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        List<CircleEnvironment> environmentPool = new List<CircleEnvironment>();
        foreach (Circle circle in _circles)
        {
            // assign environments, so that there is at least 1 of each
            if (environmentPool.Count == 0) environmentPool = Enum.GetValues(typeof(CircleEnvironment)).Cast<CircleEnvironment>().ToList();
            CircleEnvironment newEnvironment = environmentPool[Random.Range(0, environmentPool.Count)];
            circle.environment = newEnvironment;
            environmentPool.Remove(newEnvironment);
        }
    }
    
    
}

public enum CircleEnvironment 
{
    Rocks,
    Cliffs,
    Lakes,
    Winds
}

public enum CircleTemperature
{
    Cold,
    Hot
}
