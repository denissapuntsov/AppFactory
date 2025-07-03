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
    Peaks,
    Lakes,
    Caverns
}

public enum CircleTemperature
{
    Cold,
    Hot
}
