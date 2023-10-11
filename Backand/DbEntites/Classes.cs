using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using NpgsqlTypes;

namespace Backand.DbEntites
{

    public class Objects_Construction
    {
       [Key]
       public int ObjectsId { get; set; }
       public int ConstructionId { get; set; }
    }

    public class ConstructionUnitType
    {
        [Key]
        public int ConstructionUnitTypeId { get; set;}
        public string Name { get; set; }
    }

    public class ConstructionUnit
    {
        [Key]
        public int ConstructionUnitId { get; set; }
        public int ConstructionUnitTypeId { get; set; }
        public string Name { get; set; }
        public string MeasuringUnit { get; set; }
    }

    public class MaterialSet
    {
        [Key]
        public int MaterialSetId { get; set; }
        public int ConstructionId { get; set; }
    }

    public class MaterialSet_ConstructionUnit
    {
        public int MaterialSet_ConstructionUnitId { get; set; }
        public int MaterialSetId { get; set; }
        public int ConstructionUnitId { get; set; }
        public float Amount { get; set; }
    }

    public class CompanyType
    {
        public int CompanyTypeId { get; set; }
        public string Name { get; set; }
    }

    public class Company
    {
        public int CompanyId { get; set; }
        public int CompanyTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }
    public class Manufacturer
    {
        [Key]
        public int ManufacturerId { get; set; }
        public string Name { get; set; }

    }

    public class LogisticCompany
    {
        [Key]
        public int LogisticCompanyId { get; set; }
        public string Name { get; set; }

    }

    public class Storage
    {
        [Key]
        public int StorageId { get; set; }
        public string Name { get; set; }
        public NpgsqlTypes.NpgsqlPoint Coordinates { get; set; }

        public string Address { get; set; }
        public int ManufacturerId { get; set; }
    }

    public class Storage_ConstructionUnit
    {
        public int Storage_ConstructionUnitId { get; set; }
        public int StorageId { get; set; }
        public int ConstructionUnitId { get; set; }
        public float Amount { get; set; }
        public decimal Price { get; set;}
    }

    public class TransportType
    {
        public int TransportTypeId { get; set; }
        public string Name { get; set; }
    }

    public class TransportMode
    {
        public int TransportModeId { get; set; }
        public string Name { get; set; }
        public float AvgSpeed { get; set; }
        public int TransportTypeId { get; set; }
    }

    public class CoefficientType
    {
        public int CoefficientTypeId { get; set; }
        public string Name { get; set; }
    }

    public class CompanyTransport
    {
        public int CompanyTransportId { get; set; }
        public int TransportModeId { get; set; }
        public int CompanyId { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public int CoefficientTypeId { get; set; }
        public float CoefficientValue { get; set; }
    }

    public class DeliveryAbility
    {
        public int DeliveryAbilityId { get; set;}
        public int CompanyTransportId { get; set;}
        public int StorageId { get; set;}
        public int ObjectsId { get; set; }

    }

    public class UserType
    {
        public int UserTypeId { get; set;}
        public string Name { get; set; }
    }

    public class User
    {
        public int UserId { get; set;}
        public int UserTypeId { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Post { get; set; }
        public string Proto { get; set; }
        public string Token { get; set; }
    }




    /*    public class ContactInfoType
        {
            public int ContactInfoTypeId { get; set;}
            public string Name { get; set; }
        }
    */






}

