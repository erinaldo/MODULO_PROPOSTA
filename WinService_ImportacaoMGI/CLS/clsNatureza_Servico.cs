using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsNatureza_Servico.
	/// </summary>
	public class clsNatureza_Servico
	{
		public WinService_ImportacaoMGI2.XSD.Natureza_Servico dtsNatureza_Servico;
		public WinService_ImportacaoMGI2.XSD.Natureza_Servico.dtbNatureza_ServicoDataTable dtbNatureza_Servico;
		public WinService_ImportacaoMGI2.XSD.Natureza_Servico.dtbMGI_Natureza_ServicoDataTable dtbMGI_Natureza_Servico;

		public clsNatureza_Servico()
		{
			dtsNatureza_Servico = new WinService_ImportacaoMGI2.XSD.Natureza_Servico();
			dtbNatureza_Servico = dtsNatureza_Servico.dtbNatureza_Servico;
			dtbMGI_Natureza_Servico = dtsNatureza_Servico.dtbMGI_Natureza_Servico;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Natureza )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Natureza_Servico_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Natureza", strCod_Natureza );

				adp.Fill(dtbMGI_Natureza_Servico);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsNatureza_Servico.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Natureza_Servico.dtbNatureza_ServicoRow drwNew in dtbNatureza_Servico.Rows )
				{
					if ( dtbMGI_Natureza_Servico.Select("Cod_Natureza = '" + drwNew.Cod_Natureza + "' and Cod_Empresa = '" + drwNew.Cod_Empresa + "'" ).Length == 0 )
						dtbMGI_Natureza_Servico.AdddtbMGI_Natureza_ServicoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Empresa,
							drwNew.Cod_Natureza,
							drwNew.Descricao );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Natureza_Servico.dtbMGI_Natureza_ServicoRow drwOld in dtbMGI_Natureza_Servico.Select("Cod_Natureza = '" + drwNew.Cod_Natureza + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsNatureza_Servico.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Natureza_Servico_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.InsertCommand.Parameters.Add("@strCod_Natureza_P", SqlDbType.Char, 6, "Cod_Natureza" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Natureza_Servico_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.UpdateCommand.Parameters.Add("@strCod_Natureza_P", SqlDbType.Char, 6, "Cod_Natureza" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Natureza_Servico );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsNatureza_Servico.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}