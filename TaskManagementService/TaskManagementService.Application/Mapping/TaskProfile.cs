using AutoMapper;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.Application.Mapping
{
    /// <summary>
    /// Профиль маппинга для задач
    /// </summary>
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskEntity, TaskDto>();
            CreateMap<CreateTaskDto, TaskEntity>();
            CreateMap<UpdateTaskDto, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore());
        }
    }
} 