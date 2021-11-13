using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsVeiculacao.
	/// </summary>
	public class clsVeiculacao
	{
		public WinService_ImportacaoMGI2.XSD.Veiculacao dtsVeiculacao;
		public WinService_ImportacaoMGI2.XSD.Veiculacao.dtbVeiculacaoDataTable dtbVeiculacao;
		public WinService_ImportacaoMGI2.XSD.Veiculacao.dtbMGI_VeiculacaoDataTable dtbMGI_Veiculacao;

		public clsVeiculacao()
		{
			dtsVeiculacao = new WinService_ImportacaoMGI2.XSD.Veiculacao();
			dtbVeiculacao = dtsVeiculacao.dtbVeiculacao;
			dtbMGI_Veiculacao = dtsVeiculacao.dtbMGI_Veiculacao;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, int intID_Contrato )
		{
			SqlDataAdapter adp;

			try
			{
				if ( dtbMGI_Veiculacao.Select( "Origem=" + clsParametro.getParametro("Origem_MGI")  + " and ID_Contrato=" + intID_Contrato.ToString() ).Length == 0 )
				{
					adp = new SqlDataAdapter();

					adp.SelectCommand = (new SqlCommand("pr_MGI_Veiculacao_S"));
					adp.SelectCommand.CommandType = CommandType.StoredProcedure;
					adp.SelectCommand.Connection = cnn;
                    adp.SelectCommand.CommandTimeout = 0;
					adp.SelectCommand.Parameters.AddWithValue("@intOrigem_P", clsParametro.getParametro("Origem_MGI") );
					adp.SelectCommand.Parameters.AddWithValue("@intID_Contrato_P", intID_Contrato );

					adp.Fill(dtbMGI_Veiculacao);
				}
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculacao.spuCarregar() " + ex.Message.ToString() );
			}
		}

		internal void spuCarregarCompetencia( SqlConnection cnn, int intCompetencia )
		{
			SqlDataAdapter adp;

			try
			{
				if ( dtbMGI_Veiculacao.Select( "Competencia = " + intCompetencia.ToString() + " and Origem = " + clsParametro.getParametro("Origem_MGI") ).Length == 0 )
				{
					adp = new SqlDataAdapter();

					adp.SelectCommand = (new SqlCommand("pr_MGI_Veiculacao_Movimento_S"));
					adp.SelectCommand.CommandType = CommandType.StoredProcedure;
					adp.SelectCommand.Connection = cnn;
                    adp.SelectCommand.CommandTimeout = 0;
					adp.SelectCommand.Parameters.AddWithValue("@intOrigem_P", clsParametro.getParametro("Origem_MGI") );
					adp.SelectCommand.Parameters.AddWithValue("@intCompetencia_P", intCompetencia );

					adp.Fill(dtbMGI_Veiculacao);
				}
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculacao.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach(WinService_ImportacaoMGI2.XSD.Veiculacao.dtbVeiculacaoRow drwNew in dtbVeiculacao.Rows )
				{
					// Tentativa de eliminar diferença de Time Zone (GNT-03:00 Brasília) entre servidores
					int intAno = drwNew.Data_Exibicao.Year;
					int intMes = drwNew.Data_Exibicao.Month;
					int intDia = drwNew.Data_Exibicao.Day;
					string strData = "";
					strData = intAno.ToString().Trim() + "/" ;
					strData += ("0"+ intMes.ToString().Trim()).Substring(intMes.ToString().Trim().Length -1,2) + "/";
					strData += ("0"+ intDia.ToString().Trim()).Substring(intDia.ToString().Trim().Length -1,2) ;

					if ( dtbMGI_Veiculacao.Select("Cod_Veiculo = '" + drwNew.Cod_Veiculo
//						+ "' and Data_Exibicao = '" + drwNew.Data_Exibicao.ToString("yyyy/MM/dd")
						+ "' and Data_Exibicao = '" + strData
						+ "' and Cod_Programa = '" + drwNew.Cod_Programa
						+ "' and Chave_Acesso = " + drwNew.Chave_Acesso ).Length == 0 )

						dtbMGI_Veiculacao.AdddtbMGI_VeiculacaoRow(
							int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Veiculo,
//							(DateTime)drwNew.Data_Exibicao,
							drwNew.Data_Exibicao,
							drwNew.Cod_Programa,
							drwNew.Chave_Acesso,
							drwNew.ID_Contrato,
							drwNew.Competencia,
							drwNew.Cod_Tipo_Comercial,
							drwNew.Duracao,
							( drwNew.IsCod_Motivo_FalhaNull() ? null : drwNew.Cod_Motivo_Falha ),
							drwNew.Cod_Red_Produto1,
							drwNew.Cod_Red_Produto2,
							drwNew.Cod_Red_Produto3,
							drwNew.Indica_Exibido,
							drwNew.Indica_Origem,
							drwNew.Indica_Venda,
							drwNew.Indica_Financeiro,
							drwNew.Vlr_Tabela,
							drwNew.Vlr_Negociado,
							drwNew.Vlr_Gerencial,
							drwNew.Vlr_Correcao,
							drwNew.Vlr_Varejo,
							drwNew.Documento_De,
							drwNew.Documento_Para
							);
					else
						foreach(WinService_ImportacaoMGI2.XSD.Veiculacao.dtbMGI_VeiculacaoRow drwOld in dtbMGI_Veiculacao.Select("Cod_Veiculo = '" + drwNew.Cod_Veiculo
							//+ "' and Data_Exibicao = '" + drwNew.Data_Exibicao.ToString("yyyy/MM/dd")
							+ "' and Data_Exibicao = '" + strData
							+ "' and Cod_Programa = '" + drwNew.Cod_Programa
							+ "' and Chave_Acesso = " + drwNew.Chave_Acesso ) )

							if ( drwOld.Cod_Tipo_Comercial != drwNew.Cod_Tipo_Comercial
								|| drwOld.Duracao != drwNew.Duracao
								|| ( drwOld.IsCod_Motivo_FalhaNull() ? "" : drwOld.Cod_Motivo_Falha ) != ( drwNew.IsCod_Motivo_FalhaNull() ? "" : drwNew.Cod_Motivo_Falha )
								|| drwOld.Cod_Red_Produto1!=drwNew.Cod_Red_Produto1
								|| drwOld.Cod_Red_Produto2!=drwNew.Cod_Red_Produto2
								|| drwOld.Cod_Red_Produto3!=drwNew.Cod_Red_Produto3
								|| drwOld.Indica_Exibido!=drwNew.Indica_Exibido
								|| drwOld.Indica_Origem!=drwNew.Indica_Origem
								|| drwOld.Indica_Venda!=drwNew.Indica_Venda
								|| drwOld.Indica_Financeiro!=drwNew.Indica_Financeiro
								|| drwOld.Vlr_Tabela!=drwNew.Vlr_Tabela
								|| drwOld.Vlr_Negociado!=drwNew.Vlr_Negociado
								|| drwOld.Vlr_Gerencial!=drwNew.Vlr_Gerencial
								|| drwOld.Vlr_Correcao!=drwNew.Vlr_Correcao
								|| drwOld.Vlr_Varejo!=drwNew.Vlr_Varejo
								|| drwOld.Documento_De!=drwNew.Documento_De
								|| drwOld.Documento_Para!=drwNew.Documento_Para)

							{
								drwOld.Cod_Tipo_Comercial = drwNew.Cod_Tipo_Comercial;
								drwOld.Duracao = drwNew.Duracao;
								drwOld.Cod_Motivo_Falha = ( drwNew.IsCod_Motivo_FalhaNull() ? null : drwNew.Cod_Motivo_Falha );
								drwOld.Cod_Red_Produto1=drwNew.Cod_Red_Produto1;
								drwOld.Cod_Red_Produto2=drwNew.Cod_Red_Produto2;
								drwOld.Cod_Red_Produto3=drwNew.Cod_Red_Produto3;
								drwOld.Indica_Exibido=drwNew.Indica_Exibido;
								drwOld.Indica_Origem=drwNew.Indica_Origem;
								drwOld.Indica_Venda=drwNew.Indica_Venda;
								drwOld.Indica_Financeiro=drwNew.Indica_Financeiro;
								drwOld.Vlr_Tabela=drwNew.Vlr_Tabela;
								drwOld.Vlr_Negociado=drwNew.Vlr_Negociado;
								drwOld.Vlr_Gerencial=drwNew.Vlr_Gerencial;
								drwOld.Vlr_Correcao=drwNew.Vlr_Correcao;
								drwOld.Vlr_Varejo=drwNew.Vlr_Varejo;
								drwOld.Documento_De=drwNew.Documento_De;
								drwOld.Documento_Para=drwNew.Documento_Para;
							}
				}
				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculacao.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Veiculacao_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 5, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
				adp.InsertCommand.Parameters.Add("@dteData_Exibicao_P", SqlDbType.SmallDateTime, 10, "Data_Exibicao" );
				adp.InsertCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.InsertCommand.Parameters.Add("@intChave_Acesso_P", SqlDbType.SmallInt, 2, "Chave_Acesso" );
				adp.InsertCommand.Parameters.Add("@intID_Contrato_P", SqlDbType.Int, 8, "ID_Contrato" );
				adp.InsertCommand.Parameters.Add("@intCompetencia_P", SqlDbType.Int, 6, "Competencia" );
				adp.InsertCommand.Parameters.Add("@strCod_Tipo_Comercial_P", SqlDbType.Char, 2, "Cod_Tipo_Comercial" );
				adp.InsertCommand.Parameters.Add("@intDuracao_P", SqlDbType.SmallInt, 5, "Duracao" );
				adp.InsertCommand.Parameters.Add("@strCod_Motivo_Falha_P", SqlDbType.Char, 3, "Cod_Motivo_Falha" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto1_P", SqlDbType.Int, 5, "Cod_Red_Produto1" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto2_P", SqlDbType.Int, 5, "Cod_Red_Produto2" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto3_P", SqlDbType.Int, 5, "Cod_Red_Produto3" );
				adp.InsertCommand.Parameters.Add("@intIndica_Exibido_P", SqlDbType.TinyInt, 1, "Indica_Exibido" );
				adp.InsertCommand.Parameters.Add("@intIndica_Origem_P", SqlDbType.TinyInt, 1, "Indica_Origem" );
				adp.InsertCommand.Parameters.Add("@intIndica_Venda_P", SqlDbType.TinyInt, 1, "Indica_Venda" );
				adp.InsertCommand.Parameters.Add("@intIndica_Financeiro_P", SqlDbType.TinyInt, 1, "Indica_Financeiro" );
				adp.InsertCommand.Parameters.Add("@decVlr_Tabela_P", SqlDbType.Decimal, 10, "Vlr_Tabela" );
				adp.InsertCommand.Parameters.Add("@decVlr_Negociado_P", SqlDbType.Decimal, 10, "Vlr_Negociado" );
				adp.InsertCommand.Parameters.Add("@decVlr_Gerencial_P", SqlDbType.Decimal, 10, "Vlr_Gerencial" );
				adp.InsertCommand.Parameters.Add("@decVlr_Correcao_P", SqlDbType.Decimal, 10, "Vlr_Correcao" );
				adp.InsertCommand.Parameters.Add("@decVlr_Varejo_P", SqlDbType.Decimal, 10, "Vlr_Varejo" );
				adp.InsertCommand.Parameters.Add("@strDocumento_De_P", SqlDbType.Char, 10, "Documento_De" );
				adp.InsertCommand.Parameters.Add("@strDocumento_Para_P", SqlDbType.Char, 10, "Documento_Para" );

				// Elininado para não dar conflito por causa do Time Zone
				// UpdatedRowSource Atualiza os NdeID dos Novos Registros no DataTable
				// adp.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Veiculacao_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intID_Veiculacao_P", SqlDbType.BigInt, 15, "ID_Veiculacao" );
				adp.UpdateCommand.Parameters.Add("@strCod_Tipo_Comercial_P", SqlDbType.Char, 2, "Cod_Tipo_Comercial" );
				adp.UpdateCommand.Parameters.Add("@intDuracao_P", SqlDbType.SmallInt, 5, "Duracao" );
				adp.UpdateCommand.Parameters.Add("@strCod_Motivo_Falha_P", SqlDbType.Char, 3, "Cod_Motivo_Falha" );
				adp.UpdateCommand.Parameters.Add("@intCod_Red_Produto1_P", SqlDbType.Int, 5, "Cod_Red_Produto1" );
				adp.UpdateCommand.Parameters.Add("@intCod_Red_Produto2_P", SqlDbType.Int, 5, "Cod_Red_Produto2" );
				adp.UpdateCommand.Parameters.Add("@intCod_Red_Produto3_P", SqlDbType.Int, 5, "Cod_Red_Produto3" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Exibido_P", SqlDbType.TinyInt, 1, "Indica_Exibido" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Origem_P", SqlDbType.TinyInt, 1, "Indica_Origem" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Venda_P", SqlDbType.TinyInt, 1, "Indica_Venda" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Financeiro_P", SqlDbType.TinyInt, 1, "Indica_Financeiro" );
				adp.UpdateCommand.Parameters.Add("@decVlr_Tabela_P", SqlDbType.Decimal, 10, "Vlr_Tabela" );
				adp.UpdateCommand.Parameters.Add("@decVlr_Negociado_P", SqlDbType.Decimal, 10, "Vlr_Negociado" );
				adp.UpdateCommand.Parameters.Add("@decVlr_Gerencial_P", SqlDbType.Decimal, 10, "Vlr_Gerencial" );
				adp.UpdateCommand.Parameters.Add("@decVlr_Correcao_P", SqlDbType.Decimal, 10, "Vlr_Correcao" );
				adp.UpdateCommand.Parameters.Add("@decVlr_Varejo_P", SqlDbType.Decimal, 10, "Vlr_Varejo" );
				adp.UpdateCommand.Parameters.Add("@strDocumento_De_P", SqlDbType.Char, 10, "Documento_De" );
				adp.UpdateCommand.Parameters.Add("@strDocumento_Para_P", SqlDbType.Char, 10, "Documento_Para" );

				adp.Update( dtbMGI_Veiculacao );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculacao.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}