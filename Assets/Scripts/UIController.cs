using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("CAMERA")]
    public Animator camAnimator;
    public Animator creditsAnimator;

    [Header("UI")]
    public GameObject panelPause;
    public GameObject panelGame;

    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject restartButton;

    public Text carsLabel;
    public Text moneyLabel;

    public Image batteryLevelImage;
    public Sprite[] batterySprites;


    void Start()
    {
        panelPause.SetActive(true);

        playButton.SetActive(true);
        pauseButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public void Play()
    {
        panelPause.SetActive(false);
        panelGame.SetActive(true);

        camAnimator.SetBool("pause", false);
    }

    public void Pause()
    {
        pauseButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseButton);
        playButton.SetActive(false);
        restartButton.SetActive(false);

        panelPause.SetActive(true);
        panelGame.SetActive(false);

        camAnimator.SetBool("pause", true);
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        panelGame.SetActive(true);

        camAnimator.SetBool("pause", false);
    }

    public void Restart()
    {
        panelPause.SetActive(false);
        panelGame.SetActive(true);

        carsLabel.text = "0";
        moneyLabel.text = "0";
    }

    public void GameOver()
    {
        panelGame.SetActive(false);
        panelPause.SetActive(true);

        restartButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
        playButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void Exit()
    {
        creditsAnimator.SetBool("credits", true);
    }

    public void QuitApplication()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }

    /// <summary>
    /// Aggiorna lo stato della batteria indicando quanto manca al prossimo spawn.
    /// </summary>
    /// <param name="value">Il valore indica il tempo che manca al prossimo spawn. Il valore è normalizzato 0-1.</param>
    public void UpdateNormalizedSpawnTime(float value)
    {
        //Trovo la sprite più adatta
        int sprite = (int)(value / (1f / (float)batterySprites.Length));
        // Debug.Log(sprite);
        batteryLevelImage.sprite = batterySprites[sprite];
    }

    public void UpdatePoints(int cars, int points)
    {
        carsLabel.text = cars.ToString();
        moneyLabel.text = points.ToString();
    }
}
