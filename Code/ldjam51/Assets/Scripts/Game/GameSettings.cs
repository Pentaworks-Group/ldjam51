using System;

using Assets.Scripts.Model;

public class GameSettings
{
    public String Name { get; set; }
    public float Interval { get; set; } = 10;
    public float TickInterval { get; set; } = 1;
    public float TickStart { get; set; } = 5;
    public Int32 ColumnCount { get; set; } = 9;
    public Int32 RowCount { get; set; } = 9;
    public TileTypes TileTypes { get; set; }
}