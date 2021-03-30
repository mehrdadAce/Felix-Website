using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FelixWebsite.Bdo.Models.VDAB_XML
{
    [XmlRoot("PositionOpening", Namespace="http://ns.hr-xml.org/2006-02-28", IsNullable=false)]
    public class PositionOpening
    {
        [XmlAttribute("xml:lang", DataType = "language")]
        public string attrLang { get; set; }
        public PositionRecordInfo PositionRecordInfo { get; set; }
        public PositionSupplier PositionSupplier { get; set; }
        public PositionProfile PositionProfile { get; set; }
        public int NumberToFill { get; set; }
    }

    public class PositionProfile
    {
        [XmlAttribute("xml:lang", DataType = "language")]
        public string attrLang { get; set; }
        public PositionDateInfo PositionDateInfo { get; set; }
        public Organization Organization { get; set; }
        public PositionDetail PositionDetail { get; set; }
        [XmlElement("FormattedPositionDescription")]
        public List<FormattedPositionDescription> FormattedPositionDescriptions { get; set; }
        public HowToApply HowToApply { get; set; }
    }

    public class HowToApply
    {
        public PersonName PersonName { get; set; }
        public ApplicationMethod ApplicationMethod { get; set; }
    }

    public class ApplicationMethod
    {
        public Telephone Telephone { get; set; }
        public string InternetEmailAddress { get; set; }
        public string InternetWebAddress { get; set; }
        public PostalAddress PostalAddress { get; set; }
    }

    public class Telephone
    {
        public string FormattedNumber { get; set; }
    }

    public class PersonName
    {
        public string FormattedName { get; set; }
    }

    public class FormattedPositionDescription
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class PositionDetail
    {
        public PhysicalLocation PhysicalLocation { get; set; }
        public JobCategory JobCategory { get; set; }
        public string PositionTitle { get; set; }
        public string PositionClassification { get; set; }
        [XmlElement("PositionSchedule")]
        public List<string> PositionSchedule { get; set; }
        public Shift Shift { get; set; }
        [XmlElement("Competency")]
        public List<Competency> Competency { get; set; }
        public UserArea UserArea { get; set; }
    }

    public class UserArea
    {
        public Experience Experience { get; set; }
    }

    public class Experience
    {
        [XmlText]
        public int Value { get; set; }
        [XmlAttribute("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }
    }

    public class Competency
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("required")]
        public Boolean Required { get; set; }
        public CompetencyId CompetencyId { get; set; }
        public TaxonomyId TaxonomyId { get; set; }
        public CompetencyEvidence CompetencyEvidence { get; set; }
    }

    public class CompetencyEvidence
    {
        public NumericValue NumericValue { get; set; }
    }

    public class NumericValue
    {
        [XmlText]
        public string Value { get; set; }
        [XmlAttribute("minValue")]
        public string MinValue { get; set; }
        [XmlAttribute("maxValue")]
        public string MaxValue { get; set; }
    }

    public class TaxonomyId
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    public class CompetencyId
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    public class Shift
    {
        [XmlAttribute("shiftPeriod")]
        public string shiftPeriod { get; set; }
        public int Name { get; set; }
        public int Hours { get; set; }
    }

    public class JobCategory
    {
        public string TaxonomyName { get; set; }
        public string CategoryCode { get; set; }
    }

    public class PhysicalLocation
    {
        public PostalAddress PostalAddress { get; set; }
    }

    public class Area
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class Organization
    {
        public string OrganizationName { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }

    public class ContactInfo
    {
        public ContactMethod ContactMethod { get; set; }
    }

    public class PositionDateInfo
    {
        public bool StartAsSoonAsPossible { get; set; }
    }

    public class PositionSupplier
    {
        public SupplierId SupplierId { get; set; }
        public ContactMethod ContactMethod { get; set; }
    }

    public class ContactMethod
    {
        public string InternetEmailAddress { get; set; }
        public PostalAddress PostalAddress { get; set; }
    }

    public class PostalAddress
    {
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Municipality { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
    }

    public class DeliveryAddress
    {
        public string StreetName { get; set; }
        public string BuildingNumber { get; set; }
    }

    public class SupplierId
    {
        [XmlAttribute("idOwner")]
        public string IdOwner{ get; set; }
        public string IdValue { get; set; }
    }

    public class PositionRecordInfo
    {
        public Id Id { get; set; }
        public Status Status { get; set; }
    }

    public class Status
    {
        [XmlText]
        public string Current { get; set; }
        [XmlAttribute("validFrom")]
        public string ValidFrom { get; set; }
        [XmlAttribute("validTo")]
        public string ValidTo { get; set; }
    }

    public class Id
    {
        public string IdValue { get; set; }
        [XmlAttribute("idOwner")]
        public string IdOwner { get; set; }
    }
}
