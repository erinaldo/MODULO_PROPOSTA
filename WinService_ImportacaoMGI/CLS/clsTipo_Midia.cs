using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsTipo_Midia.
	/// </summary>
	public class clsTipo_Midia
	{
		public WinService_ImportacaoMGI2.XSD.Tipo_Midia dtsTipo_Midia;
		public WinService_ImportacaoMGI2.XSD.Tipo_Midia.dtbTipo_MidiaDataTable dtbTipo_Midia;
		public WinService_ImportacaoMGI2.XSD.Tipo_Midia.dtbMGI_Tipo_MidiaDataTable dtbMGI_Tipo_Midia;

		public clsTipo_Midia()
		{
			dtsTipo_Midia = new WinService_ImportacaoMGI2.XSD.Tipo_Midia();
			dtbTipo_Midia = dtsTipo_Midia.dtbTipo_Midia;
			dtbMGI_Tipo_Midia = dtsTipo_Midia.dtbMGI_Tipo_Midia;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Tipo_Midia )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Tipo_Midia_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Tipo_Midia", strCod_Tipo_Midia );

				adp.Fill(dtbMGI_Tipo_Midia);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTipo_Midia.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach(WinService_ImportacaoMGI2.XSD.Tipo_Midia.dtbTipo_MidiaRow drwNew in dtbTipo_Midia.Rows )
				{
					if ( dtbMGI_Tipo_Midia.Select("Cod_Tipo_Midia = '" + drwNew.Cod_Tipo_Midia + "'").Length == 0 )
						dtbMGI_Tipo_Midia.AdddtbMGI_Tipo_MidiaRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Tipo_Midia, drwNew.Descricao );
					else
						foreach(WinService_ImportacaoMGI2.XSD.Tipo_Midia.dtbMGI_Tipo_MidiaRow drwOld in dtbMGI_Tipo_Midia.Select("Cod_Tipo_Midia = '" + drwNew.Cod_Tipo_Midia + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTipo_Midia.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Tipo_Midia_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Tipo_Midia_P", SqlDbType.Char, 6, "Cod_Tipo_Midia" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Tipo_Midia_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Tipo_Midia_P", SqlDbType.Char, 6, "Cod_Tipo_Midia" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Tipo_Midia );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTipo_Midia.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}