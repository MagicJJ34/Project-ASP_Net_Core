using AutoMapper;
using TaskManagerApi.Models;
using TaskManagerApi.DTOs;
namespace TaskManagerApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<TaskItem, TaskResponseDto>();
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>();
        }
    }
}
