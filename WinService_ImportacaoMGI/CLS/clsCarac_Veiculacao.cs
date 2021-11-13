using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsCarac_Veiculacao.
	/// </summary>
	public class clsCarac_Veiculacao
	{
		public WinService_ImportacaoMGI2.XSD.Carac_Veiculacao dtsCarac_Veiculacao;
		public WinService_ImportacaoMGI2.XSD.Carac_Veiculacao.dtbCarac_VeiculacaoDataTable dtbCarac_Veiculacao;
		public WinService_ImportacaoMGI2.XSD.Carac_Veiculacao.dtbMGI_Carac_VeiculacaoDataTable dtbMGI_Carac_Veiculacao;

		public clsCarac_Veiculacao()
		{
			dtsCarac_Veiculacao = new WinService_ImportacaoMGI2.XSD.Carac_Veiculacao();
			dtbCarac_Veiculacao = dtsCarac_Veiculacao.dtbCarac_Veiculacao;
			dtbMGI_Carac_Veiculacao = dtsCarac_Veiculacao.dtbMGI_Carac_Veiculacao;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Caracteristica )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Carac_Veiculacao_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Caracteristica", strCod_Caracteristica );

				adp.Fill(dtbMGI_Carac_Veiculacao);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsCarac_Veiculacao.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach(WinService_ImportacaoMGI2.XSD.Carac_Veiculacao.dtbCarac_VeiculacaoRow drwNew in dtbCarac_Veiculacao.Rows )
				{
					if ( dtbMGI_Carac_Veiculacao.Select("Cod_Caracteristica = '" + drwNew.Cod_Caracteristica + "'").Length == 0 )
						dtbMGI_Carac_Veiculacao.AdddtbMGI_Carac_VeiculacaoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Caracteristica, drwNew.Descricao);
					else
						foreach(WinService_ImportacaoMGI2.XSD.Carac_Veiculacao.dtbMGI_Carac_VeiculacaoRow drwOld in dtbMGI_Carac_Veiculacao.Select("Cod_Caracteristica = '" + drwNew.Cod_Caracteristica + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsCarac_Veiculacao.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Carac_Veiculacao_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Caracteristica_P", SqlDbType.Char, 2, "Cod_Caracteristica" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 50, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Carac_Veiculacao_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Caracteristica_P", SqlDbType.Char, 2, "Cod_Caracteristica" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 50, "Descricao" );

				adp.Update( dtbMGI_Carac_Veiculacao );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsCarac_Veiculacao.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}