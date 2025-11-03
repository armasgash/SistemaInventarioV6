using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    // Este atributo es para indicar donde corre el controlador
    [Area("Admin")]
    //ACA ESTE ATRIBUTO PERMITE QUE SOLO USUARIOS AUTORIZADOS PUEDAN ENTRAR
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario + "," + DS.Role_Vendedor)]
    public class ProductoController : Controller
    {
        // GET: Producto
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;

        // GET: MarcaController
        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Categoria"),
                MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Marca")/*,
                PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Producto")*/
            };

            if(id == null)
            {
                // Crear Producto
                return View(productoVM);
            }
            else
            {
                // Actualizar Producto
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades:"Categoria,Marca");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDb == null)
            {
                return Json(new { success = false, message = "Error al eliminar el Producto" });
            }
            _unidadTrabajo.Producto.Remover(productoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado Exitosamente" });
        }

        [ActionName("ValidarSerie")]
        public async Task<IActionResult> ValidarSerie(string serie, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(x => x.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(x => x.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim() && x.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if(productoVM.Producto.Id == 0)
                {
                    // Crear
                    string upload = wwwRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    // Actualizar
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p=>p.Id == productoVM.Producto.Id, isTracking:false);
                    if (files.Count > 0)
                    {
                        // Nueva imagen ha sido cargada
                        string upload = wwwRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        // Borrar la imagen anterior
                        var anteriorImagen = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorImagen))
                        {
                            System.IO.File.Delete(anteriorImagen);
                        }
                        // Subir la nueva imagen
                        using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        productoVM.Producto.ImagenUrl = fileName + extension;
                    }
                    else
                    {
                        // Mantener la imagen anterior
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                }
                TempData[DS.Exitosa] = "Operación Exitosa!";
                await _unidadTrabajo.Guardar();

                return RedirectToAction("Index");
            } // if no es valido

            productoVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Categoria");
            productoVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Marca");
            //productoVM.PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Producto");

            return View(productoVM);
        }

        #endregion
    }
}
