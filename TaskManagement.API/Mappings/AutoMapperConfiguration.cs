using AutoMapper;
using TaskManagement.API.DTOs.Auth;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Models;

namespace TaskManagement.API.Mappings
{
    public static class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto, User>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                    .ForMember(d => d.Role, opt => opt.Ignore())
                    .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                    .ForMember(d => d.CreatedTasks, opt => opt.Ignore())
                    .ForMember(d => d.AssignedTasks, opt => opt.Ignore());

                cfg.CreateMap<CreateTaskDto, TaskItem>()
                   .ForMember(d => d.Id, opt => opt.Ignore())
                   .ForMember(d => d.Status, opt => opt.Ignore())
                   .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                   .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                   .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                   .ForMember(d => d.AssignedToUser, opt => opt.Ignore())
                   .ForMember(d => d.CreatedByUser, opt => opt.Ignore());

                cfg.CreateMap<UpdateTaskDto, TaskItem>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.Status, opt => opt.Ignore())
                    .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                    .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                    .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                    .ForMember(d => d.AssignedToUser, opt => opt.Ignore())
                    .ForMember(d => d.CreatedByUser, opt => opt.Ignore());

                cfg.CreateMap<TaskItem, TaskResponseDto>();
            });

            // Fail fast if mapping is invalid
            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }
    }
}
