using AutoMapper;
using Textile.DAL;
using Textile.DAL.Models;

namespace Textile.BLL
{
    public class TextileMapper
    {
        private static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Client, ClientModel>();
        });
  
         public static IMapper mapper = mapper = config.CreateMapper();
        public static IMapper Mapper()
        {
            return mapper;
        }
    }
}
