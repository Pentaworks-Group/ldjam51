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
                var startingTile = fieldState.Tiles[fieldState.Player.PositionX, fieldState.Player.PositionZ];

                if (CheckTileRecursive(startingTile))
                {
                    return true;
                }
            }

            return true;
        }

        private Boolean CheckTileRecursive(Tile tile)
        {

            return default;
        }
    }
}
