using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AcevedoBcoProv.Models;

namespace AcevedoBcoProv.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Index(string searchString)
        {
            using (DbModels context = new DbModels())
            {
                var empleados = from empleado in context.Empleados.Include("TiposDocumento")
                                select empleado;

                if (!string.IsNullOrEmpty(searchString))
                {
                    empleados = empleados.Where(e => e.Apellido.Contains(searchString) ||
                                                     e.Nombre.Contains(searchString));
                                                     //e.NumDocumento == int.Parse(searchString));
                }

                return View(empleados.ToList());
            }
        }




        // GET: Empleado/Details/5
        public ActionResult Details(int id)
        {
            using (DbModels context = new DbModels())
            {
                return View(context.Empleados.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            using (DbModels context = new DbModels())
            {
                // Obtener los tipos de documento desde la base de datos
                var tiposDocumento = context.TiposDocumentoes.ToList();

                // Crear una lista de SelectListItem para la lista desplegable
                var tipoDocumentoItems = tiposDocumento.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Descripcion
                }).ToList();

                // Enviar los tipos de documento a la vista utilizando ViewBag
                ViewBag.TipoDocumentoItems = tipoDocumentoItems;

                return View();
            }
        }

        [HttpPost]
        public JsonResult IsDocumentoDisponible(string NumeroDocumento)
        {
            int numeroDocumentoInt;
            if (int.TryParse(NumeroDocumento, out numeroDocumentoInt))
            {
                using (DbModels context = new DbModels())
                {
                    // Verificar si el número de documento ya existe en la base de datos
                    bool documentoExiste = context.Empleados.Any(e => e.NumDocumento == numeroDocumentoInt);

                    // Retornar el resultado de la validación como un objeto JSON
                    return Json(!documentoExiste);
                }
            }
            // Si el valor no es un número válido, retornar error.
            return Json(false);
        }

        // POST: Empleado/Create
        [HttpPost]
        public ActionResult Create(Empleado empleado)
        {
            try
            {
                // Consultar si el DNI ya está registrado
              

                // Si el DNI no está registrado, agregar el empleado Se me hizo mas practico resolverlo
                // de esta manera la validacion del dni
                using (DbModels context = new DbModels())
                {
                    var existingEmpleado = context.Empleados.FirstOrDefault(e => e.NumDocumento == empleado.NumDocumento);
                    if (existingEmpleado != null)
                    {
                        TempData["ErrorMessage"] = "El DNI ya está registrado en la base de datos.";
                        TempData["ShowConfirmation"] = true; 
                        TempData["RedirectAction"] = "Index"; 
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        context.Empleados.Add(empleado);
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
      

        // GET: Empleado/Edit/5
        public ActionResult Edit(int id)
        {
            using (DbModels context = new DbModels())
            {
                // Obtener los tipos de documento desde la base de datos
                var tiposDocumento = context.TiposDocumentoes.ToList();

                // Crear una lista de SelectListItem para la lista desplegable
                var tipoDocumentoItems = tiposDocumento.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Descripcion
                }).ToList();

                // Enviar los tipos de documento a la vista utilizando ViewBag
                ViewBag.TipoDocumentoItems = tipoDocumentoItems;

                return View(context.Empleados.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Empleado empleado)
        {
            try
            {
                using (DbModels context = new DbModels())
                {
                    context.Entry(empleado).State = EntityState.Modified;
                    context.SaveChanges();
                }

                return RedirectToAction("Index"); // Redirecciona a la vista "Index" después de guardar los cambios correctamente.
            }
            catch
            {
                return View();
            }
        }


        // GET: Empleado/Delete/5
        public ActionResult Delete(int id)
        {
            using (DbModels context = new DbModels())
            {
                return View(context.Empleados.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Empleado/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using(DbModels context = new DbModels())
                {
                    Empleado empleado =  context.Empleados.Where(x => x.Id == id).FirstOrDefault();
                    context.Empleados.Remove(empleado);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}
