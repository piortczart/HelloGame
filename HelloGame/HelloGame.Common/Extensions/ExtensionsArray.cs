using System;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsArray
    {
        private static readonly Random Random = new Random();

        public static TArray GetRandomItem<TArray>(this TArray[] array)
        {
            return array[Random.Next(0, array.Length)];
        }
    }
}