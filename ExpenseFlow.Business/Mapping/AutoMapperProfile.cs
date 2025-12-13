using AutoMapper;
using ExpenseFlow.Business.DTOs;
using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Expense, UpdatePaymentStatusDto>()
                .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Expense, ApprovedExpenseDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Kategori Yok"));


        }
    }
}
