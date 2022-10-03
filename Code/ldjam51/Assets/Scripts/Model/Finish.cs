using System;

using Newtonsoft.Json;

using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Finish
    {
        public String TemplateReference { get; set; }
        public Int32 PositionX { get; set; }
        public Int32 PositionZ { get; set; }

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
