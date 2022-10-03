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

            var attempts = 0;

            Int32 numRows = gameMode.RowCount;
            Int32 columnCount = gameMode.ColumnCount;

            if (Base.Core.SelectedGameMode.IncrementalSize != default && Base.Core.Game.State != default)
            {
                numRows += Base.Core.SelectedGameMode.IncrementalSize * Base.Core.Game.State.LevelsCompleted;
                columnCount += Base.Core.SelectedGameMode.IncrementalSize * Base.Core.Game.State.LevelsCompleted;
            }

            while (fieldState == default)
            {
                var newFieldState = new FieldState()
                {
                    IsActive = false,
                    IsPlaneVisible = isPlaneVisible,
                    ColumnCount = columnCount,
                    RowCount = numRows

                };

                GenerateFields(gameMode, newFieldState);

                if (new FieldStateValidator(newFieldState).IsValid())
                {
                    fieldState = newFieldState;
                }
                else if (attempts < 9000)
                {
                    attempts++;
                }
                else
                {
                    throw new Exception("Too many attempts! It's over 9000! Failed to generate Field!");
                }
            }

            return fieldState;
        }

        private void GenerateFields(GameSettings gameMode, FieldState fieldState)
        {
            fieldState.Tiles = new Tile[fieldState.RowCount, fieldState.ColumnCount];

            AddPlayer(gameMode, fieldState);

            AddFinish(gameMode, fieldState);

            AddMonster(gameMode, fieldState);

            for (int row = 0; row < fieldState.RowCount; row++)
            {
                for (int column = 0; column < fieldState.ColumnCount; column++)
                {
                    if (fieldState.Tiles[column, row] == default)
                    {
                        fieldState.Tiles[column, row] = gameMode.ObjectTypes.Tiles.GetRandomEntry().ToTile();
                    }
                }
            }
        }

        private void AddPlayer(GameSettings gameMode, FieldState fieldState)
        {
            var playerTile = gameMode.ObjectTypes.Players.GetRandomEntry();

            var player = new Player()
            {
                IsActive = fieldState.IsActive,
                TemplateReference = playerTile.TemplateReference,
                MaterialReference = playerTile.Materials.GetRandomEntry(),
                PositionX = UnityEngine.Random.Range(0, fieldState.ColumnCount),
                PositionZ = UnityEngine.Random.Range(0, fieldState.RowCount)
            };

            fieldState.Player = player;

            fieldState.Tiles[player.PositionX, player.PositionZ] = new Tile()
            {
                TemplateReference = "Tile_Start",
                IsStart = true,
                Material = GameFrame.Base.Resources.Manager.Materials.Get("Start")
            };
        }

        private void AddFinish(GameSettings gameMode, FieldState fieldState)
        {
            var targetTileTemplate = gameMode.ObjectTypes.Finishes.GetRandomEntry();

            GeneratePosition(fieldState, out var x, out var y);

            var finish = new Finish()
            {
                TemplateReference = targetTileTemplate.TemplateReference,
                MaterialReference = targetTileTemplate.Materials.GetRandomEntry(),
                PositionX = x,
                PositionZ = y
            };

            fieldState.Finish = finish;

            fieldState.Tiles[finish.PositionX, finish.PositionZ] = new Tile()
            {
                IsFinish = true,
                TemplateReference = finish.TemplateReference,
                ExtraTemplate = new ExtraTile()
                {
                    TemplateReference = "Finish",
                    Material = GameFrame.Base.Resources.Manager.Materials.Get("Finish")
                },
                Material = GameFrame.Base.Resources.Manager.Materials.Get("FinishLine")
            };
        }

        private void AddMonster(GameSettings gameMode, FieldState fieldState)
        {
            var targetTileTemplate = gameMode.ObjectTypes.Finishes.GetRandomEntry();

            GeneratePosition(fieldState, out var x, out var z);

            if (gameMode.ObjectTypes.Monsters?.Count > 0)
            {
                var monsterTemplate = gameMode.ObjectTypes.Monsters.GetRandomEntry();

                var monster = new Monster()
                {
                    Name = monsterTemplate.Name,
                    GameOverText = monsterTemplate.GameOverText,
                    TemplateReference = monsterTemplate.TemplateReference,
                    MaterialReference = monsterTemplate.Materials.GetRandomEntry(),
                    SoundEffects = monsterTemplate.SoundEffects,
                    PositionX = x,
                    PositionZ = z,
                };

                fieldState.Monster = monster;

                fieldState.Tiles[monster.PositionX, monster.PositionZ] = new Tile()
                {
                    TemplateReference = "Tile",
                    Material = GameFrame.Base.Resources.Manager.Materials.Get("Grass")
                };
            }
        }

        private void GeneratePosition(FieldState fieldState, out Int32 x, out Int32 z)
        {
            x = 0;
            z = 0;

            var positionFound = false;

            for (var counter = 0; counter < 5; counter++)
            {
                x = UnityEngine.Random.Range(0, fieldState.ColumnCount);
                z = UnityEngine.Random.Range(0, fieldState.RowCount);

                if (IsPositionAvailable(fieldState, x, z))
                {
                    positionFound = true;
                }
            }

            if (!positionFound)
            {
                throw new Exception("Failed to generate Position for Finish!");
            }
        }

        private Boolean IsPositionAvailable(FieldState fieldState, Int32 x, Int32 z)
        {
            var isAvailable = true;

            if (fieldState.Player != default)
            {
                if (fieldState.Player.PositionX == x && fieldState.Player.PositionZ == z)
                {
                    isAvailable = false;
                }
            }

            if (isAvailable)
            {
                if (fieldState.Finish != default)
                {
                    if (fieldState.Finish.PositionX == x && fieldState.Finish.PositionZ == z)
                    {
                        isAvailable = false;
                    }
                }
            }

            if (isAvailable)
            {
                if (fieldState.Monster != default)
                {
                    if (fieldState.Monster.PositionX == x && fieldState.Monster.PositionZ == z)
                    {
                        isAvailable = false;
                    }
                }
            }

            return isAvailable;
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
