
using FelixWebsite.Core.App_GlobalResources;

namespace FelixWebsite.Shared.enums
{
    public enum Score
    {
        nvt,
        basis,
        good,
        verygood,
        excellent
    }

    public static class ScoreExtensions
    {
        public static string GetText(this Score manual)
        {
            switch (manual)
            {
                case Score.nvt:
                    return FelixResources.talenkennis_nvt;
                case Score.basis:
                    return FelixResources.talenkennis_basic;
                case Score.good:
                    return FelixResources.talenkennis_good;
                case Score.verygood:
                    return FelixResources.talenkennis_verygood;
                case Score.excellent:
                    return FelixResources.talenkennis_excellent;
                default:
                    return FelixResources.talenkennis_nvt;
            }
        }
    }
}
