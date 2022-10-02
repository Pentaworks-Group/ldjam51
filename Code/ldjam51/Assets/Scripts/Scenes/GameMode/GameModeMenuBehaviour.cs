using System;
using System.Collections.Generic;

using Assets.Scripts.Scenes;

using UnityEngine;
using UnityEngine.UI;

public class GameModeMenuBehaviour : BaseMenuBehaviour
{

    public GameObject GameModeSlotTemplate;
    private readonly int MaxSlosts = 10;

    private readonly List<GameModeSlotBehaviour> SlotBehaviours = new();



    public GameObject CreateNewModeButton;
    public GameObject OwnModesButton;
    public GameObject GlobalModesButton;


    public bool ownMode { get; set; } = false;
    private Color32 selectedColor;
    private Color32 notSelectedColor;

    private void Awake()
    {
        if (GameHandler.AvailableGameModes == default)
        {
            Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }

        selectedColor = GlobalModesButton.GetComponent<Image>().color;
        notSelectedColor = OwnModesButton.GetComponent<Image>().color;
    }

    void Start()
    {
        LoadGameModes(GameHandler.AvailableGameModes, false);
    }

    private void LoadGameModes(List<GameSettings> modes, bool rotateSlots)
    {
        ClearSlots();
        for (int i = 0; i < modes.Count; i++)
        {
            CreateAndFillSlot(i, modes[i], rotateSlots);
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
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)index * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(index + 1) * relative);
        if (rotateSlot && Screen.width < Screen.height)
        {
            rect.rotation = new Quaternion(0, 0, 1, 1f);
        }
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        modeSlot.SetActive(true);
        modeSlot.name = "GameModeSlot" + gameSettings.Name;


        GameModeSlotBehaviour gameModeSlotBehaviour = modeSlot.GetComponent<GameModeSlotBehaviour>();
        SlotBehaviours.Add(gameModeSlotBehaviour);
        gameModeSlotBehaviour.GameSettings = gameSettings;

    }

    public void CreateNewMode()
    {

        GameSettings gameFieldSettings = new GameSettings();
        List<GameSettings> ownModes = getModesFromSlots();
        ownModes.Add(gameFieldSettings);
        LoadGameModes(ownModes, true);

    }


     public void SaveGameMode(GameSettings gameSettings)
    {
        List<GameSettings> ownModes;
        if (gameSettings != default) //when called from a global mode
        {
            ownModes = GetGameSettingsFromPlayerPref();
            ownModes.Add(gameSettings);
        } else
        {
            ownModes = getModesFromSlots();
        }
        String ownModesJson = GameFrame.Core.Json.Handler.Serialize(ownModes);
        PlayerPrefs.SetString("OwnModes", ownModesJson);
        PlayerPrefs.Save();
    }

    public void LoadGlobalModes()
    {
        GlobalModesButton.GetComponent<Image>().color = selectedColor;
        OwnModesButton.GetComponent<Image>().color = notSelectedColor;
        ownMode = false;
        CreateNewModeButton.SetActive(false);
        LoadGameModes(GameHandler.AvailableGameModes, true);
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
        GlobalModesButton.GetComponent<Image>().color = notSelectedColor;
        OwnModesButton.GetComponent<Image>().color = selectedColor;
        ownMode = true;
        CreateNewModeButton.SetActive(true);
        var ownModes = GetGameSettingsFromPlayerPref();
        LoadGameModes(ownModes, true);
    }

    private List<GameSettings> getModesFromSlots()
    {
        List<GameSettings> modes = new List<GameSettings>();
        foreach (GameModeSlotBehaviour slot in SlotBehaviours)
        {
            modes.Add(slot.GameSettings);
        }
        return modes;
    }
}
