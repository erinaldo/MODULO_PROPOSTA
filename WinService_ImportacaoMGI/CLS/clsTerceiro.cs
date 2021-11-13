using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsTerceiro.
	/// </summary>
	public class clsTerceiro
	{
		public WinService_ImportacaoMGI2.XSD.Terceiro dtsTerceiro;

		public WinService_ImportacaoMGI2.XSD.Terceiro.dtbTerceiroDataTable dtbTerceiro;
		public WinService_ImportacaoMGI2.XSD.Terceiro.dtbTerceiro_FuncaoDataTable dtbTerceiro_Funcao;
		public WinService_ImportacaoMGI2.XSD.Terceiro.dtbTerceiro_EnderecoDataTable dtbTerceiro_Endereco;

		public WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_TerceiroDataTable dtbMGI_Terceiro;
		public WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_Terceiro_FuncaoDataTable dtbMGI_Terceiro_Funcao;
		public WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_Terceiro_EnderecoDataTable dtbMGI_Terceiro_Endereco;

		public clsTerceiro()
		{
			dtsTerceiro = new WinService_ImportacaoMGI2.XSD.Terceiro();

			dtbTerceiro = dtsTerceiro.dtbTerceiro;
			dtbTerceiro_Funcao = dtsTerceiro.dtbTerceiro_Funcao; 
			dtbTerceiro_Endereco = dtsTerceiro.dtbTerceiro_Endereco; 

			dtbMGI_Terceiro = dtsTerceiro.dtbMGI_Terceiro;
			dtbMGI_Terceiro_Funcao = dtsTerceiro.dtbMGI_Terceiro_Funcao; 
			dtbMGI_Terceiro_Endereco = dtsTerceiro.dtbMGI_Terceiro_Endereco; 
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Terceiro )
		{
			try
			{
				this.sprCarregarTerceiro_Funcao( cnn );
				this.sprCarregarTerceiro( cnn, strCod_Terceiro );
				this.sprCarregarTerceiro_Endereco(cnn, strCod_Terceiro );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.spuCarregar() " + ex.Message.ToString() );
			}
		}

		private void sprCarregarTerceiro_Funcao( SqlConnection cnn )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Terceiro_Funcao_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;

				adp.Fill( dtbMGI_Terceiro_Funcao );

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprCarregarTerceiro_Funcao() " + ex.Message.ToString() );
			}
		}

		private void sprCarregarTerceiro( SqlConnection cnn, string strCod_Terceiro )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Terceiro_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				adp.SelectCommand.Parameters.AddWithValue("@strCod_Terceiro_P", strCod_Terceiro );

				adp.Fill( dtbMGI_Terceiro );

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprCarregarTerceiro() " + ex.Message.ToString() );
			}
		}

		private void sprCarregarTerceiro_Endereco( SqlConnection cnn, string strCod_Terceiro )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Terceiro_Endereco_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				adp.SelectCommand.Parameters.AddWithValue("@strCod_Terceiro_P", strCod_Terceiro );

				adp.Fill( dtbMGI_Terceiro_Endereco );

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprCarregarTerceiro_Endereco() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				// Terceiro Funcao
				foreach( WinService_ImportacaoMGI2.XSD.Terceiro.dtbTerceiro_FuncaoRow drwNew in dtbTerceiro_Funcao.Rows )
				{
					if ( dtbMGI_Terceiro_Funcao.Select("Cod_Funcao = " + drwNew.Cod_Funcao.ToString() ).Length == 0 )
						dtbMGI_Terceiro_Funcao.AdddtbMGI_Terceiro_FuncaoRow(int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Funcao,
							drwNew.Descricao);
					else
						foreach( WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_Terceiro_FuncaoRow drwOld in dtbMGI_Terceiro_Funcao.Select("Cod_Funcao = " + drwNew.Cod_Funcao.ToString()) )
						{
							if( drwNew.Descricao != drwOld.Descricao )
								drwOld.Descricao = drwNew.Descricao;
						}
				}

				// Terceiro 
				foreach( WinService_ImportacaoMGI2.XSD.Terceiro.dtbTerceiroRow drwNew in dtbTerceiro.Rows )
				{
					if ( dtbMGI_Terceiro.Select("Cod_Terceiro = '"  + drwNew.Cod_Terceiro.Replace("'","''") + "'").Length == 0 )
					{
						WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_TerceiroRow drwMGI;
						drwMGI = dtbMGI_Terceiro.NewdtbMGI_TerceiroRow();
 
						drwMGI.Origem = int.Parse( clsParametro.getParametro("Origem_MGI") );
						drwMGI.Cod_Terceiro = drwNew.Cod_Terceiro;
						drwMGI.Razao_Social = drwNew.Razao_Social;
						drwMGI.Cod_Funcao = drwNew.Cod_Funcao;
						drwMGI.Nome_Fantasia = drwNew.IsNome_FantasiaNull() ? "" : drwNew.Nome_Fantasia;
						drwMGI.CPF_CNPJ = drwNew.CPF_CNPJ;
						drwMGI.Inscricao_Estadual = drwNew.IsInscricao_EstadualNull() ? "" : drwNew.Inscricao_Estadual;
						drwMGI.Inscricao_Municipal = drwNew.IsInscricao_MunicipalNull() ? "" : drwNew.Inscricao_Municipal;

						dtbMGI_Terceiro.AdddtbMGI_TerceiroRow( drwMGI );
					}
					else
						foreach( WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_TerceiroRow drwOld in dtbMGI_Terceiro.Select("Cod_Terceiro = '" + drwNew.Cod_Terceiro.Replace("'", "''") + "'") )
						{
							if( drwNew.Razao_Social != drwOld.Razao_Social
								|| drwNew.Cod_Funcao != drwOld.Cod_Funcao 
								|| ( drwNew.IsNome_FantasiaNull() ? "" : drwNew.Nome_Fantasia  ) != ( drwOld.IsNome_FantasiaNull() ? "" : drwOld.Nome_Fantasia )
								|| drwNew.CPF_CNPJ != drwOld.CPF_CNPJ 
								|| ( drwNew.IsInscricao_EstadualNull() ? "" : drwNew.Inscricao_Estadual ) != ( drwOld.IsInscricao_EstadualNull() ? "" : drwOld.Inscricao_Estadual )
								|| ( drwNew.IsInscricao_MunicipalNull() ? "" : drwNew.Inscricao_Municipal ) != ( drwOld.IsInscricao_MunicipalNull() ? "" : drwOld.Inscricao_Municipal ) )
							{
								drwOld.Razao_Social = drwNew.IsRazao_SocialNull() ? "" : drwNew.Razao_Social;
								drwOld.Cod_Funcao = drwNew.IsCod_FuncaoNull() ? 0 : drwNew.Cod_Funcao;
								drwOld.Nome_Fantasia = drwNew.IsNome_FantasiaNull() ? "" : drwNew.Nome_Fantasia;
								drwOld.CPF_CNPJ = drwNew.IsCPF_CNPJNull() ? "" : drwNew.CPF_CNPJ;
								drwOld.Inscricao_Estadual = drwNew.IsInscricao_EstadualNull() ? "" : drwNew.Inscricao_Estadual;
								drwOld.Inscricao_Municipal = drwNew.IsInscricao_MunicipalNull() ? "" : drwNew.Inscricao_Municipal;
							}
						}
				}

				// Terceiro Endereco
				foreach( WinService_ImportacaoMGI2.XSD.Terceiro.dtbTerceiro_EnderecoRow drwNew in dtbTerceiro_Endereco.Rows )
				{
					if ( dtbMGI_Terceiro_Endereco.Select("Tipo_Endereco = '" + drwNew.Tipo_Endereco + "' and Cod_Terceiro = '" + drwNew.Cod_Terceiro.Replace("'","''") + "'" ).Length == 0 )
					{
						WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_Terceiro_EnderecoRow drwMGI;
						drwMGI = dtbMGI_Terceiro_Endereco.NewdtbMGI_Terceiro_EnderecoRow();
 
						drwMGI.Origem = int.Parse( clsParametro.getParametro("Origem_MGI") );
						drwMGI.Cod_Terceiro = drwNew.Cod_Terceiro;
						drwMGI.Tipo_Endereco = drwNew.Tipo_Endereco;
						drwMGI.Endereco = drwNew.IsEnderecoNull() ? "" : drwNew.Endereco;
						drwMGI.Numero = drwNew.IsNumeroNull() ? "" : drwNew.Numero;
						drwMGI.Complemento = drwNew.IsComplementoNull() ? "" : drwNew.Complemento;
						drwMGI.Bairro = drwNew.IsBairroNull() ? "" : drwNew.Bairro;
						drwMGI.Municipio = drwNew.IsMunicipioNull() ? "" : drwNew.Municipio;
						drwMGI.Uf = drwNew.IsUFNull() ? "" : drwNew.UF;
						drwMGI.Cep = drwNew.IsCepNull() ? "" : drwNew.Cep;
						drwMGI.Telefone = drwNew.IsTelefoneNull() ? "" : drwNew.Telefone;
						drwMGI.Fax = drwNew.IsFaxNull() ? "" : drwNew.Fax;
						drwMGI.EMail = drwNew.IsEMailNull() ? "" : drwNew.EMail;

						dtbMGI_Terceiro_Endereco.AdddtbMGI_Terceiro_EnderecoRow( drwMGI );
					}
					else
						foreach( WinService_ImportacaoMGI2.XSD.Terceiro.dtbMGI_Terceiro_EnderecoRow drwOld in dtbMGI_Terceiro_Endereco.Select("Tipo_Endereco = '" + drwNew.Tipo_Endereco + "' and Cod_Terceiro = '" + drwNew.Cod_Terceiro.Replace("'","''") + "'") )
						{
							if( ( drwNew.IsEnderecoNull() ? "" : drwNew.Endereco ) != ( drwOld.IsEnderecoNull() ? "" : drwOld.Endereco )
								|| ( drwNew.IsNumeroNull() ? "" : drwNew.Numero ) != ( drwOld.IsNumeroNull() ? "" : drwOld.Numero )
								|| ( drwNew.IsComplementoNull() ? "" : drwNew.Complemento ) != ( drwOld.IsComplementoNull() ? "" : drwOld.Complemento )
								|| ( drwNew.IsBairroNull() ? "" : drwNew.Bairro ) != ( drwOld.IsBairroNull() ? "" : drwOld.Bairro )
								|| ( drwNew.IsMunicipioNull() ? "" : drwNew.Municipio ) != ( drwOld.IsMunicipioNull() ? "" : drwOld.Municipio )
								|| ( drwNew.IsUFNull() ? "" : drwNew.UF ) != ( drwOld.IsUfNull() ? "" : drwOld.Uf )
								|| ( drwNew.IsCepNull() ? "" : drwNew.Cep ) != ( drwOld.IsCepNull() ? "" : drwOld.Cep ) 
								|| ( drwNew.IsTelefoneNull() ? "" : drwNew.Telefone ) != ( drwOld.IsTelefoneNull() ? "" : drwOld.Telefone )
								|| ( drwNew.IsFaxNull() ? "" : drwNew.Fax ) != ( drwOld.IsFaxNull() ? "" : drwOld.Fax )
								|| ( drwNew.IsEMailNull() ? "" : drwNew.EMail ) != ( drwOld.IsEMailNull() ? "" : drwOld.EMail ) )
							{
//								drwOld.Endereco = drwNew.IsEMailNull() ? "" : drwNew.Endereco;
								drwOld.Endereco = drwNew.IsEnderecoNull() ? "" : drwNew.Endereco;
								drwOld.Numero = drwNew.IsNumeroNull() ? "" : drwNew.Numero;
								drwOld.Complemento = drwNew.IsComplementoNull() ? "" : drwNew.Complemento;
								drwOld.Bairro  = drwNew.IsBairroNull() ? "" : drwNew.Bairro;
								drwOld.Municipio  = drwNew.IsMunicipioNull() ? "" : drwNew.Municipio;
								drwOld.Uf = drwNew.IsUFNull() ? "" : drwNew.UF;
								drwOld.Cep  = drwNew.IsCepNull() ? "" : drwNew.Cep;
								drwOld.Telefone = drwNew.IsTelefoneNull() ? "" : drwNew.Telefone;
								drwOld.Fax = drwNew.IsFaxNull() ? "" : drwNew.Fax;
								drwOld.EMail  = drwNew.IsEMailNull() ? "" : drwNew.EMail;
							}
						}
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.spuImportar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Atualizar

		private void sprAtualizar( SqlConnection cnn)
		{
			try
			{
				this.sprAtualizarTerceiro_Funcao( cnn);
				this.sprAtualizarTerceiro( cnn);
				this.sprAtualizarTerceiro_Endereco( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		private void sprAtualizarTerceiro_Funcao( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Terceiro_Funcao_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@intCod_Funcao_P", SqlDbType.Int, 4, "Cod_Funcao" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 50, "Descricao" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Terceiro_Funcao_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@intCod_Funcao_P", SqlDbType.Int, 4, "Cod_Funcao" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 50, "Descricao" );

				adp.Update( dtbMGI_Terceiro_Funcao );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprAtualizarTerceiro_Funcao() " + ex.Message.ToString() );
			}
		}

		private void sprAtualizarTerceiro( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Terceiro_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Terceiro_P", SqlDbType.Char, 6, "Cod_Terceiro" );
				adp.InsertCommand.Parameters.Add("@strRazao_Social_P", SqlDbType.VarChar, 50, "Razao_Social" );
				adp.InsertCommand.Parameters.Add("@intCod_Funcao_P", SqlDbType.Int, 4, "Cod_Funcao" );
				adp.InsertCommand.Parameters.Add("@strNome_Fantasia_P", SqlDbType.VarChar, 50, "Nome_Fantasia" );
				adp.InsertCommand.Parameters.Add("@strCPF_CNPJ_P", SqlDbType.VarChar, 18, "CPF_CNPJ" );
				adp.InsertCommand.Parameters.Add("@strInscricao_Estadual_P", SqlDbType.VarChar, 15, "Inscricao_Estadual" );
				adp.InsertCommand.Parameters.Add("@strInscricao_Municipal_P", SqlDbType.VarChar, 15, "Inscricao_Municipal" );
				
				adp.UpdateCommand = (new SqlCommand("pr_MGI_Terceiro_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Terceiro_P", SqlDbType.Char, 6, "Cod_Terceiro" );
				adp.UpdateCommand.Parameters.Add("@strRazao_Social_P", SqlDbType.VarChar, 50, "Razao_Social" );
				adp.UpdateCommand.Parameters.Add("@intCod_Funcao_P", SqlDbType.Int, 4, "Cod_Funcao" );
				adp.UpdateCommand.Parameters.Add("@strNome_Fantasia_P", SqlDbType.VarChar, 50, "Nome_Fantasia" );
				adp.UpdateCommand.Parameters.Add("@strCPF_CNPJ_P", SqlDbType.VarChar, 18, "CPF_CNPJ" );
				adp.UpdateCommand.Parameters.Add("@strInscricao_Estadual_P", SqlDbType.VarChar, 15, "Inscricao_Estadual" );
				adp.UpdateCommand.Parameters.Add("@strInscricao_Municipal_P", SqlDbType.VarChar, 15, "Inscricao_Municipal" );

				adp.Update( dtbMGI_Terceiro );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprAtualizarTerceiro() " + ex.Message.ToString() );
			}
		}

		private void sprAtualizarTerceiro_Endereco( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Terceiro_Endereco_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Terceiro_P", SqlDbType.Char, 6, "Cod_Terceiro" );
				adp.InsertCommand.Parameters.Add("@strTipo_Endereco_P", SqlDbType.VarChar, 10, "Tipo_Endereco" );
				adp.InsertCommand.Parameters.Add("@strEndereco_P", SqlDbType.VarChar, 40, "Endereco" );
				adp.InsertCommand.Parameters.Add("@strNumero_P", SqlDbType.VarChar, 10, "Numero" );
				adp.InsertCommand.Parameters.Add("@strComplemento_P", SqlDbType.VarChar, 30, "Complemento" );
				adp.InsertCommand.Parameters.Add("@strBairro_P", SqlDbType.VarChar, 30, "Bairro" );
				adp.InsertCommand.Parameters.Add("@strMunicipio_P", SqlDbType.VarChar, 30, "Municipio" );
				adp.InsertCommand.Parameters.Add("@strUf_P", SqlDbType.Char, 2, "Uf" );
				adp.InsertCommand.Parameters.Add("@strCep_P", SqlDbType.VarChar, 9, "Cep" );
				adp.InsertCommand.Parameters.Add("@strTelefone_P", SqlDbType.VarChar, 20, "Telefone" );
				adp.InsertCommand.Parameters.Add("@strFax_P", SqlDbType.VarChar, 20, "Fax" );
				adp.InsertCommand.Parameters.Add("@strEMail_P", SqlDbType.VarChar, 40, "EMail" );


				adp.UpdateCommand = (new SqlCommand("pr_MGI_Terceiro_Endereco_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Terceiro_P", SqlDbType.Char, 6, "Cod_Terceiro" );
				adp.UpdateCommand.Parameters.Add("@strTipo_Endereco_P", SqlDbType.VarChar, 10, "Tipo_Endereco" );
				adp.UpdateCommand.Parameters.Add("@strEndereco_P", SqlDbType.VarChar, 40, "Endereco" );
				adp.UpdateCommand.Parameters.Add("@strNumero_P", SqlDbType.VarChar, 10, "Numero" );
				adp.UpdateCommand.Parameters.Add("@strComplemento_P", SqlDbType.VarChar, 30, "Complemento" );
				adp.UpdateCommand.Parameters.Add("@strBairro_P", SqlDbType.VarChar, 30, "Bairro" );
				adp.UpdateCommand.Parameters.Add("@strMunicipio_P", SqlDbType.VarChar, 30, "Municipio" );
				adp.UpdateCommand.Parameters.Add("@strUf_P", SqlDbType.Char, 2, "Uf" );
				adp.UpdateCommand.Parameters.Add("@strCep_P", SqlDbType.VarChar, 9, "Cep" );
				adp.UpdateCommand.Parameters.Add("@strTelefone_P", SqlDbType.VarChar, 20, "Telefone" );
				adp.UpdateCommand.Parameters.Add("@strFax_P", SqlDbType.VarChar, 20, "Fax" );
				adp.UpdateCommand.Parameters.Add("@strEMail_P", SqlDbType.VarChar, 40, "EMail" );

				adp.Update( dtbMGI_Terceiro_Endereco );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsTerceiro.sprAtualizarTerceiro_Endereco() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}