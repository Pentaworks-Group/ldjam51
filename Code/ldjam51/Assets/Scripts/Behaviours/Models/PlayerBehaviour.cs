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
                var player = FieldHandler.FieldState.Player;

                var moveRequired = false;

                var x = 0f;
                var z = 0f;

                if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
                {
                    moveRequired = true;

                    z += StepSize;
                    player.PositionZ += StepSize;
                }
                else if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
                {
                    moveRequired = true;

                    x -= StepSize;
                    player.PositionX -= StepSize;
                }
                else if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
                {
                    moveRequired = true;

                    z -= StepSize;
                    player.PositionZ -= StepSize;
                }
                else if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
                {
                    moveRequired = true;

                    x += StepSize;
                    player.PositionX += StepSize;
                }

                if (moveRequired)
                {
                    this.lastMove = new Vector3(x, 0, z);
                    var end = this.transform.position + lastMove;

                    this.transform.position = Vector3.Lerp(this.transform.position, end, PlayerSpeed * Time.deltaTime);
                }
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
                        //this.transform.Translate(-lastMove, Space.World);
                        //this.FieldHandler.FieldState.Player.PositionX -= (Int32)lastMove.x;
                        //this.FieldHandler.FieldState.Player.PositionZ -= (Int32)lastMove.z;

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
