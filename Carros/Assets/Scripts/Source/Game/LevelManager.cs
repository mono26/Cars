// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Level Manager settings")]
    [SerializeField]
    protected AudioClip loseSfx;
    [SerializeField]
    protected AudioClip winSfx;

    [Header("Editor debugging")]
    [SerializeField]
    private static bool gameIsOver = false;

    public void GameOver()
    {
        gameIsOver = true;
        if (loseSfx != null) {
            //SoundManager.Instance.PlaySfx(GetComponent<AudioSource>(), loseSfx);
        }
        //LevelUIManager.Instance.ActivateGameOverUI(true);
        return;
    }
    public void WinLevel()
    {
        gameIsOver = true;
        if (winSfx != null) {
            //SoundManager.Instance.PlaySfx(GetComponent<AudioSource>(), winSfx);
        }
        //LevelUIManager.Instance.ActivateWinUI(true);
        return;
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        //LoadManager.LoadScene(mainMenuScene);
        //SoundManager.Instance.StopSound();
        return;
    }

    public void RetryLevel()
    {
        //EventManager.TriggerEvent(new GameEvent(GameEventTypes.UnPause));
        //LoadManager.LoadScene(SceneManager.GetActiveScene().name);
        return;
    }
}
