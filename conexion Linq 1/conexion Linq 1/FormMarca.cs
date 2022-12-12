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
    public partial class FormMarca : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public FormMarca()
        {
            InitializeComponent();
            Inicializar();//FUNCION QUE ME MUESTRA LOS DATOS INGRESADOS
            
        }
        //FUNCION LIMPIAR
        public void Limpiar()
        {
            foreach(Control crl in this.Controls){
                if(crl is DevExpress.XtraEditors.TextEdit)
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
            var marca = from ma in conexion.Marca
                        where ma.estado == true
                        select new
                        {
                           ID=ma.id_marca,
                           MARCA = ma.nombre

                        };


            //INGRESAR A LA TABLA
            gridControl1.DataSource = marca;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
        }

        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txteditNombre.Text != String.Empty)
            {
                //Creamos objeto que es insert
                DB.Marca marca = new DB.Marca
                {
                    nombre = txteditNombre.Text,
                    estado = true
                };

                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Marca.InsertOnSubmit(marca);

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
            indice = Convert.ToInt32( gridView1.GetFocusedRowCellValue("ID").ToString());//Seleccionamos la fila
            txteditNombre.Text = gridView1.GetFocusedRowCellValue("MARCA").ToString();//Mostramos dato
            
            simpleButtonGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;

        }

        //BOTON MODIFICAR
        private void simpleButtonModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar =( from ma in conexion.Marca
                            where ma.id_marca == indice
                            select ma).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombre = txteditNombre.Text;
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
                var modificar = (from ma in conexion.Marca
                                 where ma.id_marca == indice
                                 select ma).FirstOrDefault();

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
        private void simpleButtonCancelar_Click(object sender, EventArgs e)
        {
            //FUNCION LIMPIAR
            Limpiar();
        }
    }
}
