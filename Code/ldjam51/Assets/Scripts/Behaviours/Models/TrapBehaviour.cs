using Assets.Scripts.Game;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class TrapBehaviour : MonoBehaviour
    {
        const System.Int32 RotationAngle = 100;
        private Tile tile;

        private void Start()
        {
            this.tile = GetComponent<ExtraModelBehaviour>()?.Tile;
        }

        void Update()
        {
            if (!this.tile.FieldState.IsActive && !this.tile.FieldState.IsCompleted)
            {
                this.transform.Rotate(0, RotationAngle * Time.deltaTime, 0, Space.World);
            }
        }
    }
}
