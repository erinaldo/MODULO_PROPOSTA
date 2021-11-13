using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsTabela_Preco.
	/// </summary>
	public class clsTabela_Preco
	{
		public WinService_ImportacaoMGI2.XSD.Tabela_Preco dtsTabela_Preco;
		public WinService_ImportacaoMGI2.XSD.Tabela_Preco.dtbTabela_PrecoDataTable dtbTabela_Preco;
		public WinService_ImportacaoMGI2.XSD.Tabela_Preco.dtbMGI_Tabela_PrecoDataTable dtbMGI_Tabela_Preco;

		public clsTabela_Preco()
		{
			dtsTabela_Preco = new WinService_ImportacaoMGI2.XSD.Tabela_Preco();
			dtbTabela_Preco = dtsTabela_Preco.dtbTabela_Preco;
			dtbMGI_Tabela_Preco = dtsTabela_Preco.dtbMGI_Tabela_Preco;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, int intCompetencia )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Tabela_Preco_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				adp.SelectCommand.Parameters.AddWithValue("@intOrigem_P", int.Parse( clsParametro.getParametro("Origem_MGI") ) );
				adp.SelectCommand.Parameters.AddWithValue("@intCompetencia_P", intCompetencia );

				adp.Fill(dtbMGI_Tabela_Preco);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTabela_Preco.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Tabela_Preco.dtbTabela_PrecoRow drwNew in dtbTabela_Preco.Rows )
					if ( dtbMGI_Tabela_Preco.Select("Competencia = " + drwNew.Competencia.ToString() 
						+ " and Sequencia = " + drwNew.Sequencia.ToString()
						+ " and Indica_Tipo_Preco = " + drwNew.Indica_Tipo_Preco.ToString()
						+ " and Cod_Programa = '" + drwNew.Cod_Programa
						+ "' and Cod_Veiculo_Mercado = '" + drwNew.Cod_Veiculo_Mercado + "'" ).Length == 0 )
						dtbMGI_Tabela_Preco.AdddtbMGI_Tabela_PrecoRow(
							int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Competencia,
							drwNew.Sequencia,
							drwNew.Indica_Tipo_Preco,
							drwNew.Cod_Programa,
							drwNew.Cod_Veiculo_Mercado,
							drwNew.Valor);
					else
						foreach( WinService_ImportacaoMGI2.XSD.Tabela_Preco.dtbMGI_Tabela_PrecoRow drwOld in dtbMGI_Tabela_Preco.Select("Competencia = " + drwNew.Competencia.ToString() 
							+ " and Sequencia = " + drwNew.Sequencia.ToString()
							+ " and Indica_Tipo_Preco = " + drwNew.Indica_Tipo_Preco.ToString()
							+ " and Cod_Programa = '" + drwNew.Cod_Programa
							+ "' and Cod_Veiculo_Mercado = '" + drwNew.Cod_Veiculo_Mercado + "'" ) )
							if( drwNew.Valor != drwOld.Valor )
								drwOld.Valor = drwNew.Valor;

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTabela_Preco.spuImportar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Atualizar

		private void sprAtualizar( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Tabela_Preco_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@intCompetencia_P", SqlDbType.Int, 6, "Competencia" );
				adp.InsertCommand.Parameters.Add("@intSequencia_P", SqlDbType.TinyInt, 3, "Sequencia" );
				adp.InsertCommand.Parameters.Add("@intIndica_Tipo_Preco_P", SqlDbType.TinyInt, 3, "Indica_Tipo_Preco" );
				adp.InsertCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.InsertCommand.Parameters.Add("@strCod_Veiculo_Mercado_P", SqlDbType.Char, 5, "Cod_Veiculo_Mercado" );
				adp.InsertCommand.Parameters.Add("@decValor_P", SqlDbType.Decimal, 10, "Valor" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Tabela_Preco_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@intCompetencia_P", SqlDbType.Int, 6, "Competencia" );
				adp.UpdateCommand.Parameters.Add("@intSequencia_P", SqlDbType.TinyInt, 3, "Sequencia" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Tipo_Preco_P", SqlDbType.TinyInt, 3, "Indica_Tipo_Preco" );
				adp.UpdateCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.UpdateCommand.Parameters.Add("@strCod_Veiculo_Mercado_P", SqlDbType.Char, 5, "Cod_Veiculo_Mercado" );
				adp.UpdateCommand.Parameters.Add("@decValor_P", SqlDbType.Decimal, 10, "Valor" );

				adp.Update( dtbMGI_Tabela_Preco );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTabela_Preco.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}