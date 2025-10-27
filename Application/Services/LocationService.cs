using System;
using Application.Interfaces.Service;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repository;

namespace Application.Services
{
    public class LocationService : ILocationService
{
    private readonly ILocationRepository _repo;

    public LocationService(ILocationRepository repo) => _repo = repo;

    public async Task<IEnumerable<LocationsForFilterResponse>> GetAllAsync()
    {
        var locations = await _repo.GetAllAsync();
        return locations.Select(l => new LocationsForFilterResponse { Id = l.Id, Name = l.Name });
    }
}
}
