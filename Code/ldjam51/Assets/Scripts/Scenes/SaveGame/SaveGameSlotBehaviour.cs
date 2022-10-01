using Assets.Scripts.Core;
using Assets.Scripts.Base;

using UnityEngine;
using UnityEngine.UI;

public class SaveGameSlotBehaviour : MonoBehaviour
{
    private Text SavedOnText;
    private Text ElapsedOnText;
    private Text GameModeText;
    private Text SaveGameText;


    private GameState gameState;
    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            if (gameState != value)
            {
                gameState = value;

                this.UpdateUI();
            }
        }
    }

    public void Awake()
    {
        SavedOnText = this.gameObject.transform.Find("Details/SaveGameName").GetComponent<Text>();
        ElapsedOnText = this.gameObject.transform.Find("Details/ElapsedTime").GetComponent<Text>();
        GameModeText = this.gameObject.transform.Find("Details/ModeName").GetComponent<Text>();
        SaveGameText = this.gameObject.transform.Find("Details/SaveGameName").GetComponent<Text>();
    }


    public void OnSlotClick()
    {
        Core.Game.PlayButtonSound();
        Assets.Scripts.Base.Core.Game.Start(gameState);
        //Assets.Scripts.Base.Core.SelectedGameMode = GameFieldSettings;
        //Core.Game.ChangeScene(SceneNames.MainMenu);
    }

    private void UpdateUI()
    {
        this.SavedOnText.text = string.Format("{0:G}", this.GameState.SavedOn);
        this.GameModeText.text = this.GameState.Mode.Name;
        this.SaveGameText.text = this.GameState.SaveGameName;

        this.ElapsedOnText.text = string.Format("{0:F1}s", this.GameState.ElapsedTime);
    }
}
