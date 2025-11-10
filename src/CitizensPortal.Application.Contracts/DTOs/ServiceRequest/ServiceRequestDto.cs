using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.ServiceRequest
{
    public class ServiceRequestDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string RequestNumber { get; set; }
        public ServiceRequestType Type { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public ServiceRequestPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServiceAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string PreferredTimeSlot { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string WorkPerformed { get; set; }
        public int? Rating { get; set; }
        public string Feedback { get; set; }
        public List<ServiceRequestUpdateDto> Updates { get; set; }
    }

    public class ServiceRequestUpdateDto : EntityDto<Guid>
    {
        public string UpdateMessage { get; set; }
        public ServiceRequestStatus NewStatus { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class CreateUpdateServiceRequestDto
    {
        public ServiceRequestType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServiceAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string PreferredTimeSlot { get; set; }
    }
}
