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
                TimeRemaining = gameMode.Interval,
                NextTick = gameMode.TickStart,
                Field1 = GenerateField(gameMode, false),
                Field2 = GenerateField(gameMode, true),
            };
        }

        public FieldState GenerateField(GameSettings gameMode, Boolean isPlaneVisible)
        {
            var fieldState = default(FieldState);

            while (fieldState == default)
            {
                var newFieldState = new FieldState()
                {
                    IsActive = false,
                    IsPlaneVisible = isPlaneVisible,
                    ColumnCount = gameMode.ColumnCount,
                    RowCount = gameMode.RowCount
                };

                GenerateFields(gameMode, newFieldState);

                if (new FieldStateValidator(newFieldState).IsValid())
                {
                    fieldState = newFieldState;
                }
            }

            return fieldState;
        }

        private void GenerateFields(GameSettings gameMode, FieldState fieldState)
        {
            fieldState.Tiles = new Tile[fieldState.RowCount, fieldState.ColumnCount];

            var playerTile = gameMode.TileTypes.Players.GetRandomEntry();

            var player = new Player()
            {
                IsActive = fieldState.IsActive,
                TemplateReference = playerTile.Reference,
                MaterialReference = playerTile.Materials.GetRandomEntry(),
                PositionX = UnityEngine.Random.Range(0, fieldState.ColumnCount),
                PositionZ = UnityEngine.Random.Range(0, fieldState.RowCount)
            };

            fieldState.Player = player;

            fieldState.Tiles[player.PositionX, player.PositionZ] = new Tile()
            {
                Reference = "Tile_Start",
                IsStart = true,
                Material = GameFrame.Base.Resources.Manager.Materials.Get("Start")
            };

            var targetTileTemplate = gameMode.TileTypes.Finishes.GetRandomEntry();

            var finish = new Finish()
            {
                TemplateReference = targetTileTemplate.Reference,
                PositionX = UnityEngine.Random.Range(0, fieldState.ColumnCount),
                PositionZ = UnityEngine.Random.Range(0, fieldState.RowCount)
            };

            fieldState.Finish = finish;

            fieldState.Tiles[finish.PositionX, finish.PositionZ] = new Tile()
            {
                Reference = "Tile",
                IsFinish = true,
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
                        fieldState.Tiles[column, row] = gameMode.TileTypes.Tiles.GetRandomEntry().ToTile();
                    }
                }
            }
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
