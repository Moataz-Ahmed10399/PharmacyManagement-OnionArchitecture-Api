using Mapster;
using Pharmacy.Dto.CategoryDto;
using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Mappers
{
    public class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Category, CategoryReadDTO>.NewConfig().TwoWays();



            TypeAdapterConfig<CategoryCreateDTO, Category>.NewConfig()
               .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
               .Map(dest => dest.IsDeleted, src => false);


            //TypeAdapterConfig<CategoryReadDTO, Category>.NewConfig()
            //   .Map(dest => dest.Medicines.Count, src => src.MedicineCount);
            TypeAdapterConfig<Category, CategoryReadDTO>.NewConfig()
                 .Map(dest => dest.MedicineCount, src => src.Medicines.Count);



        }

    }
}
