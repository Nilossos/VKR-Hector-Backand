using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
    [PrimaryKey(nameof(TransportFleetId), nameof(ObjectId))]
    public class TransportFleetToObjectDistance
    {
        public int TransportFleetId { get; init; }

        public int ObjectId { get; init; }

        public decimal? Distance { get; init; }
        
        public TransportFleet TransportFleet { get; set; }

    }
}
