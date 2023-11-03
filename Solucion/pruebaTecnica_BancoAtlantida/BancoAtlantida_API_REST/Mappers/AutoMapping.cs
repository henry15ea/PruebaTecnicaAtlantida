using AutoMapper;
using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.Entidades;

namespace BancoAtlantida_API_REST.Mappers
{
    //esta clase sirve para mapperar modelos automaticamente
    public class AutoMapping : Profile
    {

        public AutoMapping() {
            CreateMap< HistorialCompraByMesDTO, HistorialComprasByFechaEntidad> ();
        }
    }
}
