using AutoMapper;

namespace Model.DTO
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Costumer, CostumerDTO>();
            CreateMap<CostumerDTO, Costumer>();
        }
    }
}
