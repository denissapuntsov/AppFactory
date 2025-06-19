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
    
    public void AddPoints(Customer customer, Circle circle)
    {
        CustomerType customerType = customer.currentType;
        
        switch (customerType)
        {
            case CustomerType.Positive:
                switch (customer.targetIndex)
                {
                    case 1:
                        Score += customer.targetEnvironment == circle.environment ? 1f : 0;
                        break;
                    case 2:
                        Score += customer.targetTemperature == circle.temperature ? 1f : 0;
                        break;
                }
                break;
            case CustomerType.Negative:
                switch (customer.targetIndex)
                {
                    case 1:
                        Score += customer.targetEnvironment == circle.environment ? 0 : 1f;
                        break;
                    case 2:
                        Score += customer.targetTemperature == circle.temperature ? 0 : 1f;
                        break;
                }
                break;
            case CustomerType.Neutral:
                Score += 1;
                break;
            case CustomerType.PositiveComplex:
                Score += customer.targetEnvironment == circle.environment ? 0.5f : 0;
                Score += customer.targetTemperature == circle.temperature ? 0.5f : 0;
                break;
            case CustomerType.NegativeComplex:
                Score += customer.targetEnvironment == circle.environment ? 0 : 0.5f;
                Score += customer.targetTemperature == circle.temperature ? 0 : 0.5f;
                break;
            
            /*case CustomerType.Common:
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
                break;*/
        }
    }
}
