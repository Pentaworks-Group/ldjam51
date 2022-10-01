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

                this.UpdateUI();
            }
        }
    }

    public void Awake()
    {
        GameModeText = this.gameObject.transform.Find("Details/ModeName").GetComponent<Text>();
    }


    public void OnSlotClick()
    {
        Core.Game.PlayButtonSound();
        Assets.Scripts.Base.Core.SelectedGameMode = GameSettings;
        Core.Game.ChangeScene(SceneNames.MainMenu);
    }

    private void UpdateUI()
    {
        this.GameModeText.text = this.GameSettings.Name;
    }
}
