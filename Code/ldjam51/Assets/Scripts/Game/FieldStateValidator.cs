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
                    if (CheckTileRecursive(fieldState.Player.PositionX - 1, fieldState.Player.PositionZ))
                    {
                        return true;
                    }
                    else if (CheckTileRecursive(fieldState.Player.PositionX, fieldState.Player.PositionZ - 1))
                    {
                        return true;
                    }
                    else if (CheckTileRecursive(fieldState.Player.PositionX + 1, fieldState.Player.PositionZ))
                    {
                        return true;
                    }
                    else if (CheckTileRecursive(fieldState.Player.PositionX, fieldState.Player.PositionZ + 1))
                    {
                        return true;
                    }
                }
            }

            //UnityEngine.Debug.Log("Generated impossible Field! :D rekt");

            return false;
        }

        private Boolean CheckTileRecursive(Int32 x, Int32 z)
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
                        if (CheckTileRecursive(x - 1, z))
                        {
                            return true;
                        }
                        else if (CheckTileRecursive(x, z - 1))
                        {
                            return true;
                        }
                        else if (CheckTileRecursive(x, z + 1))
                        {
                            return true;
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
