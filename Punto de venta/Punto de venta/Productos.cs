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
    public partial class Productos : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public Productos()
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
            txtNombreProducto.Focus();//COLOCAR CURSOS EN EL TXT
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
            var productos = from prod in conexion.Productos
                            where prod.estado == true
                            select new
                            {
                                ID = prod.id_producto,
                                NOMBRE_DE_PRODUCTO = prod.nombre,
                                DESCRIPCION = prod.descripcion,
                                PRECIO_COSTO= prod.precio_costo,
                                CANTIDAD=prod.cantidad,
                                PRECIO_VENTA=prod.precio_venta,
                                ID_PROVEEDOR=prod.Proveedor.Id_proveedor,//primero tomamos el id
                                PROVEEDOR=prod.Proveedor.nombre_compania//ya podemos acceder a los datos de proveedor
                                 
                            };


            //INGRESAR A LA TABLA
            gridControl1.DataSource = productos;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
            this.gridView1.Columns["ID_PROVEEDOR"].Visible = false;


            //**COMBOS DE FORMULARIO**//
            //TRAEMOS DATOS
            var proveedor = from p in conexion.Proveedor
                        where p.estado == true
                        select new
                        {
                            ID = p.Id_proveedor,
                            NOMBRE = p.nombre_compania
                        };

            //LLENAR COMBO
            comboProveedor.Properties.DataSource = proveedor;
            comboProveedor.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboProveedor.Properties.ValueMember = "ID";

        }

        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombreProducto.Text != String.Empty && txtDescripcion.Text != String.Empty && txtPrecioCosto.Text != String.Empty && txtCantidad.Text != String.Empty && txtPrecioVenta.Text!=String.Empty)
            {
                //Creamos objeto que es insert
                DB.Productos productos = new DB.Productos
                {
                   
                    nombre = txtNombreProducto.Text,
                    descripcion = txtDescripcion.Text,
                    precio_costo = Convert.ToDecimal(txtPrecioCosto.Text),
                    cantidad = Convert.ToInt32(txtCantidad.Text),
                    precio_venta=Convert.ToDecimal(txtPrecioVenta.Text),
                    Id_proveedor=Convert.ToInt32(comboProveedor.EditValue),
                    estado = true
                     
                };

                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Productos.InsertOnSubmit(productos);

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
            txtNombreProducto.Text = gridView1.GetFocusedRowCellValue("NOMBRE_DE_PRODUCTO").ToString();//Mostramos dato
            txtDescripcion.Text = gridView1.GetFocusedRowCellValue("DESCRIPCION").ToString();//Mostramos dato
            txtPrecioCosto.Text = gridView1.GetFocusedRowCellValue("PRECIO_COSTO").ToString();//Mostramos dato
            txtCantidad.Text = gridView1.GetFocusedRowCellValue("CANTIDAD").ToString();//Mostramos dato
            txtPrecioVenta.Text = gridView1.GetFocusedRowCellValue("PRECIO_VENTA").ToString();//Mostramos dato
            comboProveedor.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID_PROVEEDOR").ToString());//Mostramos dato

            simpleButtonGuardar.Enabled = false;//Desabilitamos boton guardar
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
        }


        //BOTON MODIFICAR
        private void btnModificar_Click(object sender, EventArgs e)
        {
            //CONSULTAMOS Y TRAEMOS AL PRIMERO QUE ENCUENTRE CON FirstOrDefault
            //con sultas con  linq crear variable
            var modificar = (from P in conexion.Productos
                             where P.id_producto == indice
                             select P).FirstOrDefault();

            //MODIFICAMOS
            modificar.nombre = txtNombreProducto.Text;
            modificar.descripcion = txtDescripcion.Text;
            modificar.precio_costo = Convert.ToDecimal(txtPrecioCosto.Text);
            modificar.cantidad = Convert.ToInt32(txtCantidad.Text);
            modificar.precio_venta = Convert.ToDecimal(txtPrecioVenta.Text);
            modificar.Id_proveedor = Convert.ToInt32(comboProveedor.EditValue);
    

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
                var eliminar = (from P in conexion.Productos
                                 where P.id_producto == indice
                                 select P).FirstOrDefault();

                //MODIFICAMOS
                eliminar.estado = false;


                //GUARDAMOS
                conexion.SubmitChanges();

                //FUNCION LIMPIAR
                Limpiar();
                //NOTIFICAMOS AL USUARIO
                alertControl1.Show(this, "Aviso", "Registro eliminado con exito");

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
