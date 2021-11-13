using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsVeiculo.
	/// </summary>
	public class clsVeiculo
	{
		public WinService_ImportacaoMGI2.XSD.Veiculo dtsVeiculo;
		public WinService_ImportacaoMGI2.XSD.Veiculo.dtbVeiculoDataTable dtbVeiculo;
		public WinService_ImportacaoMGI2.XSD.Veiculo.dtbMGI_VeiculoDataTable dtbMGI_Veiculo;

		public clsVeiculo()
		{
			dtsVeiculo = new WinService_ImportacaoMGI2.XSD.Veiculo();
			dtbVeiculo = dtsVeiculo.dtbVeiculo;
			dtbMGI_Veiculo = dtsVeiculo.dtbMGI_Veiculo;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Veiculo )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Veiculo_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Veiculo", strCod_Veiculo );

				adp.Fill(dtbMGI_Veiculo);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculo.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Veiculo.dtbVeiculoRow drwNew in dtbVeiculo.Rows )
					if ( dtbMGI_Veiculo.Select("Cod_Veiculo = '" + drwNew.Cod_Veiculo + "'").Length == 0 )
						dtbMGI_Veiculo.AdddtbMGI_VeiculoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Veiculo,
							drwNew.Cod_Empresa, 
							drwNew.Nome_Veiculo );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Veiculo.dtbMGI_VeiculoRow drwOld in dtbMGI_Veiculo.Select("Cod_Veiculo = '" + drwNew.Cod_Veiculo + "'") )
							if( drwNew.Nome_Veiculo != drwOld.Nome_Veiculo
								|| drwNew.Cod_Empresa != drwOld.Cod_Empresa )
							{
								drwOld.Cod_Empresa = drwNew.Cod_Empresa;
								drwOld.Nome_Veiculo = drwNew.Nome_Veiculo;
							}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculo.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Veiculo_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
				adp.InsertCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.InsertCommand.Parameters.Add("@strNome_Veiculo_P", SqlDbType.VarChar, 30, "Nome_Veiculo" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Veiculo_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
				adp.UpdateCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.UpdateCommand.Parameters.Add("@strNome_Veiculo_P", SqlDbType.VarChar, 30, "Nome_Veiculo" );

				adp.Update( dtbMGI_Veiculo );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsVeiculo.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}