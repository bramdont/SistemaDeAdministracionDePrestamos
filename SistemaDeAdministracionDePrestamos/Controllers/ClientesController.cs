using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaDeAdministracionDePrestamos.Models;
using PagedList;

namespace SistemaDeAdministracionDePrestamos.Controllers
{
    public class ClientesController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Clientes
        public ActionResult Index(int page = 1)
        {
            //El metodo ToPagedList es posible usarlo luego de descargar la referencia PagedList.Mvc en la nugetPackager
            var model = _db.Clientes.OrderBy(c => c.Nombre).ToList().ToPagedList(page, 10);

            //Si la solicitud es del tipo ajax, solo se actualizara el contenido parcial del a vida
            // en el partialView es donde esta la tabla donde se ponen los datos de los clientes
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Clientes", model);
            }
            //Si la solicitud no es del tipo ajax, se devolvera la vista completa
            if (model != null)
            {
                return View(model);
            }

            return HttpNotFound();
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = _db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Cedula,Direccion,Telefono")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _db.Clientes.Add(cliente);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = _db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Cedula,Direccion,Telefono")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(cliente).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = _db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = _db.Clientes.Find(id);
            _db.Clientes.Remove(cliente);
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
