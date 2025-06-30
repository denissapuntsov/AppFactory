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
        stampFace.color = score switch
        {
            0 => Color.red,
            > 0.75f => Color.green,
            _ => Color.yellow
        };
        
        stampFace.gameObject.transform.localPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
    }

    private void OnDisable()
    {
        stampFace.color = Color.clear;
    }
}
