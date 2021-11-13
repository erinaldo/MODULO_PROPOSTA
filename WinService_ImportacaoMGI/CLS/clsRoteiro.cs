using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsRoteiro.
	/// </summary>
	public class clsRoteiro
	{
		public WinService_ImportacaoMGI2.XSD.Roteiro dtsRoteiro;
		public WinService_ImportacaoMGI2.XSD.Roteiro.dtbRoteiroDataTable dtbRoteiro;
		private WinService_ImportacaoMGI2.XSD.Roteiro.dtbRoteiroExclusaoDataTable dtbRoteiroExclusao;

		public clsRoteiro()
		{
			dtsRoteiro = new WinService_ImportacaoMGI2.XSD.Roteiro();
			dtbRoteiro = dtsRoteiro.dtbRoteiro;
			dtbRoteiroExclusao = dtsRoteiro.dtbRoteiroExclusao;
		}

		#region Importar

		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				if ( dtbRoteiro.Rows.Count > 0 )
				{
					dtbRoteiroExclusao.Clear();

					foreach( WinService_ImportacaoMGI2.XSD.Roteiro.dtbRoteiroRow drwNew in dtbRoteiro.Rows )
                    { 
						if ( dtbRoteiroExclusao.Select( "Cod_Veiculo = '" + drwNew.Cod_Veiculo + "' and Data_Exibicao = '" + drwNew.Data_Exibicao.ToString("yyyy/MM/dd") + "'").Length == 0 )
                        { 
							dtbRoteiroExclusao.AdddtbRoteiroExclusaoRow( drwNew.Cod_Veiculo, drwNew.Data_Exibicao );
                        }

                        /*
                        foreach( WinService_ImportacaoMGI2.XSD.Roteiro.dtbRoteiroRow drwNew in dtbRoteiro.Rows )
                            dtbMGI_Roteiro.AdddtbMGI_RoteiroRow(
                                int.Parse( clsParametro.getParametro("Origem_MGI") ),
                                drwNew.Cod_Veiculo,
                                drwNew.Data_Exibicao,
                                drwNew.Cod_Programa,
                                drwNew.Breaks,
                                drwNew.Tipo_Breaks,
                                drwNew.Horario_Exibicao,
                                drwNew.Cod_Tipo_Comercial,
                                drwNew.Duracao,
                                drwNew.Titulo_Comercial,
                                drwNew.Cod_Cliente,
                                drwNew.Razao_Social_Cliente,
                                drwNew.Nome_Fantasia_Cliente,
                                drwNew.Cod_Red_Produto1,
                                drwNew.Cod_Red_Produto2,
                                drwNew.Cod_Red_Produto3 );
                                */
                    }
                    this.sprAtualizar( cnn);
				}
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsRoteiro.spuImportar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Atualizar

		private void sprAtualizar( SqlConnection cnn)
		{
			this.sprExcluir( cnn);
			this.sprInserir( cnn);
		}

		private void sprExcluir( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				//adp = new SqlDataAdapter();
                
                
                foreach (DataRow drw in dtbRoteiroExclusao.Rows)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "pr_MGI_Roteiro_D";
                    cmd.Parameters.AddWithValue("@intOrigem_P", clsParametro.getParametro("Origem_MGI"));
                    cmd.Parameters.AddWithValue("@strCod_Veiculo_P", drw["Cod_Veiculo"]);
                    cmd.Parameters.AddWithValue("@dteData_Exibicao_P", drw["Data_Exibicao"]);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                //adp.InsertCommand = (new SqlCommand("pr_MGI_Roteiro_D"));
                //adp.InsertCommand.CommandType = CommandType.StoredProcedure;
                //adp.InsertCommand.Connection = cnn;
                //adp.InsertCommand.Transaction = tran;
                //adp.InsertCommand.Parameters.Add("@intOrigem_P", clsParametro.getParametro("Origem_MGI"));
                //adp.InsertCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
                //adp.InsertCommand.Parameters.Add("@dteData_Exibicao_P", SqlDbType.DateTime, 10, "Data_Exibicao" );
                //adp.Update( dtbRoteiroExclusao );
            }
			catch ( Exception ex )
			{
				throw new Exception ( "clsRoteiro.sprExcluir() " + ex.Message.ToString() );
			}
		}

		private void sprInserir( SqlConnection cnn)
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.InsertCommand = (new SqlCommand("pr_MGI_Roteiro_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", clsParametro.getParametro("Origem_MGI"));
				adp.InsertCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
				adp.InsertCommand.Parameters.Add("@dteData_Exibicao_P", SqlDbType.DateTime, 10, "Data_Exibicao" );
				adp.InsertCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.InsertCommand.Parameters.Add("@intBreaks_P", SqlDbType.SmallInt, 2, "Breaks" );
				adp.InsertCommand.Parameters.Add("@intTipo_Breaks_P", SqlDbType.SmallInt, 2, "Tipo_Breaks" );
				adp.InsertCommand.Parameters.Add("@dteHorario_Exibicao_P", SqlDbType.DateTime, 10, "Horario_Exibicao" );
				adp.InsertCommand.Parameters.Add("@strCod_Tipo_Comercial_P", SqlDbType.Char, 2, "Cod_Tipo_Comercial" );
				adp.InsertCommand.Parameters.Add("@intDuracao_P", SqlDbType.Int, 3, "Duracao" );
				adp.InsertCommand.Parameters.Add("@strTitulo_Comercial_P", SqlDbType.VarChar, 30, "Titulo_Comercial" );
				adp.InsertCommand.Parameters.Add("@strCod_Cliente_P", SqlDbType.Char, 6, "Cod_Cliente" );
				adp.InsertCommand.Parameters.Add("@strRazao_Social_Cliente_P", SqlDbType.VarChar, 50, "Razao_Social_Cliente" );
				adp.InsertCommand.Parameters.Add("@strNome_Fantasia_Cliente_P", SqlDbType.VarChar, 50, "Nome_Fantasia_Cliente" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto1_P", SqlDbType.Int, 10, "Cod_Red_Produto1" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto2_P", SqlDbType.Int, 10, "Cod_Red_Produto2" );
				adp.InsertCommand.Parameters.Add("@intCod_Red_Produto3_P", SqlDbType.Int, 10, "Cod_Red_Produto3" );
                
				adp.Update( dtbRoteiro );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsRoteiro.sprInserir() " + ex.Message.ToString() );
			}
		}


		#endregion
	}
}