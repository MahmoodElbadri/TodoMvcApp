using AutoMapper;

namespace Todo.Models.DTO;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TodoTaskAddRequest, TodoTask>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();
        CreateMap<TodoTaskUpdateRequest, TodoTask>()
            .ReverseMap();
        CreateMap<TodoTaskResponse, TodoTask>()
            .ReverseMap();
    }
}
