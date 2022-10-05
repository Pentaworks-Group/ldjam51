using System;

namespace Assets.Scripts.Game
{
    public class FieldStateValidator
    {
        private readonly FieldState fieldState;
        private Boolean[,] visitedTilesByIndex;

        public FieldStateValidator(FieldState fieldState)
        {
            this.fieldState = fieldState;

            if (this.fieldState != default)
            {
                this.visitedTilesByIndex = new Boolean[fieldState.ColumnCount, fieldState.RowCount];
            }
        }

        public Boolean IsValid()
        {
            if ((fieldState.Player != default)
                && (fieldState.Finish != default)
                && (fieldState.Tiles.Length == fieldState.ColumnCount * fieldState.RowCount))
            {
                if ((fieldState.Player.PositionX != fieldState.Finish.PositionX) && (fieldState.Player.PositionZ != fieldState.Finish.PositionZ))
                {
                    if (fieldState.Monsters?.Count > 0)
                    {
                        foreach (var monster in fieldState.Monsters)
                        {
                            visitedTilesByIndex[monster.PositionX, monster.PositionZ] = true;
                        }
                    }

                    if (CheckTileRecursive(fieldState.Player.PositionX - 1, fieldState.Player.PositionZ, 0))
                    {
                        return true;
                    }
                    else if (CheckTileRecursive(fieldState.Player.PositionX, fieldState.Player.PositionZ - 1, 1))
                    {
                        return true;
                    }
                    else if (CheckTileRecursive(fieldState.Player.PositionX + 1, fieldState.Player.PositionZ, 2))
                    {
                        return true;
                    }
                    else if (CheckTileRecursive(fieldState.Player.PositionX, fieldState.Player.PositionZ + 1, 3))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Boolean CheckTileRecursive(Int32 x, Int32 z, Int32 previousDirection)
        {
            var tile = GetTile(x, z);

            if (tile != default)
            {
                if (!visitedTilesByIndex[x, z])
                {
                    visitedTilesByIndex[x, z] = true;

                    if (tile.IsFinish)
                    {
                        return true;
                    }
                    else if (tile.ExtraTemplate == default)
                    {
                        var excludedDirection = previousDirection + 2;

                        if (excludedDirection > 3)
                        {
                            excludedDirection -= 4;
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            if (i != excludedDirection)
                            {
                                switch (i)
                                {
                                    case 0: x -= 1; break;
                                    case 1: z -= 1; break;
                                    case 2: x += 1; break;
                                    case 3: z += 1; break;
                                }

                                if (CheckTileRecursive(x, z, i))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return default;
        }

        private Tile GetTile(Int32 x, Int32 z)
        {
            if ((x > 0 && x < this.fieldState.ColumnCount) && ((z > 0 && z < this.fieldState.RowCount)))
            {
                return fieldState.Tiles[x, z];
            }

            return default;
        }

        private Boolean CheckTileRecursive(Tile tile)
        {

            return default;
        }
    }
}
