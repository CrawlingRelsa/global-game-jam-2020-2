using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region PUBLIC VARIABLES
    public Player player;
    public bool isGameRunning = false;

    [Header("Controllers")]
    public UIController uiController;
    public DamagedCarConfigurator damagedCarConfigurator;

    [Header("Cars")]
    public int points = 0;
    public int repairedCars = 0;
    public float difficultyIncreasePerRepairCar = 0.5f;
    public List<Car> cars;

    [Header("Car params")]
    public int carSlots = 5;
    public float carBoxLength = 5f;
    public Transform startPoint;
    public Transform destinationPoint;
    public float carRepairTime;
    public float elapsedTime = 0f;
    public float elapsedTimeSinceLastCarSpawn = 0f;
    public float startPositionOffset = 20f;
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
        startPoint.position = new Vector3(destinationPoint.position.x, destinationPoint.position.y, destinationPoint.position.z - (carBoxLength * carSlots) - startPositionOffset);
    }

    void Update()
    {
        // FIXME
        player.canMove = isGameRunning;

        if (!isGameRunning) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= elapsedTimeSinceLastCarSpawn + carRepairTime)
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

        SpawnCar();
    }

    public void Pause()
    {
        Debug.Log("Pause");

        isGameRunning = false;

        uiController.Pause();
    }

    public void Resume()
    {
        Debug.Log("Resume");

        isGameRunning = true;

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

        carRepairTime = car.GetRepairTime();
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
