using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace presentacion
{
    public partial class frmAgregarArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAgregarArticulo(Articulo articulo)
        {
            InitializeComponent();
            Text = "Modificar artículo";
            this.articulo = articulo;
        }
        public frmAgregarArticulo()
        {
            InitializeComponent();
        }

        private void frmAgregarArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pboAgregarImagen.Load(imagen);
            }
            catch (Exception)
            {

                pboAgregarImagen.Load("https://www.came-educativa.com.ar/upsoazej/2022/03/placeholder-4.png");
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            if (articulo == null)
            {
                articulo = new Articulo();
            }
            articulo.Codigo = txtCodigo.Text;
            articulo.Nombre = txtNombre.Text;
            articulo.Descripcion = txtDescripcion.Text;
            articulo.Marca = (Marca)cboMarca.SelectedItem;
            articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
            articulo.ImagenUrl = txtImagenUrl.Text;
            articulo.Precio = decimal.Parse(txtPrecio.Text);
            if (articulo.Id != 0)
            {
                negocio.modificarArticulo(articulo);
                MessageBox.Show("Artículo modificado con éxito!");
            }
            else
            {
                negocio.agregarArticulo(articulo);
                MessageBox.Show("Artículo agregado con éxito!");
            }
            if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["articulo-app"] + archivo.SafeFileName);
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg | png|*.png";
            try
            {
                if (archivo.ShowDialog() == DialogResult.OK)
                {
                    txtImagenUrl.Text = archivo.FileName;
                    cargarImagen(archivo.FileName);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
