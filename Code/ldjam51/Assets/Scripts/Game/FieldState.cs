using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game
{
    public class FieldState
    {
        public Boolean IsActive { get; set; }
        public Int32 RowCount { get; set; }
        public Int32 ColumnCount { get; set; }
        public Field[][] Fields { get; set; }
    }
}
