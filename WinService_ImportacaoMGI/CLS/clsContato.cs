using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsContato.
	/// </summary>
	public class clsContato
	{
		public WinService_ImportacaoMGI2.XSD.Contato dtsContato;
		public WinService_ImportacaoMGI2.XSD.Contato.dtbContatoDataTable dtbContato;
		public WinService_ImportacaoMGI2.XSD.Contato.dtbContato_EmpresaDataTable dtbContato_Empresa;

		public WinService_ImportacaoMGI2.XSD.Contato.dtbMGI_ContatoDataTable dtbMGI_Contato;
		public WinService_ImportacaoMGI2.XSD.Contato.dtbMGI_Contato_EmpresaDataTable dtbMGI_Contato_Empresa;

		public clsContato()
		{
			dtsContato = new WinService_ImportacaoMGI2.XSD.Contato();
			dtbContato = dtsContato.dtbContato;
			dtbContato_Empresa = dtsContato.dtbContato_Empresa;

			dtbMGI_Contato = dtsContato.dtbMGI_Contato;
			dtbMGI_Contato_Empresa = dtsContato.dtbMGI_Contato_Empresa;
		}


		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Contato )
		{
			this.sprCarregarContato( cnn, strCod_Contato );
			this.sprCarregarContato_Empresa( cnn, strCod_Contato );
		}

		#endregion

		#region Carregar Contato

		private void sprCarregarContato( SqlConnection cnn, string strCod_Contato )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Contato_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Contato", strCod_Contato );

				adp.Fill(dtbMGI_Contato);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContato.sprCarregarContato() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Carregar Contato Empresa

		private void sprCarregarContato_Empresa( SqlConnection cnn, string strCod_Contato )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Contato_Empresa_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Contato", strCod_Contato );

				adp.Fill(dtbMGI_Contato_Empresa);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContato.sprCarregarContato_Empresa() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Contato.dtbContatoRow drwNew in dtbContato.Rows )
					if ( dtbMGI_Contato.Select("Cod_Contato = '" + drwNew.Cod_Contato + "'").Length == 0 )
						dtbMGI_Contato.AdddtbMGI_ContatoRow( int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Contato, 
							drwNew.Nome, 
							( drwNew.IsCGCNull() ? "": drwNew.CGC ),
							! drwNew.IsData_DesativacaoNull());
					else
						foreach( WinService_ImportacaoMGI2.XSD.Contato.dtbMGI_ContatoRow drwOld in dtbMGI_Contato.Select("Cod_Contato = '" + drwNew.Cod_Contato + "'") )
							if( drwNew.Nome != drwOld.Nome
								|| ( drwNew.IsCGCNull() ? "": drwNew.CGC ) != drwOld.CPF_CNPJ
								|| drwNew.IsData_DesativacaoNull() == drwOld.Indica_Desativado )
							{
								drwOld.Nome = drwNew.Nome;
								drwOld.CPF_CNPJ = ( drwNew.IsCGCNull() ? "": drwNew.CGC );
								drwOld.Indica_Desativado = ! drwNew.IsData_DesativacaoNull();
							}
				
				// Remove todos os relacionamentos inexistentes entre contato e empresa
//				foreach( WinService_ImportacaoMGI2.XSD.Contato.dtbMGI_Contato_EmpresaRow drwOld in dtbMGI_Contato_Empresa.Rows )
//					if ( dtbContato_Empresa.Select("Cod_Contato = '" + drwOld.Cod_Contato + "' and Cod_Empresa = '" + drwOld.Cod_Empresa + "'").Length == 0 )
//						drwOld.Delete();

				// Insere todos os novos relacionamentos entre contato e empresa
				foreach( WinService_ImportacaoMGI2.XSD.Contato.dtbContato_EmpresaRow drwNew in dtbContato_Empresa.Rows )
					if ( dtbMGI_Contato_Empresa.Select("Cod_Contato = '" + drwNew.Cod_Contato + "' and Cod_Empresa = '" + drwNew.Cod_Empresa + "'").Length == 0 )
						dtbMGI_Contato_Empresa.AdddtbMGI_Contato_EmpresaRow( int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Contato,
							drwNew.Cod_Empresa );

				this.sprAtualizarContato( cnn);
				this.sprAtualizarContato_Empresa( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContato.spuImportar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Atualizar Contato

		private void sprAtualizarContato( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Contato_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Contato_P", SqlDbType.Char, 5, "Cod_Contato" );
				adp.InsertCommand.Parameters.Add("@strNome_P", SqlDbType.VarChar, 50, "Nome" );
				adp.InsertCommand.Parameters.Add("@strCPF_CNPJ_P", SqlDbType.Char, 18, "CPF_CNPJ" );
				adp.InsertCommand.Parameters.Add("@bitIndica_Desativado_P", SqlDbType.Bit, 1, "Indica_Desativado" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Contato_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Contato_P", SqlDbType.Char, 5, "Cod_Contato" );
				adp.UpdateCommand.Parameters.Add("@strNome_P", SqlDbType.VarChar, 50, "Nome" );
				adp.UpdateCommand.Parameters.Add("@strCPF_CNPJ_P", SqlDbType.Char, 18, "CPF_CNPJ" );
				adp.UpdateCommand.Parameters.Add("@bitIndica_Desativado_P", SqlDbType.Bit, 1, "Indica_Desativado" );

				adp.Update( dtbMGI_Contato );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContato.sprAtualizarContato() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Atualizar Contato Empresa

		private void sprAtualizarContato_Empresa( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Contato_Empresa_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Contato_P", SqlDbType.Char, 5, "Cod_Contato" );
				adp.InsertCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );

				adp.DeleteCommand = (new SqlCommand("pr_MGI_Contato_Empresa_D"));
				adp.DeleteCommand.CommandType = CommandType.StoredProcedure;
				adp.DeleteCommand.Connection = cnn;
				//adp.DeleteCommand.Transaction = tran;
				adp.DeleteCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.DeleteCommand.Parameters.Add("@strCod_Contato_P", SqlDbType.Char, 5, "Cod_Contato" );
				adp.DeleteCommand.Parameters.Add("@strCod_Empresa_P", SqlDbType.Char, 3, "Cod_Empresa" );

				adp.Update( dtbMGI_Contato_Empresa );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsContato.sprAtualizarContato_Empresa() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}