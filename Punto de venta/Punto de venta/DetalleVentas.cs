using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_venta
{
    public partial class DetalleVentas : Form
    {

        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public DetalleVentas()
        {
            InitializeComponent();
            Inicializar();//FUNCION QUE ME MUESTRA LOS DATOS INGRESADOS
        }


        //FUNCION INICIALIZAR
        public void Inicializar()
        {
            //con sultas con  linq crear variable
            var ventas = from V in conexion.Transaccion
                            where V.estado == true
                            select new
                            {
                                RECIBO = V.id_transaccion,
                                FECHA = V.fecha,
                                TOTAL = V.total,
                                
                            };

            //INGRESAR A LA TABLA
            gridControl1.DataSource = ventas;
          



        }

    }
}
