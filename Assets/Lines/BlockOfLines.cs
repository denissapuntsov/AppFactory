using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockOfLines", menuName = "Scriptable Objects/BlockOfLines")]
public class BlockOfLines : ScriptableObject
{
    public List<string> lines = new List<string>();
}
