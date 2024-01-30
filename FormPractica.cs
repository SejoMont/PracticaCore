using PracticaCore.Models;
using PracticaCore.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaCore
{
    public partial class FormPractica : Form
    {
        RepositoryClientesPedidos repo;
        public string codCliente = "";
        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryClientesPedidos();
            this.LoadClientes();
        }

        public void LoadClientes()
        {
            List<string> clientes = this.repo.GetClientes();
            foreach (string data in clientes)
            {
                this.cmbclientes.Items.Add(data);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string empresa = this.cmbclientes.SelectedItem.ToString();
            Cliente cliente = this.repo.GetDatosCliente(empresa);

            this.codCliente = cliente.CodigoCliente;

            this.txtempresa.Text = cliente.Empresa;
            this.txtcontacto.Text = cliente.Contacto;
            this.txtcargo.Text = cliente.Cargo;
            this.txtciudad.Text = cliente.Ciudad;
            this.txttelefono.Text = cliente.Telefono;

            this.lstpedidos.Items.Clear();
            List<string> pedidos = this.repo.GetPedidosCliente(this.codCliente);
            foreach (string data in pedidos)
            {
                this.lstpedidos.Items.Add(data);
            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            string codigoPedido = txtcodigopedido.Text;
            string codigoCliente = this.codCliente;
            string fechaEntrega = txtfechaentrega.Text;
            string formaEnvio = txtformaenvio.Text;
            int importe = int.Parse(txtimporte.Text);

            int results = this.repo.InsertarPedido(codigoPedido, codigoCliente, fechaEntrega, formaEnvio, importe);
            MessageBox.Show("Registros añadidos: " + results);

            this.lstpedidos.Items.Clear();

            List<string> pedidos = this.repo.GetPedidosCliente(this.codCliente);
            foreach (string data in pedidos)
            {
                this.lstpedidos.Items.Add(data);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codPedido = this.lstpedidos.SelectedItem.ToString();
            Pedidos pedido = this.repo.GetDatosPedido(codPedido);

            this.txtcodigopedido.Text = pedido.CodigoCliente;
            this.txtfechaentrega.Text = pedido.FechaEntrega;
            this.txtformaenvio.Text = pedido.FormaEnvio;
            this.txtimporte.Text = pedido.Importe;

        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string codPedido = this.lstpedidos.SelectedItem.ToString();

            int results = this.repo.BorrarPedido(codPedido);
            MessageBox.Show("Registros añadidos: " + results);
            this.lstpedidos.Items.Clear();

            List<string> pedidos = this.repo.GetPedidosCliente(this.codCliente);
            foreach (string data in pedidos)
            {
                this.lstpedidos.Items.Add(data);
            }
        }
    }
}
