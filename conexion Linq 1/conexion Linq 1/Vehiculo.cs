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
    public partial class Vehiculo : Form
    {

        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public Vehiculo()
        {
            InitializeComponent();
            Inicializar();//LLAMAMOS A LA FUNCION Y MOSTRAMOS DATOS
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
            //LIMPIAMOS COMBOS
            comboMarca.EditValue = -1;
            comboModelo.EditValue = -1;
            comboYear.EditValue = -1;
            ComboCondicion.EditValue = -1;
            comboDueno.EditValue = -1;

            txtPlaca.Focus();//COLOCAR CURSOS EN EL TXT
            String indice = string.Empty;
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

            var vehiculo = from v in conexion.Vehiculo
                        where v.estado == true
                        select new
                        {
                            PLACA = v.id_vehiculo,
                            MODELO=v.Modelo.nombre,
                            ID_MODELO=v.id_modelo, //PODER SELECCIONAR EL DATO Y MOSTRARLO 
                            MARCA=v.Modelo.Marca.nombre,
                            ID_MARCA=v.Modelo.id_marca,
                            AÑO=v.Año.descripcion,
                            ID_AÑO=v.id_año,
                            CONDICION=v.Condicion.nombre,
                            ID_CONDICION=v.id_condicion,
                            DUEÑO=v.Dueño.nombre1+' '+ v.Dueño.nombre2 + ' '+ v.Dueño.apellido1 + ' ' + v.Dueño.apellido2 + ' ' + v.Dueño.apellidoCasad,
                            ID_DUEÑO=v.id_dueño
                        };

            //INGRESAR A LA TABLA
            gridControl1.DataSource = vehiculo;
            this.gridView1.Columns["ID_MODELO"].Visible = false;//Ocultamos la columna del ID
            this.gridView1.Columns["ID_MARCA"].Visible = false;//Ocultamos la columna del MARCA
            this.gridView1.Columns["ID_AÑO"].Visible = false;
            this.gridView1.Columns["ID_CONDICION"].Visible = false;
            this.gridView1.Columns["ID_DUEÑO"].Visible = false;






            
            //TRAEMOS DATOS MARCA
            var marca = from m in conexion.Marca
                         where m.estado == true
                         select new
                         {
                             ID = m.id_marca,
                             NOMBRE = m.nombre
                         };

            //TRAEMOS DATOS AÑO
            var año = from a in conexion.Año
                        where a.estado == true
                        select new
                        {
                            ID = a.id_año,
                            NOMBRE = a.descripcion
                        };

            //TRAEMOS DATOS CONDICION
            var condicion = from c in conexion.Condicion
                      where c.estado == true
                      select new
                      {
                          ID = c.id_condicion,
                          NOMBRE = c.nombre
                      };

            //TRAEMOS DATOS Dueño
            var dueno = from du in conexion.Dueño
                            where du.estado == true
                            select new
                            {
                                ID = du.id_dueño,
                                NOMBRE = du.nombre1 + ' ' + du.nombre2 + ' ' + du.apellido1 + ' ' + du.apellido2 + ' ' + du.apellidoCasad
                            };



           

            //LLENAR COMBO MARCA
            comboMarca.Properties.DataSource = marca;
            comboMarca.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboMarca.Properties.ValueMember = "ID";

            //LLENAR COMBO AÑO
            comboYear.Properties.DataSource = año;
            comboYear.Properties.DisplayMember = "NOMBRE";
            comboYear.Properties.ValueMember = "ID";

            //LLENAR COMBO CONDICION
            ComboCondicion.Properties.DataSource = condicion;
            ComboCondicion.Properties.DisplayMember = "NOMBRE";
            ComboCondicion.Properties.ValueMember = "ID";


            //LLENAR COMBO DUEÑO
            comboDueno.Properties.DataSource = dueno;
            comboDueno.Properties.DisplayMember = "NOMBRE";
            comboDueno.Properties.ValueMember = "ID";
        }



        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txtPlaca.Text != String.Empty)
            {
                //CREAMOS OBJETO VEHICULO
                DB.Vehiculo vehiculo = new DB.Vehiculo
                {
                    id_vehiculo = txtPlaca.Text,
                    id_modelo = Convert.ToInt32(comboModelo.EditValue),
                    id_año=Convert.ToInt32(comboYear.EditValue),
                    id_condicion=Convert.ToInt32(ComboCondicion.EditValue),
                    id_dueño=Convert.ToInt32(comboDueno.EditValue),
                    estado = true
                };


                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Vehiculo.InsertOnSubmit(vehiculo);

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
        String indice=String.Empty;
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            //  indice = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID").ToString());//Seleccionamos la fila

            indice = gridView1.GetFocusedRowCellValue("PLACA").ToString();
            txtPlaca.Enabled = false;//BLOQUEAMOS PLACA


            //Mostramos datos
            txtPlaca.Text = gridView1.GetFocusedRowCellValue("PLACA").ToString();//Mostramos dato          
            comboModelo.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_MODELO").ToString());
            comboMarca.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_MARCA").ToString());
            comboYear.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_AÑO").ToString());
            ComboCondicion.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_CONDICION").ToString());
            comboDueno.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_DUEÑO").ToString());
           


            btnGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            


        }

        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from v in conexion.Vehiculo
                             where v.id_vehiculo == indice
                             select v).FirstOrDefault();

            //MODIFICAMOS
            modificar.id_modelo = Convert.ToInt32(comboModelo.EditValue); //MODELO
            modificar.Modelo.id_marca = Convert.ToInt32(comboMarca.EditValue);//Genero
            modificar.id_año = Convert.ToInt32(comboYear.EditValue);
            modificar.id_condicion = Convert.ToInt32(ComboCondicion.EditValue);
            modificar.id_dueño = Convert.ToInt32(comboDueno.EditValue);

            //GUARDAMOS
            conexion.SubmitChanges();

            //FUNCION LIMPIAR
            Limpiar();
            //NOTIFICAMOS AL USUARIO
            alertControl1.Show(this, "Aviso", "Registro actualizado con exito");

            Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS
            txtPlaca.Enabled = true;//HABILITAR CAMPO DE PLACA
        }

        //BOTON ELIMINAR
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from v in conexion.Vehiculo
                             where v.id_vehiculo == indice
                             select v).FirstOrDefault();

            //MODIFICAMOS
            modificar.estado = false;

            //GUARDAMOS
            conexion.SubmitChanges();

            //FUNCION LIMPIAR
            Limpiar();
            //NOTIFICAMOS AL USUARIO
            alertControl1.Show(this, "Aviso", "Registro actualizado con exito");

            Inicializar();//ACTUALIZAMOS DATOS Y MOSTRAMOS
            txtPlaca.Enabled = true;//HABILITAR CAMPO DE PLACA
        }

        //BOTON CANCELAR
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();//Funcion limpiar
            txtPlaca.Enabled = true;//HABILITAR CAMPO DE PLACA
        }


        //Filtrar por campos
        private void comboMarca_EditValueChanged(object sender, EventArgs e)
        {
            
            int IdMarca = Convert.ToInt32(comboMarca.EditValue);//Almacenamos el id que tiene el combo en marca
            //**COMBOS DE FORMULARIO**//
            //TRAEMOS DATOS MODELO
            var modelo = from mo in conexion.Modelo
                         where mo.estado == true &&
                         mo.id_marca==IdMarca
                        
                         select new
                         {
                             ID = mo.id_modelo,
                             NOMBRE = mo.nombre
                         };

            //LLENAR COMBO MODELO
            comboModelo.Properties.DataSource = modelo;
            comboModelo.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboModelo.Properties.ValueMember = "ID";
        }
    }
}
