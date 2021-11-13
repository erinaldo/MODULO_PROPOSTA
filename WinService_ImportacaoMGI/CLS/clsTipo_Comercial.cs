using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsTipo_Comercial.
	/// </summary>
	public class clsTipo_Comercial
	{
		public WinService_ImportacaoMGI2.XSD.Tipo_Comercial dtsTipo_Comercial;
		public WinService_ImportacaoMGI2.XSD.Tipo_Comercial.dtbTipo_ComercialDataTable dtbTipo_Comercial;
		public WinService_ImportacaoMGI2.XSD.Tipo_Comercial.dtbMGI_Tipo_ComercialDataTable dtbMGI_Tipo_Comercial;

		public clsTipo_Comercial()
		{
			dtsTipo_Comercial = new WinService_ImportacaoMGI2.XSD.Tipo_Comercial();
			dtbTipo_Comercial = dtsTipo_Comercial.dtbTipo_Comercial;
			dtbMGI_Tipo_Comercial = dtsTipo_Comercial.dtbMGI_Tipo_Comercial;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Tipo_Comercial )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Tipo_Comercial_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Tipo_Comercial", strCod_Tipo_Comercial );

				adp.Fill(dtbMGI_Tipo_Comercial);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTipo_Comercial.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Tipo_Comercial.dtbTipo_ComercialRow drwNew in dtbTipo_Comercial.Rows )
				{
					if ( dtbMGI_Tipo_Comercial.Select("Cod_Tipo_Comercial = '" + drwNew.Cod_Tipo_Comercial + "'").Length == 0 )
						dtbMGI_Tipo_Comercial.AdddtbMGI_Tipo_ComercialRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Tipo_Comercial, drwNew.Descricao );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Tipo_Comercial.dtbMGI_Tipo_ComercialRow drwOld in dtbMGI_Tipo_Comercial.Select("Cod_Tipo_Comercial = '" + drwNew.Cod_Tipo_Comercial + "'") )
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTipo_Comercial.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Tipo_Comercial_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Tipo_Comercial_P", SqlDbType.Char, 2, "Cod_Tipo_Comercial" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Tipo_Comercial_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Tipo_Comercial_P", SqlDbType.Char, 2, "Cod_Tipo_Comercial" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 30, "Descricao" );

				adp.Update( dtbMGI_Tipo_Comercial );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTipo_Comercial.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}