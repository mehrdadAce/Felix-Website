using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Configuration;
using System.Xml.Serialization;
using FelixWebsite.Core.Helpers;
using System.Collections.Generic;
using FelixWebsite.Bdo.Models.JobOffer;
using FelixWebsite.Bdo.Models.VDAB_XML;

namespace FelixWebsite.Bll.Helpers.VDAB
{
    public static class XmlHelper
    {
        /// <summary>
        /// This Method converts an ConvertedUmbracoJobOffer object to a XML-Document based on the PositionOpening class.
        /// </summary>
        /// <param name="job">ConvertedUmbracoJobOffer, all properties of this object are required</param>
        /// <returns>XmlDocument to send to the VDAB database or the VDAB OLA</returns>
        public static XmlDocument ConvertConvertedUmbracoJobOfferToVDABXML(this ConvertedUmbracoJobOffer job)
        {
            try
            {
                PositionOpening positionOpening = new PositionOpening() { attrLang = "NL" };

                // Set unique Id for joboffer
                positionOpening.PositionRecordInfo = SetPositionRecordInfo(job.Id, job.VdabName);
                // Set timespan for joboffer
                positionOpening.PositionRecordInfo.Status = SetStatus(job.ValidFrom, job.ValidTo , job.Status);
                // Set supplierId
                positionOpening.PositionSupplier = SetPositionSupplier(ConfigHelper.SupplierId, job.VdabJobEmail);
                
                positionOpening.PositionProfile = SetPositionProfile();
                positionOpening.PositionProfile.Organization = SetOrganisationDetails(job.CompanyInfo.Name, job.CompanyInfo.StreetName, job.CompanyInfo.BuildingNumber, job.CompanyInfo.PostalCode, job.CompanyInfo.Municipality, "BE");
                positionOpening.PositionProfile.PositionDetail = new PositionDetail();
                positionOpening.PositionProfile.PositionDetail.PhysicalLocation = SetPhysicalLocation(job.CompanyInfo.StreetName, job.CompanyInfo.BuildingNumber, job.CompanyInfo.PostalCode, job.CompanyInfo.Municipality, "BE");
                positionOpening.PositionProfile.PositionDetail.JobCategory = SetJobCategory(job.Competences.Id);
                positionOpening.PositionProfile.PositionDetail.PositionTitle = job.PositionTitle;

                positionOpening.PositionProfile.PositionDetail.PositionClassification = job.LaborContract.Code;

                List<string> posSchedules = new List<string>();
                posSchedules.Add(job.LaborSystem.Code);
                posSchedules.Add(job.LaborArrangement.Code);
                positionOpening.PositionProfile.PositionDetail.PositionSchedule = SetPositionSchedules(posSchedules);

                #region Competences

                List<Competency> competences = new List<Competency>();

                competences = SetStudyRequirements(competences, job.DiplomaSelector.ToList());

                competences = SetDriversLicense(competences, job.DriverLicense.ToList());

                competences = SetSERVRequirements(competences, job.Competences.competences);

                competences = SetLanguageRequirements(competences, job.Languages.ToList());

                positionOpening.PositionProfile.PositionDetail.Competency = competences;

                #endregion

                positionOpening.PositionProfile.PositionDetail.UserArea = SetExperience(job.ExperienceInMonths);
                positionOpening.PositionProfile.FormattedPositionDescriptions = SetFormattedPositionDescriptions(job.JobDescription, job.RequiredQualifications, job.RenumerationDescription);
                positionOpening.PositionProfile.HowToApply = SetHowToApply(job.CompanyInfo.Name, job.CompanyInfo.PhoneNumber, job.VdabJobEmail, job.CompanyInfo.WebsiteUrl, positionOpening.PositionProfile.Organization);
                positionOpening.NumberToFill = 1;

                return ConvertObjectToXML(positionOpening);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static PhysicalLocation SetPhysicalLocation(string streetName, string buildingNumber, string postalCode, string municipality, string countryCode)
        {
            PhysicalLocation physicalLocation = new PhysicalLocation()
            {
                PostalAddress = new PostalAddress()
                {
                    CountryCode = countryCode,
                    PostalCode = postalCode,
                    Municipality = municipality,
                    DeliveryAddress = new DeliveryAddress()
                    {
                        StreetName = streetName,
                        BuildingNumber = buildingNumber
                    }
                }
            };
            return physicalLocation;
        }

        private static XmlDocument ConvertObjectToXML(PositionOpening positionOpening)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PositionOpening));

