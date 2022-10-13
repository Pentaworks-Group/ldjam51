using System;

using Assets.Scripts.Behaviours.Monsters;
using Assets.Scripts.Scenes.PlayField;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class PlayerBehaviour : ModelBehaviour
    {
        public const Int32 StepSize = 1;

        private UnityEngine.Vector3 lastMove = UnityEngine.Vector3.zero;

        public FieldHandler FieldHandler;

        void Update()
        {
            var moveRequired = false;

            var x = 0f;
            var z = 0f;

            if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                moveRequired = true;

                z += StepSize;
            }
            else if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                moveRequired = true;

                x -= StepSize;
            }
            else if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)))
            {
                moveRequired = true;

                z -= StepSize;
            }
            else if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
            {
                moveRequired = true;

                x += StepSize;
            }

            if (moveRequired)
            {
                if (Move(new UnityEngine.Vector3(x, 0, z)))
                {
                    Base.Core.Game.EffectsAudioManager.Play("Step");
                }
            }
        }

        public void MoveRight()
        {
            if (Move(new UnityEngine.Vector3(StepSize, 0, 0)))
            {
                Base.Core.Game.EffectsAudioManager.Play("Step");
            }
        }

        public void MoveDown()
        {
            if (Move(new UnityEngine.Vector3(0, 0, -StepSize)))
            {
                Base.Core.Game.EffectsAudioManager.Play("Step");
            }
        }

        public void MoveLeft()
        {
            if (Move(new UnityEngine.Vector3(-StepSize, 0, 0)))
            {
                Base.Core.Game.EffectsAudioManager.Play("Step");
            }
        }

        public void MoveUp()
        {
            if (Move(new UnityEngine.Vector3(0, 0, StepSize)))
            {
                Base.Core.Game.EffectsAudioManager.Play("Step");
            }
        }

        private Boolean Move(UnityEngine.Vector3 movementVector)
        {
            var player = FieldHandler?.FieldState?.Player;

            if (player?.IsActive == true)
            {
                var tiles = this.FieldHandler.FieldState.Tiles;

                if (movementVector.x != 0)
                {
                    var newX = FieldHandler.FieldState.Player.PositionX + (Int32)movementVector.x;

                    if (newX >= 0 && newX < tiles.GetLength(0))
                    {
                        FieldHandler.FieldState.Player.PositionX += (Int32)movementVector.x;
                    }
                    else
                    {
                        Base.Core.Game.EffectsAudioManager.Play("Bonk");
                        return false;
                    }

                }

                if (movementVector.z != 0)
                {
                    var newZ = FieldHandler.FieldState.Player.PositionZ + (Int32)movementVector.z;

                    if (newZ >= 0 && newZ < tiles.GetLength(1))
                    {
                        FieldHandler.FieldState.Player.PositionZ += (Int32)movementVector.z;
                    }
                    else
                    {
                        Base.Core.Game.EffectsAudioManager.Play("Bonk");
                        return false;
                    }
                }

                this.lastMove = movementVector;

                this.transform.Translate(movementVector * 2, Space.World);

                return true;
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.FieldHandler.FieldState.IsPrepared)
            {
                var targetBehaviour = other.GetComponent<ModelBehaviour>();

                if (targetBehaviour.Tile != default)
                {
                    if (targetBehaviour.Tile.FieldState == this.FieldHandler.FieldState)
                    {
                        if (targetBehaviour.Tile.IsFinish)
                        {
                            if (!this.FieldHandler.FieldState.IsCompleted)
                            {
                                this.FieldHandler.FieldState.IsCompleted = true;
                                this.FieldHandler.SetActive(false);

                                Base.Core.Game.EffectsAudioManager.Play("Woohoo");
                            }
                        }
                        else if (targetBehaviour.Tile.ExtraTemplate?.IsDeadly == true)
                        {
                            Base.Core.Game.EffectsAudioManager.Play("PlayerHit");

                            Base.Core.Game.State.WatchOutForText = $"the {targetBehaviour.Tile.ExtraTemplate.Name}";
                            Base.Core.Game.State.DeathReason = targetBehaviour.Tile.ExtraTemplate.GameOverText;

                            Base.Core.Game.ChangeScene(SceneNames.GameOver);
                        }
                        else
                        {
                            this.Move(-this.lastMove);

                            Base.Core.Game.EffectsAudioManager.Play("Bonk");
                        }
                    }
                }
                else if (targetBehaviour is MonsterBehaviour monsterBehaviour)
                {
                    if (monsterBehaviour.FieldHandler.FieldState == this.FieldHandler.FieldState)
                    {
                        Base.Core.Game.EffectsAudioManager.Play("Awww");

                        Base.Core.Game.State.WatchOutForText = $"the {monsterBehaviour.Monster.Name}";
                        Base.Core.Game.State.DeathReason = monsterBehaviour.Monster.GameOverText;

                        Base.Core.Game.ChangeScene(SceneNames.GameOver);
                    }
                }
            }
        }

        public void OnCollisionEnter(Collision collision)
        {

        }
    }
}
