using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ArrayExtensions
    {
        public static void SetValueAtRandom<TArray>(this TArray[,] array, TArray value)
        {
            if (array != default)
            {
                var random1 = Random.Range(0, array.GetLength(0) - 1);
                var random2 = Random.Range(0, array.GetLength(1) - 1);

                array[random1, random2] = value;
            }
        }
    }
}
