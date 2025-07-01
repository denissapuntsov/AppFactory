using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CircleManager : MonoBehaviour
{
    private List<Circle> _circles = new List<Circle>();
    public static Circle CurrentCircle;

    [SerializeField] private List<CircleVisual> circleVisuals;
    
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

    public Sprite GetCircleSprite(CircleTemperature temperature, CircleEnvironment environment)
    {
        foreach (CircleVisual visual in circleVisuals)
        {
            if (visual.temperature == temperature && visual.environment == environment) return visual.sprite;
        }
        
        Debug.LogError($"No visual corresponding to pair {environment}/{temperature} found; please check circleVisuals list.");
        return null;
    }

    public void BlockRandomCircle(int amount)
    {
        foreach (Circle circle in _circles) circle.IsBlocked = false;
        for (int i = 0; i < amount; i++)
        {
            _circles[Random.Range(0, _circles.Count)].IsBlocked = true;
        }
    }
    
}

public enum CircleEnvironment 
{
    Plains,
    Cliffs,
    Lakes,
    Caverns
}

public enum CircleTemperature
{
    Cold,
    Hot
}
