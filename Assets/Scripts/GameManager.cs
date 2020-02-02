using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region PUBLIC VARIABLES
    public bool isGameRunning = false;

    [Header("Controllers")]
    public UIController uiController;
    public DamagedCarConfigurator damagedCarConfigurator;

    [Header("Cars")]
    public float points = 0f;
    public int repairedCars = 0;
    public Car[] availableCars;
    public List<Car> cars;

    [Header("Car params")]
    public int carSlots = 5;
    public float carBoxLength = 5f;
    public Transform startPoint;
    public Transform destinationPoint;
    public float carSpawnInterval = 5f;
    public float elapsedTime = 0f;
    public float elapsedTimeSinceLastCarSpawn = 0f;
    #endregion

    #region PRIVATE VARIABLES

    #endregion

    #region UNITY INTERFACE
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startPoint.position = new Vector3(destinationPoint.position.x, destinationPoint.position.y, destinationPoint.position.z - (carBoxLength * carSlots));
    }

    void Update()
    {
        if (!isGameRunning) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= elapsedTimeSinceLastCarSpawn + carSpawnInterval)
        {
            SpawnCar();
            elapsedTimeSinceLastCarSpawn = elapsedTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }
    #endregion

    #region PUBLIC INTERFACE
    public void Play()
    {
        Debug.Log("Play");

        isGameRunning = true;

        uiController.Play();
    }

    public void Pause()
    {
        Debug.Log("Pause");

        uiController.Pause();
    }

    public void Resume()
    {
        Debug.Log("Resume");

        uiController.Resume();
    }

    public void Restart()
    {
        Debug.Log("Restart");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region PRIVATE METHODS
    private void SpawnCar()
    {
        if (cars.Count >= carSlots)
        {
            //TODO Game Over
            GameOver();
            return;
        }

        Car car = damagedCarConfigurator.GetCar();
        car.LoadParts();
        cars.Add(car);

        car.transform.position = startPoint.position;
        car.gameObject.layer = LayerMask.NameToLayer("Car");
    }

    private void GameOver()
    {
        isGameRunning = false;

        uiController.GameOver();
    }

    #endregion


}
