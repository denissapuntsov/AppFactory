using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CircleManager : MonoBehaviour
{
    [SerializeField] private List<Circle> circles ;
    public static Circle CurrentCircle;

    [SerializeField] private List<CircleVisual> circleVisuals;
    
    private void Awake()
    {
        List<CircleEnvironment> environmentPool = new List<CircleEnvironment>();
        CircleEnvironment lastEnvironment = new CircleEnvironment();
        foreach (Circle circle in circles)
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
        foreach (Circle circle in circles) circle.IsBlocked = false;
        for (int i = 0; i < amount; i++)
        {
            circles[Random.Range(0, circles.Count)].IsBlocked = true;
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
