using System;
using System.Data;
using System.Data.SqlClient; 
using ImportacaoMGI.CLS;

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsContrato.
	/// </summary>
	public class clsContrato
	{
		public WinService_ImportacaoMGI2.XSD.Contrato dtsContrato;
		public WinService_ImportacaoMGI2.XSD.Contrato.dtbContratoDataTable dtbContrato;
		private WinService_ImportacaoMGI2.XSD.MGIContrato dtsMGIContrato;
		public WinService_ImportacaoMGI2.XSD.MGIContrato.dtbMGI_ContratoDataTable dtbMGI_Contrato;
		public clsContrato()
		{
			dtsContrato = new WinService_ImportacaoMGI2.XSD.Contrato();
			dtbContrato = dtsContrato.dtbContrato;

			dtsMGIContrato = new WinService_ImportacaoMGI2.XSD.MGIContrato();
			dtbMGI_Contrato = dtsMGIContrato.dtbMGI_Contrato;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, int intID_Contrato )
		{
			SqlDataAdapter adp;
			
			ImportacaoMGI.CLS.clsLOG.psuLog("Processando o Contrato: " + intID_Contrato.ToString(),false);
	 

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Contrato_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				adp.SelectCommand.Parameters.AddWithValue("@intID_Contrato_P", intID_Contrato );

				adp.Fill(dtbMGI_Contrato);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContrato.spuCarregar() " + ex.Message.ToString() );
			}
		}

		internal void spuCarregar( SqlConnection cnn, string strCod_Empresa, int intCompetencia )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Contrato_Movimento_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				adp.SelectCommand.Parameters.AddWithValue("@strCod_Empresa_P", strCod_Empresa );
				adp.SelectCommand.Parameters.AddWithValue("@intCompetencia_P", intCompetencia );

				adp.Fill(dtbMGI_Contrato);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContrato.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				
				foreach( WinService_ImportacaoMGI2.XSD.Contrato.dtbContratoRow drwNew in dtbContrato.Rows )
					if ( dtbMGI_Contrato.Select("ID_Contrato = '" + drwNew.ID_Contrato.ToString() + "'").Length == 0 )
						dtbMGI_Contrato.AdddtbMGI_ContratoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.ID_Contrato,	
							drwNew.Cod_Empresa_Faturamento,
							drwNew.Numero_Negociacao,
							drwNew.Cod_Empresa,
							drwNew.Numero_Mr,
							drwNew.Sequencia_Mr,
							drwNew.Competencia,
							drwNew.Numero_PI,
							drwNew.Data_Cadastramento,
							drwNew.Cod_Tipo_Midia,
							drwNew.Cod_Agencia,
							drwNew.Cod_Cliente,
							( drwNew.IsCod_IntermediarioNull() ? null : drwNew.Cod_Intermediario ),
							drwNew.Cod_Nucleo,
							drwNew.Cod_Contato,
							drwNew.Indica_Grade,
							drwNew.Indica_Midia,
							drwNew.Indica_Desvinculado,
							drwNew.Indica_Cancelado,
							drwNew.Motivo_Cancelamento,
							( drwNew.IsCod_Programa_PatrocinadoNull() ? null : drwNew.Cod_Programa_Patrocinado ),
							drwNew.Comissao_Agencia,
							drwNew.Comissao_Intermediario );
					else
						foreach( WinService_ImportacaoMGI2.XSD.MGIContrato.dtbMGI_ContratoRow drwOld in dtbMGI_Contrato.Select("ID_Contrato = '" + drwNew.ID_Contrato + "'") )
							if ( drwOld.Numero_Negociacao != drwNew.Numero_Negociacao
								|| drwOld.Numero_PI != drwNew.Numero_PI
								|| drwOld.Data_Cadastramento != drwNew.Data_Cadastramento
								|| drwOld.Cod_Tipo_Midia != drwNew.Cod_Tipo_Midia
								|| drwOld.Cod_Agencia != drwNew.Cod_Agencia
								|| drwOld.Cod_Cliente != drwNew.Cod_Cliente
								|| ( drwOld.IsCod_IntermediarioNull() ? "" : drwOld.Cod_Intermediario ) != ( drwNew.IsCod_IntermediarioNull() ? "" : drwNew.Cod_Intermediario )
								|| drwOld.Cod_Nucleo != drwNew.Cod_Nucleo
								|| drwOld.Cod_Contato != drwNew.Cod_Contato
								|| drwOld.Indica_Grade != drwNew.Indica_Grade
								|| drwOld.Indica_Midia != drwNew.Indica_Midia
								|| drwOld.Indica_Desvinculado != drwNew.Indica_Desvinculado
								|| drwOld.Indica_Cancelado != drwNew.Indica_Cancelado
								|| drwOld.Motivo_Cancelamento != drwNew.Motivo_Cancelamento
								|| ( drwOld.IsCod_Programa_PatrocinadoNull() ? "" : drwOld.Cod_Programa_Patrocinado ) != ( drwNew.IsCod_Programa_PatrocinadoNull() ? "" : drwNew.Cod_Programa_Patrocinado )
								|| drwOld.Comissao_Agencia != drwNew.Comissao_Agencia
								|| drwOld.Comissao_Intermediario != drwNew.Comissao_Intermediario )
							{

								drwOld.Numero_Negociacao = drwNew.Numero_Negociacao;
								drwOld.Numero_PI = drwNew.Numero_PI;;
								drwOld.Data_Cadastramento = drwNew.Data_Cadastramento;
								drwOld.Cod_Tipo_Midia = drwNew.Cod_Tipo_Midia;
								drwOld.Cod_Agencia = drwNew.Cod_Agencia;
								drwOld.Cod_Cliente = drwNew.Cod_Cliente;
								drwOld.Cod_Intermediario = ( drwNew.IsCod_IntermediarioNull() ? null : drwNew.Cod_Intermediario );
								drwOld.Cod_Nucleo = drwNew.Cod_Nucleo;
								drwOld.Cod_Contato = drwNew.Cod_Contato;
								drwOld.Indica_Grade = drwNew.Indica_Grade;
								drwOld.Indica_Midia = drwNew.Indica_Midia;
								drwOld.Indica_Desvinculado = drwNew.Indica_Desvinculado;
								drwOld.Indica_Cancelado = drwNew.Indica_Cancelado;
								drwOld.Motivo_Cancelamento = drwNew.Motivo_Cancelamento;
								drwOld.Cod_Programa_Patrocinado = ( drwNew.IsCod_Programa_PatrocinadoNull() ? null : drwNew.Cod_Programa_Patrocinado );
								drwOld.Comissao_Agencia = drwNew.Comissao_Agencia;
								drwOld.Comissao_Intermediario = drwNew.Comissao_Intermediario;
							}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContrato.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Contrato_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@intID_Contrato_P", SqlDbType.Int, 10, "ID_Contrato" );
				adp.InsertCommand.Parameters.Add("@strCod_Empresa_Faturamento_P", SqlDbType.Char, 3, "Cod_Empresa_Faturamento" );
				adp.InsertCommand.Parameters.Add("@intNumero_Negociacao_P", SqlDbType.Decimal, 18, "Numero_Negociacao" );
				adp.InsertCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.InsertCommand.Parameters.Add("@intNumero_Mr_P", SqlDbType.Int, 10, "Numero_Mr" );
				adp.InsertCommand.Parameters.Add("@intSequencia_Mr_P", SqlDbType.SmallInt, 5, "Sequencia_Mr" );
				adp.InsertCommand.Parameters.Add("@intCompetencia_P", SqlDbType.Int, 6, "Competencia" );
				adp.InsertCommand.Parameters.Add("@strNumero_PI_P", SqlDbType.VarChar, 20, "Numero_PI" );
				adp.InsertCommand.Parameters.Add("@dteData_Cadastramento_P", SqlDbType.SmallDateTime, 10, "Data_Cadastramento" );
				adp.InsertCommand.Parameters.Add("@strCod_Tipo_Midia_P", SqlDbType.Char, 6, "Cod_Tipo_Midia" );
				adp.InsertCommand.Parameters.Add("@strCod_Agencia_P", SqlDbType.Char, 6, "Cod_Agencia" );
				adp.InsertCommand.Parameters.Add("@strCod_Cliente_P", SqlDbType.Char, 6, "Cod_Cliente" );
				adp.InsertCommand.Parameters.Add("@strCod_Intermediario_P", SqlDbType.Char, 6, "Cod_Intermediario" );
				adp.InsertCommand.Parameters.Add("@strCod_Nucleo_P", SqlDbType.Char, 7, "Cod_Nucleo" );
				adp.InsertCommand.Parameters.Add("@strCod_Contato_P", SqlDbType.Char, 5, "Cod_Contato" );
				adp.InsertCommand.Parameters.Add("@intIndica_Grade_P", SqlDbType.TinyInt, 1, "Indica_Grade" );
				adp.InsertCommand.Parameters.Add("@intIndica_Midia_P", SqlDbType.TinyInt, 1, "Indica_Midia" );
				adp.InsertCommand.Parameters.Add("@intIndica_Desvinculado_P", SqlDbType.TinyInt, 1, "Indica_Desvinculado" );
				adp.InsertCommand.Parameters.Add("@intIndica_Cancelado_P", SqlDbType.TinyInt, 1, "Indica_Cancelado" );
				adp.InsertCommand.Parameters.Add("@strMotivo_Cancelamento_P", SqlDbType.VarChar, 50, "Motivo_Cancelamento" );
				adp.InsertCommand.Parameters.Add("@strCod_Programa_Patrocinado_P", SqlDbType.Char, 4, "Cod_Programa_Patrocinado" );
				adp.InsertCommand.Parameters.Add("@decComissao_Agencia_P", SqlDbType.Decimal, 16, "Comissao_Agencia" );
				adp.InsertCommand.Parameters.Add("@decComissao_Intermediario_P", SqlDbType.Decimal, 16, "Comissao_Intermediario" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Contrato_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@intID_Contrato_P", SqlDbType.Int, 10, "ID_Contrato" );
				adp.UpdateCommand.Parameters.Add("@intNumero_Negociacao_P", SqlDbType.Decimal, 18, "Numero_Negociacao" );
				adp.UpdateCommand.Parameters.Add("@strNumero_PI_P", SqlDbType.VarChar, 20, "Numero_PI" );
				adp.UpdateCommand.Parameters.Add("@dteData_Cadastramento_P", SqlDbType.SmallDateTime, 10, "Data_Cadastramento" );
				adp.UpdateCommand.Parameters.Add("@strCod_Tipo_Midia_P", SqlDbType.Char, 6, "Cod_Tipo_Midia" );
				adp.UpdateCommand.Parameters.Add("@strCod_Agencia_P", SqlDbType.Char, 6, "Cod_Agencia" );
				adp.UpdateCommand.Parameters.Add("@strCod_Cliente_P", SqlDbType.Char, 6, "Cod_Cliente" );
				adp.UpdateCommand.Parameters.Add("@strCod_Intermediario_P", SqlDbType.Char, 6, "Cod_Intermediario" );
				adp.UpdateCommand.Parameters.Add("@strCod_Nucleo_P", SqlDbType.Char, 7, "Cod_Nucleo" );
				adp.UpdateCommand.Parameters.Add("@strCod_Contato_P", SqlDbType.Char, 5, "Cod_Contato" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Grade_P", SqlDbType.TinyInt, 1, "Indica_Grade" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Midia_P", SqlDbType.TinyInt, 1, "Indica_Midia" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Desvinculado_P", SqlDbType.TinyInt, 1, "Indica_Desvinculado" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Cancelado_P", SqlDbType.TinyInt, 1, "Indica_Cancelado" );
				adp.UpdateCommand.Parameters.Add("@strMotivo_Cancelamento_P", SqlDbType.VarChar, 50, "Motivo_Cancelamento" );
				adp.UpdateCommand.Parameters.Add("@strCod_Programa_Patrocinado_P", SqlDbType.Char, 4, "Cod_Programa_Patrocinado" );
				adp.UpdateCommand.Parameters.Add("@decComissao_Agencia_P", SqlDbType.Decimal, 16, "Comissao_Agencia" );
				adp.UpdateCommand.Parameters.Add("@decComissao_Intermediario_P", SqlDbType.Decimal, 16, "Comissao_Intermediario" );

				adp.Update( dtbMGI_Contrato );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContrato.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}