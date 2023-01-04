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
    public partial class Ventas : Form
    {
        //Conectar a linq mi base de datos
        DB.MyDataBaseDataContext conexion = new DB.MyDataBaseDataContext(Clases.General.Cadena);

        public Ventas()
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
             txtCantidad.Focus();//COLOCAR CURSOS EN EL TXT

            //LIMPIAMOS COMBOS
            comboCajera.EditValue = -1;
            comboCliente.EditValue = -1;
            comboProducto.EditValue = -1;

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
            var transaccion = from t in conexion.detalle_transaccion
                            where t.estado == true
                            select new
                            {
                                ID = t.id_transaccion,
                                NO_FACTURA=t.Transaccion.id_transaccion,
                                ID_CLIENTE=t.Transaccion.Id_cliente,//primero tomamos el id
                                CLIENTE = t.Transaccion.Cliente.nombres + ' ' + t.Transaccion.Cliente.apellidos,//ya podemos acceder a los datos de proveedor
                                ID_CAJERO=t.Transaccion.id_cajero,
                                CAJERO=t.Transaccion.Cajero.nombre,                          
                                CANTIDAD = t.cantidad,
                                ID_PRODUCTO=t.Productos.id_producto,
                                PRODUCTO=t.Productos.nombre,
                                PRECIO=t.precio,
                                ID_TOTAL=t.Transaccion.id_transaccion,
                                TOTAL=t.Transaccion.total,
                                ID_FECHA=t.Transaccion.id_transaccion,
                                FECHA=t.Transaccion.fecha
                            };


            //INGRESAR A LA TABLA
            gridControl1.DataSource = transaccion;
            this.gridView1.Columns["ID"].Visible = false;//Ocultamos la columna del ID
            this.gridView1.Columns["ID_CLIENTE"].Visible = false;
            this.gridView1.Columns["ID_CAJERO"].Visible = false;
            this.gridView1.Columns["ID_PRODUCTO"].Visible = false;
            this.gridView1.Columns["ID_TOTAL"].Visible = false;
            this.gridView1.Columns["ID_FECHA"].Visible = false;

            //**COMBOS DE FORMULARIO**//
            //TRAEMOS DATOS CAJERO
            var cajera = from ca in conexion.Cajero
                            where ca.estado == true
                            select new
                            {
                                ID = ca.id_cajero,
                                NOMBRE = ca.nombre
                            };

            //TRAEMOS DATOS CLIENTE
            var cliente = from cli in conexion.Cliente
                         where cli.estado == true
                         select new
                         {
                             ID = cli.Id_cliente,
                             NOMBRE = cli.nombres + ' ' + cli.apellidos                
                         };

            //TRAEMOS DATOS COMBO PRODUCTO
            var producto = from p in conexion.Productos
                          where p.estado == true
                          select new
                          {
                              ID = p.id_producto,
                              NOMBRE = p.nombre
                          };

            //TRAEMOS DATOS COMBO PRODUCTO
            var pro = from p in conexion.Productos
                           where p.estado == true
                           select new
                           {
                               ID = p.id_producto,
                               NOMBRE = p.precio_venta
                           };



            //LLENAR COMBO CLIENTE
            comboCliente.Properties.DataSource = cliente;
            comboCliente.Properties.DisplayMember = "NOMBRE";
            comboCliente.Properties.ValueMember = "ID";

            //LLENAR COMBO CAJERA
            comboCajera.Properties.DataSource = cajera;
            comboCajera.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboCajera.Properties.ValueMember = "ID";
         

            //LLENAR COMBO PRODUCTO
            comboProducto.Properties.DataSource = producto ;
            comboProducto.Properties.DisplayMember = "NOMBRE";
            comboProducto.Properties.ValueMember = "ID";

            //LLENAR COMBO PRECIO
            comboPrecio.Properties.DataSource = pro;
            comboPrecio.Properties.DisplayMember = "NOMBRE";
            comboPrecio.Properties.ValueMember = "ID";

        }

        //Filtrar por campos
        private void comboProducto_EditValueChanged(object sender, EventArgs e)
        {

            int IdProducto = Convert.ToInt32(comboProducto.EditValue);//Almacenamos el id que tiene el combo en marca
            //**COMBOS DE FORMULARIO**//
            //TRAEMOS DATOS MODELO
            var productos = from p in conexion.Productos
                            where p.estado == true &&
                            p.id_producto == IdProducto

                            select new
                            {
                                ID = p.id_producto,
                                NOMBRE = p.precio_venta
                            };

            //LLENAR COMBO MODELO
            comboPrecio.Properties.DataSource = productos;
            comboPrecio.Properties.DisplayMember = "NOMBRE"; //MOSTRAR O SELECCIONAR
            comboPrecio.Properties.ValueMember = "ID";
        }

        //CALCULAR TOTAL A PAGAR
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text!=string.Empty && comboPrecio.Text!=string.Empty)
            {
                decimal total = (Convert.ToDecimal(txtCantidad.Text)) * (Convert.ToDecimal(comboPrecio.Text));
                txtTotal.Text = total.ToString();
            }

        }

        //BOTON GUARDAR
        private void simpleButtonGuardar_Click(object sender, EventArgs e)
        {
            if (comboCliente.Text != String.Empty && comboCajera.Text != String.Empty && txtCantidad.Text != String.Empty && comboProducto.Text != String.Empty && comboPrecio.Text != String.Empty && txtTotal.Text!=String.Empty && txtFecha.Text!=String.Empty)
            {
                //Creamos objeto que es insert
                DB.Ventas venta = new DB.Ventas
                {

                    Nit = txtNit.Text,
                    Id_cliente = Convert.ToInt32(comboCliente.EditValue),
                   
                    estado = true

                };

                //Conexion con linq, tabla que inserte y marca
                //INSERTAMOS OBJETO PENDIENTE DE GUARDAR
                conexion.Ventas.InsertOnSubmit(venta);

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

        
    }



}
