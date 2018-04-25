using SistemaDeAdministracionDePrestamos.Models;
using SistemaDeAdministracionDePrestamos.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeAdministracionDePrestamos.BusinessLogic
{
    public class PrestamosService
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public List<PrestamoViewModel> ObtenerPrestamosRealizados()
        {
            List<PrestamoViewModel> prestRealizados = new List<PrestamoViewModel>();
            var model = _db.Prestamos.OrderBy(p => p.Fecha).ToList();

            if (model == null)
            {
                return null;
            }

            // Este bucle solo tomara los prestamos que no estan pendientes
            foreach (var prestamo in model)
            {
                if (!EstaPendiente(prestamo))
                {
                    prestRealizados.Add(new PrestamoViewModel
                    {
                        Id = prestamo.Id,
                        Cliente = (_db.Clientes.Find(prestamo.ClienteId)).Nombre,
                        Monto = prestamo.Monto,
                        Fecha = prestamo.Fecha,
                        Status = (prestamo.Estatus == true) ? PrestamoViewModel.estatus.Activo : PrestamoViewModel.estatus.Inactivo
                    });
                }
            }

            return prestRealizados;
        }

        public PrestamoViewModel ObtenerPrestamo(int? id)
        {
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

            PrestamoViewModel prestamoViewModel = new PrestamoViewModel()
            {
                Id = pModel.Id,
                Cliente = (_db.Clientes.Find(pModel.ClienteId)).Nombre,
                Monto = pModel.Monto,
                Fecha = pModel.Fecha,
                Status = estatus,
                Recibos = recibos
            };

            return prestamoViewModel;
        }

        //Este metodo genera una lista con los nombres de los clientes y los prepara
        // para que puedan utilizarse en el dropDownList (que sean del tipo SelectListItem)
        public IEnumerable<SelectListItem> ObtenerListaClientes()
        {
            List<string> nombreClientes = _db.Clientes.Select(c => c.Nombre).ToList();

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
        public List<Recibo> GenerarRecibos(PrestamoViewModel pModel)
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
        //Este metodo averiguara si un prestamo esta pendiente o no
        public bool EstaPendiente(Prestamo prestam)
        {

            DateTime fechaActual = DateTime.Now;
            DateTime fechaPrestam = prestam.Fecha;

            int result = DateTime.Compare(fechaPrestam, fechaActual);

            // El metodo DateTime.Compare() devuelve un numero negativo 
            // si la primera fecha es menor a la segunda
            if (prestam.Estatus == false || result >= 0)
            {
                return true;
            }
            return false;

        }
    }
}