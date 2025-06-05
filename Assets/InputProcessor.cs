using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InputProcessor : MonoBehaviour
{
    private bool _hasTapped, _hasSwiped;
    private string _lastSwipeDirection;

    private void OnEnable()
    {
        InputManager.OnTap += OnTap;
        InputManager.OnSwipe += OnSwipe;
    }

    private void LateUpdate() 
    {
        if (_hasSwiped)
        {
            ProcessSwipe(_lastSwipeDirection);
            _hasSwiped = false;
            _hasTapped = false;
        }

        else if (_hasTapped)
        {
            ProcessTap();
            _hasTapped = false;
        }
    }

    private void OnTap()
    {
        _hasTapped = true;
    }

    private void OnSwipe(string direction)
    {
        _hasSwiped = true;
        _lastSwipeDirection = direction;
    }
    
    private void ProcessTap()
    {
        Debug.Log("tap");
    }

    private void ProcessSwipe(string direction)
    {
        Debug.Log("swipe");
    }
}
