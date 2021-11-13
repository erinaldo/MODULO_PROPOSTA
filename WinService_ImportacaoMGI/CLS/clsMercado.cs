using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsMercado.
	/// </summary>
	public class clsMercado
	{
		public WinService_ImportacaoMGI2.XSD.Mercado dtsMercado;
		public WinService_ImportacaoMGI2.XSD.Mercado.dtbMercadoDataTable dtbMercado;
		public WinService_ImportacaoMGI2.XSD.Mercado.dtbMercado_ComposicaoDataTable dtbMercado_Composicao;
		public WinService_ImportacaoMGI2.XSD.Mercado.dtbMGI_MercadoDataTable dtbMGI_Mercado;
		public WinService_ImportacaoMGI2.XSD.Mercado.dtbMGI_Mercado_ComposicaoDataTable dtbMGI_Mercado_Composicao;

		public clsMercado()
		{
			dtsMercado = new WinService_ImportacaoMGI2.XSD.Mercado();
			dtbMercado = dtsMercado.dtbMercado;
			dtbMercado_Composicao = dtsMercado.dtbMercado_Composicao; 
			dtbMGI_Mercado = dtsMercado.dtbMGI_Mercado;
			dtbMGI_Mercado_Composicao = dtsMercado.dtbMGI_Mercado_Composicao; 
		}

		#region Carregar
		internal void spuCarregar( SqlConnection cnn, string strCod_Mercado )
		{
			this.sprCarregarMercado( cnn, strCod_Mercado );
			this.sprCarregarMercado_Composicao( cnn, strCod_Mercado );
		}

		private void sprCarregarMercado( SqlConnection cnn, string strCod_Mercado )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Mercado_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Mercado", strCod_Mercado );

				adp.Fill(dtbMGI_Mercado);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMercado.sprCarregarMercado() " + ex.Message.ToString() );
			}
		}

		private void sprCarregarMercado_Composicao( SqlConnection cnn, string strCod_Mercado )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Mercado_Composicao_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Mercado", strCod_Mercado );

				adp.Fill( dtbMGI_Mercado_Composicao );

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMercado.sprCarregarMercado_Composicao() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Mercado.dtbMercadoRow drwNew in dtbMercado.Rows )
				{
					if ( dtbMGI_Mercado.Select("Cod_Mercado = '" + drwNew.Cod_Mercado + "'").Length == 0 )
						dtbMGI_Mercado.AdddtbMGI_MercadoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Mercado,
							drwNew.Nome );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Mercado.dtbMGI_MercadoRow drwOld in dtbMGI_Mercado.Select("Cod_Mercado = '" + drwNew.Cod_Mercado + "'") )
							if( drwNew.Nome != drwOld.Nome )
								drwOld.Nome = drwNew.Nome;
				}

				foreach( WinService_ImportacaoMGI2.XSD.Mercado.dtbMercado_ComposicaoRow drwNew in dtbMercado_Composicao.Rows )
				{
					if ( dtbMGI_Mercado_Composicao.Select("Cod_Mercado = '" + drwNew.Cod_Mercado + "' and Cod_Veiculo = '" + drwNew.Cod_Veiculo + "'" ).Length == 0 )
						dtbMGI_Mercado_Composicao.AdddtbMGI_Mercado_ComposicaoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Mercado,
							drwNew.Cod_Veiculo );
				}

				foreach( WinService_ImportacaoMGI2.XSD.Mercado.dtbMGI_Mercado_ComposicaoRow drwOld in dtbMGI_Mercado_Composicao.Rows )
				{
					if ( dtbMercado_Composicao.Select("Cod_Mercado = '" + drwOld.Cod_Mercado + "' and Cod_Veiculo = '" + drwOld.Cod_Veiculo + "'" ).Length == 0 )
						drwOld.Delete();
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMercado.spuImportar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Atualizar
		private void sprAtualizar( SqlConnection cnn)
		{
			this.sprAtualizarMercado( cnn);
			this.sprAtualizarMercado_Composicao( cnn);
		}

		private void sprAtualizarMercado( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Mercado_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Mercado_P", SqlDbType.Char, 5, "Cod_Mercado" );
				adp.InsertCommand.Parameters.Add("@strNome_P", SqlDbType.VarChar, 50, "Nome" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Mercado_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Mercado_P", SqlDbType.Char, 5, "Cod_Mercado" );
				adp.UpdateCommand.Parameters.Add("@strNome_P", SqlDbType.VarChar, 50, "Nome" );

				adp.Update( dtbMGI_Mercado );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMercado.sprAtualizarMercado() " + ex.Message.ToString() );
			}
		}

		private void sprAtualizarMercado_Composicao( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Mercado_Composicao_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Mercado_P", SqlDbType.Char, 5, "Cod_Mercado" );
				adp.InsertCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Mercado_Composicao_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Mercado_P", SqlDbType.Char, 5, "Cod_Mercado" );
				adp.UpdateCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );

				adp.DeleteCommand = (new SqlCommand("pr_MGI_Mercado_Composicao_D"));
				adp.DeleteCommand.CommandType = CommandType.StoredProcedure;
				adp.DeleteCommand.Connection = cnn;
				//adp.DeleteCommand.Transaction = tran;
				adp.DeleteCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.DeleteCommand.Parameters.Add("@strCod_Mercado_P", SqlDbType.Char, 5, "Cod_Mercado" );
				adp.DeleteCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );

				adp.Update( dtbMGI_Mercado_Composicao );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMercado.sprAtualizarMercado_Composicao() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}