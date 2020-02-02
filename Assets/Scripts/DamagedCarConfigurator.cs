using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[System.Serializable]
public class PartConfiguration
{
    public string name;
    public GameObject[] repaired;
    public GameObject[] damaged;
}

[System.Serializable]
public class CarConfiguration
{
    public string name;
    public GameObject carRoot;
    public PartConfiguration[] partsConfigurations;

}
public class DamagedCarConfigurator : MonoBehaviour
{
    public CarConfiguration[] carConfiguration;

    public void Shuffle(PartConfiguration[] partsConfigurations)
    {
        for (int i = 0; i < partsConfigurations.Length; i++)
        {
            int randomIndex = Random.Range(0, partsConfigurations.Length);
            PartConfiguration tmp = partsConfigurations[randomIndex];
            partsConfigurations[randomIndex] = partsConfigurations[i];
            partsConfigurations[i] = tmp;
        }
    }

    // FIXME: randomWheelIndex selection
    public Car GetCar()
    {
        CarConfiguration randomCar = carConfiguration[Random.Range(0, carConfiguration.Length)];
        GameObject carInstance = GameObject.Instantiate(randomCar.carRoot, Vector3.zero, randomCar.carRoot.transform.localRotation);
        int randomWheeIndex = Random.Range(0, 6);

        int issuesNumber = GameManager.Instance.repairedCars / 3 + 1;
        int selectedIssues = 0;
        Shuffle(randomCar.partsConfigurations);
        for (int i = 0; i < randomCar.partsConfigurations.Length; i++)
        {
            PartConfiguration partConfiguration = randomCar.partsConfigurations[i];
            GameObject prefab;
            if (selectedIssues < issuesNumber)
            {
                prefab = partConfiguration.damaged[partConfiguration.damaged.Length > 1 ? randomWheeIndex : 0];
            }
            else
            {
                prefab = partConfiguration.repaired[partConfiguration.repaired.Length > 1 ? randomWheeIndex : 0];
            }

            GameObject instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Transform parent = carInstance.transform.Find(partConfiguration.name);
            if (parent)
            {
                instance.transform.SetParent(parent);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localEulerAngles = Vector3.zero;
            }

            selectedIssues++;
        }


        return randomCar.carRoot.gameObject.GetComponent<Car>();
    }

    [ContextMenu("Create car in editor")]
    private void CreateCarInEditor()
    {
        Car car = GetCar();
        car.transform.position = Vector3.zero;
    }
}
