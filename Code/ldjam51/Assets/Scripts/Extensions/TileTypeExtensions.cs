using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Game;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Extensions
{
    public static class TileTypeExtensions
    {
        public static Tile ToTile(this TileType tileType)
        {
            if (tileType != default)
            {
                var tile = new Tile()
                {
                    TemplateReference = tileType.Reference,
                    IsDeadly = tileType.IsDeadly,
                    Material = GameFrame.Base.Resources.Manager.Materials.Get(tileType.Materials.GetRandomEntry())
                };

                if (tileType.Extras?.Count > 0)
                {
                    float chance = Base.Core.SelectedGameMode.ExtraChance;
                    var incr = Base.Core.SelectedGameMode.IncrementalSpawn;
                    if (incr != default && Base.Core.Game.State != default)
                    {
                        var lvl = Base.Core.Game.State.LevelsCompleted;
                        chance += lvl * incr;
                    }
                    if (UnityEngine.Random.value < chance)
                    {
                        float take = UnityEngine.Random.value;
                        foreach (TileType t in Base.Core.SelectedGameMode.ObjectTypes.Extras)
                        {
                            float weight = Base.Core.SelectedGameMode.GetExtraWeights()[tileType.Reference].GetValueOrDefault(t.Reference, 0f);
                            if (weight > take)
                            {
                                tile.ExtraTemplate = t.ToTile();
                                break;
                            }
                        }

                    }
                }

                return tile;
            }

            return default;
        }
    }
}
