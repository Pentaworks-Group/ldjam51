using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;

public class GameSettings
{
    public String Name { get; set; }
    public float Interval { get; set; } = 10;
    public float TickInterval { get; set; } = 1;
    public float TickStart { get; set; } = 5;
    public Int32 ColumnCount { get; set; } = 9;
    public Int32 RowCount { get; set; } = 9;
    public ObjectTypes ObjectTypes { get; set; }
    public float IncrementalSpawn { get; set; }
    public Int32 IncrementalSize { get; set; }
    public float ExtraChance { get; set; } = 0.2f;
    private Dictionary<String, Dictionary<String, float>> ExtraWeights { get; set; }


    public Dictionary<String, Dictionary<String, float>> GetExtraWeights()
    {
        if (this.ExtraWeights == default)
        {
            this.ExtraWeights = GetExtraWeightsForList(ObjectTypes.Tiles);
        }
        return this.ExtraWeights;
    }

    private Dictionary<String, Dictionary<String, float>> GetExtraWeightsForList(List<TileType> tileTypeList)
    {
        Dictionary<String, Dictionary<String, float>> weights = new();
        foreach (TileType tileType in tileTypeList)
        {
            Dictionary<String, float> extraWeights = GetExtraWeights(tileType);
            weights[tileType.Reference] = extraWeights;
        }

        return weights;
    }

    private Dictionary<String, float> GetExtraWeights(TileType tileType)
    {
        Dictionary<String, float> weights = new Dictionary<String, float>();
        float weightSum = 0;
        foreach (KeyValuePair<String, float> t in tileType.Extras)
        {
            float weight = t.Value;
            weightSum += weight;
            weights[t.Key] = weightSum;
        }

        foreach (String key in weights.Keys.ToList())
        {
            weights[key] = weights[key] / weightSum;
        }

        return weights;
    }
}