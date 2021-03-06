﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DgrosStore.Models.customValidation;
namespace DgrosStore.Models
{
    public class Client : Person
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Debe introducir el apellido del cliente")]
        [MinLength(3, ErrorMessage = "El apellido debe tener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El apellido debe ser de 20 o menos caracteres")]
        public string LastName { get; set; }

        [IdCardValidation]
        [Required(ErrorMessage ="El campo cedula es obligatorio")]
        [RegularExpression(@"\b\d{3}\-?\d{7}\-?\d{1}\b", ErrorMessage ="la cedula no es valida")]
        public string IdCard { get; set; }

        //collecion de ventas a clientes(facturas)
        public ICollection<Sales> Sales { get; set; }
    }
}