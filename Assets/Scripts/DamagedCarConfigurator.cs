using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartConfiguration
{
    public string name;
    public GameObject repaired;
    public GameObject damaged;
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

    // public Car GetRandomCar (float difficulty)
    public Car GetRandomCar()
    {
        CarConfiguration randomizedCar = carConfiguration[Random.Range(0, carConfiguration.Length)];

        for (int i = 0; i < randomizedCar.partsConfigurations.Length; i++)
        {
            PartConfiguration partConfiguration = randomizedCar.partsConfigurations[i];
            float isDamaged = Mathf.RoundToInt(Random.Range(0, 1));

            GameObject prefab;
            if (isDamaged == 1)
            {
                prefab = partConfiguration.damaged;
            }
            else
            {
                prefab = partConfiguration.repaired;
            }

            GameObject instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Transform parent = instance.transform.Find(partConfiguration.name);
            if (parent)
            {
                instance.transform.SetParent(parent);
            }
        }

        return randomizedCar.carRoot.gameObject.GetComponent<Car>();
    }
}
