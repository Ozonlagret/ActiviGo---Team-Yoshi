using Application.DTOs.Requests;
using Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IActivitySessionService
    {
        Task<ActivitySessionResponse> CreateAsync(CreateActivitySessionRequest req, CancellationToken ct = default);
    }
}
