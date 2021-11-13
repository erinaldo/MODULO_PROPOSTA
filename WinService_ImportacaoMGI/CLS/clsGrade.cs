using System;
using System.Data;
using System.Data.SqlClient; 

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsGrade.
	/// </summary>
	public class clsGrade
	{
		public WinService_ImportacaoMGI2.XSD.Grade dtsGrade;
		public WinService_ImportacaoMGI2.XSD.Grade.dtbGradeDataTable dtbGrade;
		public WinService_ImportacaoMGI2.XSD.Grade.dtbMGI_GradeDataTable dtbMGI_Grade;

		public clsGrade()
		{
			dtsGrade = new WinService_ImportacaoMGI2.XSD.Grade();
			dtbGrade = dtsGrade.dtbGrade;
			dtbMGI_Grade = dtsGrade.dtbMGI_Grade;
		}

		#region Carregar

		private void sprCarregar( SqlConnection cnn,  DateTime dteDataExibicao )
		{
			SqlDataAdapter adp;

			try
			{
				adp = new SqlDataAdapter();

				adp.SelectCommand = (new SqlCommand("pr_MGI_Grade_S"));
				adp.SelectCommand.CommandType = CommandType.StoredProcedure;
				adp.SelectCommand.Connection = cnn;
                adp.SelectCommand.CommandTimeout = 0;
				//adp.SelectCommand.Transaction = tran;
				adp.SelectCommand.Parameters.AddWithValue("@intOrigem_P", int.Parse( clsParametro.getParametro("Origem_MGI") ) );
				adp.SelectCommand.Parameters.AddWithValue("@dteData_Exibicao_P", dteDataExibicao );

				adp.Fill(dtbMGI_Grade);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsGrade.spuCarregar() " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Importar
		internal void spuImportar( SqlConnection cnn)
		{
			try
			{
				// Cria uma datatable com uma coluna de data p/ carregar a MGI_Grade do dia de uma só vez, independente do programa
				DataTable dtbGradeCarregar = new DataTable("GradeCarregar");
				DataColumn dclDataExibicao = new DataColumn();
				dclDataExibicao.ColumnName = "Data_Exibicao";
				dtbGradeCarregar.Columns.Add( dclDataExibicao ); //, System.Type.GetType( "string" ) );

				// Preparando para Carregar somente as Datas que a Classe ainda nao tem
				// Alimenta datas que ainda não carregou na MGI_Grade
				foreach( WinService_ImportacaoMGI2.XSD.Grade.dtbGradeRow drwNew in dtbGrade.Rows )
				{
					// Tentativa de eliminar diferença de Time Zone (GNT-03:00 Brasília) entre servidores
					int intAno = drwNew.Data_Exibicao.Year;
					int intMes = drwNew.Data_Exibicao.Month;
					int intDia = drwNew.Data_Exibicao.Day;
					string strData = "";
					strData = intAno.ToString().Trim() + "/" ;
					strData += ("0"+ intMes.ToString().Trim()).Substring(intMes.ToString().Trim().Length -1,2) + "/";
					strData += ("0"+ intDia.ToString().Trim()).Substring(intDia.ToString().Trim().Length -1,2) ;

					if ( dtbGradeCarregar.Select( "Data_Exibicao = '" + strData + "'").Length == 0 )
					{
						if ( dtbMGI_Grade.Select( "Data_Exibicao = '" + strData + "'").Length == 0 )
						{
							DataRow drw = dtbGradeCarregar.NewRow();
							drw["Data_Exibicao"] = strData;
							dtbGradeCarregar.Rows.Add( drw );
						}
					}
				}

				// Carrega MGI_Grade somente as Datas que a Classe ainda nao tem
				foreach( DataRow drwCarregar in dtbGradeCarregar.Rows )
				{
					this.sprCarregar( cnn, DateTime.Parse( drwCarregar["Data_Exibicao"].ToString() ) ); 
				}

				// Lê grade oriunda do XML e atualiza MGI_Grade
				foreach( WinService_ImportacaoMGI2.XSD.Grade.dtbGradeRow drwNew in dtbGrade.Rows )
				{
					// Tentativa de eliminat diferença de Time Zone (GNT-03:00 Brasília) entre servidores
					int intAno = drwNew.Data_Exibicao.Year;
					int intMes = drwNew.Data_Exibicao.Month;
					int intDia = drwNew.Data_Exibicao.Day;
					string strData = "";
					strData = intAno.ToString().Trim() + "/" ;
					strData += ("0"+ intMes.ToString().Trim()).Substring(intMes.ToString().Trim().Length -1,2) + "/";
					strData += ("0"+ intDia.ToString().Trim()).Substring(intDia.ToString().Trim().Length -1,2) ;

					if ( dtbMGI_Grade.Select("Cod_Veiculo = '" + drwNew.Cod_Veiculo
											+ "' and Data_Exibicao = '" + strData
											+ "' and Cod_Programa = '" + drwNew.Cod_Programa
											+ "' and Indica_Grade = " + drwNew.Indica_Grade.ToString() ).Length == 0 )

						dtbMGI_Grade.AdddtbMGI_GradeRow(
							int.Parse( clsParametro.getParametro("Origem_MGI") ),
							drwNew.Cod_Veiculo,
							drwNew.Data_Exibicao,
							drwNew.Cod_Programa,
							drwNew.Indica_Grade,
							drwNew.Indica_Desativado,
							drwNew.Disponibilidade,
							drwNew.Absorvido);
					else
						foreach( WinService_ImportacaoMGI2.XSD.Grade.dtbMGI_GradeRow drwOld in dtbMGI_Grade.Select("Cod_Veiculo = '" + drwNew.Cod_Veiculo
							+ "' and Data_Exibicao = '" + strData
							+ "' and Cod_Programa = '" + drwNew.Cod_Programa
							+ "' and Indica_Grade = " + drwNew.Indica_Grade.ToString() ) )

							if( drwOld.Indica_Desativado != drwNew.Indica_Desativado
								|| drwOld.Disponibilidade != drwNew.Disponibilidade 
								|| drwOld.Absorvido != drwNew.Absorvido )
							{
								drwOld.Indica_Desativado = drwNew.Indica_Desativado;
								drwOld.Disponibilidade = drwNew.Disponibilidade;
								drwOld.Absorvido = drwNew.Absorvido;
							}
				}

				this.sprAtualizar( cnn);
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsGrade.spuImportar() " + ex.Message.ToString() );
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

				adp.InsertCommand = (new SqlCommand("pr_MGI_Grade_I"));
				adp.InsertCommand.CommandType = CommandType.StoredProcedure;
				adp.InsertCommand.Connection = cnn;
				//adp.InsertCommand.Transaction = tran;
				adp.InsertCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.InsertCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
				adp.InsertCommand.Parameters.Add("@dteData_Exibicao_P", SqlDbType.DateTime, 15, "Data_Exibicao" );
				adp.InsertCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.InsertCommand.Parameters.Add("@intIndica_Grade_P", SqlDbType.Int, 5, "Indica_Grade" );
				adp.InsertCommand.Parameters.Add("@bitIndica_Desativado_P", SqlDbType.Bit, 1, "Indica_Desativado" );
				adp.InsertCommand.Parameters.Add("@intDisponibilidade_P", SqlDbType.Int, 5, "Disponibilidade" );
				adp.InsertCommand.Parameters.Add("@intAbsorvido_P", SqlDbType.Int, 5, "Absorvido" );

				adp.UpdateCommand = (new SqlCommand("pr_MGI_Grade_U"));
				adp.UpdateCommand.CommandType = CommandType.StoredProcedure;
				adp.UpdateCommand.Connection = cnn;
				//adp.UpdateCommand.Transaction = tran;
				adp.UpdateCommand.Parameters.Add("@intOrigem_P", SqlDbType.Int, 10, "Origem" );
				adp.UpdateCommand.Parameters.Add("@strCod_Veiculo_P", SqlDbType.Char, 3, "Cod_Veiculo" );
				adp.UpdateCommand.Parameters.Add("@dteData_Exibicao_P", SqlDbType.DateTime, 15, "Data_Exibicao" );
				adp.UpdateCommand.Parameters.Add("@strCod_Programa_P", SqlDbType.Char, 4, "Cod_Programa" );
				adp.UpdateCommand.Parameters.Add("@intIndica_Grade_P", SqlDbType.Int, 5, "Indica_Grade" );
				adp.UpdateCommand.Parameters.Add("@bitIndica_Desativado_P", SqlDbType.Bit, 1, "Indica_Desativado" );
				adp.UpdateCommand.Parameters.Add("@intDisponibilidade_P", SqlDbType.Int, 5, "Disponibilidade" );
				adp.UpdateCommand.Parameters.Add("@intAbsorvido_P", SqlDbType.Int, 5, "Absorvido" );

				adp.Update( dtbMGI_Grade );
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsGrade.sprAtualizar() " + ex.Message.ToString() );
			}
		}

		#endregion
	}
}