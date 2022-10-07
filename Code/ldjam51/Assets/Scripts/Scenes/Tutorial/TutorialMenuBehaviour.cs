
using Assets.Scripts.Base;
using Assets.Scripts.Scenes;

public class TutorialMenuBehaviour : BaseMenuBehaviour
{
    public void ToGame()
    {
        Core.Game.PlayButtonSound();
        //if (Core.Game.State != default)
        //{

            Core.Game.Start(Core.Game.State);
        //}
        //else
        //{
        //    Core.Game.Start();
        //}
    }
}
