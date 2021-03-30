using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Core.Helpers
{
    public static class AcquisitionImageTypeHelper
    {
        public static List<string> OutsidePhotos => new List<string>
        {
            AcquisitionImageTypes.LEFT_FRONT, AcquisitionImageTypes.RIGHT_FRONT, AcquisitionImageTypes.LEFT_BACK, AcquisitionImageTypes.RIGHT_BACK
        };

        public static List<string> InsidePhotos => new List<string>
        {
            AcquisitionImageTypes.FRONTSEATS, AcquisitionImageTypes.BACKSEATS, AcquisitionImageTypes.DASH, AcquisitionImageTypes.KM
        };

        public static List<string> DamagePhotos => new List<string> {
            AcquisitionImageTypes.DMG_OUTSIDE, AcquisitionImageTypes.DMG_INSIDE
        };

        public static List<string> DocumentationPhotos => new List<string>
        {
            AcquisitionImageTypes.ENROLLMENT_FRONT, AcquisitionImageTypes.ENROLLMENT_BACK, AcquisitionImageTypes.EXAMINATION
        };

        public static List<string> InsidePhotosDamages => new List<string>
        {
           AcquisitionImageTypes.KM_DAMAGES, AcquisitionImageTypes.CHASSIS_NUMBER
        };

        public static List<string> DocumentationPhotosDamages => new List<string>
        {
            AcquisitionImageTypes.ENROLLMENT_FRONT_DAMAGES, AcquisitionImageTypes.ENROLLMENT_BACK_DAMAGES, AcquisitionImageTypes.PROOF_OF_INSURANCE
        };

        public static Dictionary<string, string> AcquisitionImageTypeAndName = new Dictionary<string, string>()
        {
            { AcquisitionImageTypes.LEFT_FRONT, FelixResources.acquisition_photos_leftFrontTitle },
            { AcquisitionImageTypes.RIGHT_FRONT, FelixResources.acquisition_photos_rightFrontTitle },
            { AcquisitionImageTypes.LEFT_BACK, FelixResources.acquisition_photos_leftBackTitle },
            { AcquisitionImageTypes.RIGHT_BACK, FelixResources.acquisition_photos_rightBackTitle },
            { AcquisitionImageTypes.FRONTSEATS, FelixResources.acquisition_photos_frontseatsTitle },
            { AcquisitionImageTypes.BACKSEATS, FelixResources.acquisition_photos_backseatsTitle },
            { AcquisitionImageTypes.DASH, FelixResources.acquisition_photos_dashTitle },
            { AcquisitionImageTypes.KM, FelixResources.acquisition_photos_kmTitle },
            { AcquisitionImageTypes.KM_DAMAGES, FelixResources.acquisition_photos_kmTitle },
            { AcquisitionImageTypes.DMG_OUTSIDE, FelixResources.acquisition_photos_damageOutsideTitle },
            { AcquisitionImageTypes.DMG_INSIDE, FelixResources.acquisition_photos_damageInsideTitle },
            { AcquisitionImageTypes.ENROLLMENT_FRONT, FelixResources.acquisition_photos_frontEnrollmentTitle },
            { AcquisitionImageTypes.ENROLLMENT_BACK, FelixResources.acquisition_photos_backEnrollmentTitle },
            { AcquisitionImageTypes.ENROLLMENT_FRONT_DAMAGES, FelixResources.acquisition_photos_frontEnrollmentTitle },
            { AcquisitionImageTypes.ENROLLMENT_BACK_DAMAGES, FelixResources.acquisition_photos_backEnrollmentTitle },
            { AcquisitionImageTypes.EXAMINATION, FelixResources.acquisition_photos_examinationProofTitle },
            { AcquisitionImageTypes.CHASSIS_NUMBER, FelixResources.acquisition_photos_chassisNumberTitle },
            { AcquisitionImageTypes.PROOF_OF_INSURANCE, FelixResources.acquisition_photos_proofOfInsuranceTitle }
        };

        public static Dictionary<string, int> AcquisitionImageTypeOrder = new Dictionary<string, int>()
        {
            { AcquisitionImageTypes.LEFT_FRONT, 0 },
            { AcquisitionImageTypes.RIGHT_FRONT, 1 },
            { AcquisitionImageTypes.LEFT_BACK, 2 },
            { AcquisitionImageTypes.RIGHT_BACK, 3 },
            { AcquisitionImageTypes.FRONTSEATS, 4 },
            { AcquisitionImageTypes.BACKSEATS, 5},
            { AcquisitionImageTypes.DASH, 6 },
            { AcquisitionImageTypes.KM, 7 },
            { AcquisitionImageTypes.DMG_OUTSIDE, 8 },
            { AcquisitionImageTypes.DMG_INSIDE, 9 },
            { AcquisitionImageTypes.ENROLLMENT_FRONT, 10},
            { AcquisitionImageTypes.ENROLLMENT_BACK, 11 },
            { AcquisitionImageTypes.EXAMINATION, 12 },
            { AcquisitionImageTypes.KM_DAMAGES, 5 },
            { AcquisitionImageTypes.CHASSIS_NUMBER, 6},
            { AcquisitionImageTypes.ENROLLMENT_FRONT_DAMAGES, 8 },
            { AcquisitionImageTypes.ENROLLMENT_BACK_DAMAGES, 9},
            { AcquisitionImageTypes.PROOF_OF_INSURANCE, 10 }
        };
    }
}
