using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace conexion_Linq_1
{
    public partial class Condicion : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public Condicion()
        {
            InitializeComponent();
            Inicializar();//LLAMAMOS A LA FUNCION Y MOSTRAMOS DATOS EN LA TABLA
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
            txtCondicion.Focus();//COLOCAR CURSOS EN EL TXT
            indice = -1;
            //ESTADO DE BOTONES
            simpleButtonGuardar.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
        }


        //FUNCION INICIALIZAR
        public void Inicializar()
        {
            //con sultas con  linq crear variable
            var condicion = from co in conexion.Condicion
                            where co.estado == true
                            select new
                            {
                                ID = co.id_condicion,
                                CONDICION =co.nombre

                            };


            //INGRESAR A LA TABLA el objeto condicion
            gridControl1.DataSource = condicion;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
        }

        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txtCondicion.Text!=String.Empty)
            {
                //Creamos objeto condi
                DB.Condicion condicion = new DB.Condicion
                {
                    nombre = txtCondicion.Text,
                    estado = true
                };

                //Conexion con linq
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Condicion.InsertOnSubmit(condicion);

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
                alertControl1.Show(this,  "Aviso", "Ingrese datos");
            }
        }

        //ENCONTRAMOS EL INDICE EN LA TABLA
        int indice = -1;
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            indice = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID").ToString());//Seleccionamos la fila
            txtCondicion.Text = gridView1.GetFocusedRowCellValue("CONDICION").ToString();//Mostramos dato

            simpleButtonGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
        }


        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from co in conexion.Condicion
                             where co.id_condicion == indice
                             select co).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombre = txtCondicion.Text;
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
                var modificar = (from co in conexion.Condicion
                                 where co.id_condicion == indice
                                 select co).FirstOrDefault();

                //MODIFICAMOS
                modificar.estado = false;
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
