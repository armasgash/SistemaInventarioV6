using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            var productoDb = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);
            if (productoDb != null)
            {
                if (producto.ImagenUrl != null)
                {
                    productoDb.ImagenUrl = producto.ImagenUrl;
                }
                productoDb.NumeroSerie = producto.NumeroSerie;
                productoDb.CodigoInterno = producto.CodigoInterno;
                productoDb.CodigoAlternativo = producto.CodigoAlternativo;
                productoDb.CodigoBarras = producto.CodigoBarras;
                productoDb.UnidadMedida = producto.UnidadMedida;
                productoDb.UnidadxEmpaque = producto.UnidadxEmpaque;
                productoDb.Existencia = producto.Existencia;
                productoDb.Descripcion = producto.Descripcion;
                productoDb.NombreCorto = producto.NombreCorto;
                productoDb.Precio = producto.Precio;
                productoDb.Costo = producto.Costo;
                productoDb.CategoriaId = producto.CategoriaId;
                productoDb.MarcaId = producto.MarcaId;
                productoDb.PadreId = producto.PadreId;
                productoDb.Estado = producto.Estado;

                _db.SaveChanges();
            }
        }
        public IEnumerable<SelectListItem> ObtenerTodosDropDownLista(string obj)
        {
            if(obj == "Categoria")
            {
                return _db.Categorias.Where(c => c.Estado==true).Select(c => new SelectListItem()
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _db.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem()
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Producto")
            {
                return _db.Productos.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
