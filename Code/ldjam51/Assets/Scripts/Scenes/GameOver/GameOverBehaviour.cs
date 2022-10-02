using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Scenes;

using UnityEngine;

public class GameOverBehaviour : BaseMenuBehaviour
{

    private void Awake()
    {
        if (GameHandler.AvailableGameModes == default)
        {
            Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
    }

}
