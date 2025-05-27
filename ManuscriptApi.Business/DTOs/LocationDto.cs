using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuscriptApi.Business.DTOs
{
    public class LocationDto
    {
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
    }
}
