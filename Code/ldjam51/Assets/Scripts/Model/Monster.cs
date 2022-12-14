using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Monster
    {
        public Boolean IsActive { get; set; }
        public String Name { get; set; }
        public String GameOverText { get; set; }
        public String TemplateReference { get; set; }
        public List<String> SoundEffects { get; set; }
        public Int32 PositionX { get; set; }
        public Int32 PositionZ { get; set; }
        public Int32 FaceDirection { get; set; }
        public String MaterialReference { get; set; }

        private Material material;
        [JsonIgnore]
        public Material Material
        {
            get
            {
                if ((this.material == default) && (!String.IsNullOrEmpty(MaterialReference)))
                {
                    this.material = GameFrame.Base.Resources.Manager.Materials.Get(MaterialReference);
                }

                return material;
            }
            set
            {
                if (this.material != value)
                {
                    this.material = value;
                    this.MaterialReference = value?.name;
                }
            }
        }
    }
}
