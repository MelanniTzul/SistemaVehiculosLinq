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
    public partial class FormDueno : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public FormDueno()
        {
            InitializeComponent();
            Inicializar();
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
            comboGenero.EditValue = -1;

            txteditNombre.Focus();//COLOCAR CURSOS EN EL TXT
            int indice = -1;
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
          
            var dueno = from du in conexion.Dueño
                        where du.estado == true
                        select new
                        {
                            ID = du.id_dueño,
                            PRIMER_NOMBRE = du.nombre1,
                            SEGUNDO_NOMBRE =du.nombre2,
                            PRIMER_APELLIDO=du.apellido1,
                            SEGUNDO_APELLIDO=du.apellido2,
                            APELLIDO_CASADO = du.apellidoCasad,
                            TELEFONO = du.telefono,
                            DPI = du.dpi,
                            GENERO = du.Genero.nombre,                  
                            DIRECCION =du.direccion,
                            NIT=du.nit,
                            CORREO=du.correo,
                            ID_GENERO=du.Genero.id_genero
                      
                        };
            
            //INGRESAR A LA TABLA
            gridControl1.DataSource = dueno;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
            this.gridView1.Columns["ID_GENERO"].Visible = false;


            //**COMBOS DE FORMULARIO**//
            //TRAEMOS DATOS
            var genero = from ge in conexion.Genero
                        where ge.estado == true
                        select new
                        {
                            ID = ge.id_genero,
                            NOMBRE = ge.nombre
                        };

            //LLENAR COMBO
            comboGenero.Properties.DataSource = genero;

            comboGenero.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboGenero.Properties.ValueMember = "ID";
        }

        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {

            if (txteditNombre.Text != String.Empty)
            {
                //CREAMOS OBJETO DUEÑO
                DB.Dueño dueno= new DB.Dueño
                {
                    id_genero = Convert.ToInt32(comboGenero.EditValue),
                    nombre1 = txteditNombre.Text,
                    nombre2 = txtSegundoNombre.Text,
                    apellido1 = txtPrimerApellido.Text,
                    apellido2=txtSegundoApellido.Text,
                    apellidoCasad=txtApellidoCasa.Text,
                    telefono=txtTelefono.Text,
                    dpi=txtTelefono.Text,
                    direccion=txtDireccion.Text,
                    nit=txtNit.Text,
                    correo=txtCorreo.Text,
                    estado = true
                };


                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Dueño.InsertOnSubmit(dueno);

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
            //Mostramos datos
            txteditNombre.Text = gridView1.GetFocusedRowCellValue("PRIMER_NOMBRE").ToString();//Mostramos dato
            txtSegundoNombre.Text = gridView1.GetFocusedRowCellValue("SEGUNDO_NOMBRE").ToString();//Mostramos nombre en el txt
            txtPrimerApellido.Text = gridView1.GetFocusedRowCellValue("PRIMER_APELLIDO").ToString();
            txtSegundoApellido.Text = gridView1.GetFocusedRowCellValue("SEGUNDO_APELLIDO").ToString();
            txtApellidoCasa.Text = gridView1.GetFocusedRowCellValue("APELLIDO_CASADO").ToString();
            txtTelefono.Text = gridView1.GetFocusedRowCellValue("TELEFONO").ToString();
            txtDpi.Text = gridView1.GetFocusedRowCellValue("DPI").ToString();
            comboGenero.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_GENERO").ToString());
            txtDireccion.Text = gridView1.GetFocusedRowCellValue("DIRECCION").ToString();
            txtNit.Text = gridView1.GetFocusedRowCellValue("NIT").ToString();
            txtCorreo.Text = gridView1.GetFocusedRowCellValue("CORREO").ToString();              
           

            btnGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
        }

        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from du in conexion.Dueño
                             where du.id_dueño == indice
                             select du).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombre1 = txteditNombre.Text; //Primer Nombre
            modificar.nombre2 = txtSegundoNombre.Text;//Segundo nombre
            modificar.apellido1 = txtPrimerApellido.Text;//Apellido1
            modificar.apellido2 = txtSegundoApellido.Text;//Apellido2
            modificar.apellidoCasad = txtApellidoCasa.Text;//Apellido Casada
            modificar.telefono = txtTelefono.Text;
            modificar.dpi = txtDpi.Text;
            modificar.id_genero = Convert.ToInt32(comboGenero.EditValue);//Genero
            modificar.direccion = txtDireccion.Text;
            modificar.nit = txtDireccion.Text;
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
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from du in conexion.Dueño
                             where du.id_dueño == indice
                             select du).FirstOrDefault();

            //MODIFICAMOS
            modificar.estado=false; //Primer Nombre
            


            //GUARDAMOS
            conexion.SubmitChanges();

            //FUNCION LIMPIAR
            Limpiar();
            //NOTIFICAMOS AL USUARIO
            alertControl1.Show(this, "Aviso", "Registro actualizado con exito");

            Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS

            comboGenero.EditValue = String.Empty;

        }

        //BOTON CANCELAR
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //FUNCION LIMPIAR
            Limpiar();
        }
    }
}
