using System;

using Assets.Scripts.Scenes.PlayField;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class PlayerBehaviour : ModelBehaviour
    {
        public const float PlayerSpeed = 5;

        private Vector3 lastMove;

        public const Int32 StepSize = 2;
        public FieldHandler FieldHandler;

        void Update()
        {
            if (this.FieldHandler?.FieldState?.Player?.IsActive == true)
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
                    this.Move(new Vector3(x, 0, z));
                }
            }
        }

        public void MoveRight()
        {
            this.Move(new Vector3(StepSize, 0, 0));
            FieldHandler.FieldState.Player.PositionX += StepSize;
        }
                
        public void MoveDown()
        {
            this.Move(new Vector3(StepSize, 0, 0));
        }

        public void MoveLeft()
        {
            this.Move(new Vector3(-StepSize, 0, 0));
        }

        public void MoveUp()
        {
            this.Move(new Vector3(0, 0, StepSize));
        }

        private void Move(Vector3 movementVector)
        {
            if (movementVector.x != 0)
            {
                FieldHandler.FieldState.Player.PositionX += (Int32)movementVector.x;
            }

            if (movementVector.z != 0)
            {
                FieldHandler.FieldState.Player.PositionZ += (Int32)movementVector.z;
            }

            this.transform.Translate(movementVector, Space.World);

            //var targetPosition = this.transform.position + movementVector;

            //this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.time);
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
                        Debug.Log("Yay");
                        this.FieldHandler.FieldState.IsCompleted = true;
                        this.FieldHandler.SetActive(false);
                    }
                    else if (targetBehaviour.Tile.ExtraTemplate?.IsDeadly == true)
                    {
                        // Lost

                        Debug.Log("PEPSI");
                        Base.Core.Game.ChangeScene(SceneNames.GameOver);
                    }
                    else
                    {
                        this.transform.Translate(-lastMove, Space.World);
                        this.FieldHandler.FieldState.Player.PositionX -= (Int32)lastMove.x;
                        this.FieldHandler.FieldState.Player.PositionZ -= (Int32)lastMove.z;

                        Base.Core.Game.EffectsAudioManager.Play("Bonk");
                    }
                }
            }
        }

        public void OnCollisionEnter(Collision collision)
        {

        }
    }
}
