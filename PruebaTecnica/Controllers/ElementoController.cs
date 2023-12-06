using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models;
using System.Collections.Generic;
using System.Linq;


namespace PruebaTecnica.Controllers
{
    public class ElementoController : Controller
    {
        public IActionResult Index()
        {

            // Esta lista para tener la persistencia de los datos Debe ser una consulta en la base de datos que devuelva la lista 
            // Esta lista puede ser mas grande o mas pequeña dependiendo del ejemplo
            var elementos = new List<Elemento>
            {
                new Elemento { Nombre = "Cuerda", Peso = 5, Calorias = 3 },
                new Elemento { Nombre = "Casco", Peso = 3, Calorias = 5 },
                new Elemento { Nombre = "Mosquetones", Peso = 5, Calorias = 2 },
                new Elemento { Nombre = "Arnés", Peso = 1, Calorias = 8 },
                new Elemento { Nombre = "Botiquín", Peso = 2, Calorias = 3 }
            };
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


        // Con este metodo se seleccionaria la misma cantidad de elementos pero menos optimizado 
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



    }
}
