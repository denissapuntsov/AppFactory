using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlavorTextGroup", menuName = "Scriptable Objects/Flavor Text Group")]
public class FlavorTextGroup : ScriptableObject
{
    public CustomerType targetType;

    public List<string> neutralLines;
    public List<FlavorTextEnv> flavorTextEnvs;
    public List<FlavorTextTemp> flavorTextTemps;
    public List<FlavorTextComplex> flavorTextComplexes;
}

[System.Serializable]
public struct FlavorTextEnv
{
    public CircleEnvironment environment;
    public string text;
}

[System.Serializable]
public struct FlavorTextTemp
{
    public CircleTemperature temperature;
    public string text;
}

[System.Serializable]
public struct FlavorTextComplex
{
    public CircleEnvironment environment;
    public CircleTemperature temperature;
    public string text;
}
