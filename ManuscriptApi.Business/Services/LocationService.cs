using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.DataAccess.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class LocationService : DapperCrudService<Location>
    {
        public LocationService(ILocationRepository repository) : base(repository)
        {
        }
    }
}
