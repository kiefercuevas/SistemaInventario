using System.Collections.Generic;
namespace DgrosStore.Models
{
    public class Shopping :Transaction
    {
        //propiedades de navegacion tienda
        public Store Store { get; set; }
        public int StoreId { get; set; }

        //propiedades de navegacion Proveedor
        public Provider Provider { get; set; }
        public int ProviderId { get; set; }

        //colleccion de compras
        public virtual ICollection<ShoppingProducts> ShoppingProducts { get; set; }
    }
}