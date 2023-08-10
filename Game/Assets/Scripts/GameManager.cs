using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Character PlayerCharacter;
    public GameUI_Manager UIManager;


    private bool IsGameOver;

    private void Awake()
    {
        PlayerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    public void GameIsFinished()
    {
        Debug.Log("Game is Finished");
    }

    private void Update()
    {
        if (IsGameOver)
            return;

        if (Input.GetKeyUp(KeyCode.Escape))
            UIManager.TogglePauseUI();

        if(PlayerCharacter.CurrentState == Character.CharacterState.Dead)
        {
            IsGameOver = true;
            GameOver();
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
