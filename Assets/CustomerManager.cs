using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    private void Start()
    {
        Randomise();
    }

    private void Randomise()
    {
        Customer.Instance.targetCircle = Random.Range(1, 9);
        // randomise visuals
    }

    private void OnDisable()
    {
        InputManager.OnTap -= Randomise;
    }
}
