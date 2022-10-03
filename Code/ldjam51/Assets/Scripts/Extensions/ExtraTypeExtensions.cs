using Assets.Scripts.Core;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Extensions
{
    public static class ExtraTypeExtensions
    {
        public static ExtraTile ToTile(this ExtraType extraType)
        {
            if (extraType != default)
            {
                var extraTile = new ExtraTile()
                {
                    Name = extraType.Name,                    
                    TemplateReference = extraType.TemplateReference,
                    IsDeadly = extraType.IsDeadly,
                    GameOverText = extraType.GameOverText,
                    SoundEffects = extraType.SoundEffects,
                    MaterialReference = extraType.Materials.GetRandomEntry(),
                };

                return extraTile;
            }

            return default;
        }
    }
}
