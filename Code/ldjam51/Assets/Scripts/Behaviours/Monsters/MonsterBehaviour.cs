using Assets.Scripts.Model;
using Assets.Scripts.Scenes.PlayField;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Monsters
{
    public class MonsterBehaviour : ModelBehaviour
    {
        public const System.Int32 StepSize = 1;
        public const float moveInterval = 1;

        private float nextMove = moveInterval;

        public FieldHandler FieldHandler;
        public Monster Monster;

        void Update()
        {
            if (Time.timeScale != 0)
            {
                if ((Monster.IsActive) && (!this.FieldHandler.FieldState.IsCompleted))
                {
                    if (nextMove < 0)
                    {
                        MoveRandom();

                        nextMove = moveInterval;
                    }
                    else
                    {
                        nextMove -= Time.deltaTime;
                    }
                }
            }
        }

        private void MoveRandom()
        {
            if (UnityEngine.Random.value < 0.5)
            {
                var remainingAttempts = 4;

                while (remainingAttempts > 0)
                {
                    var x = 0;
                    var z = 0;
                    var direction = 0;

                    var movementDirection = UnityEngine.Random.Range(0, 4);

                    switch (movementDirection)
                    {
                        case 0:
                            x -= 1;
                            direction = 3;
                            break;

                        case 1:
                            z -= 1;
                            direction = 0;
                            break;

                        case 2:
                            x += 1;
                            direction = 1;
                            break;

                        case 3:
                            z += 1;
                            direction = 2;
                            break;
                    }

                    if (IsMovePossible(x, z))
                    {
                        if (direction != Monster.FaceDirection)
                        {
                            var difference = (Monster.FaceDirection - direction) * 90;

                            Monster.FaceDirection = direction;
                            this.transform.Rotate(new UnityEngine.Vector3(0, 0, difference));
                        }

                        var newPosition = new UnityEngine.Vector3(x, 0, z);

                        this.Move(newPosition);

                        if (Monster.SoundEffects?.Count > 0)
                        {
                            Base.Core.Game.EffectsAudioManager.Play(Monster.SoundEffects.GetRandomEntry());
                        }

                        break;
                    }
                    else
                    {
                        remainingAttempts--;
                    }
                }
            }
        }

        private System.Boolean IsMovePossible(System.Int32 x, System.Int32 z)
        {
            x += Monster.PositionX;
            z += Monster.PositionZ;

            var tiles = this.FieldHandler.FieldState.Tiles;

            if ((x >= 0 && x < tiles.GetLength(0))
                && (z >= 0 && z < tiles.GetLength(1)))
            {
                var tile = this.FieldHandler.FieldState.Tiles[x, z];

                if (tile != default)
                {
                    if ((!tile.IsFinish) && (!tile.IsStart) && (tile.ExtraTemplate == default))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Move(UnityEngine.Vector3 movementVector)
        {
            if (Monster?.IsActive == true)
            {
                if (movementVector.x != 0)
                {
                    Monster.PositionX += (System.Int32)movementVector.x;
                }

                if (movementVector.z != 0)
                {
                    Monster.PositionZ += (System.Int32)movementVector.z;
                }

                this.transform.Translate(movementVector * 2, Space.World);
            }
        }
    }
}
