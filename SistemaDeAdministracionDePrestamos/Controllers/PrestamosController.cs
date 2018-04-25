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
using SistemaDeAdministracionDePrestamos.BusinessLogic;

namespace SistemaDeAdministracionDePrestamos.Controllers
{
    public partial class PrestamosController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Prestamos
        public ActionResult Index(int page = 1)
        {
            IPagedList<PrestamoViewModel> viewModel;
            PrestamosService prestamosServ = new PrestamosService();
            

            //introducimos los datos de los prestamos en un prestamoViewModel que
            // es un modelo que nos facilitara mostrar datos de varias tablas juntas
            // para una mejor comprension de la informacion por parte del usuario final

            //Este consulta traera todos los prestamos (realizados y pendientes)
            //var todosPrestamos = (from vm in model
            //                 orderby vm.Cliente
            //                 select new PrestamoViewModel
            //                 {
            //                     Id = vm.Id,
            //                     Cliente = (_db.Clientes.Find(vm.ClienteId)).Nombre,
            //                     Monto = vm.Monto,
            //                     Fecha = vm.Fecha,
            //                     Status = (vm.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo
            //                 }).ToList();

           

            // esta linea de codigo tomara los prestamos no pendientes y lo convertira en IPageList
            viewModel = prestamosServ.ObtenerPrestamosRealizados().ToPagedList(page, 10);

            if (viewModel == null)
            {
                return HttpNotFound();
            }
            //Si la solicitud es de tipo ajax, solo se volvera a cargar una porcion de la pagina actual
            // (la tabla con los datos de los prestamos)
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Prestamos", viewModel);
            }
            // Si la solicutid no es de tipo ajax, se cargara la vista completa
            return View(viewModel);
        }

        // GET: Prestamos/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PrestamoViewModel prestamoViewModel = new PrestamoViewModel();
            PrestamosService prestamosServ = new PrestamosService();

            prestamoViewModel = prestamosServ.ObtenerPrestamo(id);

            if (prestamoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(prestamoViewModel);
        }

        // GET: Prestamos/Create
        public ActionResult Create()
        {
            PrestamoViewModel model = new PrestamoViewModel();
            PrestamosService prestamosServ = new PrestamosService();

            #region Porpular DropDownList con nombres de clientes
            
            model.Clientes = prestamosServ.ObtenerListaClientes();

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

            PrestamosService prestamosServ = new PrestamosService();

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

                if (!prestamosServ.EstaPendiente(pModel))
                {
                    //Se busca el prestamo recien hecho para utilizar su id para crear los recibos de ese prestamo
                    prestamoViewModel.Id =
                        (_db.Prestamos
                        .Where(p => p.ClienteId == Cliente.Id && p.Monto == prestamoViewModel.Monto && p.Fecha == prestamoViewModel.Fecha).Single()
                        ).Id;

                    List<Recibo> recibos = prestamosServ.GenerarRecibos(prestamoViewModel);

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
                }
                
                return RedirectToAction("Index");
            }

            return View(prestamoViewModel);
        }


        // GET: Prestamos/Edit/5
        public ActionResult Edit(int? id)
        {

            //PrestamoViewModel prestamoViewModel = new PrestamoViewModel();
            PrestamosService prestamosServ = new PrestamosService();

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
                Clientes = prestamosServ.ObtenerListaClientes(),
                Monto = pModel.Monto,
                Fecha = pModel.Fecha,
                Status = status

            };

            //prestamoViewModel = prestamosServ.ObtenerPrestamo(id);

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
