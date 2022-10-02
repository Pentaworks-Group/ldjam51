using System;
using System.Collections.Generic;

using Assets.Scripts.Base;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    private Assets.Scripts.Core.GameState[] gameStates;
    public UnityEvent<Boolean> PauseToggled = new UnityEvent<Boolean>();

    public List<GameObject> ObjectsToHide = new();

    public GameObject Menu;
    public GameObject MenuArea;
    public GameObject OptionsArea;
    public GameObject SaveGameArea;
    public Button BackButton;
    public Button ContinueButton;

    public void ToggleMenu()
    {
        if (Menu.activeSelf == true)
        {
            if (this.OptionsArea.activeSelf)
            {
                this.OnBackButtonClicked();
            }
            else
            {
                Core.Game.PlayButtonSound();
                Hide();

                this.PauseToggled.Invoke(false);
                foreach (GameObject gameObject in ObjectsToHide)
                {
                    gameObject.SetActive(true);
                }
            }
        }
        else
        {
            Core.Game.PlayButtonSound();

            this.PauseToggled.Invoke(true);
            foreach (GameObject gameObject in ObjectsToHide)
            {
                gameObject.SetActive(false);
            }
            Show();
        }
    }

    public void ShowSavedGames()
    {
        Core.Game.PlayButtonSound();

        SetVisible(saveGame: true);
    }

    public void Hide()
    {
        Menu.SetActive(false);
     
        Time.timeScale = 1;
    }

    public void Show()
    {
        Time.timeScale = 0;

        CursorMode cursorMode = CursorMode.Auto;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);

        SetVisible(pauseMenu: true);

        Menu.SetActive(true);

        Core.Game.AmbienceAudioManager.Stop();
    }

    public void OnBackButtonClicked()
    {
        Core.Game.PlayButtonSound();
        SetVisible(pauseMenu: true);
    }

    public void ShowOptions()
    {
        Core.Game.PlayButtonSound();
        this.SetVisible(options: true);
    }

    public void Quit()
    {
        Time.timeScale = 1;

        Core.Game.PlayButtonSound();
        Core.Game.Stop();
        Core.Game.ChangeScene(SceneNames.MainMenu);
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }
    }

    private void SetVisible(Boolean pauseMenu = false, Boolean options = false, Boolean saveGame = false)
    {
        this.MenuArea.SetActive(pauseMenu);
        this.OptionsArea.SetActive(options);
        this.SaveGameArea.SetActive(saveGame);

        this.ContinueButton.gameObject.SetActive(pauseMenu);
        this.BackButton.gameObject.SetActive(!pauseMenu);
    }
}
