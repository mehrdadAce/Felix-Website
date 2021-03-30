using FelixWebsite.Core.App_GlobalResources;

namespace FelixWebsite.Shared.enums
{
    public enum TyreState
    {
        Goed = 1,
        Matig,
        Slecht
    }

    public static class TyreStateExtensions
    {
        public static string GetText(this TyreState manual)
        {
            switch (manual)
            {
                case TyreState.Goed:
                    return FelixResources._2dehands_tyrestate1;
                case TyreState.Matig:
                    return FelixResources._2dehands_tyrestate2;
                case TyreState.Slecht:
                    return FelixResources._2dehands_tyrestate3;
                default:
                    return FelixResources._2dehands_tyrestate3;
            }
        }
    }
}
