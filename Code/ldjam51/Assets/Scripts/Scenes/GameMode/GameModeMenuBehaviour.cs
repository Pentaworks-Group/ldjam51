using System;
using System.Collections.Generic;

using Assets.Scripts.Scenes;

using UnityEngine;
using UnityEngine.UI;

public class GameModeMenuBehaviour : BaseMenuBehaviour
{

    public GameObject GameModeSlotTemplate;
    private readonly int MaxSlosts = 10;
    private int SlotIndex = 0;

    private readonly List<GameModeSlotBehaviour> SlotBehaviours = new();
    private List<GameSettings> modes = new();



    public GameObject CreateNewModeButton;
    public GameObject OwnModesButton;
    public GameObject GlobalModesButton;
    public GameObject UpButton;
    public GameObject DownButton;


    public bool ownMode { get; set; } = false;
    private Color32 selectedColor;
    private Color32 notSelectedColor;

    protected override void CustomAwake()
    {
        selectedColor = GlobalModesButton.GetComponent<Image>().color;
        notSelectedColor = OwnModesButton.GetComponent<Image>().color;
    }


    void Start()
    {
        LoadGlobalModes();
        CheckMoveButtonVisibillity();
    }


    private void CheckMoveButtonVisibillity()
    {
        if (modes?.Count > SlotIndex + MaxSlosts)
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
        UpdateSlots(true);
        CheckMoveButtonVisibillity();
    }

    public void MoveDown()
    {
        SlotIndex -= MaxSlosts;
        UpdateSlots(true);
        CheckMoveButtonVisibillity();
    }

    private void UpdateSlots(bool rotateSlots)
    {
        ClearSlots();
        if (this.modes?.Count > 0)
        {
            int upperBound;
            if (SlotIndex > this.modes.Count)
            {
                SlotIndex = modes.Count - modes.Count % MaxSlosts;
            }
            if (MaxSlosts + SlotIndex < this.modes.Count)
            {
                upperBound = MaxSlosts + SlotIndex;
            }
            else
            {
                upperBound = this.modes.Count;
            }
            for (int i = SlotIndex; i < upperBound; i++)
            {
                CreateAndFillSlot(i, this.modes[i], rotateSlots);
            }
        }

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

    private void CreateAndFillSlot(int index, GameSettings gameSettings, bool rotateSlot)
    {

        GameObject modeSlot = Instantiate(GameModeSlotTemplate, new Vector3(0, 0, 0), Quaternion.identity, GameModeSlotTemplate.transform.parent);
        float relative = 1f / MaxSlosts;
        RectTransform rect = modeSlot.GetComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(0, 0, 0);
        int relativeIndex = index % MaxSlosts;
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)relativeIndex * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(relativeIndex + 1) * relative);
        if (rotateSlot && Screen.width < Screen.height)
        {
            rect.rotation = new Quaternion(0, 0, 1, 1f);
        }
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        modeSlot.SetActive(true);
        modeSlot.name = "GameModeSlot" + gameSettings.Name;


        GameModeSlotBehaviour gameModeSlotBehaviour = modeSlot.GetComponent<GameModeSlotBehaviour>();
        gameModeSlotBehaviour.index = index;
        SlotBehaviours.Add(gameModeSlotBehaviour);
        gameModeSlotBehaviour.GameSettings = gameSettings;
        gameModeSlotBehaviour.IsOwnMode = ownMode;
        gameModeSlotBehaviour.UpdateUI();
    }



     public void SaveGameMode(GameModeSlotBehaviour slotBehaviour)
    {
        if (slotBehaviour.index != -1) 
        {
            int index = slotBehaviour.index;
            this.modes[index] = slotBehaviour.GameSettings;
        } else
        {
            if (ownMode != true)
            {
                modes = GetGameSettingsFromPlayerPref();
            }
            modes.Add(slotBehaviour.GameSettings);
            SlotIndex = modes.Count - modes.Count % MaxSlosts;
        }
        SaveGameModes();
        UpdateSlots(true);
    }


    public void DeleteGameMode(GameModeSlotBehaviour slotBehaviour)
    {
        modes.RemoveAt(slotBehaviour.index);
        SaveGameModes();
        UpdateSlots(true);
    }

    private void SaveGameModes()
    {
        String ownModesJson = GameFrame.Core.Json.Handler.Serialize(modes);
        PlayerPrefs.SetString("OwnModes", ownModesJson);
        PlayerPrefs.Save();
    }

    private void UpdateModeButtonColors()
    {
        if (ownMode)
        {
            GlobalModesButton.GetComponent<Image>().color = selectedColor;
            OwnModesButton.GetComponent<Image>().color = notSelectedColor;
        } else
        {
            GlobalModesButton.GetComponent<Image>().color = notSelectedColor;
            OwnModesButton.GetComponent<Image>().color = selectedColor;
        }
    }



    private List<GameSettings> GetGameSettingsFromPlayerPref()
    {
        String ownModesJson = PlayerPrefs.GetString("OwnModes");
        List<GameSettings> ownModes;
        if (!String.IsNullOrEmpty(ownModesJson))
        {
            ownModes = GameFrame.Core.Json.Handler.Deserialize<List<GameSettings>>(ownModesJson);
        }
        else
        {
            ownModes = new List<GameSettings>();
        }
        return ownModes;
    }

    public void LoadOwnModes()
    {
        ownMode = true;
        CreateNewModeButton.SetActive(true);
        UpdateModeButtonColors();
        modes = GetGameSettingsFromPlayerPref();
        UpdateSlots(false);
        CheckMoveButtonVisibillity();
    }

    public void LoadGlobalModes()
    {
        ownMode = false;
        CreateNewModeButton.SetActive(false);
        UpdateModeButtonColors();
        modes = GameHandler.AvailableGameModes;
        UpdateSlots(false);
        CheckMoveButtonVisibillity();
    }
}
