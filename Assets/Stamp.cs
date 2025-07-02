using UnityEngine;
using UnityEngine.UI;

public class Stamp : MonoBehaviour
{
    [SerializeField] private Image stampFace;
    private Image _stampBase;

    private void OnEnable()
    {
        stampFace.color = Color.clear;
    }

    public void DisplayStamp(float score)
    {
        if (score <= 0) stampFace.color = Color.red;
        else if (score > 0.75f) stampFace.color = Color.green;
        else stampFace.color = Color.yellow;
        
        stampFace.gameObject.transform.localPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
    }

    private void OnDisable()
    {
        stampFace.color = Color.clear;
    }
}
