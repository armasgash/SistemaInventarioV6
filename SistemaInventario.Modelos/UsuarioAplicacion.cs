using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        [Required(ErrorMessage="Requerido")]
        [MaxLength(80)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string CedulaRif { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string Telefono { get; set; }

        [MaxLength(10)]
        public string CodigoVendedor { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(80)]
        public string Pais { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(80)]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(80)]
        public string Ciudad { get; set; }


        [NotMapped]
        public string Role { get; set; }
    }
}
