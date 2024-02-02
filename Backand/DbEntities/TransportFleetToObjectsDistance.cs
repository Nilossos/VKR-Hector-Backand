using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
    [PrimaryKey(nameof(TransportFleetId), nameof(ObjectsId))]
    public class TransportFleetToObjectsDistance
    {
        public int TransportFleetId { get; init; }

        public int ObjectsId { get; init; }

        public decimal? Distance { get; init; }
    }
}
