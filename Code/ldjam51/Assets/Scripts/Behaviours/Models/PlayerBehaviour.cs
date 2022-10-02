using System;

using Assets.Scripts.Model;
using Assets.Scripts.Scenes.PlayField;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class PlayerBehaviour : ModelBehaviour
    {
        public const Int32 StepSize = 2;
        public Player Player;
        public FieldHandler PlayField;

        void Update()
        {
            if (this.Player?.IsActive == true)
            {
                if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
                {
                    this.transform.Translate(0, 0, StepSize, Space.World);
                    this.Player.PositionZ += StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    this.transform.Translate(-StepSize, 0, 0, Space.World);
                    this.Player.PositionX -= StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    this.transform.Translate(0, 0, -StepSize, Space.World);
                    this.Player.PositionZ -= StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    this.transform.Translate(StepSize, 0, 0, Space.World);
                    this.Player.PositionX += StepSize;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Player != default)
            {
                Debug.Log($"Bonk: {other.gameObject.name} - Player: {Player.PositionX};{Player.PositionZ}");

                var targetBehaviour = other.GetComponent<ModelBehaviour>();

                if (targetBehaviour.Tile != default)
                {
                    if (targetBehaviour.Tile.IsFinish)
                    {
                        // Won
                    }
                    else if (targetBehaviour.Tile.ExtraTemplate?.IsDeadly == true)
                    {
                        // Lost
                    }
                }
            }
        }
    }
}
