using AutoMapper;
using CitizensPortal.Application.Contracts.DTOs.Citizen;
using CitizensPortal.Application.Contracts.DTOs.IssueReport;
using CitizensPortal.Application.Contracts.DTOs.Application;
using CitizensPortal.Application.Contracts.DTOs.Bill;
using CitizensPortal.Application.Contracts.DTOs.Notification;
using CitizensPortal.Application.Contracts.DTOs.Ticket;
using CitizensPortal.Domain.Entities;

namespace CitizensPortal.Application
{
    public class CitizensPortalApplicationAutoMapperProfile : Profile
    {
        public CitizensPortalApplicationAutoMapperProfile()
        {
            // Citizen mappings
            CreateMap<Citizen, CitizenDto>();
            CreateMap<CreateUpdateCitizenDto, Citizen>();

            // IssueReport mappings
            CreateMap<IssueReport, IssueReportDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));
            CreateMap<CreateUpdateIssueReportDto, IssueReport>();
            CreateMap<IssueAttachment, IssueAttachmentDto>();
            CreateMap<IssueComment, IssueCommentDto>();

            // Application mappings
            CreateMap<Domain.Entities.Application, ApplicationDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));
            CreateMap<CreateUpdateApplicationDto, Domain.Entities.Application>();
            CreateMap<ApplicationDocument, ApplicationDocumentDto>();

            // Bill mappings
            CreateMap<Bill, BillDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));

            // Notification mappings
            CreateMap<Notification, NotificationDto>();

            // Ticket mappings
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));
            CreateMap<CreateUpdateTicketDto, Ticket>();
            CreateMap<TicketMessage, TicketMessageDto>();
        }
    }
}
