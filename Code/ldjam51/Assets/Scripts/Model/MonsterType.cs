﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class MonsterType
    {
        public String Name { get; set; }
        public String TemplateReference { get; set; }
        public String GameOverText { get; set; }
        public List<String> SoundEffects { get; set; }
        public List<String> Materials { get; internal set; }
    }
}