            XmlDocument xml = null;

            using (MemoryStream memStm = new MemoryStream())
            {
                serializer.Serialize(memStm, positionOpening, GetNameSpace());

                memStm.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                using (var xtr = XmlReader.Create(memStm, settings))
                {
                    xml = new XmlDocument();
                    xml.Load(xtr);
                }
            }
            return xml;
        }

        #region Set values in XML-document
        private static Status SetStatus(DateTime validFrom, DateTime validTo, string status)
        {
            return new Status() { Current = status, ValidFrom = validFrom.ToString("yyyy-MM-dd"), ValidTo = validTo.ToString("yyyy-MM-dd") };
        }

        private static List<Competency> SetLanguageRequirements(List<Competency> competences, List<LanguageCompetence> languages)
        {
            foreach (LanguageCompetence language in languages)
            {
                if (language.Selected == true && language.Score > 0)
                {
                    competences.Add(new Competency()
                    {
                        Name = "Language",
                        Required = true,
                        CompetencyId = new CompetencyId()
                        {
                            Id = language.Code,
                            Description = language.Name
                        },
                        TaxonomyId = new TaxonomyId()
                        {
                            Id = "ISO 639-1",
                            Description = "Two-character language codes in accordance with ISO 639-1"
                        },
                        CompetencyEvidence = new CompetencyEvidence()
                        {
                            NumericValue = new NumericValue()
                            {
                                MinValue = "1.0",
                                MaxValue = "5.0",
                                Value = (int)language.Score + ".0"
                            }
                        }
                    });
                }
            }
            return competences;
        }

        private static List<Competency> SetDriversLicense(List<Competency> competences, List<DriversLicense> driverLicenseList)
        {
            foreach (DriversLicense driversLicense in driverLicenseList)
            {
                competences.Add(new Competency()
                {
                    Name = "Drivers License",
                    Required = true,
                    CompetencyId = new CompetencyId()
                    {
                        Id = driversLicense.Code,
                        Description = driversLicense.Beschrijving
                    },
                    TaxonomyId = new TaxonomyId()
                    {
                        Id = "91/439/EEC",
                        Description = "Directive 91/439/EEC of 29 July 1991 on driving licenses"
                    }
                });
            }
            return competences;
        }

        private static List<Competency> SetSERVRequirements(List<Competency> competences, List<Competence> competencesList)
        {
            foreach (Competence item in competencesList)
            {
                competences.Add(new Competency()
                {
                    Name = "SERV competence",
                    Required = true,
                    CompetencyId = new CompetencyId()
                    {
                        Id = item.Id,
                        Description = item.Beschrijving
                    },
                });
            }
            return competences;
        }

        private static List<Competency> SetStudyRequirements(List<Competency> competences, List<Study> studies)
        {
            if (studies.Count() != 0)
            {
                foreach (Study study in studies)
                {
                    competences.Add(new Competency()
                    {
                        Name = "Study Code",
                        Required = true,
                        CompetencyId = new CompetencyId()
                        {
                            Id = study.Code,
                            Description = study.Beschrijving
                        },
                        TaxonomyId = new TaxonomyId()
                        {
                            Id = "StudyCodes 2.0",
                            Description = "Description StudyCodes 2.0"
                        }
                    });
                }
            }
            else
            {
                competences.Add(new Competency()
                {
                    Name = "Study Code",
                    Required = true,
                    CompetencyId = new CompetencyId()
                    {
                        Id = "A",
                        Description = "Geen Geen Specifieke Studievereisten|Aucune exigence d'études spécifiques||"
                    },
                    TaxonomyId = new TaxonomyId()
                    {
                        Id = "StudyCodes 2.0",
                        Description = "Description StudyCodes 2.0"
                    }
                });
            }
            return competences;
        }

