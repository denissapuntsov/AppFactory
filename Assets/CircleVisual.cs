using UnityEngine;

[CreateAssetMenu(fileName = "CircleVisual", menuName = "Scriptable Objects/CircleVisual")]
public class CircleVisual : ScriptableObject
{
    public Sprite sprite;
    public CircleTemperature temperature;
    public CircleEnvironment environment;
}
