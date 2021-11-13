using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsPrograma.
	/// </summary>
	public class clsPrograma
	{
		public WinService_ImportacaoMGI2.XSD.Programa dtsPrograma;
		public WinService_ImportacaoMGI2.XSD.Programa.dtbProgramaDataTable dtbPrograma;
		public WinService_ImportacaoMGI2.XSD.Programa.dtbMGI_ProgramaDataTable dtbMGI_Programa;

		public clsPrograma()
		{
			dtsPrograma = new WinService_ImportacaoMGI2.XSD.Programa();
			dtbPrograma = dtsPrograma.dtbPrograma;
			dtbMGI_Programa = dtsPrograma.dtbMGI_Programa;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Programa )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Programa_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Programa", strCod_Programa );

				adp.Fill(dtbMGI_Programa);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsPrograma.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Programa.dtbProgramaRow drwNew in dtbPrograma.Rows )
				{
					if ( dtbMGI_Programa.Select("Cod_Programa = '" + drwNew.Cod_Programa + "'").Length == 0 )
						dtbMGI_Programa.AdddtbMGI_ProgramaRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Programa, 
							drwNew.Cod_Genero,
							drwNew.Titulo,
							drwNew.Indica_Desativado,
							drwNew.Indica_Evento,
							drwNew.Indica_Rotativo,
							drwNew.Indica_Local);
					else
						foreach( WinService_ImportacaoMGI2.XSD.Programa.dtbMGI_ProgramaRow drwOld in dtbMGI_Programa.Select("Cod_Programa = '" + drwNew.Cod_Programa + "'") )
							if( drwNew.Titulo != drwOld.Titulo 
								|| drwNew.Cod_Genero 		!= drwOld.Cod_Genero 
								|| drwNew.Indica_Desativado != drwOld.Indica_Desativado 
								|| drwNew.Indica_Evento 	!= drwOld.Indica_Programa_Evento 
								|| drwNew.Indica_Local 		!= drwOld.Indica_Programa_Local 
								|| drwNew.Indica_Rotativo 	!= drwOld.Indica_Programa_Rotativo )
							{
								drwOld.Cod_Genero = drwNew.Cod_Genero;
								drwOld.Titulo = drwNew.Titulo;
								drwOld.Indica_Desativado = drwNew.Indica_Desativado;
								drwOld.Indica_Programa_Evento = drwNew.Indica_Evento;
								drwOld.Indica_Programa_Local = drwNew.Indica_Local;
								drwOld.Indica_Programa_Rotativo = drwNew.Indica_Rotativo;
							}
				}
				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsPrograma.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Programa_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.InsertCommand.Parameters.Add("@strCod_Genero_P", SqlDbType.Char, 4, "Cod_Genero" );
				adp.InsertCommand.Parameters.Add("@strTitulo_P", SqlDbType.VarChar, 30, "Titulo" );
				adp.InsertCommand.Parameters.Add("@bitIndica_Desativado_P", SqlDbType.Bit, 1, "Indica_Desativado" );
				adp.InsertCommand.Parameters.Add("@bitIndica_Programa_Evento_P", SqlDbType.Bit, 1, "Indica_Programa_Evento" );
				adp.InsertCommand.Parameters.Add("@bitIndica_Programa_Rotativo_P", SqlDbType.Bit, 1, "Indica_Programa_Rotativo" );
				adp.InsertCommand.Parameters.Add("@bitIndica_Programa_Local_P", SqlDbType.Bit, 1, "Indica_Programa_Local" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Programa_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.UpdateCommand.Parameters.Add("@strCod_Genero_P", SqlDbType.Char, 4, "Cod_Genero" );
				adp.UpdateCommand.Parameters.Add("@strTitulo_P", SqlDbType.VarChar, 30, "Titulo" );
				adp.UpdateCommand.Parameters.Add("@bitIndica_Desativado_P", SqlDbType.Bit, 1, "Indica_Desativado" );
				adp.UpdateCommand.Parameters.Add("@bitIndica_Programa_Evento_P", SqlDbType.Bit, 1, "Indica_Programa_Evento" );
				adp.UpdateCommand.Parameters.Add("@bitIndica_Programa_Rotativo_P", SqlDbType.Bit, 1, "Indica_Programa_Rotativo" );
				adp.UpdateCommand.Parameters.Add("@bitIndica_Programa_Local_P", SqlDbType.Bit, 1, "Indica_Programa_Local" );

				adp.Update( dtbMGI_Programa );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsPrograma.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}