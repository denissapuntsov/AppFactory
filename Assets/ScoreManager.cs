using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, customerSatisfactionCoefficientText;
    
    private float _customerSatisfactionCoefficient;

    private float _score;
    public float Score
    {
        get => _score;
        private set
        {
            _score = value;
            scoreText.text = $"Score: {value}";
            _customerSatisfactionCoefficient = _score / (ShiftManager.Instance.CurrentCustomerIndex + 1) * 100;
            customerSatisfactionCoefficientText.text = $"Customer satisfaction: {_customerSatisfactionCoefficient}%";
        }
    }
    
    public void AddPoints(Customer customer, int circleIndex)
    {
        CustomerType customerType = customer.currentType;
        int distance;
        
        switch (customerType)
        {
            case CustomerType.Common:
                Score += 1;
                break;
            case CustomerType.Deep:
                if (circleIndex < 4) break;
                if (circleIndex == 4)
                {
                    Score += 0.5f;
                    break;
                }
                Score += 0.5f + (circleIndex - 4) * 0.125f;
                break;
            case CustomerType.Avoidant:
                distance = Mathf.Abs(circleIndex - customer.targetCircle);
                Score += Mathf.Clamp(distance / 2f, min: 0f, max: 1f);
                break;
            case CustomerType.Precise:
                distance = Mathf.Abs(circleIndex - customer.targetCircle);
                Score += distance > 2.0f ? 0 : distance / 2.0f;
                break;
        }
    }
}
