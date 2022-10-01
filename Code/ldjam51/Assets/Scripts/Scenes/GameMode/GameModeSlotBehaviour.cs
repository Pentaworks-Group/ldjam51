using Assets.Scripts.Core;
using Assets.Scripts.Base;

using UnityEngine;
using UnityEngine.UI;

public class GameModeSlotBehaviour : MonoBehaviour
{
    private Text GameModeText;


    private GameSettings gameSettings;
    public GameSettings GameSettings
    {
        get
        {
            return gameSettings;
        }
        set
        {
            if (gameSettings != value)
            {
                gameSettings = value;
                UpdateUI();
            }
        }
    }

    public void Awake()
    {
        GameModeText = this.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
    }


    private Text GetGameModeText()
    {
        if (this.GameModeText == default)
        {
            this.GameModeText = this.gameObject.transform.Find("SelectAndInfo/Details/ModeName").GetComponent<Text>();
        }
        return this.GameModeText;
    }

    public void OnSlotClick()
    {
        Core.Game.PlayButtonSound();
        Assets.Scripts.Base.Core.SelectedGameMode = GameSettings;
        Core.Game.ChangeScene(SceneNames.MainMenu);
    }

    private void UpdateUI()
    {
        GetGameModeText().text = GameSettings.Name;
    }
}
