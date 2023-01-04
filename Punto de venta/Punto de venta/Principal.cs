using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Punto_de_venta
{
    public partial class Principal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Principal()
        {
            InitializeComponent();
        }

        //BOTON DE CLIENTE
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cliente formCliente = new Cliente();
            formCliente.MdiParent = this;
            formCliente.Show();
        }

        //BOTON DE CAJERO
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cajero formCajero = new Cajero();
            formCajero.MdiParent = this;
            formCajero.Show();      
        }

        //BOTON PRODUCTOS
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Productos formProductos = new Productos();
            formProductos.MdiParent = this;
            formProductos.Show();
        }

        //BOTON VENTAS
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Ventas formVentas = new Ventas();
            formVentas.MdiParent = this;
            formVentas.Show();
        }

        //BOTON DETALLE VENTAS
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            DetalleVentas formDetalleVentas = new DetalleVentas();
            formDetalleVentas.MdiParent = this;
            formDetalleVentas.Show();

        }


        //BOTON COMPRAS
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            Compras formCom = new Compras();
            formCom.MdiParent = this;
            formCom.Show();
        }

        //BOTON PROVEEDOR
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            Proveedor formPro = new Proveedor();
            formPro.MdiParent = this;
            formPro.Show();

        }

        //BOTON CAJERO
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cajero formCajero = new Cajero();
            formCajero.MdiParent = this;
            formCajero.Show();
        }
    }
}