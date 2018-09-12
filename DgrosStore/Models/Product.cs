using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.customValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgrosStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [ProductNameValidation]
        [Required(ErrorMessage ="El nombre del producto no puede estar vacio")]
        [MinLength(3,ErrorMessage ="El nombre debe contener al menos 3 caracteres")]
        [MaxLength(20,ErrorMessage ="El nombre es demasiado largo , debe ser 20 caracteres o menos")]
        public string Name { get; set; }


        [Required(ErrorMessage ="Debe introducir el precio de compra")]
        [ShoppingPriceValidation]
        public float ShoppingPrice { get; set; }



        [Required(ErrorMessage ="Debe introducir la cantidad del inventario")]
        [Range(1,100,ErrorMessage ="la cantidad debe estar entre 1 y 100")]
        public int Stock { get; set; }

        public string Description { get; set; }


        public bool State { get; set; }


        public string Image { get; set; }


        [Required(ErrorMessage = "Debe de introducir el precio de compra")]
        [SellingPriceValidation]
        public float SellingPrice { get; set; }


        [Required(ErrorMessage = "Debe de introducir una cantidad minima al inventario")]
        [Range(1, 100, ErrorMessage = "la cantidad debe estar entre 1 y 100")]
        public int MinimunStock { get; set; }

        //propiedades de navegacion Categoria
        public Category Category { get; set; }


        [Required(ErrorMessage ="Debe de elegir una categoria")]
        public int CategoryId { get; set; }

        //propiedades de navegacion tienda
        public Store Store { get; set; }


        [Required(ErrorMessage = "Debe de elegir una tienda")]
        public int StoreId { get; set; }

        //colleccion de ventas
        public virtual ICollection<SalesProducs> SalesProducs { get; set; }

        //colleccion de compras
        public virtual ICollection<ShoppingProducts> ShoppingProducts { get; set; }
    }
}