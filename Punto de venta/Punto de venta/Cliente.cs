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
    public partial class Cliente : Form
    {

        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public Cliente()
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
            txtNombre.Focus();//COLOCAR CURSOS EN EL TXT
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
            var cliente = from c in conexion.Cliente
                            where c.estado == true
                            select new
                            {
                                ID = c.Id_cliente,
                                NOMBRES = c.nombres,
                                APELLIDOS = c.apellidos,
                                DIRECCION = c.direccion,
                                TELEFONO=c.telefono,
                                CORREO = c.correo
                            };


            //INGRESAR A LA TABLA
            gridControl1.DataSource = cliente;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
        }



        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != String.Empty && txtApellidos.Text != String.Empty && txtDireccion.Text != String.Empty && txtTelefono.Text != String.Empty && txtCorreo.Text!=String.Empty)
            {
                //Creamos objeto que es insert
                DB.Cliente cliente = new DB.Cliente
                {
                    nombres = txtNombre.Text,
                    apellidos = txtApellidos.Text,
                    direccion=txtDireccion.Text,
                    telefono = txtTelefono.Text,
                    correo = txtCorreo.Text,
                    estado = true
                };

                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Cliente.InsertOnSubmit(cliente);

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
            txtNombre.Text = gridView1.GetFocusedRowCellValue("NOMBRES").ToString();//Mostramos dato
            txtApellidos.Text = gridView1.GetFocusedRowCellValue("APELLIDOS").ToString();//Mostramos dato
            txtDireccion.Text = gridView1.GetFocusedRowCellValue("DIRECCION").ToString();//Mostramos dato
            txtTelefono.Text = gridView1.GetFocusedRowCellValue("TELEFONO").ToString();//Mostramos dato
            txtCorreo.Text = gridView1.GetFocusedRowCellValue("CORREO").ToString();//Mostramos dato

            simpleButtonGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;

        }

        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from c in conexion.Cliente
                             where c.Id_cliente == indice
                             select c).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombres = txtNombre.Text;
            modificar.apellidos = txtApellidos.Text;
            modificar.direccion = txtDireccion.Text;
            modificar.telefono = txtTelefono.Text;
            modificar.correo = txtCorreo.Text;

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
                var eliminar = (from c in conexion.Cliente
                                where c.Id_cliente == indice
                                select c).FirstOrDefault();

                //MODIFICAMOS
                eliminar.estado = false;
                //GUARDAMOS
                conexion.SubmitChanges();

                //FUNCION LIMPIAR
                Limpiar();
                //NOTIFICAMOS AL USUARIO
                alertControl1.Show(this, "Aviso", "Registro eliminado exitosamente");

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
