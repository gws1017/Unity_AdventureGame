using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Character PlayerCharacter;
    public GameUI_Manager UIManager;

    [SerializeField]
    private string GameScneeSound;
    [SerializeField]
    private string MainMenuSound;
    private bool IsGameOver;

    private void Awake()
    {
        PlayerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")
        {
            SoundManager.instance.PlayBGM(GameScneeSound);
        }
        else if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            SoundManager.instance.PlayBGM(MainMenuSound);
        }
    }

    private void GameOver()
    {
        UIManager.ShowGameOverUI();
    }

    public void GameIsFinished()
    {
        UIManager.ShowGameIsFinishedUI();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (IsGameOver)
                return;

            if (Input.GetKeyUp(KeyCode.Escape))
                UIManager.TogglePauseUI();

            if (PlayerCharacter.CurrentState == Character.CharacterState.Dead)
            {
                IsGameOver = true;
                GameOver();
            }
        }
    }

    public void ReturnTotheMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
