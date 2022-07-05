using System.Collections.Generic;

namespace Traka.FobService.Model
{
    public class TrakaSystem : Entity
    {
        public string Name { get; set; }

        public IEnumerable<Fob> Fobs { get; set; }
    }
}
