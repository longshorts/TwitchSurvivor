using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class GameManager : Singleton<GameManager>
{

    public enum GameStateType
    {
        Journey,
        Combat
    }

    public UnityEvent ChangeGameStateEvent;
    public GameStateType GameState { get; private set; }

    void Start()
    {
        GameState = GameStateType.Combat;
        ChangeGameState(GameStateType.Combat);
    }

    /// <summary>
    /// Changes the game state
    /// </summary>
    /// <param name="newState">New state to change to</param>
    public void ChangeGameState(GameStateType newState)
    {
        GameState = newState;
        ChangeGameStateEvent.Invoke();
    }
}
