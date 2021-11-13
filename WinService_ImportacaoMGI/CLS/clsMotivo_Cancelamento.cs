using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsMotivo_Cancelamento.
	/// </summary>
	public class clsMotivo_Cancelamento
	{
		public WinService_ImportacaoMGI2.XSD.Motivo_Cancelamento dtsMotivo_Cancelamento;
		public WinService_ImportacaoMGI2.XSD.Motivo_Cancelamento.dtbMotivo_CancelamentoDataTable dtbMotivo_Cancelamento;
		public WinService_ImportacaoMGI2.XSD.Motivo_Cancelamento.dtbMGI_Motivo_CancelamentoDataTable dtbMGI_Motivo_Cancelamento;

		public clsMotivo_Cancelamento()
		{
			dtsMotivo_Cancelamento = new WinService_ImportacaoMGI2.XSD.Motivo_Cancelamento();
			dtbMotivo_Cancelamento = dtsMotivo_Cancelamento.dtbMotivo_Cancelamento;
			dtbMGI_Motivo_Cancelamento = dtsMotivo_Cancelamento.dtbMGI_Motivo_Cancelamento;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Cancelamento )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Motivo_Cancelamento_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Cancelamento", strCod_Cancelamento );

				adp.Fill(dtbMGI_Motivo_Cancelamento);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMotivo_Cancelamento.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Motivo_Cancelamento.dtbMotivo_CancelamentoRow drwNew in dtbMotivo_Cancelamento.Rows )
				{
					if ( dtbMGI_Motivo_Cancelamento.Select("Cod_Cancelamento = '" + drwNew.Cod_Cancelamento + "'").Length == 0 )
						dtbMGI_Motivo_Cancelamento.AdddtbMGI_Motivo_CancelamentoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Cancelamento, drwNew.Descricao );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Motivo_Cancelamento.dtbMGI_Motivo_CancelamentoRow drwOld in dtbMGI_Motivo_Cancelamento.Select("Cod_Cancelamento = '" + drwNew.Cod_Cancelamento + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMotivo_Cancelamento.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Motivo_Cancelamento_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Cancelamento_P", SqlDbType.Char, 3, "Cod_Cancelamento" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Motivo_Cancelamento_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Cancelamento_P", SqlDbType.Char, 3, "Cod_Cancelamento" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Motivo_Cancelamento );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMotivo_Cancelamento.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}