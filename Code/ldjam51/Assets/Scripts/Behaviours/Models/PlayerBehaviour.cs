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
        public Player Player;
        public FieldHandler PlayField;

        void Update()
        {
            if (this.Player?.IsActive == true)
            {
                if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
                {
                    this.lastMove = new Vector3(0, 0, StepSize);

                    this.transform.Translate(lastMove, Space.World);
                    this.Player.PositionZ += StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    this.lastMove = new Vector3(-StepSize, 0, 0);

                    this.transform.Translate(lastMove, Space.World);
                    this.Player.PositionX -= StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    this.lastMove = new Vector3(0, 0, -StepSize);

                    this.transform.Translate(lastMove, Space.World);
                    this.Player.PositionZ -= StepSize;
                }
                else if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    this.lastMove = new Vector3(StepSize, 0, 0);

                    this.transform.Translate(lastMove, Space.World);
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
                        Debug.Log("Yay");
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
                        this.Player.PositionX -= (Int32)lastMove.x;
                        this.Player.PositionZ -= (Int32)lastMove.z;

                        Base.Core.Game.EffectsAudioManager.Play("Bonk");
                    }
                }
            }
        }
    }
}
