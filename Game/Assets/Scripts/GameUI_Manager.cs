using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI_Manager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    public GameManager GM;

    public TMPro.TextMeshProUGUI CoinText;
    public Slider HealthSlider;
    public Button RestartBtn;
    public Button MainMenuBtn;

    public GameObject UI_Pause;
    public GameObject UI_GameOver;
    public GameObject UI_GameIsFinished;

    private enum GameUI_State
    {
        GamePlay,Pause,GameOver,GameIsFinished
    }
    GameUI_State CurrentState;

    private void Start()
    {
        SwitchUIStateTo(GameUI_State.GamePlay);
    }
    void Update()
    {
        HealthSlider.value = GM.PlayerCharacter.GetComponent<Health>().CurrentHealthPercentage;
        CoinText.text = GM.PlayerCharacter.Coin.ToString();
    }

    void SwitchUIStateTo(GameUI_State state)
    {
        UI_Pause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_GameIsFinished.SetActive(false);


        Time.timeScale = 1;

        switch (state) 
        {
            case GameUI_State.GamePlay:
                break;
            case GameUI_State.Pause:
                Time.timeScale = 0;
                UI_Pause.SetActive(true);
                break;
            case GameUI_State.GameOver:
                UI_GameOver.SetActive(true);
                break;
            case GameUI_State.GameIsFinished:
                UI_GameIsFinished.SetActive(true);
                break;
        }

        CurrentState = state;

    }

    public void TogglePauseUI()
    {
        if (CurrentState == GameUI_State.GamePlay)
            SwitchUIStateTo(GameUI_State.Pause);
        else if (CurrentState == GameUI_State.Pause)
            SwitchUIStateTo(GameUI_State.GamePlay);
    }

    public void Button_MainMenu()
    {
        GM.ReturnTotheMainMenu();
    }

    public void Button_Restart()
    {
        GM.Restart();
    }

    public void Button_Attack()
    {
        playerInput.AttackClick();
    }
    public void Button_Slide()
    {
        playerInput.SlideClick();
    }
    public void ShowGameOverUI()
    {
        SwitchUIStateTo(GameUI_State.GameOver);
    }
    public void ShowGameIsFinishedUI()
    {
        SwitchUIStateTo(GameUI_State.GameIsFinished);
    }
}
