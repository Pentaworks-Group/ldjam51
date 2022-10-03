using System;

using Assets.Scripts.Base;
using Assets.Scripts.Scenes;

using TMPro;

public class GameOverBehaviour : BaseMenuBehaviour
{
    protected override void CustomAwake()
    {
        var gameState = Core.Game.State;

        SetText("ContentArea/DeathReasonText", gameState.DeathReason);
        SetText("ContentArea/BottomTextArea/WatchOutForText", $"And be sure to watch out for {gameState.WatchOutForText}!");
    }

    private void SetText(String textMeshPath, String text)
    {
        var deathReasonText = transform.Find(textMeshPath)?.GetComponent<TextMeshProUGUI>();

        if (deathReasonText != default)
        {
            deathReasonText.text = text;
        }
    }
}
