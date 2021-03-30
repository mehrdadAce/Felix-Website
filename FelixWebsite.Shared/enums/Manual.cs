using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Core;

namespace FelixWebsite.Core.enums
{
    public enum Manual
    {
        Volledig = 1,
        Gedeeltelijk,
        Niet
    }

    public static class ManualExtensions
    {
        public static string GetText(this Manual manual)
        {
            switch (manual)
            {
                case Manual.Volledig:
                    return FelixResources._2dehands_manual1;
                case Manual.Gedeeltelijk:
                    return FelixResources._2dehands_manual2;
                case Manual.Niet:
                    return FelixResources._2dehands_manual3;
                default:
                    return FelixResources._2dehands_manual3;
            }
        }
    }
}
