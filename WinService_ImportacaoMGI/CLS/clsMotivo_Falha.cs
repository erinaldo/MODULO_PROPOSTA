using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsMotivo_Falha.
	/// </summary>
	public class clsMotivo_Falha
	{
		public WinService_ImportacaoMGI2.XSD.Motivo_Falha dtsMotivo_Falha;
		public WinService_ImportacaoMGI2.XSD.Motivo_Falha.dtbMotivo_FalhaDataTable dtbMotivo_Falha;
		public WinService_ImportacaoMGI2.XSD.Motivo_Falha.dtbMGI_Motivo_FalhaDataTable dtbMGI_Motivo_Falha;

		public clsMotivo_Falha()
		{
			dtsMotivo_Falha = new WinService_ImportacaoMGI2.XSD.Motivo_Falha();
			dtbMotivo_Falha = dtsMotivo_Falha.dtbMotivo_Falha;
			dtbMGI_Motivo_Falha = dtsMotivo_Falha.dtbMGI_Motivo_Falha;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Motivo_Falha )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Motivo_Falha_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Motivo_Falha", strCod_Motivo_Falha );

				adp.Fill(dtbMGI_Motivo_Falha);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMotivo_Falha.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Motivo_Falha.dtbMotivo_FalhaRow drwNew in dtbMotivo_Falha.Rows )
				{
					if ( dtbMGI_Motivo_Falha.Select("Cod_Motivo_Falha = '" + drwNew.Cod_Motivo_Falha + "'").Length == 0 )
						dtbMGI_Motivo_Falha.AdddtbMGI_Motivo_FalhaRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Motivo_Falha, drwNew.Descricao );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Motivo_Falha.dtbMGI_Motivo_FalhaRow drwOld in dtbMGI_Motivo_Falha.Select("Cod_Motivo_Falha = '" + drwNew.Cod_Motivo_Falha + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMotivo_Falha.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Motivo_Falha_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Motivo_Falha_P", SqlDbType.Char, 3, "Cod_Motivo_Falha" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Motivo_Falha_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Motivo_Falha_P", SqlDbType.Char, 3, "Cod_Motivo_Falha" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Motivo_Falha );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsMotivo_Falha.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}