
using Assets.Scripts.Base;
using Assets.Scripts.Scenes;

public class TutorialMenuBehaviour : BaseMenuBehaviour
{
    public void ToGame()
    {
        Core.Game.Options.ShowTutorial = false;
        Core.Game.PlayButtonSound();
        if (Core.Game.State != default)
        {

            Core.Game.Start(Core.Game.State);
            Core.Game.ChangeScene(SceneNames.PlayFieldScene);
        }
        else
        {
            Core.Game.Start();
        }
    }


    public void ToMenu()
    {
        Core.Game.Stop();
        ToMainMenu();
    }
}
