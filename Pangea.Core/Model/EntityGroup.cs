using System.Collections.Generic;

namespace Pangea.Core.Model
{
    public class MapEntityGroup
    {
        public MapEntityGroup(string id, IList<MapEntity> entities)
        {
            Id = id;
            Entities = entities;
        }

        public string Id { get; set; }
        public IList<MapEntity> Entities { get; set; }
    }
}
