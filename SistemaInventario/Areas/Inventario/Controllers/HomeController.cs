using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.Especificaciones;
using SistemaInventario.Modelos.ViewModels;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //ACA SE HABILITA LA UNIDAD DE TRABAJO PORQUE NECESITO TRAERME TODOS LOS PRODUCTOS DE LA BASE DE DATOS A ESTA VISTA Y SE CARGUEN EN EL INDEX
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, string busqueda ="", string busquedaActual="")
        {
            if(!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;

            if (pageNumber < 1) { pageNumber = 1; }

            // ACA CONTROLAMOS LA CANTIDAD DE ARTICULOS POR PAGINA
            Parametros parametros = new Parametros()
            {
                PageNumber= pageNumber,
                PageSize = 4
            };

            //IEnumerable<Producto> productoLista = await _unidadTrabajo.Producto.ObtenerTodos();

            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            if(!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Descripcion.Contains(busqueda));
            }

            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; // clase CSS para desactivar el boton
            ViewData["Siguiente"] = ""; // clase CSS para desactivar el boton

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }

            return View(resultado);
            //return View(productoLista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
