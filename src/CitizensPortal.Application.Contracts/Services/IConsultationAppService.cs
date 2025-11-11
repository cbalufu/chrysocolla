using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Community;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IConsultationAppService : ICrudAppService<ConsultationDto, Guid>
    {
        Task<List<ConsultationDto>> GetActiveConsultationsAsync();
        Task AddCommentAsync(Guid id, string comment);
    }
}
