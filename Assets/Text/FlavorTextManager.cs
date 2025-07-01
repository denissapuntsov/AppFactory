using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class FlavorTextManager : MonoBehaviour
{
    [SerializeField] private List<FlavorTextGroup> flavorTextGroups;

    public string GetFlavorText(Customer currentCustomer)
    {
        string lineToReturn = null;
        FlavorTextGroup selectedGroup = null;
        selectedGroup = GetGroupByCustomerType(currentCustomer);
        
        if (!selectedGroup)
        {
            Debug.LogError("No corresponding group found!");
            return null;
        }

        lineToReturn = GetLineByRequirements(selectedGroup, currentCustomer);

        return lineToReturn;
    }

    private string GetLineByRequirements(FlavorTextGroup selectedGroup, Customer currentCustomer)
    {
        List<string> lines = new List<string>();
        
        switch (selectedGroup.targetType)
        {
            case CustomerType.Neutral:
                lines = selectedGroup.neutralLines;
                break;
            
            case CustomerType.Env:
            case CustomerType.AntiEnv:
                foreach (FlavorTextEnv flavorText in selectedGroup.flavorTextEnvs)
                {
                    if (flavorText.environment == currentCustomer.targetEnvironment) lines.Add(flavorText.text);
                }

                break;

            case CustomerType.Temp:
            case CustomerType.AntiTemp:
                foreach (FlavorTextTemp flavorText in selectedGroup.flavorTextTemps)
                {
                    if (flavorText.temperature == currentCustomer.targetTemperature) lines.Add(flavorText.text);
                }

                break;

            case CustomerType.NegativeComplex:
            case CustomerType.PositiveComplex:
                foreach (FlavorTextComplex flavorText in selectedGroup.flavorTextComplexes)
                {
                    if (flavorText.temperature == currentCustomer.targetTemperature &&
                        flavorText.environment == currentCustomer.targetEnvironment) 
                        lines.Add(flavorText.text);
                }

                break;
        }
        
        return lines[Random.Range(0, lines.Count)];
    }

    private FlavorTextGroup GetGroupByCustomerType(Customer customer)
    {
        foreach (FlavorTextGroup group in flavorTextGroups)
        {
            if (group.targetType == customer.currentType) return group;
        }

        return null;
    }
}
