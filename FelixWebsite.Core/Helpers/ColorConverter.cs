using System.Globalization;

namespace FelixWebsite.Core.Helpers
{
    public static class ColorConverter
    {
        public static string ConvertHexToRgba(this string hexValue, decimal opacity)
        {
            if (hexValue.Length != 6)
                return "rgba(0,0,0,0." + opacity + ")";
            int red = int.Parse(hexValue.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int green = int.Parse(hexValue.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int blue = int.Parse(hexValue.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            return "rgba(" + red + "," + green + "," + blue + ", 0." + opacity + ")";
        }
    }
}
