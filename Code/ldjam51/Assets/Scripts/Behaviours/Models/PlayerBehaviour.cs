using System;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class PlayerBehaviour : ModelBehaviour
    {
        public const Int32 StepSize = 2;

        void Update()
        {
            if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                this.transform.Translate(0, 0, StepSize, Space.World);
            }
            else if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                this.transform.Translate(-StepSize, 0, 0, Space.World);
            }
            else if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)))
            {
                this.transform.Translate(0, 0, -StepSize, Space.World);
            }
            else if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
            {
                this.transform.Translate(StepSize, 0, 0, Space.World);
            }
        }
    }
}
