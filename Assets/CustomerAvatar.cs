using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerAvatar : MonoBehaviour
{
    [Header("Weighted Avatar Pools")] 
    [SerializeField] private List<AvatarElement> eyes;
    [SerializeField] private List<AvatarElement> mouths;
    [SerializeField] private List<AvatarElement> horns;

    [Header("Type-Specific Hats")] 
    public AvatarElement deepHat;
    public AvatarElement commonHat;
    public AvatarElement noHat;

    private List<Sprite> _weightedEyes, _weightedMouths, _weightedHorns; 

    private void OnEnable()
    {
        _weightedEyes = GetSpritePool(eyes);
        _weightedMouths = GetSpritePool(mouths);
        _weightedHorns = GetSpritePool(horns);
    }

    private List<Sprite> GetSpritePool(List<AvatarElement> listToProcess)
    {
        List<Sprite> result = new List<Sprite>();
        
        foreach (AvatarElement element in listToProcess)
        {
            for (int i = element.weight; i > 0; i--)
            {
                result.Add(element.sprite);
            }
        }
        
        return result;
    }

    public List<Sprite> GetRandomAvatar()
    {
        return new List<Sprite>()
        {
            _weightedEyes[Random.Range(0, _weightedEyes.Count)],
            _weightedMouths[Random.Range(0, _weightedMouths.Count)],
            _weightedHorns[Random.Range(0, _weightedHorns.Count)]
        };
    }
}
