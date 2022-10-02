using System;

namespace Assets.Scripts.Model
{
    public class Player
    {
        public Boolean IsActive { get; set; }
        public String TemplateReference { get; set; }
        public Int32 PositionX { get; set; }
        public Int32 PositionZ { get; set; }
    }
}
