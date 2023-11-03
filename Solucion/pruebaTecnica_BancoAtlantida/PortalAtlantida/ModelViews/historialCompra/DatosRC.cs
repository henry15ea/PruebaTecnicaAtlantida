using System.Collections.Generic;

namespace PortalAtlantida.ModelViews.historialCompra
{
    public class DatosRC<T>
    {
        public bool status { get; set; }
        public List<T> result { get; set; }
    }
}
