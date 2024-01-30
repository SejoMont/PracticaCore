using PracticaCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region
//create procedure SP_CLIENTES
//as
//	select * from clientes
//go

//create procedure SP_DATOS_CLIENTE
//(@EMPRESA NVARCHAR(30))
//as
//	select * from clientes where EMPRESA = @EMPRESA
//go

//create procedure SP_PEDIDOS_CLIENTE
//(@COD_CLIENTE NVARCHAR(10))
//as
//	select * from pedidos where CodigoCliente=@COD_CLIENTE
//go

//create procedure SP_INSERTAR_PEDIDO
//(
//@COD_PEDIDO NVARCHAR(30),
//@COD_CLIENTE NVARCHAR(30),
//@FECHA_ENTREGA NVARCHAR(30),
//@FORMA_ENVIO NVARCHAR(30),
//@IMPORTE INT
//)
//AS
//INSERT INTO pedidos
//     VALUES
//           (@COD_PEDIDO, @COD_CLIENTE, @FECHA_ENTREGA, @FORMA_ENVIO, @IMPORTE)
//GO

//create procedure SP_DATOS_PEDIDO
//(@COD_PEDIDO NVARCHAR(30))
//as
//	select * from pedidos where CodigoPedido = @COD_PEDIDO
//go

//CREATE PROCEDURE SP_BORRAR_PEDIDO
//(@COD_PEDIDO NVARCHAR(30))
//AS
//    DELETE FROM pedidos WHERE CodigoPedido = @COD_PEDIDO
//GO
#endregion


namespace PracticaCore.Repositories
{
    public class RepositoryClientesPedidos
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryClientesPedidos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=NETCORE;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> GetClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> clientes = new List<string>();
            while (this.reader.Read())
            {
                clientes.Add(this.reader["EMPRESA"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public Cliente GetDatosCliente(string nombreCliente)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DATOS_CLIENTE";

            SqlParameter pamEmpresa = new SqlParameter("@EMPRESA", nombreCliente);
            this.com.Parameters.Add(pamEmpresa);
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            Cliente cliente = new Cliente();
            cliente.CodigoCliente = this.reader["CodigoCliente"].ToString();
            cliente.Empresa = this.reader["Empresa"].ToString();
            cliente.Contacto = this.reader["Contacto"].ToString();
            cliente.Cargo = this.reader["Cargo"].ToString();
            cliente.Ciudad = this.reader["Ciudad"].ToString();
            cliente.Telefono = this.reader["Telefono"].ToString();

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return cliente;
        }

        public List<string> GetPedidosCliente(string codCliente) {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";

            SqlParameter pamCodCliente = new SqlParameter("@COD_CLIENTE", codCliente);
            this.com.Parameters.Add(pamCodCliente);
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            List<string> pedidos = new List<string>();
            while (this.reader.Read())
            {
                pedidos.Add(this.reader["CodigoPedido"].ToString());
            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return pedidos;
        }

        public int InsertarPedido(string CodPedido, string CodCliente, string FechEntrega, string FormaEnvio, int Importe)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTAR_PEDIDO";

            SqlParameter pamCodPedido = new SqlParameter("@COD_PEDIDO", CodPedido);
            this.com.Parameters.Add(pamCodPedido);

            SqlParameter pamCodCliente = new SqlParameter("@COD_CLIENTE", CodCliente);
            this.com.Parameters.Add(pamCodCliente);

            SqlParameter pamFechaEntrega = new SqlParameter("@FECHA_ENTREGA", FechEntrega); // Corregido
            this.com.Parameters.Add(pamFechaEntrega);

            SqlParameter pamFormaEnvio = new SqlParameter("@FORMA_ENVIO", FormaEnvio); // Corregido
            this.com.Parameters.Add(pamFormaEnvio);

            SqlParameter pamImporte = new SqlParameter("@IMPORTE", Importe); // Corregido
            this.com.Parameters.Add(pamImporte);

            this.cn.Open();
            int results = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

            return results;
        }

        public Pedidos GetDatosPedido(string CodPedido)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DATOS_PEDIDO";

            SqlParameter pamCodPedido = new SqlParameter("@COD_PEDIDO", CodPedido);
            this.com.Parameters.Add(pamCodPedido);

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            Pedidos pedido = new Pedidos();

            if (this.reader.Read())
            {
                pedido.CodigoPedido = this.reader["CodigoPedido"].ToString();
                pedido.CodigoCliente = this.reader["CodigoCliente"].ToString();
                pedido.FechaEntrega = this.reader["FechaEntrega"].ToString();
                pedido.FormaEnvio = this.reader["FormaEnvio"].ToString();
                pedido.Importe = this.reader["Importe"].ToString();
            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return pedido;
        }

        public int BorrarPedido(string codPedido)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_BORRAR_PEDIDO";

            SqlParameter pamCodPedido = new SqlParameter("@COD_PEDIDO", codPedido);
            this.com.Parameters.Add(pamCodPedido);

            this.cn.Open();
            int results = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

            return results;
        }

    }
}
