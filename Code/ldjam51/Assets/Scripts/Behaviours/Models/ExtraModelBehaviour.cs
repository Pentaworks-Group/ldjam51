using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Behaviours.Models
{
    public class ExtraModelBehaviour : ModelBehaviour
    {
        public Boolean IsRotatable = true;


        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Boom: Collision");

            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }

            // Play a sound if the colliding objects had a big impact.
            //if (collision.relativeVelocity.magnitude > 2)
            //    audioSource.Play();
        }
    }
}
