using System;

using Assets.Scripts.Extensions;
using Assets.Scripts.Game;
using Assets.Scripts.Model;

using GameFrame.Core.Audio.Multi;
using GameFrame.Core.Audio.Single;
using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        public const Int32 FieldCount = 9;

        public ContinuousAudioManager AmbienceAudioManager { get; set; }
        public ContinuousAudioManager BackgroundAudioManager { get; set; }
        public EffectsAudioManager EffectsAudioManager { get; set; }

        public System.Collections.Generic.List<AudioClip> AudioClipListMenu { get; set; }
        public System.Collections.Generic.List<AudioClip> AudioClipListGame1 { get; set; }
        public System.Collections.Generic.List<AudioClip> AudioClipListGame2 { get; set; }
        public System.Collections.Generic.List<AudioClip> AudioClipListTransition { get; set; }
        public System.Collections.Generic.List<AudioClip> AmbientClipList { get; set; }

        public System.Collections.Generic.List<AudioClip> ShopClipList { get; set; }

        protected override GameState InitializeGameState()
        {
            var gameMode = Base.Core.SelectedGameMode;

            return new GameState()
            {
                CurrentScene = SceneNames.PlayFieldScene,
                Mode = gameMode,
                TimeRemaining = 10,
                Field1 = GenerateField(true),
                Field2 = GenerateField(false),
            };
        }

        private FieldState GenerateField(Boolean isActive)
        {
            var fieldState = new FieldState()
            {
                IsActive = isActive,
                ColumnCount = FieldCount,
                RowCount = FieldCount
            };

            GenerateFields(fieldState);

            return fieldState;
        }

        private void GenerateFields(FieldState fieldState)
        {
            fieldState.Tiles = new Tile[fieldState.RowCount, fieldState.ColumnCount];

            var playerTile = GetRandomPlayerTileTemplate();

            var player = new Player()
            {
                IsActive = fieldState.IsActive,
                TemplateReference = playerTile.Reference,
                PositionX = UnityEngine.Random.Range(0, fieldState.ColumnCount),
                PositionZ = UnityEngine.Random.Range(0, fieldState.RowCount)
            };

            fieldState.Player = player;

            fieldState.Tiles[player.PositionX, player.PositionZ] = new Tile()
            {
                Reference = "Tile_Start",
                Material = GameFrame.Base.Resources.Manager.Materials.Get("Start")
            };

            var targetTileTemplate = GetRandomFinishTileTemplate();

            var finish = new Finish()
            {
                TemplateReference = targetTileTemplate.Reference,
                PositionX = UnityEngine.Random.Range(0, fieldState.ColumnCount),
                PositionY = UnityEngine.Random.Range(0, fieldState.RowCount)
            };

            fieldState.Finish = finish;

            fieldState.Tiles[finish.PositionX, finish.PositionY] = new Tile()
            {
                Reference = "Tile",
                ExtraTemplate = new Tile()
                {
                    Reference = "Finish",
                    Material = GameFrame.Base.Resources.Manager.Materials.Get("Finish")
                },
                Material = GameFrame.Base.Resources.Manager.Materials.Get("FinishLine")
            };

            for (int row = 0; row < fieldState.RowCount; row++)
            {
                for (int column = 0; column < fieldState.ColumnCount; column++)
                {
                    if (fieldState.Tiles[column, row] == default)
                    {
                        fieldState.Tiles[column, row] = GetRandomTileTemplate().ToTile();
                    }
                }
            }
        }

        private TileType GetRandomTileTemplate()
        {
            if (GameHandler.AvailableTileTypes.Tiles?.Count > 0)
            {
                return GameHandler.AvailableTileTypes.Tiles.GetRandomEntry();
            }

            return default;
        }

        private TileType GetRandomPlayerTileTemplate()
        {
            if (GameHandler.AvailableTileTypes?.Player?.Count > 0)
            {
                return GameHandler.AvailableTileTypes.Player.GetRandomEntry();

            }

            return default;
        }

        private TileType GetRandomFinishTileTemplate()
        {
            if (GameHandler.AvailableTileTypes?.Finish?.Count > 0)
            {
                return GameHandler.AvailableTileTypes.Finish.GetRandomEntry();

            }

            return default;
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions()
            {
                AreAnimationsEnabled = true,
                EffectsVolume = 0.7f,
                BackgroundVolume = 0.9f,
                AmbienceVolume = 0.125f
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }

        public void PlayButtonSound()
        {
            EffectsAudioManager.Play("Button");
        }
    }
}
