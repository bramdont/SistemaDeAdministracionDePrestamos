using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaDeAdministracionDePrestamos.Models;
using SistemaDeAdministracionDePrestamos.Models.ViewModels;
using PagedList;

namespace SistemaDeAdministracionDePrestamos.Controllers
{
    public class PrestamosController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Prestamos
        public ActionResult Index(int page = 1)
        {
            var model = _db.Prestamos.ToList();

            if (model == null)
            {
                return HttpNotFound();
            }

            //introducimos los datos de los prestamos en un prestamoViewModel que
            // es un modelo que nos facilitara mostrar datos de varias tablas juntas
            // para una mejor comprension de la informacion por parte del usuario final
            var ViewModel = (from vm in model
                             orderby vm.Cliente
                             select new PrestamoViewModel
                             {
                                 Id = vm.Id,
                                 Cliente = (_db.Clientes.Find(vm.ClienteId)).Nombre,
                                 Monto = vm.Monto,
                                 Fecha = vm.Fecha,
                                 Status = (vm.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo
                             }).ToList().ToPagedList(page, 10);

            //Si la solicitud es de tipo ajax, solo se volvera a cargar una porcion de la pagina actual
            // (la tabla con los datos de los prestamos)
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Prestamos", ViewModel);
            }
            // Si la solicutid no es de tipo ajax, se cargara la vista completa
            return View(ViewModel);
        }

