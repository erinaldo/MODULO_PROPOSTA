using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsNucleo.
	/// </summary>
	public class clsNucleo
	{
		public WinService_ImportacaoMGI2.XSD.Nucleo dtsNucleo;
		public WinService_ImportacaoMGI2.XSD.Nucleo.dtbNucleoDataTable dtbNucleo;
		public WinService_ImportacaoMGI2.XSD.Nucleo.dtbMGI_NucleoDataTable dtbMGI_Nucleo;

		public clsNucleo()
		{
			dtsNucleo = new WinService_ImportacaoMGI2.XSD.Nucleo();
			dtbNucleo = dtsNucleo.dtbNucleo;
			dtbMGI_Nucleo = dtsNucleo.dtbMGI_Nucleo;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Nucleo )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Nucleo_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Nucleo", strCod_Nucleo );

				adp.Fill(dtbMGI_Nucleo);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsNucleo.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Nucleo.dtbNucleoRow drwNew in dtbNucleo.Rows )
				{
					if ( dtbMGI_Nucleo.Select("Cod_Nucleo = '" + drwNew.Cod_Nucleo + "'").Length == 0 )
						dtbMGI_Nucleo.AdddtbMGI_NucleoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Nucleo, drwNew.Descricao );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Nucleo.dtbMGI_NucleoRow drwOld in dtbMGI_Nucleo.Select("Cod_Nucleo = '" + drwNew.Cod_Nucleo + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsNucleo.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Nucleo_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Nucleo_P", SqlDbType.Char, 7, "Cod_Nucleo" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Nucleo_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Nucleo_P", SqlDbType.Char, 7, "Cod_Nucleo" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Nucleo );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsNucleo.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}