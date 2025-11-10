using AutoMapper;
using CitizensPortal.Application.Contracts.DTOs.Citizen;
using CitizensPortal.Application.Contracts.DTOs.IssueReport;
using CitizensPortal.Application.Contracts.DTOs.Application;
using CitizensPortal.Application.Contracts.DTOs.Bill;
using CitizensPortal.Application.Contracts.DTOs.Notification;
using CitizensPortal.Application.Contracts.DTOs.Ticket;
using CitizensPortal.Application.Contracts.DTOs.Payment;
using CitizensPortal.Application.Contracts.DTOs.Document;
using CitizensPortal.Application.Contracts.DTOs.Property;
using CitizensPortal.Application.Contracts.DTOs.Appointment;
using CitizensPortal.Application.Contracts.DTOs.Certificate;
using CitizensPortal.Application.Contracts.DTOs.Infrastructure;
using CitizensPortal.Application.Contracts.DTOs.Community;
using CitizensPortal.Application.Contracts.DTOs.Emergency;
using CitizensPortal.Application.Contracts.DTOs.ServiceRequest;
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

            // PHASE 2 - Enhanced Services Mappings

            // Payment mappings
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<PaymentAllocation, PaymentAllocationDto>();
            CreateMap<CreatePaymentAllocationDto, PaymentAllocation>();

            // Document mappings
            CreateMap<Domain.Entities.Document, DocumentDto>();
            CreateMap<CreateUpdateDocumentDto, Domain.Entities.Document>();

            // Property mappings
            CreateMap<Property, PropertyDto>();
            CreateMap<CreateUpdatePropertyDto, Property>();
            CreateMap<PropertyOwnership, PropertyOwnershipDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));

            // Appointment mappings
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));
            CreateMap<CreateUpdateAppointmentDto, Appointment>();
            CreateMap<Department, DepartmentDto>();

            // Certificate mappings
            CreateMap<CertificateRequest, CertificateRequestDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));
            CreateMap<CreateUpdateCertificateRequestDto, CertificateRequest>();
            CreateMap<Certificate, CertificateDto>();

            // PHASE 3 - Advanced Features Mappings

            // Infrastructure mappings
            CreateMap<InfrastructureProject, InfrastructureProjectDto>();
            CreateMap<CreateUpdateInfrastructureProjectDto, InfrastructureProject>();
            CreateMap<ProjectUpdate, ProjectUpdateDto>();

            // Community mappings
            CreateMap<Survey, SurveyDto>();
            CreateMap<CreateUpdateSurveyDto, Survey>();
            CreateMap<SurveyQuestion, SurveyQuestionDto>();
            CreateMap<Consultation, ConsultationDto>();
            CreateMap<Vote, VoteDto>();
            CreateMap<VoteOption, VoteOptionDto>();
            CreateMap<ForumPost, ForumPostDto>()
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.ForumTopic != null ? src.ForumTopic.Name : string.Empty));

            // Emergency mappings
            CreateMap<EmergencyAlert, EmergencyAlertDto>();
            CreateMap<CreateUpdateEmergencyAlertDto, EmergencyAlert>();
            CreateMap<EvacuationRoute, EvacuationRouteDto>();

            // Service Request mappings
            CreateMap<ServiceRequest, ServiceRequestDto>()
                .ForMember(dest => dest.CitizenName, opt => opt.MapFrom(src => src.Citizen != null ? $"{src.Citizen.FirstName} {src.Citizen.LastName}" : string.Empty));
            CreateMap<CreateUpdateServiceRequestDto, ServiceRequest>();
            CreateMap<ServiceRequestUpdate, ServiceRequestUpdateDto>();
        }
    }
}