        // GET: Prestamos/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pModel = _db.Prestamos.Find(id);
            //La expresion luego de la variable estatus es un if de forma conta.
            // si la condicion dada se cumple, la variable tendra el valor de lo que aparece luego del signo
            //de interrogacion, si la condicion no se cumple, el valor sera el que aparece luego de los dos puntos.
            var estatus = (pModel.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo;
           
            //Se buscan todos los recibos pertenecientes a un prestamo
            // para mostrarlos en los detalles del prestamo.
            var recibos = _db.Recibos
                .Where(r => r.PrestamoId == id)
                .ToList();

            PrestamoViewModel prestamoViewModel = new PrestamoViewModel
            {
                Id = pModel.Id,
                Cliente = (_db.Clientes.Find(pModel.ClienteId)).Nombre,
                Monto = pModel.Monto,
                Fecha = pModel.Fecha,
                Status = estatus,
                Recibos = recibos
            };

            if (prestamoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(prestamoViewModel);
        }

        // GET: Prestamos/Create
        public ActionResult Create()
        {
            #region Porpular DropDownList con nombres de clientes
            List<string> NombreClientes = _db.Clientes.Select(c => c.Nombre).ToList();

            PrestamoViewModel model = new PrestamoViewModel();

            model.Clientes = obtenerListaClientes(NombreClientes);
            #endregion

            return View(model);
        }


        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cliente,Monto,Fecha,Status")] PrestamoViewModel prestamoViewModel)
        {

            var Cliente = _db.Clientes.Where(c => c.Nombre == prestamoViewModel.Cliente).First();

            var status = (prestamoViewModel.Status == PrestamoViewModel.estatus.Activo) ? true : false;

            var pModel = new Prestamo
            {
                ClienteId = Cliente.Id,
                Monto = prestamoViewModel.Monto,
                Fecha = prestamoViewModel.Fecha,
                Estatus = status
            };

            if (ModelState.IsValid)
            {
                //Se guarda el prestamo para que genere un id de prestamo
                _db.Prestamos.Add(pModel);
                _db.SaveChanges();

                //Se busca el prestamo recien hecho para utilizar su id para crear los recibos de ese prestamo
                prestamoViewModel.Id =
                    (_db.Prestamos
                    .Where(p => p.ClienteId == Cliente.Id && p.Monto == prestamoViewModel.Monto && p.Fecha == prestamoViewModel.Fecha).Single()
                    ).Id;

                List<Recibo> recibos = GenerarRecibos(prestamoViewModel);

                // Si no se pueden generar los recibos (devuleve null), 
                //se elimina el prestamo de la db para poder intentarlo de nuevo
                if (recibos == null)
                {
                    _db.Prestamos.Remove(pModel);
                    _db.SaveChanges();
                    return HttpNotFound();
                }
                //Si el resulgado de GenerarRecibos no es null, se guardan los recibos en la db
                foreach (var recibo in recibos)
                {
                    _db.Recibos.Add(recibo);
                    _db.SaveChanges();
                }


                return RedirectToAction("Index");
            }

            return View(prestamoViewModel);
        }


        // GET: Prestamos/Edit/5
        public ActionResult Edit(int? id)
        {
            #region Popular dropDownList con nombres de clientes
            List<string> NombreClientes = _db.Clientes.Select(c => c.Nombre).ToList();
            #endregion

            Prestamo pModel = _db.Prestamos.Find(id);
            Cliente cliente = _db.Clientes.Find(pModel.ClienteId);
            var status = (pModel.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            PrestamoViewModel prestamoViewModel = new PrestamoViewModel
            {
                Id = pModel.Id,
                Cliente = cliente.Nombre,
                Clientes = obtenerListaClientes(NombreClientes),
                Monto = pModel.Monto,
                Fecha = pModel.Fecha,
                Status = status

            };
            if (prestamoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(prestamoViewModel);
        }

        // POST: Prestamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cliente,Monto,Fecha,Status")] PrestamoViewModel prestamoViewModel)
        {
            Prestamo pModel = new Prestamo();

            if (ModelState.IsValid)
            {
                var cliente = _db.Clientes.Where(c => c.Nombre == prestamoViewModel.Cliente).First();
                var status = (prestamoViewModel.Status == PrestamoViewModel.estatus.Activo) ? true : false;

                pModel = new Prestamo
                {
                    Id = prestamoViewModel.Id,
                    ClienteId = cliente.Id,
                    Monto = prestamoViewModel.Monto,
                    Fecha = prestamoViewModel.Fecha,
                    Estatus = status
                };

                _db.Entry(pModel).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prestamoViewModel);

        }

        // GET: Prestamos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Prestamo pModel = _db.Prestamos.Find(id);

            if (pModel == null)
            {
                return HttpNotFound();
            }
            else
            {
                var estatus = (pModel.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo;

                PrestamoViewModel prestamoViewModel = new PrestamoViewModel
                {
                    Id = pModel.Id,
                    Cliente = (_db.Clientes.Find(pModel.ClienteId)).Nombre,
                    Monto = pModel.Monto,
                    Fecha = pModel.Fecha,
                    Status = estatus
                };

                return View(prestamoViewModel);
            }
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamo pModel = _db.Prestamos.Find(id);
            _db.Prestamos.Remove(pModel);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Este metodo genera una lista con los nombres de los clientes y los prepara
        // para que puedan utilizarse en el dropDownList (que sean del tipo SelectListItem)
        private IEnumerable<SelectListItem> obtenerListaClientes(List<string> nombreClientes)
        {
            List<SelectListItem> SelectListItem = new List<SelectListItem>();

            foreach (var item in nombreClientes)
            {
                SelectListItem.Add(new SelectListItem
                {
                    Value = item,
                    Text = item
                });
            }

            return SelectListItem;
        }

        //Este metodo será invocado por el controlador de prestamos,
        // al hacerlo, generará los recibos correspondientes a dicho prestamo.
        private List<Recibo> GenerarRecibos(PrestamoViewModel pModel)
        {

            if (pModel == null)
            {
                return null;
            }

            else
            {
                List<Recibo> receipts = new List<Recibo>();

                int monto = (pModel.Monto / 10);

                DateTime fecha = pModel.Fecha;

                int idPrestamo = pModel.Id;

                bool status = (pModel.Status == PrestamoViewModel.estatus.Activo) ? true : false;

                for (int i = 1; i <= 13; i++)
                {
                    fecha = fecha.AddDays(7); 

                    receipts.Add(new Recibo
                    {
                        Cuota = i,
                        MontoPago = monto,
                        FechaPago = fecha,
                        Estatus = status,
                        PrestamoId = idPrestamo

                    });
                }

                return receipts;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
