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
    public partial class Modelo : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        
        public Modelo()
        {
            InitializeComponent();
            Inicializar();//CARGAMOS DATOS
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

            //LIMPIAR COMBO
            comboMarca.EditValue = -1;

            txtModelo.Focus();//COLOCAR CURSOS EN EL TXT
             indice = -1;

            //ESTADO DE BOTONES
            btnGuardar.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        //FUNCION INICIALIZAR
        public void Inicializar()
        {
            //**GRID PRINCIPAL**//
            //con sultas con  linq crear variable
            var modelo = from mo in conexion.Modelo
                        where mo.estado == true
                        select new
                        {
                            ID = mo.id_modelo,
                            MARCA = mo.Marca.nombre,
                            MODELO = mo.nombre,
                            ID_MARCA = mo.Marca.id_marca
                        };

            //INGRESAR A LA TABLA
            gridControl1.DataSource = modelo;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
            this.gridView1.Columns["ID_MARCA"].Visible = false;

            //**COMBOS DE FORMULARIO**//
            //TRAEMOS DATOS
            var marca = from ma in conexion.Marca
                        where ma.estado == true
                        select new
                        {
                            ID = ma.id_marca,
                            NOMBRE= ma.nombre
                        };

            //LLENAR COMBO
            comboMarca.Properties.DataSource = marca;

            comboMarca.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboMarca.Properties.ValueMember = "ID";
            

        }

        //BOTON GUARDAR
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtModelo.Text != String.Empty)
            {
                //CREAMOS OBJETO MODELO
                DB.Modelo modelo = new DB.Modelo
                {
                    id_marca = Convert.ToInt32(comboMarca.EditValue),
                    nombre = txtModelo.Text,
                    estado = true
                };


                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Modelo.InsertOnSubmit(modelo);

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

      

        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from mo in conexion.Modelo
                             where mo.id_modelo == indice
                             select mo).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombre = txtModelo.Text; //Modelo
            modificar.id_marca = Convert.ToInt32(comboMarca.EditValue);//Marca
           
            //GUARDAMOS
            conexion.SubmitChanges();

            //FUNCION LIMPIAR
            Limpiar();
            //NOTIFICAMOS AL USUARIO
            alertControl1.Show(this, "Aviso", "Registro actualizado con exito");

            Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS
        }

        //ENCONTRAMOS EL INDICE EN LA TABLA
        int indice = -1;
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            indice = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID").ToString());//Seleccionamos la fila
          //  txtModelo.Text = gridView1.GetFocusedRowCellValue("MODELO").ToString();//Mostramos dato
            comboMarca.EditValue = Convert.ToInt32( gridView1.GetFocusedRowCellValue("ID_MARCA").ToString());

            btnGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;

        }

        //BOTON ELIMINAR
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro que desea eliminar esta marca?", "Atengo", MessageBoxButtons.OKCancel)==DialogResult.OK) {

                //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
                //con sultas con  linq crear variable
                var modificar = (from mo in conexion.Modelo
                                 where mo.id_modelo == indice
                                 select mo).FirstOrDefault();

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
