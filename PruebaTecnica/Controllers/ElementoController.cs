using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Data;
using PruebaTecnica.Models;
using System.Collections.Generic;
using System.Linq;


namespace PruebaTecnica.Controllers
{
    public class ElementoController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ElementoController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Elemento> elementos = _db.tblelementos.ToList();
            return View(elementos);

        }




        // Peticion desde la vista Elemento 
        [HttpPost]
        public IActionResult CalcularElementos(int pesoMaximo, int caloriasMinimas, List<Elemento> elementos)
        {
            List<Elemento> elementosOptimos = SeleccionarElementos(elementos, pesoMaximo, caloriasMinimas);
            return Json(elementosOptimos);
        }



        // Metodo para Seleccionar la mejor combinacion posible 
        private List<Elemento> SeleccionarElementos(List<Elemento> elementos, int pesoMaximo, int caloriasMinimas)
        {
            List<Elemento> mejorSeleccion = new List<Elemento>();
            int mejorPeso = 0;
            int mejorCalorias = 0;

            // Ordenamos la lista de menor a mayor por peso 
            elementos = elementos.OrderByDescending(e => e.Peso).ToList();

            // Genera todas las combinaciones posibles
            for (int i = 0; i < (1 << elementos.Count); i++)
            {
                List<Elemento> seleccionActual = new List<Elemento>();
                int pesoActual = 0;
                int caloriasTotales = 0;

                for (int j = 0; j < elementos.Count; j++)
                {
                    if ((i & (1 << j)) > 0)
                    {
                        seleccionActual.Add(elementos[j]);
                        pesoActual += elementos[j].Peso;
                        caloriasTotales += elementos[j].Calorias;
                    }
                }

                // Verifica si cumple con los requisitos y mejora la mejor selección actual
                if (pesoActual <= pesoMaximo && caloriasTotales >= caloriasMinimas &&
                    (mejorSeleccion.Count == 0 || pesoActual > mejorPeso || (pesoActual == mejorPeso && caloriasTotales > mejorCalorias)))
                {
                    mejorSeleccion = seleccionActual;
                    mejorPeso = pesoActual;
                    mejorCalorias = caloriasTotales;
                }
            }

            return mejorSeleccion;
        }


        // Con este metodo se seleccionaria para el ejemplo (10 peso  y 15 calorias) la misma cantidad de elementos pero menos peso y mas calorias 
        // En ejercicios de optimiacion hay diferentes alternativas todo depende de las reglas de negocio
        /* private List<Elemento> SeleccionarElementos(List<Elemento> elementos, int pesoMaximo, int caloriasMinimas)
         {
             List<Elemento> elementosSeleccionados = new List<Elemento>();
             int pesoActual = 0;
             int caloriasTotales = 0;

             // Ordena los elementos por la relación calorías/peso de mayor a menor
             elementos = elementos.OrderByDescending(e => (double)e.Calorias / e.Peso).ToList();

             foreach (var elemento in elementos)
             {
                 if (pesoActual + elemento.Peso <= pesoMaximo)
                 {
                     elementosSeleccionados.Add(elemento);
                     pesoActual += elemento.Peso;
                     caloriasTotales += elemento.Calorias;

                     // Verifica si se ha alcanzado o superado las calorías mínimas después de agregar el elemento
                     if (caloriasTotales > caloriasMinimas)
                     {
                         break;  // Sale del bucle si se alcanzan las calorías mínimas
                     }
                 }
             }

             return elementosSeleccionados;
         }*/

        public IActionResult AgregarElemento()
        {
            return View();
        }

        //Enviamos los datos del formulario y mostramos la tabla Viaje
        [HttpPost]
        public IActionResult AgregarElemento(Elemento Obj)
        {
            if (ModelState.IsValid)
            {
                _db.tblelementos.Add(Obj);
                _db.SaveChanges();
                TempData["Mensaje"] = "El elemento ha sido ingresado con exito.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "No se pudo ingresar el elemento.";
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult EliminarElemento(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Elemento Obj = _db.tblelementos.Find(id);

            if (Obj == null)
            {
                return NotFound();
            }

            _db.tblelementos.Remove(Obj);
            _db.SaveChanges();

            // Devuelve un objeto JSON con las propiedades esperadas
            return Json(new { success = true, message = "El artículo se eliminó correctamente." });
        }

        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Elemento elemento = _db.tblelementos.Find(id);

            if (elemento == null)
            {

                return NotFound();
            }
            return View(elemento);
        }


        [HttpPost]
        public IActionResult Editar(Elemento Obj)
        {

            if (ModelState.IsValid)
            {

                _db.tblelementos.Update(Obj);
                _db.SaveChanges();
                TempData["Mensaje"] = "El Elemeto  ha sido Editado con éxito.";
                return RedirectToAction("Index");

            }
            TempData["Error"] = "El Elemento no fue editado correctamente.";
            return RedirectToAction("Index");



        }

    }
}
