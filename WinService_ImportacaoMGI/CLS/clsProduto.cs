	using System;
	using System.Data;
	using System.Data.SqlClient; 

	namespace ImportacaoMGI.CLS
	{
	/// <summary>
	/// Summary description for clsProduto.
	/// </summary>
	public class clsProduto
	{
		public WinService_ImportacaoMGI2.XSD.Produto dtsProduto;
		public WinService_ImportacaoMGI2.XSD.Produto.dtbProdutoDataTable dtbProduto;
		public WinService_ImportacaoMGI2.XSD.Produto.dtbMGI_ProdutoDataTable dtbMGI_Produto;

		public clsProduto()
		{
			dtsProduto = new WinService_ImportacaoMGI2.XSD.Produto();
			dtbProduto = dtsProduto.dtbProduto;
			dtbMGI_Produto = dtsProduto.dtbMGI_Produto;
		}

		#region Carregar

		internal void spuCarregar( SqlConnection cnn, string strCod_Red_Produto )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Produto_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Parameters.AddWithValue("@PCod_Red_Produto", strCod_Red_Produto );

				adp.Fill(dtbMGI_Produto);

			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsProduto.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				foreach( WinService_ImportacaoMGI2.XSD.Produto.dtbProdutoRow drwNew in dtbProduto.Rows )
				{
					if ( dtbMGI_Produto.Select("Cod_Red_Produto = " + drwNew.Cod_Red_Produto.ToString() ).Length == 0 )
						dtbMGI_Produto.AdddtbMGI_ProdutoRow( int.Parse( clsParametro.getParametro("Origem_MGI") ), 
							drwNew.Cod_Red_Produto,
							drwNew.Descricao,
							drwNew.Cod_Root,
							drwNew.Nivel);
					else
						foreach( WinService_ImportacaoMGI2.XSD.Produto.dtbMGI_ProdutoRow drwOld in dtbMGI_Produto.Select("Cod_Red_Produto = " + drwNew.Cod_Red_Produto.ToString() ) )
							if( drwNew.Cod_Root != drwOld.Cod_Root 
								|| drwNew.Descricao != drwOld.Descricao 
								|| drwNew.Nivel != drwOld.Nivel )
							{
								drwOld.Descricao = drwNew.Descricao;
								drwOld.Cod_Root = drwNew.Cod_Root;
								drwOld.Nivel = drwNew.Nivel;
							}
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsProduto.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Produto_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto_P", SqlDbType.Int, 10, "Cod_Red_Produto" );
				adp.InsertCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 40, "Descricao" );
				adp.InsertCommand.Parameters.Add("@intCod_Root_P", SqlDbType.Int, 10, "Cod_Root" );
				adp.InsertCommand.Parameters.Add("@intNivel_P", SqlDbType.Int, 10, "Nivel" );
				
				adp.UpdateCommand = (new SqlCommand("pr_MGI_Produto_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@intCod_Red_Produto_P", SqlDbType.Int, 10, "Cod_Red_Produto" );
				adp.UpdateCommand.Parameters.Add("@strDescricao_P", SqlDbType.VarChar, 40, "Descricao" );
				adp.UpdateCommand.Parameters.Add("@intCod_Root_P", SqlDbType.Int, 10, "Cod_Root" );
				adp.UpdateCommand.Parameters.Add("@intNivel_P", SqlDbType.Int, 10, "Nivel" );

				adp.Update( dtbMGI_Produto );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsProduto.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
	}