using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class FlavorTextManager : MonoBehaviour
{
    public List<FlavorText> neutralTexts, positiveTemperatureTexts, negativeTemperatureTexts, positiveEnvironmentTexts, negativeEnvironmentTexts;

    public string GetFlavorText(Customer currentCustomer)
    {
        string textToReturn = null;
        switch (currentCustomer.currentType)
        {
            case CustomerType.Neutral:
                textToReturn = GetText();
                break;
            case CustomerType.Positive:
                textToReturn = "Wants: ";
                textToReturn += currentCustomer.targetIndex == 1
                    ? $"{currentCustomer.targetEnvironment}"
                    : $"{currentCustomer.targetTemperature}";
                break;
            case CustomerType.PositiveComplex:
                textToReturn = "Wants: ";
                textToReturn += $"{currentCustomer.targetTemperature} and {currentCustomer.targetEnvironment}";
                break;
            case CustomerType.Negative:
                textToReturn = "Doesn't want: ";
                textToReturn += currentCustomer.targetIndex == 1
                    ? $"{currentCustomer.targetEnvironment}"
                    : $"{currentCustomer.targetTemperature}";
                break;
            case CustomerType.NegativeComplex:
                textToReturn = "Doesn't want: ";
                textToReturn += $"{currentCustomer.targetTemperature} and {currentCustomer.targetEnvironment}";
                break;
        }
        return textToReturn;
    }

    private string GetText(bool positive, CircleEnvironment environment)
    {
        return null;
    }

    private string GetText(bool positive, CircleTemperature temperature)
    {
        return null;
    }

    private string GetText()
    {
        return neutralTexts[Random.Range(0, neutralTexts.Count)].text;
    }
}
