using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region PUBLIC VARIABLES
    public Car[] availableCars;
    public List<Car> cars;

    [Header("Car params")]
    public int carSlots = 5;
    public float carBoxLength = 5f;
    public Transform startPoint;
    public Transform destinationPoint;
    #endregion

    #region PRIVATE VARIABLES
    public float carSpawnInterval = 5f;
    public float elapsedTime = 0f;
    public float elapsedTimeSinceLastCarSpawn = 0f;
    #endregion

    #region UNITY INTERFACE
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= elapsedTimeSinceLastCarSpawn + carSpawnInterval)
        {
            SpawnCar();
            elapsedTimeSinceLastCarSpawn = elapsedTime;
        }
    }
    #endregion

    #region PUBLIC INTERFACE
    public void Init()
    {
        startPoint.position = new Vector3(destinationPoint.position.x - (carBoxLength * carSlots), destinationPoint.position.y, destinationPoint.position.z);
    }
    #endregion

    #region PRIVATE METHODS
    private void SpawnCar()
    {
        if (cars.Count >= carSlots)
        {
            //TODO Game Over
            return;
        }

        Car car = availableCars[Random.Range(0, availableCars.Length)];
        cars.Add(car);

        GameObject instance = GameObject.Instantiate(car.gameObject, startPoint.position, car.transform.rotation);
        instance.layer = LayerMask.NameToLayer("car");
    }
    #endregion


}
