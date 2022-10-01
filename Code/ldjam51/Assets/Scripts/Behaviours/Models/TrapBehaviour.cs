using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class TrapBehaviour : MonoBehaviour
    {
        const System.Int32 RotationAngle = 100;

        void Update()
        {
            this.transform.Rotate(0, RotationAngle * Time.deltaTime, 0, Space.World);
        }
    }
}
