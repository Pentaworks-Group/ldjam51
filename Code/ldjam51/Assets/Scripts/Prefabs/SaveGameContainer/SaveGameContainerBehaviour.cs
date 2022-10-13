using System;
using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Core;

using UnityEngine;

public class SaveGameContainerBehaviour : MonoBehaviour
{

    public GameObject SaveGameSlotTemplate;
    private GameObject SaveButton;
    private GameObject UpButton;
    private GameObject DownButton;
    private readonly int MaxSlosts = 10;
    private int SlotIndex = 0;

    private readonly List<SaveGameSlotBehaviour> SlotBehaviours = new();
    private List<GameState> savedGames = new();

    private void Awake()
    {
        SaveButton = transform.Find("NewSaveButton").gameObject;
        UpButton = transform.Find("SaveGames/UpButton").gameObject;
        DownButton = transform.Find("SaveGames/DownButton").gameObject;
    }

    void Start()
    {
        ReloadSaveGames();
        if (Core.Game.State == default)
        {
            SaveButton.SetActive(false);
        }
    }

    public void ReloadSaveGames()
    {
        UpdateSlostsFromPrevab();
        CheckMoveButtonVisibillity();

    }

    private void CheckMoveButtonVisibillity()
    {
        if (savedGames?.Count > SlotIndex + MaxSlosts)
        {
            UpButton.SetActive(true);
        }
        else
        {
            UpButton.SetActive(false);
        }
        if (SlotIndex > 0)
        {
            DownButton.SetActive(true);
        }
        else
        {
            DownButton.SetActive(false);
        }
    }

    public void MoveUp()
    {
        SlotIndex += MaxSlosts;
        UpdateSlots();
        CheckMoveButtonVisibillity();
    }

    public void MoveDown()
    {
        SlotIndex -= MaxSlosts;
        UpdateSlots();
        CheckMoveButtonVisibillity();
    }

    public void OverrideSave(SaveGameSlotBehaviour saveGameSlotBehaviour)
    {
        int index = saveGameSlotBehaviour.index;
        Assets.Scripts.Base.Core.Game.State.SavedOn = DateTime.Now;
        this.savedGames[index] = Assets.Scripts.Base.Core.Game.State;
        SaveGames();
        UpdateSlots();
    }

    public void DeleteSave(SaveGameSlotBehaviour saveGameSlotBehaviour)
    {
        int index = saveGameSlotBehaviour.index;
        this.savedGames.RemoveAt(index);
        SaveGames();
        UpdateSlots();
    }


    private void UpdateSlostsFromPrevab()
    {
        var savedGamesJson = PlayerPrefs.GetString("SavedGames");

        if (!System.String.IsNullOrEmpty(savedGamesJson))
        {
            try
            {
                this.savedGames = GameFrame.Core.Json.Handler.Deserialize<List<GameState>>(savedGamesJson);
                Debug.Log($"Found GameStates: {this.savedGames.Count}");
            }
            catch
            {
            }

            UpdateSlots();
        }
    }

    private void UpdateSlots()
    {
        ClearSlots();
        if (this.savedGames?.Count > 0)
        {
            int upperBound;
            if (MaxSlosts + SlotIndex < this.savedGames.Count)
            {
                upperBound = MaxSlosts + SlotIndex;
            }
            else
            {
                upperBound = this.savedGames.Count;
            }
            for (int i = SlotIndex; i < upperBound; i++)
            {
                CreateAndFillSlot(i, this.savedGames[i]);
            }
        }
    }

    public void LoadGame(SaveGameSlotBehaviour saveGameSlotBehaviour)
    {
        Core.Game.PlayButtonSound();
        //Assets.Scripts.Base.Core.SelectedGameMode = gameState.Mode;
        Assets.Scripts.Base.Core.Game.Start(saveGameSlotBehaviour.GameState);
        Time.timeScale = 1; //required if called from pausemenu
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
        int relativeIndex = index % MaxSlosts;
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)relativeIndex * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(relativeIndex + 1) * relative);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        modeSlot.SetActive(true);
        modeSlot.name = "SaveGameName " + gameState.SaveGameName;


        SaveGameSlotBehaviour saveGameSlotBehaviour = modeSlot.GetComponent<SaveGameSlotBehaviour>();
        saveGameSlotBehaviour.index = index;
        this.SlotBehaviours.Add(saveGameSlotBehaviour);
        saveGameSlotBehaviour.GameState = gameState;

    }


    public void SaveNewGame()
    {
        Assets.Scripts.Base.Core.Game.State.SavedOn = DateTime.Now;
        this.savedGames.Add(Assets.Scripts.Base.Core.Game.State);
        SaveGames();
    }

    private void SaveGames()
    {
        var savedGamesJson = GameFrame.Core.Json.Handler.Serialize(this.savedGames);
        PlayerPrefs.SetString("SavedGames", savedGamesJson);
        PlayerPrefs.Save();
        UpdateSlots();
    }
}
