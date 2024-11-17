using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backand.DbEntities;

[Table("Object_TransportType")]
public partial class ObjectTransportType
{
    [Key]
    [Column("Object_TransportTypeId")]
    public int ObjectTransportTypeId { get; set; }

    public int ObjectId { get; set; }

    public int TransportTypeId { get; set; }
    public virtual ObjectEntity? Object { get; set; }
    public virtual TransportType? TransportType { get; set; }
}
