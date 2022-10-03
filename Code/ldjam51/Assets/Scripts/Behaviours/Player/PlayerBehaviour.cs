using System;
using System.Collections.Generic;

using Assets.Scripts.Behaviours.Monsters;
using Assets.Scripts.Scenes.PlayField;

using GameFrame.Core.Extensions;

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
                this.Move(new UnityEngine.Vector3(x, 0, z));
            }
        }

        public void MoveRight()
        {
            this.Move(new UnityEngine.Vector3(StepSize, 0, 0));
        }

        public void MoveDown()
        {
            this.Move(new UnityEngine.Vector3(0, 0, -StepSize));
        }

        public void MoveLeft()
        {
            this.Move(new UnityEngine.Vector3(-StepSize, 0, 0));
        }

        public void MoveUp()
        {
            this.Move(new UnityEngine.Vector3(0, 0, StepSize));
        }

        private void Move(UnityEngine.Vector3 movementVector)
        {
            var player = FieldHandler?.FieldState?.Player;

            if (player?.IsActive == true)
            {
                if (movementVector.x != 0)
                {
                    FieldHandler.FieldState.Player.PositionX += (Int32)movementVector.x;
                }

                if (movementVector.z != 0)
                {
                    FieldHandler.FieldState.Player.PositionZ += (Int32)movementVector.z;
                }

                this.lastMove = movementVector;

                this.transform.Translate(movementVector * 2, Space.World);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.FieldHandler?.FieldState?.Player != default)
            {
                var targetBehaviour = other.GetComponent<ModelBehaviour>();

                if (targetBehaviour.Tile != default)
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
                        var possibleAudios = new List<string> { "Awww", "PlayerHit" };
                        var choice = possibleAudios.GetRandomEntry();

                        Base.Core.Game.EffectsAudioManager.Play(choice);
                        Base.Core.Game.ChangeScene(SceneNames.GameOver);
                    }
                    else
                    {
                        this.Move(-this.lastMove);

                        Base.Core.Game.EffectsAudioManager.Play("Bonk");
                    }
                }
                else if (targetBehaviour is MonsterBehaviour)
                {
                    var possibleAudios = new List<string> { "Awww", "PlayerHit" };
                    var choice = possibleAudios.GetRandomEntry();

                    Base.Core.Game.EffectsAudioManager.Play(choice);

                    Base.Core.Game.ChangeScene(SceneNames.GameOver);
                }
            }
        }

        public void OnCollisionEnter(Collision collision)
        {

        }
    }
}