        private static List<FormattedPositionDescription> SetFormattedPositionDescriptions(string jobDescription, string requiredQualifications, string remunerationDescription)
        {
            List<FormattedPositionDescription> formattedPositionDescriptions = new List<FormattedPositionDescription>();

            formattedPositionDescriptions.Add(new FormattedPositionDescription
            {
                Name = "jobDescription",
                Value = jobDescription
            });

            formattedPositionDescriptions.Add(new FormattedPositionDescription
            {
                Name = "requiredQualifications",
                Value = requiredQualifications
            });

            formattedPositionDescriptions.Add(new FormattedPositionDescription
            {
                Name = "remunerationDescription",
                Value = remunerationDescription
            });

            return formattedPositionDescriptions;
        }

        private static UserArea SetExperience(int amountOfMonths)
        {
            UserArea userExperience = new UserArea()
            {
                Experience = new Experience()
                {
                    Value = amountOfMonths,
                    UnitOfMeasure = "Months"
                }
            };
            return userExperience;
        }

        private static List<string> SetPositionSchedules(List<string> list)
        {
            List<string> contractTypes = new List<string>();
            foreach (string item in list)
            {
                contractTypes.Add(item);
            }
            return contractTypes;
        }

        // [Required] SERV competence pattern Id
        private static JobCategory SetJobCategory(string competencePatternId)
        {
            JobCategory jobCategory = new JobCategory()
            {
                TaxonomyName = "COMPETENTSJABLOON",
                CategoryCode = competencePatternId
            };
            return jobCategory;
        }

        private static HowToApply SetHowToApply(string organisationName, string phoneNumber, string emailAddress, string websiteUrl, Organization organization)
        {
            HowToApply howToApply = new HowToApply();
            howToApply.PersonName = new PersonName() { FormattedName = organisationName };

            howToApply.ApplicationMethod = new ApplicationMethod();

            howToApply.ApplicationMethod.Telephone = new Telephone() { FormattedNumber = phoneNumber };

            howToApply.ApplicationMethod.InternetEmailAddress = emailAddress;
            howToApply.ApplicationMethod.InternetWebAddress = websiteUrl;

            howToApply.ApplicationMethod.PostalAddress = organization.ContactInfo.ContactMethod.PostalAddress;
            return howToApply;
        }

        private static Organization SetOrganisationDetails(string organisationName, string streetName, string buildingNumber, string postalCode, string municipality, string countryCode)
        {
            Organization organization = new Organization() { OrganizationName = organisationName };
            organization.ContactInfo = new ContactInfo();
            organization.ContactInfo.ContactMethod = new ContactMethod();
            organization.ContactInfo.ContactMethod.PostalAddress = new PostalAddress()
            {
                CountryCode = countryCode,
                PostalCode = postalCode,
                Municipality = municipality,
                DeliveryAddress = new DeliveryAddress()
                {
                    StreetName = streetName,
                    BuildingNumber = buildingNumber
                }
            };
            return organization;
        }

        private static PositionProfile SetPositionProfile()
        {
            PositionProfile positionProfile = new PositionProfile() { attrLang = "NL" };

            positionProfile.PositionDateInfo = new PositionDateInfo()
            {
                StartAsSoonAsPossible = true
            };

            return positionProfile;
        }

        private static PositionSupplier SetPositionSupplier(string idVDAB, string emailStore)
        {
            PositionSupplier positionSupplier = new PositionSupplier();
            positionSupplier.SupplierId = new SupplierId()
            {
                IdOwner = "VDAB",
                IdValue = idVDAB
            };

            positionSupplier.ContactMethod = new ContactMethod()
            {
                InternetEmailAddress = emailStore
            };
            return positionSupplier;
        }

        private static PositionRecordInfo SetPositionRecordInfo(string idValue, string idOwner)
        {
            PositionRecordInfo positionRecordInfo = new PositionRecordInfo();
            positionRecordInfo.Id = new Id()
            {
                IdValue = idValue,
                IdOwner = idOwner
            };
            return positionRecordInfo;
        }

        private static XmlSerializerNamespaces GetNameSpace()
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("ns2", ConfigHelper.NameSpaceXML);
            return namespaces;
        }
        #endregion
    }
}
