using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState State;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.Day);
    }

    private void OnEnable()
    {
        TavernEventsManager.OnHeartRepaired += OnHeartRepairedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnHeartRepaired -= OnHeartRepairedHandler;
    }

    private void OnHeartRepairedHandler()
    {
        Instance.StartCoroutine(NightTimerCoroutine());
    }

    public static void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.GameStart:
                StartScreenHandler();
                break;
            case GameState.Day:
                DayHandler();
                break;
            case GameState.Night:
                NightHandler();
                break;
            case GameState.NightAutoFight:
                NightAutoFightHandler();
                break;
            default:
                break;
        }
    }

    private static void StartScreenHandler()
    {
      
    }

    private static void NightAutoFightHandler()
    {
        TavernEventsManager.NightStarted();
        //Instance.StartCoroutine(NightAutoFightTimingCoroutine(5));
    }

    private static IEnumerator NightAutoFightTimingCoroutine(int seconds)
    {
        int counter = seconds;
        TavernEventsManager.CameraSwitchedToCardGame();
        while (counter > 0)
        {
            counter--;
            if (counter == 3)
            {
                TavernEventsManager.SwitchedToNigthAutoFightCanvas();
            }
            yield return new WaitForSeconds(1);
        }
    }

    private static void DayHandler()
    {
        TavernEventsManager.DayStarted();
        TavernEventsManager.CameraSwitchedToFollowPlayer();
        TavernEventsManager.SwitchedToDayCanvas();
    }

    private static void NightHandler()
    {
        TavernEventsManager.NightStarted();
        Instance.StartCoroutine(CardGameTiming(6));
    }

    private static IEnumerator NightTimerCoroutine()
    {
        yield return new WaitForSeconds(GameConfigManager.SecondsToNightStarts);
        UpdateGameState(GameState.NightAutoFight);
    }

    private static IEnumerator CardGameTiming(int seconds)
    {
        int counter = seconds;
        TavernEventsManager.CameraSwitchedToCardGame();
        while (counter > 0)
        {
            counter--;
            if(counter == 3)
            {
                TavernEventsManager.SwitchedToNightCanvas();
            }
            if(counter == 2)
            {
                TavernEventsManager.CardsRendered();
            }
            yield return new WaitForSeconds(1);
        }
    }

    
    
    public enum GameState
    {
        GameStart,
        Day,
        Night,
        NightAutoFight
    }
}