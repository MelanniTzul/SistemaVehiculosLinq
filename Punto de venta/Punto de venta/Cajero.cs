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
    public partial class Cajero : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public Cajero()
        {
            InitializeComponent();
            Inicializar();//FUNCION QUE ME MUESTRA LOS DATOS INGRESADOS
        }

        //FUNCION LIMPIAR
        public void Limpiar()
        {
            foreach (Control crl in this.Controls)
            {
                if (crl is DevExpress.XtraEditors.TextEdit)
                {
                    crl.Text = string.Empty;
                }
            }

            txteditNombre.Focus();//COLOCAR CURSOS EN EL TXT
            int indice = -1;
            //ESTADO DE BOTONES
            simpleButtonGuardar.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        //FUNCION INICIALIZAR
        public void Inicializar()
        {
            //con sultas con  linq crear variable
            var cajero = from c in conexion.Cajero
                         where c.estado == true
                         select new
                         {
                             ID = c.id_cajero,
                             NOMBRE = c.nombre,
                             DIRECCION = c.direccion,
                             TELEFONO = c.telefono,
                             CORREO = c.correo
                         };


            //INGRESAR A LA TABLA
            gridControl1.DataSource = cajero;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
        }

        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txteditNombre.Text != String.Empty && txtdireccion.Text != String.Empty && txttelefono.Text != String.Empty && txtcorreo.Text != String.Empty)
            {
                //Creamos objeto que es insert
                DB.Cajero cajero = new DB.Cajero
                {
                    nombre = txteditNombre.Text,
                    direccion = txtdireccion.Text,
                    telefono = txttelefono.Text,
                    correo = txtcorreo.Text,
                    estado = true
                };

                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Cajero.InsertOnSubmit(cajero);

                //GUARDAR OBJETO O GUARDAR CAMBIOS
                conexion.SubmitChanges();

                //FUNCION LIMPIAR
                Limpiar();
                //NOTIFICAMOS AL USUARIO
                alertControl1.Show(this, "Aviso", "Sea creado registro con exito");

                Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS

            }
            else
            {
                alertControl1.Show(this, "Aviso", "Ingrese datos");
            }
        }


        //ENCONTRAMOS EL INDICE EN LA TABLA
        int indice = -1;
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            indice = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID").ToString());//Seleccionamos la fila
            txteditNombre.Text = gridView1.GetFocusedRowCellValue("NOMBRE").ToString();//Mostramos dato
            txtdireccion.Text = gridView1.GetFocusedRowCellValue("DIRECCION").ToString();//Mostramos dato
            txttelefono.Text = gridView1.GetFocusedRowCellValue("TELEFONO").ToString();//Mostramos dato
            txtcorreo.Text = gridView1.GetFocusedRowCellValue("CORREO").ToString();//Mostramos dato

            simpleButtonGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;

        }

        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from c in conexion.Cajero
                             where c.id_cajero == indice
                             select c).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombre = txteditNombre.Text;
            modificar.direccion = txtdireccion.Text;
            modificar.telefono = txttelefono.Text;
            modificar.correo = txtcorreo.Text;

            //GUARDAMOS
            conexion.SubmitChanges();

            //FUNCION LIMPIAR
            Limpiar();
            //NOTIFICAMOS AL USUARIO
            alertControl1.Show(this, "Aviso", "Registro actualizado con exito");

            Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS
        }

        //BOTON ELIMINAR
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro que desea eliminar el registro?", "Atento", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
                //con sultas con  linq crear variable
                var eliminar = (from c in conexion.Cajero
                                 where c.id_cajero == indice
                                 select c).FirstOrDefault();

                //MODIFICAMOS
                eliminar.estado = false;

                //GUARDAMOS
                conexion.SubmitChanges();

                //FUNCION LIMPIAR
                Limpiar();
                //NOTIFICAMOS AL USUARIO
                alertControl1.Show(this, "Aviso", "Registro actualizado con exito");

                Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS
            }

        }

        //BOTON CANCELAR
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //FUNCION LIMPIAR
            Limpiar();
        }
    }
}
