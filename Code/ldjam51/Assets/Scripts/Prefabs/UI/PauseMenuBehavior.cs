using System;
using System.Collections.Generic;

using Assets.Scripts.Base;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public UnityEvent<Boolean> PauseToggled = new UnityEvent<Boolean>();

    public List<GameObject> ObjectsToHide = new();

    public GameObject Menu;
    public GameObject MenuArea;
    public GameObject OptionsArea;
    public GameObject SaveGameArea;
    public GameObject GUI;
    public Button BackButton;
    public Button ContinueButton;

    private Transform inputFieldLeft;
    private Transform inputFieldRight;
    // Start is called before the first frame update
    void Start()
    {
        inputFieldLeft = GUI.transform.Find("InputFieldLeft");
        inputFieldRight = GUI.transform.Find("InputFieldRight");
        Hide();
        ReloadUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }
    }

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
                ReloadUI(); //TODO prevent double activating/deactivating
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
    }

    public void OnBackButtonClicked()
    {
        if (this.SaveGameArea.activeSelf)
        {
            Core.Game.SaveOptions();
        }

        Core.Game.PlayButtonSound();
        SetVisible(pauseMenu: true);
    }

    private void ReloadUI()
    {
        switch (Core.Game.Options.MobileInterface)
        {
            case "None":
                inputFieldLeft.gameObject.SetActive(false);
                inputFieldRight.gameObject.SetActive(false);
                break;
            case "Left":
                inputFieldLeft.gameObject.SetActive(true);
                inputFieldRight.gameObject.SetActive(false);
                break;
            case "Right":
                inputFieldLeft.gameObject.SetActive(false);
                inputFieldRight.gameObject.SetActive(true);
                break;
            default:
                inputFieldLeft.gameObject.SetActive(false);
                inputFieldRight.gameObject.SetActive(true);
                break;
        }
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



    private void SetVisible(Boolean pauseMenu = false, Boolean options = false, Boolean saveGame = false)
    {
        this.MenuArea.SetActive(pauseMenu);
        this.OptionsArea.SetActive(options);
        this.SaveGameArea.SetActive(saveGame);

        this.ContinueButton.gameObject.SetActive(pauseMenu);
        this.BackButton.gameObject.SetActive(!pauseMenu);
    }
}
