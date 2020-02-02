using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("CAMERA")]
    public Animator camAnimator;

    [Header("UI")]
    public GameObject panelPause;
    public GameObject panelGame;

    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject restartButton;

    public Text carsLabel;
    public Text moneyLabel;


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
        playButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void UpdatePoints(int cars, int points)
    {
        carsLabel.text = cars.ToString();
        moneyLabel.text = points.ToString();
    }
}
