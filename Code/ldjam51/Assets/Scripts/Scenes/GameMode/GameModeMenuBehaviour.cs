using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Scenes;

using UnityEngine;

public class GameModeMenuBehaviour : BaseMenuBehaviour
{

    public GameObject GameModeSlotTemplate;
    private readonly int MaxSlosts = 10;

    private readonly List<GameModeSlotBehaviour> SlotBehaviours = new();

    private void Awake()
    {
        if (GameHandler.AvailableGameModes == default)
        {
            Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
    }

    void Start()
    {
        LoadGameModes(GameHandler.AvailableGameModes);
    }

    private void LoadGameModes(List<GameSettings> modes)
    {
        ClearSlots();
        for (int i = 0; i < modes.Count; i++)
        {
            CreateAndFillSlot(i, modes[i]);
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

    private void CreateAndFillSlot(int index, GameSettings gameSettings)
    {

        GameObject modeSlot = Instantiate(GameModeSlotTemplate, new Vector3(0, 0, 0), Quaternion.identity, GameModeSlotTemplate.transform.parent);
        float relative = 1f / MaxSlosts;
        RectTransform rect = modeSlot.GetComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)index * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(index + 1) * relative);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        modeSlot.SetActive(true);
        modeSlot.name = "GameModeSlot" + gameSettings.Name;


        GameModeSlotBehaviour gameModeSlotBehaviour = modeSlot.GetComponent<GameModeSlotBehaviour>();
        SlotBehaviours.Add(gameModeSlotBehaviour);
        gameModeSlotBehaviour.GameSettings = gameSettings;

    }

}
