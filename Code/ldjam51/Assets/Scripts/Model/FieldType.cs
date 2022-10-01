using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class FieldType
    {
        public String TemplateReference { get; set; }
        public List<String> Materials { get; set; }
        public List<String> ExtraTemplateReference { get; set; }
    }
}
