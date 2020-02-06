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
    public int increaseIssuesNumberEveryRepairedCars = 3;
    public float difficultyIncreasePerRepairedCar = 0.5f;
    public List<Car> cars;

    [Header("Car params")]
    public int carSlots = 5;
    public float carBoxLength = 5f;
    public Transform startPoint;
    public Transform destinationPoint;
    public float carRepairTime = 0f;
    public float maximumRemainingTime = 60f;
    public float remainingTime = 60f;
    public float timeIncreasePerRepairedCar = 20f;
    public float elapsedTime = 0f;
    public bool mustForceCarSpawn = false;
    public float elapsedTimeSinceLastCarSpawn = 0f;
    public float startPositionOffset = 20f;
    #endregion

    #region PRIVATE VARIABLES
    private bool playerHasPlayedGame = false;
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
        player.canMove = isGameRunning;

        if (!isGameRunning)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            GameOver();
            return;
        }

        if (mustForceCarSpawn)
        {
            SpawnCar();
            elapsedTimeSinceLastCarSpawn = elapsedTime;

            mustForceCarSpawn = false;
        }
        else
        {

            if (elapsedTime > 0 && elapsedTimeSinceLastCarSpawn > 0)
            {
                uiController.UpdateNormalizedSpawnTime(Mathf.Min(maximumRemainingTime, remainingTime) / maximumRemainingTime);
            }

            if (elapsedTime >= elapsedTimeSinceLastCarSpawn + carRepairTime)
            {
                if (cars.Count < carSlots)
                {
                    SpawnCar();
                    elapsedTimeSinceLastCarSpawn = elapsedTime;
                }
            }
        }

    }

    private void LateUpdate()
    {
        //se premo il tasto azione
        if (Input.GetButtonDown("Cancel"))
        {
            if (!playerHasPlayedGame)
                return;

            if (isGameRunning)
                Pause();
            else
                Resume();
        }
    }

    #endregion

    public void AddPoints(int points)
    {
        this.points += points;
    }

    public void FixCar(Car car)
    {
        cars.Remove(car);

        repairedCars += 1;
        uiController.UpdatePoints(repairedCars, points);

        remainingTime += timeIncreasePerRepairedCar;

        if (cars.Count < carSlots)
        {
            mustForceCarSpawn = true;
        }
    }

    #region PUBLIC INTERFACE
    public void Play()
    {
        playerHasPlayedGame = true;
        isGameRunning = true;

        uiController.Play();
    }

    public void Pause()
    {
        isGameRunning = false;

        uiController.Pause();
    }

    public void Resume()
    {
        isGameRunning = true;

        uiController.Resume();
    }

    public void Restart()
    {
        Debug.Log("Restart");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        uiController.Exit();
    }

    #endregion

    #region PRIVATE METHODS
    private void SpawnCar()
    {
        Car car = damagedCarConfigurator.GetCar();
        car.transform.position = startPoint.position;
        car.gameObject.layer = LayerMask.NameToLayer("Car");

        car.LoadParts();
        carRepairTime = car.GetRepairTime();

        cars.Add(car);
    }

    private void GameOver()
    {
        playerHasPlayedGame = false;
        isGameRunning = false;

        uiController.GameOver();
    }

    #endregion


}
