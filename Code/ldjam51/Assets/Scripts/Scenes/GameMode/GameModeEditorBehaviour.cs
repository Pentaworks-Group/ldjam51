using UnityEngine;
using UnityEngine.UI;

public class GameModeEditorBehaviour : MonoBehaviour
{
    private TMPro.TMP_InputField inputField;
    private GameModeSlotBehaviour openSlot;
    public GameObject DefaultView;
    public GameModeMenuBehaviour GameModeMenuBehaviour;

    private Text SaveButton;

    private Text GetSaveButton()
    {
        if (SaveButton == default)
        {
            SaveButton = this.gameObject.transform.Find("Save/Text").GetComponent<Text>();
        }
        return SaveButton;
    }

    public void ToggleEditor()
    {
        if (DefaultView != default)
        {
            DefaultView.SetActive(!DefaultView.activeSelf);
        }
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            if (GameModeMenuBehaviour.ownMode)
            {
                GetSaveButton().text = "Save";
            }
            else
            {
                GetSaveButton().text = "Save as own";
            }
        }
        else
        {
            if (!GameModeMenuBehaviour.ownMode)
            {
                GetSaveButton().text = "Save as own";
                GameModeMenuBehaviour.LoadOwnModes();
            }
        }

    }

    public void CreateNewGameSettings()
    {
        GameModeSlotBehaviour slotToEdit = new GameModeSlotBehaviour();
        slotToEdit.GameSettings = new GameSettings();
        slotToEdit.index = -1;
        OpenGameFieldSettings(slotToEdit);
    }


    public void OpenGameFieldSettings(GameModeSlotBehaviour slotToEdit)
    {
        if (inputField == default)
        {
            inputField = transform.GetChild(0).GetComponent<TMPro.TMP_InputField>();
        }

        if (!GameModeMenuBehaviour.ownMode)
        {
            slotToEdit.index = -1;
        }

        openSlot = slotToEdit;
        inputField.text = GameFrame.Core.Json.Handler.SerializePretty(slotToEdit.GameSettings);
        ToggleEditor();
    }

    public void SaveSettings()
    {
        var gameSettings = GameFrame.Core.Json.Handler.Deserialize<GameSettings>(inputField.text);
        openSlot.GameSettings = gameSettings;

        GameModeMenuBehaviour.SaveGameMode(openSlot);
    }

}
