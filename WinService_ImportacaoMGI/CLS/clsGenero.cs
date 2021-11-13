using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsGenero.
	/// </summary>
	public class clsGenero
	{
		public WinService_ImportacaoMGI2.XSD.Genero dtsGenero;
		public WinService_ImportacaoMGI2.XSD.Genero.dtbGeneroDataTable dtbGenero;
		public WinService_ImportacaoMGI2.XSD.Genero.dtbMGI_GeneroDataTable dtbMGI_Genero;

		public clsGenero()
		{
			dtsGenero = new WinService_ImportacaoMGI2.XSD.Genero();
			dtbGenero = dtsGenero.dtbGenero;
			dtbMGI_Genero = dtsGenero.dtbMGI_Genero;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Genero )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Genero_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Genero", strCod_Genero );

				adp.Fill(dtbMGI_Genero);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsGenero.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Genero.dtbGeneroRow drwNew in dtbGenero.Rows )
				{
					if ( dtbMGI_Genero.Select("Cod_Genero = '" + drwNew.Cod_Genero + "'").Length == 0 )
						dtbMGI_Genero.AdddtbMGI_GeneroRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Genero, drwNew.Descricao );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Genero.dtbMGI_GeneroRow drwOld in dtbMGI_Genero.Select("Cod_Genero = '" + drwNew.Cod_Genero + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsGenero.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Genero_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Genero_P", SqlDbType.Char, 4, "Cod_Genero" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Genero_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Genero_P", SqlDbType.Char, 4, "Cod_Genero" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Genero );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsGenero.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}