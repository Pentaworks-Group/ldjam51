using Assets.Scripts.Core;
using Assets.Scripts.Base;

using UnityEngine;
using UnityEngine.UI;

public class SaveGameSlotBehaviour : MonoBehaviour
{
    private Text SavedOnText;
    private Text ElapsedOnText;
    private Text GameModeText;
    private Text LevelText;

    public int index;

    private GameObject OverrideButton;
    private GameObject DeleteButton;

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
        //this.gameObject.SetActive(true);
        DeleteButton = this.gameObject.transform.Find("DeleteButton").gameObject;
        OverrideButton = this.gameObject.transform.Find("OverrideButton").gameObject;
        SavedOnText = this.gameObject.transform.Find("LeftSide/Details/SavedOn").GetComponent<Text>();
        ElapsedOnText = this.gameObject.transform.Find("LeftSide/Details/ElapsedTime").GetComponent<Text>();
        GameModeText = this.gameObject.transform.Find("LeftSide/Details/ModeName").GetComponent<Text>();
        LevelText = this.gameObject.transform.Find("LeftSide/Details/Level").GetComponent<Text>();
    }

    public void Start()
    {
        if (Core.Game.State == default)
        {
            OverrideButton.SetActive(false);
        } else
        {
            DeleteButton.SetActive(false);
        }
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
        this.LevelText.text = this.GameState.LevelsCompleted.ToString();

        this.ElapsedOnText.text = string.Format("{0:F1}s", this.GameState.ElapsedTime);
    }
}
