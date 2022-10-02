using System;
using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Core;
using Assets.Scripts.Scenes;

using UnityEngine;

public class SaveGameContainerBehaviour : MonoBehaviour
{

    public GameObject SaveGameSlotTemplate;
    private GameObject SaveButton;
    private readonly int MaxSlosts = 10;

    private readonly List<SaveGameSlotBehaviour> SlotBehaviours = new();

    private void Awake()
    {
        SaveButton = transform.Find("NewSaveButton").gameObject;
    }


    public void OverrideSave(SaveGameSlotBehaviour saveGameSlotBehaviour)
    {
        saveGameSlotBehaviour.GameState = Assets.Scripts.Base.Core.Game.State;

        SaveGameSlots();
    }

    private void SaveGameSlots()
    {
        var savedGames = new List<GameState>();
        foreach (SaveGameSlotBehaviour saveGameSlot in SlotBehaviours)
        {
            savedGames.Add(saveGameSlot.GameState);
        }
        SaveGames(savedGames);
    }

    void Start()
    {
        var savedGamesJson = PlayerPrefs.GetString("SavedGames");

        if (!System.String.IsNullOrEmpty(savedGamesJson))
        {
            var savedGames = GameFrame.Core.Json.Handler.Deserialize<List<GameState>>(savedGamesJson);
            UpdateSlots(savedGames);
        }
        if (Core.Game.State == default)
        {
            SaveButton.SetActive(false);
        }
    }

    private void UpdateSlots(List<GameState> savedGames)
    {
        ClearSlots();
        if (savedGames?.Count > 0)
        {
            Debug.Log($"Found GameStates: {savedGames.Count}");

            for (int i = 0; i < savedGames?.Count; i++)
            {
                CreateAndFillSlot(i, savedGames[i]);
            }
        }
    }

    public void LoadGame(SaveGameSlotBehaviour saveGameSlotBehaviour)
    {
        Core.Game.PlayButtonSound();
        //Assets.Scripts.Base.Core.SelectedGameMode = gameState.Mode;
        Assets.Scripts.Base.Core.Game.Start(saveGameSlotBehaviour.GameState);
    }

    private void ClearSlots()
    {
        for (int i = SlotBehaviours.Count - 1; i >= 0; i--)
        {
            GameObject slot = SlotBehaviours[i].gameObject;
            GameObject.Destroy(slot);
            SlotBehaviours.RemoveAt(i);
        }
    }

    private void CreateAndFillSlot(int index, GameState gameState)
    {

        GameObject modeSlot = Instantiate(SaveGameSlotTemplate, new Vector3(0, 0, 0), Quaternion.identity, SaveGameSlotTemplate.transform.parent);
        float relative = 1f / MaxSlosts;
        RectTransform rect = modeSlot.GetComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)index * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(index + 1) * relative);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        modeSlot.SetActive(true);
        modeSlot.name = "SaveGameName " + gameState.SaveGameName;


        SaveGameSlotBehaviour saveGameSlotBehaviour = modeSlot.GetComponent<SaveGameSlotBehaviour>();
        SlotBehaviours.Add(saveGameSlotBehaviour);
        saveGameSlotBehaviour.GameState = gameState;

    }


    public void SaveNewGame()
    {
        var savedGames = new List<GameState>();
        var savedGamesJson = PlayerPrefs.GetString("SavedGames");

        if (!System.String.IsNullOrEmpty(savedGamesJson))
        {
             savedGames = GameFrame.Core.Json.Handler.Deserialize<List<GameState>>(savedGamesJson);            
        }

        savedGames.Add(Assets.Scripts.Base.Core.Game.State);
        SaveGames(savedGames);
    }

    private void SaveGames(List<GameState> savedGames)
    {
        var savedGamesJson = GameFrame.Core.Json.Handler.Serialize(savedGames);
        PlayerPrefs.SetString("SavedGames", savedGamesJson);
        PlayerPrefs.Save();
        UpdateSlots(savedGames);
    }
}
