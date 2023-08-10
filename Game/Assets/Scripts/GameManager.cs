using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Character PlayerCharacter;

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

        if(PlayerCharacter.CurrentState == Character.CharacterState.Dead)
        {
            IsGameOver = true;
            GameOver();
        }
    }
}
