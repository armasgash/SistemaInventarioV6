using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Requerido")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }



        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string CodigoInterno { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string CodigoAlternativo { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string CodigoBarras { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20)]
        public string UnidadMedida { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public double UnidadxEmpaque { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public double Existencia { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100)]
        public string  Descripcion { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100)]
        public string NombreCorto { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public double Costo { get; set; }

        public string ImagenUrl { get; set; }

        public bool Estado { get; set; }

        //RELACIONES
        [Required(ErrorMessage = "Requerido")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        public int? PadreId { get; set; }
        public virtual Producto Padre { get; set; }

    }
}
