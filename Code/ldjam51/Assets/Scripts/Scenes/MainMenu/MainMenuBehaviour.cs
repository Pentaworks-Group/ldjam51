
using System;
using System.Collections.Generic;

using Assets.Scripts.Base;

using GameFrame.Core.Audio.Multi;
using GameFrame.Core.Audio.Single;

using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public EffectsAudioManager EffectsAudioManager;
    public ContinuousAudioManager AmbienceAudioManager;
    public ContinuousAudioManager BackgroundAudioManager;


    public GameObject QuitButton;
    public void StartGame()
    {
        if (Assets.Scripts.Base.Core.Game.Options.ShowTutorial)
        {
            Core.Game.PlayButtonSound();
            Core.Game.ChangeScene(SceneNames.Tutorial);
        } else
        {
            Core.Game.PlayButtonSound();
            Core.Game.Start();

        }
    }

    public void ShowSavedGames()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.SavedGames);
    }

    public void ShowModes()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.GameModeScene);
    }

    public void ShowOptions()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.OptionsScene);
    }

    public void ShowCredits()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.Credits);
    }

    public void Quit()
    {
        Core.Game.PlayButtonSound();
        Assets.Scripts.Base.Core.Game.SaveOptions();
        Application.Quit();
        Screen.fullScreen = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        StartAudioManagers();
        LoadGameSettings();
        HideIfWGL();
    }

    private void HideIfWGL()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QuitButton.SetActive(false);
        }
    }

    private void StartAudioManagers()
    {
        if (Core.Game.EffectsAudioManager == default)
        {
            Core.Game.EffectsAudioManager = this.EffectsAudioManager;
            Core.Game.EffectsAudioManager.Volume = Core.Game.Options.EffectsVolume;
            Core.Game.EffectsAudioManager.Initialize();
        }

        if (Core.Game.AmbienceAudioManager == default)
        {
            Core.Game.AmbienceAudioManager = this.AmbienceAudioManager;
            Core.Game.AmbienceAudioManager.Volume = Core.Game.Options.AmbienceVolume;
            Core.Game.AmbienceAudioManager.Initialize();
        }

        if (Core.Game.BackgroundAudioManager == default)
        {
            Core.Game.BackgroundAudioManager = this.BackgroundAudioManager;
            Core.Game.BackgroundAudioManager.Volume = Core.Game.Options.BackgroundVolume;
            Core.Game.BackgroundAudioManager.Initialize();
        }

        Core.Game.AudioClipListMenu = new List<AudioClip>()
        {
            GameFrame.Base.Resources.Manager.Audio.Get("Background_01")
        };

        Core.Game.AmbienceAudioManager.Clips = Core.Game.AudioClipListMenu;

        Core.Game.AmbienceAudioManager.Resume();
    }

    public void LoadGameSettings()
    {
        if (GameHandler.AvailableGameModes == default)
        {
            String filePath = Application.streamingAssetsPath + "/GameSettings.json";
            StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets<List<GameSettings>>(filePath, SetGameSettings));
        }
    }

    private List<GameSettings> SetGameSettings(List<GameSettings> gameSettings)
    {
        GameHandler.AvailableGameModes = gameSettings;

        if (Assets.Scripts.Base.Core.SelectedGameMode == default)
        {
            Assets.Scripts.Base.Core.SelectedGameMode = gameSettings[0];
        }

        return gameSettings;
    }

    public void ReloadSettingsClick()
    {
        Core.Game.PlayButtonSound();
        String filePath = Application.streamingAssetsPath + "/GameSettings.json";
        StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets<List<GameSettings>>(filePath, SetGameSettingsForce));
    }

    private List<GameSettings> SetGameSettingsForce(List<GameSettings> gameSettings)
    {
        GameHandler.AvailableGameModes = gameSettings;

        Assets.Scripts.Base.Core.SelectedGameMode = gameSettings[0];

        return gameSettings;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

}
