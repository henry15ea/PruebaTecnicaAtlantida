using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.tools;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using BancoAtlantida_API_REST.Models.requestEntidad;
using BancoAtlantida_API_REST.Entidades;

namespace BancoAtlantida_API_REST.Models
{
    public class HistorialComprasMesModel
    {
        private SqlConnection conection = null;
        private RequestModelEntidad requestModelEntidad = null;

        //esta funcion devuelve automaticamente el historial de la compra de el mes actual y el anterior
        public RequestModelEntidad fn_HistrialCompraDosMeses(HistorialComprasByFechaEntidad datos)
        {

            bool resp = false;
            requestModelEntidad = new RequestModelEntidad();

            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();
                // Obtener la fecha actual
                DateTime fechaActual = DateTime.Now;
                // Obtener el primer día del mes anterior
                DateTime primerDiaMesAnterior = new DateTime(fechaActual.Year, fechaActual.Month - 1, 1);

                // Obtener el último día del mes anterior
                DateTime primerDiaMesActual = new DateTime(fechaActual.Year, fechaActual.Month, 1);
                DateTime ultimoDiaMesActual = primerDiaMesActual.AddMonths(1).AddDays(-1);

                // Convertir las fechas al formato requerido
                string VfechaInicio = primerDiaMesAnterior.ToString("yyyy-MM-dd");
                string VfechaFin = ultimoDiaMesActual.ToString("yyyy-MM-dd");

                
               

                var parameters = new
                {
                    id_cuenta = datos.IdCuenta.Trim(),
                    fechaInicio = VfechaInicio,
                    fechaFinal = VfechaFin
                };

                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        //ejecutamos el procedure

                        var resultData = connection.Query(
                            "sp_selectComprasByFechaMes",
                            parameters,
                            commandType: CommandType.StoredProcedure).AsList();

                        //resp = result.Any();
                        if (resultData.Count >= 0)
                        {
                            requestModelEntidad.result = resultData;
                            requestModelEntidad.status = true;
                        }
                        else
                        {
                            requestModelEntidad.status = false;
                        }
                    }

                    conection.Close();
                }
                else
                {
                    conection.Open();

                    using (var connection = conection)
                    {
                        
                        //ejecutamos el procedure
                        var resultData = connection.Query(
                            "sp_selectComprasByFechaMes",
                            parameters,
                            commandType: CommandType.StoredProcedure).AsList();

                        //resp = result.Any();
                        if (resultData.Count >= 0)
                        {
                            requestModelEntidad.result = resultData;
                            requestModelEntidad.status = true;
                        }
                        else
                        {
                            requestModelEntidad.status = false;
                        }
                    }

                    conection.Close();

                }



                return requestModelEntidad;
            }
            catch (Exception e)
            {

                return requestModelEntidad;
            }


        }
    }
}
