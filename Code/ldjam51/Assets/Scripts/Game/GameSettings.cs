using System;

public class GameSettings
{
    public String Name { get; set; }
    public float Interval { get; set; } = 10;
    public Int32 ColumnCount { get; set; } = 9;
    public Int32 RowCount { get; set; } = 9;
}