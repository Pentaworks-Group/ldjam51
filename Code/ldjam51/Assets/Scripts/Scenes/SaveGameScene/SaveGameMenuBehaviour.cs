using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Scenes;

using UnityEngine;

public class SaveGameMenuBehaviour : BaseMenuBehaviour
{

    public GameObject SaveGameSlotTemplate;
    private readonly int MaxSlosts = 10;

    private List<SaveGameSlotBehaviour> SlotBehaviours = new List<SaveGameSlotBehaviour>();

    void Start()
    {
        var savedGamesJson = PlayerPrefs.GetString("SavedGames");

        if (!System.String.IsNullOrEmpty(savedGamesJson))
        {
            var savedGames = GameFrame.Core.Json.Handler.Deserialize<Assets.Scripts.Core.GameState[]>(savedGamesJson);

            ClearSlots();
            if (savedGames?.Length > 0)
            {
                Debug.Log($"Found GameStates: {savedGames.Length}");

                for (int i = 0; i < 5; i++)
                {
                    CreateAndFillSlot(i, savedGames[i]);
                }
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

    private void CreateAndFillSlot(int index, GameState gameState)
    {

        GameObject modeSlot = Instantiate(SaveGameSlotTemplate, new Vector3(0, 0, 0), Quaternion.identity, SaveGameSlotTemplate.transform.parent);
        float relative = 1f / MaxSlosts;
        RectTransform rect = modeSlot.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)index * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(index + 1) * relative);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        modeSlot.SetActive(true);


        SaveGameSlotBehaviour saveGameSlotBehaviour = modeSlot.GetComponent<SaveGameSlotBehaviour>();
        SlotBehaviours.Add(saveGameSlotBehaviour);
        saveGameSlotBehaviour.GameState = gameState;

    }

}
