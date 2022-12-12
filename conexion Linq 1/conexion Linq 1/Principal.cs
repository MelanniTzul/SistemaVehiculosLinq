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

namespace conexion_Linq_1
{
    public partial class Principal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Principal()
        {
            InitializeComponent();
           // lblFecha.Caption = DateTime.Now.ToShortDateString();//Fecha corta
            labelFecha.Caption = DateTime.Now.ToLongDateString();//Fecha larga
        }

        //MOSTRAR FORMU DE MARCA
        private void barButtonItemMarca_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormMarca forMarca = new FormMarca();//Creo formulario 
            forMarca.MdiParent = this;//Establecer MdiParent que sera este
            forMarca.Show();//Mostramos
        }

        //MOSTRAR FORM DE MODELO 
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Modelo formodelo = new Modelo();
            formodelo.MdiParent = this;
            formodelo.Show();
        }

        //MOSTRAR FORM DE DUEÑO
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormDueno dueno = new FormDueno();
            dueno.MdiParent = this;
            dueno.Show();
        }

        //MOSTRAR FORM DE AÑO
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            year year = new year();
            year.MdiParent = this;
            year.Show();

        }
        //MOSTRAR FORM DE CONDICION
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Condicion condi = new Condicion();
            condi.MdiParent = this;
            condi.Show();

        }
        //MOSTRAR FORM DE VEHICULO
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Vehiculo V = new Vehiculo();
            V.MdiParent = this;
            V.Show();
            
        }
    }
}