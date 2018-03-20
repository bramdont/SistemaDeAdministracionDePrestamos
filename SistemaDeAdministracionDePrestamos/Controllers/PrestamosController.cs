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

namespace SistemaDeAdministracionDePrestamos.Controllers
{
    public class PrestamosController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Prestamos
        public ActionResult Index()
        {
            var model = _db.Prestamos.ToList();


            var ViewModel = (from vm in model
                             //where vm.Estatus == true
                             select new PrestamoViewModel
                             {
                                 Id = vm.Id,
                                 Cliente = (_db.Clientes.Find(vm.ClienteId)).Nombre,
                                 Monto = vm.Monto,
                                 Fecha = vm.Fecha,
                                 Status = (vm.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo
                             }).ToList();

            return View(ViewModel.OrderBy(c => c.Cliente));
        }

        // GET: Prestamos/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Prestamos.Find(id);
            var estatus = (model.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo;

            PrestamoViewModel prestamoViewModel = new PrestamoViewModel
            {
                Id = model.Id,
                Cliente = (_db.Clientes.Find(model.ClienteId)).Nombre,
                Monto = model.Monto,
                Fecha = model.Fecha,
                Status = estatus
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

            var pModel = new Prestamo
            {
                ClienteId = Cliente.Id,
                Monto = prestamoViewModel.Monto,
                Fecha = prestamoViewModel.Fecha,
                Estatus = (prestamoViewModel.Status == PrestamoViewModel.estatus.Activo) ? true : false
            };

            if (ModelState.IsValid)
            {
                _db.Prestamos.Add(pModel);
                _db.SaveChanges();
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
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //PrestamoViewModel prestamoViewModel = _db.PrestamoViewModels.Find(id);
            //if (prestamoViewModel == null)
            //{
            //    return HttpNotFound();
            //}
            return View(/*prestamoViewModel*/);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //PrestamoViewModel prestamoViewModel = _db.PrestamoViewModels.Find(id);
            //_db.PrestamoViewModels.Remove(prestamoViewModel);
            //_db.SaveChanges();
            return RedirectToAction("Index");
        }
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

        //private IEnumerable<SelectListItem> obtenerListaEstatus (List<PrestamoViewModel.estatus> statu)
        //{
        //    List<SelectListItem> SelectListItem = new List<SelectListItem>();

        //    foreach (var item in statu)
        //    {
        //        SelectListItem.Add(new SelectListItem
        //        {
        //            Value = item.ToString(),
        //            Text = item.ToString()
        //        });
        //    }
        //    return SelectListItem;
        //}
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
