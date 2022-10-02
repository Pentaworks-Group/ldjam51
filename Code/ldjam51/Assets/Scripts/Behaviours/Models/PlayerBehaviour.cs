using System;

using Assets.Scripts.Model;
using Assets.Scripts.Scenes.PlayField;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class PlayerBehaviour : ModelBehaviour
    {
        private Vector3 lastMove;

        public const Int32 StepSize = 2;
        public FieldHandler FieldHandler;

        void Update()
        {
            if (this.FieldHandler?.FieldState?.IsActive == true)
            {
                if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
                {
                    this.lastMove = new Vector3(0, 0, StepSize);

                    this.transform.Translate(lastMove, Space.World);
                    this.FieldHandler.FieldState.Player.PositionZ += StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    this.lastMove = new Vector3(-StepSize, 0, 0);

                    this.transform.Translate(lastMove, Space.World);
                    this.FieldHandler.FieldState.Player.PositionX -= StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    this.lastMove = new Vector3(0, 0, -StepSize);

                    this.transform.Translate(lastMove, Space.World);
                    this.FieldHandler.FieldState.Player.PositionZ -= StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    this.lastMove = new Vector3(StepSize, 0, 0);

                    this.transform.Translate(lastMove, Space.World);
                    this.FieldHandler.FieldState.Player.PositionX += StepSize;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.FieldHandler.FieldState.Player != default)
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
    }
}
