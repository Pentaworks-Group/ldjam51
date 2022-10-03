using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class ExtraType
    {
        public String Name { get; set; }
        public String TemplateReference { get; set; }
        public Boolean IsDeadly { get; set; }
        public String GameOverText { get; set; }
        public List<String> SoundEffects { get; set; }
        public List<String> Materials { get; set; }
    }
}
