using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    private List<Circle> _circles = new List<Circle>();
    private int _totalCapacity;
    
    private void OnEnable()
    {
        _circles = FindObjectsByType<Circle>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        foreach (Circle circle in _circles) _totalCapacity += circle.Capacity;
    }
    
    
}
