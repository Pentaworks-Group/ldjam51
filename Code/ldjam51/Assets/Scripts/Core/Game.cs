using System;
using System.Linq;

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

            //var fieldsToGenerate = 50000;
            //var averageGenerationDuration = 0d;
            //var averageValidationDuration = 0d;
            //var invalidFields = 0;

            //var totalStopwatch = new Stopwatch();

            //for (int i = 0; i < fieldsToGenerate; i++)
            //{
            //    totalStopwatch.Start();

            //    var newFieldState = new FieldState()
            //    {
            //        IsActive = false,
            //        IsPlaneVisible = true,
            //        ColumnCount = gameMode.ColumnCount,
            //        RowCount = gameMode.RowCount,
            //    };

            //    var stopwatch = new Stopwatch();

            //    stopwatch.Start();
            //    GenerateFields(gameMode, newFieldState);
            //    stopwatch.Stop();

            //    totalStopwatch.Stop();
            //    averageGenerationDuration = (i * stopwatch.ElapsedTicks + averageGenerationDuration) / (i + 1);
            //    totalStopwatch.Start();

            //    var validationStopwatch = new Stopwatch();
            //    validationStopwatch.Start();

            //    if (!new FieldStateValidator(newFieldState).IsValid())
            //    {
            //        invalidFields++;
            //    }

            //    validationStopwatch.Stop();
            //    totalStopwatch.Stop();

            //    averageValidationDuration = (i * validationStopwatch.ElapsedTicks + averageValidationDuration) / (i + 1);
            //}

            //UnityEngine.Debug.Log(String.Format("Generated {0} fields. {1}/{0} ({2}%) Invaild.", fieldsToGenerate, invalidFields, (invalidFields * 100 / fieldsToGenerate)));
            //UnityEngine.Debug.Log(String.Format("Total time: {0} - Average generation: {1:#0.00} Ticks - Average validation: {2:#0.00} Ticks", totalStopwatch.Elapsed, averageGenerationDuration, averageValidationDuration));

            var gameState = new GameState()
            {
                CurrentScene = SceneNames.PlayFieldScene,
                Mode = gameMode,
                TimeRemaining = gameMode.Interval,
                NextTick = gameMode.TickStart
            };

            GenerateFields(gameMode, gameState);

            return gameState;
        }

        private void GenerateFields(GameSettings gameMode, GameState gameState)
        {
            if (gameMode?.FieldAmount > 0)
            {
                var firstField = true;

                for (int i = 0; i < gameMode.FieldAmount; i++)
                {
                    var field = GenerateField(gameMode, firstField);

                    firstField = false;

                    if (field != default)
                    {
                        gameState.Fields.Add(field);
                    }
                }
            }
            else
            {
                throw new Exception("Either no GameMode provided or FouldCount less than 1");
            }
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

                GenerateTiles(gameMode, newFieldState);

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

        private void GenerateTiles(GameSettings gameMode, FieldState fieldState)
        {
            fieldState.Tiles = new Tile[fieldState.RowCount, fieldState.ColumnCount];

            AddPlayer(gameMode, fieldState);

            AddFinish(gameMode, fieldState);

            AddMonsters(gameMode, fieldState);

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
            var playerType = gameMode.ObjectTypes.Players.GetRandomEntry();

            var player = new Player()
            {
                IsActive = fieldState.IsActive,
                TemplateReference = playerType.TemplateReference,
                MaterialReference = playerType.Materials.GetRandomEntry(),
                PositionX = UnityEngine.Random.Range(0, fieldState.ColumnCount),
                PositionZ = UnityEngine.Random.Range(0, fieldState.RowCount)
            };

            fieldState.Player = player;

            var playerTileType = playerType.Tile;

            if (playerTileType == default)
            {
                playerTileType = new PlayerTileType()
                {
                    TemplateReference = "Tile_Start",
                    Materials = new System.Collections.Generic.List<string>()
                    {
                        "Start"
                    }
                };
            }

            fieldState.Tiles[player.PositionX, player.PositionZ] = new Tile()
            {
                IsStart = true,
                TemplateReference = playerTileType.TemplateReference,
                MaterialReference = playerTileType.Materials.GetRandomEntry(),
            };
        }

        private void AddFinish(GameSettings gameMode, FieldState fieldState)
        {
            var finishType = gameMode.ObjectTypes.Finishes.GetRandomEntry();

            GeneratePosition(fieldState, out var x, out var y);

            var finish = new Finish()
            {
                TemplateReference = finishType.TemplateReference,
                MaterialReference = finishType.Materials.GetRandomEntry(),
                PositionX = x,
                PositionZ = y
            };

            fieldState.Finish = finish;

            var extraTile = default(ExtraTile);

            if (finishType.Extras?.Count > 0)
            {
                var extraTemplate = finishType.Extras.GetRandomEntry();

                if (!string.IsNullOrEmpty(extraTemplate))
                {
                    extraTile = new ExtraTile()
                    {
                        TemplateReference = extraTemplate,
                        MaterialReference = extraTemplate
                    };
                }
            }

            if (extraTile == default)
            {
                extraTile = new ExtraTile()
                {
                    TemplateReference = "Finish",
                    MaterialReference = "Finish"
                };
            }

            fieldState.Tiles[finish.PositionX, finish.PositionZ] = new Tile()
            {
                IsFinish = true,
                TemplateReference = finishType.TemplateReference,
                ExtraTemplate = extraTile,
                MaterialReference = finishType.Materials.GetRandomEntry()
            };
        }

        private void AddMonsters(GameSettings gameMode, FieldState fieldState)
        {
            if ((gameMode.ObjectTypes.Monsters?.Count > 0) && (gameMode.MonsterAmount > 0))
            {
                for (int i = 1; i <= gameMode.MonsterAmount; i++)
                {
                    GeneratePosition(fieldState, out var x, out var z);

                    var monsterType = gameMode.ObjectTypes.Monsters.GetRandomEntry();

                    var monster = new Monster()
                    {
                        Name = monsterType.Name,
                        GameOverText = monsterType.GameOverText,
                        TemplateReference = monsterType.TemplateReference,
                        MaterialReference = monsterType.Materials.GetRandomEntry(),
                        SoundEffects = monsterType.SoundEffects,
                        PositionX = x,
                        PositionZ = z,
                    };

                    if (fieldState.Monsters == default)
                    {
                        fieldState.Monsters = new System.Collections.Generic.List<Monster>();
                    }

                    fieldState.Monsters.Add(monster);

                    var monsterTileType = monsterType.Tile;

                    if (monsterTileType == default)
                    {
                        monsterTileType = new MonsterTileType()
                        {
                            TemplateReference = "Tile",
                            Materials = new System.Collections.Generic.List<string>()
                        {
                            "Grass"
                        }
                        };
                    }

                    fieldState.Tiles[monster.PositionX, monster.PositionZ] = new Tile()
                    {
                        TemplateReference = monsterTileType.TemplateReference,
                        MaterialReference = monsterTileType.Materials.GetRandomEntry()
                    };
                }
            }
        }

        private void GeneratePosition(FieldState fieldState, out Int32 x, out Int32 z)
        {
            x = 0;
            z = 0;

            var positionFound = false;

            for (var counter = 0; counter < 10; counter++)
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
                throw new Exception("Failed to generate Position!");
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
                if (fieldState.Monsters?.Count > 0)
                {
                    foreach (var monster in fieldState.Monsters)
                    {
                        if (monster.PositionX == x && monster.PositionZ == z)
                        {
                            isAvailable = false;
                        }
                    }
                }
            }

            return isAvailable;
        }

        private ExtraTile GetExtraTile(GameSettings gameMode, String tileReference)
        {
            if (!String.IsNullOrEmpty(tileReference))
            {
                return gameMode.ObjectTypes.Extras.FirstOrDefault(e => e.Name == tileReference)?.ToTile();
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
