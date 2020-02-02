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

    private CarConfiguration randomizedCar;
    private GameObject carRoot;
    private GameObject prefab;
    private PartConfiguration partConfiguration;
    private List<PartConfiguration> copyCarParts;
    private int isDamaged;
    private GameObject instance;
    private Transform parent;

    public Car GetRandomCar(int difficulty)
    {
        randomizedCar = carConfiguration[Random.Range(0, carConfiguration.Length)];
        carRoot = GameObject.Instantiate(randomizedCar.carRoot, Vector3.zero, randomizedCar.carRoot.transform.localRotation);

        //Scelgo una tra le ruote disponibili (da migliorare)
        int randomWheelIdx = Random.Range(0, 6);

        copyCarParts = randomizedCar.partsConfigurations.ToList();

        int issues = 3;

        for (int i = 0; i < issues; i++) 
        { 
            partConfiguration = copyCarParts[Random.Range(0, copyCarParts.Count)];
            
            GeneratePart(partConfiguration, randomWheelIdx, true, carRoot);

            copyCarParts.Remove(partConfiguration);
        }

        for (int i = 0; i < copyCarParts.Count; i++)
        {
            partConfiguration = copyCarParts[Random.Range(0, copyCarParts.Count)];
            
            GeneratePart(partConfiguration, randomWheelIdx, false, carRoot);
        }

        return randomizedCar.carRoot.gameObject.GetComponent<Car>();
    }

    private void GeneratePart (PartConfiguration partConfiguration, int randomWheelIdx, bool damaged, GameObject carRoot)
    {
        if(damaged)
        prefab = partConfiguration.damaged[partConfiguration.damaged.Length > 1 ? randomWheelIdx : 0];
        else
        prefab = partConfiguration.repaired[partConfiguration.repaired.Length > 1 ? randomWheelIdx : 0];
        
        instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        parent = carRoot.transform.Find(partConfiguration.name);
        if (parent)
        {
            instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
        }
    }

    [ContextMenu("Create car in editor")]
    private void CreateCarInEditor()
    {
        Car car = GetRandomCar(1);
        car.transform.position = Vector3.zero;
    }
}
