using Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IActivityService
    {
        Task<IEnumerable<GetActivityDto>> GetAllAsync();
        Task<GetActivityDto> GetByIdAsync(int id);
    }
}
