using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;

namespace Assets.Scripts.Model
{
    public class ExtraTile
    {
        public String Name { get; set; }
        public String TemplateReference { get; set; }
        public Boolean IsDeadly { get; set; }
        public String GameOverText { get; set; }
        public List<String> SoundEffects { get; set; }
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
