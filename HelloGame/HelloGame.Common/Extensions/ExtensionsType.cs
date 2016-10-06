using System;

namespace HelloGame.Common.Extensions
{
    public static class EnumHelper
    {
        public static TEnum GetRandomEnumValue<TEnum>()
        {
            var v = Enum.GetValues(typeof(TEnum));
            return (TEnum) v.GetValue(new Random().Next(v.Length));
        }
    }
}