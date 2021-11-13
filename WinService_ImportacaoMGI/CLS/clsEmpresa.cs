using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsEmpresa.
	/// </summary>
	public class clsEmpresa
	{
		public WinService_ImportacaoMGI2.XSD.Empresa dtsEmpresa;
		public WinService_ImportacaoMGI2.XSD.Empresa.dtbEmpresaDataTable dtbEmpresa;
		public WinService_ImportacaoMGI2.XSD.Empresa.dtbMGI_EmpresaDataTable dtbMGI_Empresa;

		public clsEmpresa()
		{
			dtsEmpresa = new WinService_ImportacaoMGI2.XSD.Empresa();
			dtbEmpresa = dtsEmpresa.dtbEmpresa;
			dtbMGI_Empresa = dtsEmpresa.dtbMGI_Empresa;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Empresa )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Empresa_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Empresa", strCod_Empresa );

				adp.Fill(dtbMGI_Empresa);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsEmpresa.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Empresa.dtbEmpresaRow drwNew in dtbEmpresa.Rows )
				{
					if ( dtbMGI_Empresa.Select("Cod_Empresa = '" + drwNew.Cod_Empresa + "'").Length == 0 )
						dtbMGI_Empresa.AdddtbMGI_EmpresaRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Empresa,drwNew.Razao_Social );
					else
						foreach( WinService_ImportacaoMGI2.XSD.Empresa.dtbMGI_EmpresaRow drwOld in dtbMGI_Empresa.Select("Cod_Empresa = '" + drwNew.Cod_Empresa + "'") )
							if( drwNew.Razao_Social != drwOld.Razao_Social )
								drwOld.Razao_Social = drwNew.Razao_Social;
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsEmpresa.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Empresa_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.InsertCommand.Parameters.Add("@strRazao_Social_P", SqlDbType.VarChar, 50, "Razao_Social" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Empresa_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );
				adp.UpdateCommand.Parameters.Add("@strRazao_Social_P", SqlDbType.VarChar, 50, "Razao_Social" );

				adp.Update( dtbMGI_Empresa );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsEmpresa.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}