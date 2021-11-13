using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace ImportacaoMGI
{
	/// <summary>
	/// Summary description for clsImportacao.
	/// </summary>
	public class clsImportacao 
	{
		#region Construtor

		private DirectoryInfo dirXML;
		private string strPathXML_MGI;
		private string strPathRAR_MGI;

		private CLS.clsCarac_Veiculacao			oCarac_Veiculacao;
		private CLS.clsContato 					oContato;
		private CLS.clsContrato					oContrato;
		private CLS.clsContrato_Duplicata		oContrato_Duplicata;
		private CLS.clsDuplicata				oDuplicata;
		private CLS.clsEmpresa					oEmpresa;
		private CLS.clsGenero					oGenero;
		private CLS.clsGrade					oGrade;
		private CLS.clsMercado					oMercado;
		private CLS.clsMotivo_Cancelamento		oMotivo_Cancelamento;
		private CLS.clsMotivo_Falha				oMotivo_Falha;
		private CLS.clsNatureza_Servico			oNatureza_Servico;
		private CLS.clsNucleo					oNucleo;
		private CLS.clsProduto					oProduto;
		private CLS.clsPrograma					oPrograma;
		private CLS.clsRoteiro					oRoteiro;
		private CLS.clsTabela_Preco				oTabela_Preco;
		private CLS.clsTerceiro					oTerceiro;
		private CLS.clsTipo_Comercial			oTipo_Comercial;
		private CLS.clsTipo_Midia				oTipo_Midia;
		private CLS.clsVeiculacao				oVeiculacao;
		private CLS.clsVeiculo					oVeiculo;

		public clsImportacao()
		{
			try
			{
				strPathXML_MGI = CLS.clsParametro.getParametro("PathXML_MGI");
				strPathRAR_MGI = CLS.clsParametro.getParametro("PathRAR_MGI");

				if ( strPathXML_MGI.LastIndexOf("\\") < ( strPathXML_MGI.Length - 1 ) )
					strPathXML_MGI += "\\";

				if ( strPathRAR_MGI.LastIndexOf("\\") < ( strPathRAR_MGI.Length - 1 ) )
					strPathRAR_MGI += "\\";

				dirXML = new DirectoryInfo( strPathXML_MGI );

				this.sprCarregar();
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsImportacao.sprCarregar() " + ex.Message.ToString() );
			}			
		}

		#endregion

//		#region Executar
//
//		public string Executar()
//		{
//			string strRetorno = "";
//			try
//			{
//				strRetorno += this.sprRARDesCompacta();
//				//strRetorno += this.spuFTP();
//				strRetorno += this.sprRARDesCompacta();
//				this.sprArquivosXML_Tabela_Apoio(); 
//				this.sprArquivosXML_Movimento();
//			}
//			catch ( Exception ex )
//			{
//				throw new Exception ( "clsImportacao.Executar() " + ex.Message.ToString() );
//			}
//	
//			return ( strRetorno );
//		}
//
//		#endregion

		#region Importar Arquivos de Movimento

		public string spuArquivosXML_Movimento()
		{
			SqlConnection cnn = new SqlConnection();
			SqlTransaction tran;
			string	strRetorno = "";

			try
			{
				dirXML.Refresh();

				cnn = CLS.clsConexao.ObterConexao();
				cnn.Open();

                // Carrega XML e Atualiza o DB MGI tabela contrato
                CLS.clsLOG.psuLog("Processando Contratos", false);
                foreach ( FileInfo fli in dirXML.GetFiles("contrato*.xml") )
				{
					if ( fli.FullName.LastIndexOf("_T_") > 0 )
					{
						string strCod_Empresa = fli.FullName.Substring( fli.FullName.LastIndexOf("_T_") + 3, 3);
						int intCompetencia = int.Parse( fli.FullName.Substring( fli.FullName.LastIndexOf("_" + strCod_Empresa + "_") + 5, 6) );

						oContrato.spuCarregar( cnn, strCod_Empresa, intCompetencia );
					}
					else
					{
						int intID_Contrato = int.Parse( fli.FullName.Substring( fli.FullName.LastIndexOf("_P_") + 3, 8) );

						oContrato.spuCarregar( cnn, intID_Contrato );
					}
					//tran = cnn.BeginTransaction();

					oContrato.dtbContrato.Clear();
					oContrato.dtsContrato.ReadXml( fli.FullName, XmlReadMode.Auto  );
					oContrato.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

                // Carrega XML e Atualiza o DB MGI tabela Veiculacao
                CLS.clsLOG.psuLog("Processando Veiculações", false);
                foreach ( FileInfo fli in dirXML.GetFiles("veiculacao*.xml") )
				{
					if ( fli.FullName.LastIndexOf("_T_") > 0 )
					{
						int intCompetencia = int.Parse( fli.FullName.Substring( fli.FullName.LastIndexOf("_T_") + 7, 6) );

						oVeiculacao.spuCarregarCompetencia( cnn, intCompetencia );
					}
					else
					{
						int intID_Veiculacao = int.Parse( fli.FullName.Substring( fli.FullName.LastIndexOf("_P_") + 3, 8) );

						oVeiculacao.spuCarregar( cnn, intID_Veiculacao );
					}
					//tran = cnn.BeginTransaction();

					oVeiculacao.dtbVeiculacao.Clear();
					oVeiculacao.dtsVeiculacao.ReadXml( fli.FullName, XmlReadMode.Auto  );
					oVeiculacao.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += " ("+ oVeiculacao.dtbMGI_Veiculacao.Rows.Count.ToString().Trim() + " linhas)";
					strRetorno += "; ";

					oVeiculacao.dtbMGI_Veiculacao.Clear();

					fli.Delete();
				}

                // Carrega XML e Atualiza o DB MGI tabela Grade
                CLS.clsLOG.psuLog("Processando Grade", false);
                foreach ( FileInfo fli in dirXML.GetFiles("Grade*.xml") )
				{
					if ( fli.FullName.LastIndexOf("_T_") > 0 )
					{
//						int intCompetencia = int.Parse( fli.FullName.Substring( fli.FullName.LastIndexOf("_T_") + 7, 6) );
//
//						oGrade.spuCarregar( cnn, intCompetencia );
//
						//tran = cnn.BeginTransaction();

						oGrade.dtbGrade.Clear();
						oGrade.dtsGrade.ReadXml( fli.FullName, XmlReadMode.Auto  );
						oGrade.spuImportar( cnn );

						//tran.Commit();

						strRetorno += fli.Name;
						strRetorno += " ("+ oGrade.dtbMGI_Grade.Rows.Count.ToString().Trim() + " linhas)";
						strRetorno += "; ";

						oGrade.dtbMGI_Grade.Clear();

						fli.Delete();
					}
				}

                // Carrega XML e Atualiza o DB MGI tabela Tabela_Preco
                CLS.clsLOG.psuLog("Processando Tabela de Preços", false);
                foreach ( FileInfo fli in dirXML.GetFiles("Tabela_Preco*.xml") )
				{
					if ( fli.FullName.LastIndexOf("_T_") > 0 )
					{
						int intCompetencia = int.Parse( fli.FullName.Substring( fli.FullName.LastIndexOf("_T_") + 7, 6) );

						oTabela_Preco.spuCarregar( cnn, intCompetencia );

						//tran = cnn.BeginTransaction();

						oTabela_Preco.dtbTabela_Preco.Clear();
						oTabela_Preco.dtsTabela_Preco.ReadXml( fli.FullName, XmlReadMode.Auto  );
						oTabela_Preco.spuImportar( cnn );

						//tran.Commit();

						strRetorno += fli.Name;
						strRetorno += "; ";

						fli.Delete();
					}
				}

                // Carrega XML e Atualiza o DB MGI tabela Roteiro
                CLS.clsLOG.psuLog("Processando Roteiro", false);
                foreach ( FileInfo fli in dirXML.GetFiles("Roteiro*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oRoteiro.dtbRoteiro.Clear();
					oRoteiro.dtsRoteiro.ReadXml( fli.FullName, XmlReadMode.Auto  );
					oRoteiro.spuImportar( cnn );

					//tran.Commit();
                    strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				cnn.Close();
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsImportacao.spuArquivosXML_Movimento() " + ex.Message.ToString() );
			}
			finally
			{
				if ( cnn.State == ConnectionState.Open )
				{
					cnn.Close();
				}
				cnn.Dispose();
				cnn = null;
			}

			return ( strRetorno );

		}

		#endregion

		#region Importar Arquivos de Apoio

		public string spuArquivosXML_Tabela_Apoio()
		{
			SqlConnection cnn = new SqlConnection();
			SqlTransaction tran;
			string	strRetorno = "";

			try
			{
				dirXML.Refresh();

				cnn = CLS.clsConexao.ObterConexao();
				cnn.Open(); 

				// Carrega XML e Atualiza o DB MGI tabela Carac_Veiculacao
                CLS.clsLOG.psuLog("Processando Tabela de Caracteristica da Veiculacao",false );
                
				foreach( FileInfo fli in dirXML.GetFiles("carac_veiculacao*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oCarac_Veiculacao.dtbCarac_Veiculacao.Clear();
					oCarac_Veiculacao.dtsCarac_Veiculacao.ReadXml( fli.FullName, XmlReadMode.Auto );
					oCarac_Veiculacao.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Contato
                CLS.clsLOG.psuLog("Processando Tabela de Contato", false);
				foreach( FileInfo fli in dirXML.GetFiles("Contato*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oContato.dtbContato.Clear();
					oContato.dtbContato_Empresa.Clear();
					oContato.dtsContato.ReadXml( fli.FullName, XmlReadMode.Auto );

					if ( fli.FullName.LastIndexOf("_T_") > 0 )
					{
						// Remove todos os relacionamentos inexistentes entre contato e empresa
						foreach( WinService_ImportacaoMGI2.XSD.Contato.dtbMGI_Contato_EmpresaRow drwOld in oContato.dtbMGI_Contato_Empresa.Rows )
							if ( oContato.dtbContato_Empresa.Select("Cod_Contato = '" + drwOld.Cod_Contato + "' and Cod_Empresa = '" + drwOld.Cod_Empresa + "'").Length == 0 )
								drwOld.Delete();
					}

					oContato.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Empresa
                CLS.clsLOG.psuLog("Processando Tabela de Empresas", false);
				foreach( FileInfo fli in dirXML.GetFiles("Empresa*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oEmpresa.dtbEmpresa.Clear();
					oEmpresa.dtsEmpresa.ReadXml( fli.FullName, XmlReadMode.Auto );
					oEmpresa.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Genero
                CLS.clsLOG.psuLog("Processando Tabela de Generos", false);
				foreach( FileInfo fli in dirXML.GetFiles("Genero*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oGenero.dtbGenero.Clear();
					oGenero.dtsGenero.ReadXml( fli.FullName, XmlReadMode.Auto );
					oGenero.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Mercado
                CLS.clsLOG.psuLog("Processando Tabela de Mercados", false);
				foreach( FileInfo fli in dirXML.GetFiles("Mercado*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oMercado.dtbMercado_Composicao.Clear();
					oMercado.dtbMercado.Clear();
                    oMercado.dtsMercado.ReadXml(fli.FullName, XmlReadMode.IgnoreSchema);
					oMercado.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Motivo_Cancelamento
                CLS.clsLOG.psuLog("Processando Tabela de Motivo_Cancelamento", false);
				foreach( FileInfo fli in dirXML.GetFiles("Motivo_Cancelamento*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oMotivo_Cancelamento.dtbMotivo_Cancelamento.Clear();
					oMotivo_Cancelamento.dtsMotivo_Cancelamento.ReadXml( fli.FullName, XmlReadMode.Auto );
					oMotivo_Cancelamento.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Motivo_Falha
                CLS.clsLOG.psuLog("Processando Tabela de Motivo_Falha", false);
				foreach( FileInfo fli in dirXML.GetFiles("Motivo_Falha*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oMotivo_Falha.dtbMotivo_Falha.Clear();
					oMotivo_Falha.dtsMotivo_Falha.ReadXml( fli.FullName, XmlReadMode.Auto );
					oMotivo_Falha.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Natureza_Servico
                CLS.clsLOG.psuLog("Processando Tabela de Natureza_Servico", false);
				foreach( FileInfo fli in dirXML.GetFiles("Natureza_Servico*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oNatureza_Servico.dtbNatureza_Servico.Clear();
					oNatureza_Servico.dtsNatureza_Servico.ReadXml( fli.FullName, XmlReadMode.Auto );
					oNatureza_Servico.spuImportar( cnn);

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Nucleo
                CLS.clsLOG.psuLog("Processando Tabela de Nucleo", false);
				foreach( FileInfo fli in dirXML.GetFiles("Nucleo*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oNucleo.dtbNucleo.Clear();
					oNucleo.dtsNucleo.ReadXml( fli.FullName, XmlReadMode.Auto );
					oNucleo.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Produto
                CLS.clsLOG.psuLog("Processando Tabela de Produto", false);
				foreach( FileInfo fli in dirXML.GetFiles("Produto*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oProduto.dtbProduto.Clear();
					oProduto.dtsProduto.ReadXml( fli.FullName, XmlReadMode.Auto );
					oProduto.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Programa
                CLS.clsLOG.psuLog("Processando Tabela de Programa", false);
				foreach( FileInfo fli in dirXML.GetFiles("Programa*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oPrograma.dtbPrograma.Clear();
					oPrograma.dtsPrograma.ReadXml( fli.FullName, XmlReadMode.Auto );
					oPrograma.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Terceiro
                CLS.clsLOG.psuLog("Processando Tabela de Terceiro", false);
				foreach( FileInfo fli in dirXML.GetFiles("Terceiro*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oTerceiro.dtbTerceiro_Endereco.Clear();
					oTerceiro.dtbTerceiro.Clear();
					oTerceiro.dtbTerceiro_Funcao.Clear();

					oTerceiro.dtsTerceiro.ReadXml( fli.FullName, XmlReadMode.IgnoreSchema);
					oTerceiro.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Tipo_Comercial
                CLS.clsLOG.psuLog("Processando Tabela de Tipo_Comercial", false);
				foreach( FileInfo fli in dirXML.GetFiles("Tipo_Comercial*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oTipo_Comercial.dtbTipo_Comercial.Clear();
					oTipo_Comercial.dtsTipo_Comercial.ReadXml( fli.FullName, XmlReadMode.Auto );
					oTipo_Comercial.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Tipo_Midia
                CLS.clsLOG.psuLog("Processando Tabela de Tipo_Midia", false);
				foreach( FileInfo fli in dirXML.GetFiles("Tipo_Midia*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oTipo_Midia.dtbTipo_Midia.Clear();
					oTipo_Midia.dtsTipo_Midia.ReadXml( fli.FullName, XmlReadMode.Auto );
					oTipo_Midia.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				// Carrega XML e Atualiza o DB MGI tabela Veiculo
                CLS.clsLOG.psuLog("Processando Tabela de Veiculo", false);
				foreach( FileInfo fli in dirXML.GetFiles("Veiculo*.xml") )
				{
					//tran = cnn.BeginTransaction();

					oVeiculo.dtbVeiculo.Clear();
					oVeiculo.dtsVeiculo.ReadXml( fli.FullName, XmlReadMode.Auto );
					oVeiculo.spuImportar( cnn );

					//tran.Commit(); 

					strRetorno += fli.Name;
					strRetorno += "; ";

					fli.Delete();
				}

				////tran.Commit();
				cnn.Close();
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsImportacao.spuArquivosXML_Tabela_Apoio() " + ex.Message.ToString() );
			}
			finally
			{
				if ( cnn.State == ConnectionState.Open )
				{
					cnn.Close();
				}
				cnn.Dispose();
				cnn = null;
			}

			return ( strRetorno );

		}

		#endregion

		#region FTP

		public string spuFTP()
		{
			string strEXEFTP;
			string strArguments;
			string strRetorno = "";

			try
			{
				strEXEFTP = CLS.clsParametro.getParametro("EXEFTP");

				if ( strEXEFTP.LastIndexOf("\\") < ( strEXEFTP.Length - 1 ) )
					strEXEFTP += "\\";

				using (StreamWriter ofile = new StreamWriter( strPathXML_MGI + "IMPORTA.TXT", false ) ) 
				{
					ofile.WriteLine( "OPEN  " + CLS.clsParametro.getParametro("EndFTP") );	// Abre Conexo Remot FTP
					ofile.WriteLine( CLS.clsParametro.getParametro("UsrFTP") );		// Usuário Remoto
					ofile.WriteLine( CLS.clsParametro.getParametro("PwsFTP") );		// Senha Remota

					//ofile.WriteLine( "ASCII" );								// Tipo de transmissao Ascii
					ofile.WriteLine( "BINARY" );								// Tipo de transmissao Binary
					ofile.WriteLine( "PROMPT" );								// Desligar prompt interativo
					ofile.WriteLine( "LCD \"" + strPathXML_MGI.Substring( 0, ( strPathXML_MGI.Length -1 ) ) + "\""  );		// Posicionar no Computador Local
					ofile.WriteLine( "CD  \"" + CLS.clsParametro.getParametro("PathFTP") + "\""  );	// Posicionar no Computador Remoto
					ofile.WriteLine( "MGET *.rar" );							// Receber arquivos
					ofile.WriteLine( "MDELETE *.rar" );							// Apagar arquivos no Computador Remoto
					ofile.WriteLine( "CLOSE" );									// Encerrar
					ofile.WriteLine( "BYE" );									// Encerrar
					ofile.Close();
				}

				if ( ! new FileInfo( strEXEFTP + "FTP.EXE" ).Exists )
				{
					throw new Exception ( "clsImportacao.spuFTP() 'Não foi possivel encontrar o arquivo " + strEXEFTP + "FTP.EXE'" );
				}

				using( Process myProcess = new Process() )
				{
					strArguments = " /c ";
					strArguments += strEXEFTP;
					strArguments += @"FTP.EXE -v -s:";
					strArguments += strPathXML_MGI;
					strArguments += @"IMPORTA.TXT";

					myProcess.StartInfo.FileName = "CMD.EXE";
					myProcess.StartInfo.Arguments = strArguments;
					myProcess.StartInfo.CreateNoWindow = false;
					myProcess.StartInfo.WorkingDirectory = strPathXML_MGI;
					myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					myProcess.StartInfo.UseShellExecute = false;
					myProcess.Start();

					//myProcess.WaitForExit();
					myProcess.WaitForExit( int.Parse( CLS.clsParametro.getParametro("TimeoutFTP") ) * 1000 );

					if ( ! myProcess.HasExited )
					{
						//myProcess.Kill();

						// Forca o processo ftp.exe a ser encerrado
						using( Process myProcess2 = new Process() )
						{
							myProcess2.StartInfo.FileName = "CMD.EXE";
							myProcess2.StartInfo.Arguments = " /c " + strPathRAR_MGI + "KILL.EXE FTP";
							myProcess2.StartInfo.CreateNoWindow = false;
							myProcess2.StartInfo.WorkingDirectory = strPathRAR_MGI;
							myProcess2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
							myProcess2.StartInfo.UseShellExecute = false;
							myProcess2.Start();

							myProcess2.WaitForExit( int.Parse( CLS.clsParametro.getParametro("TimeoutFTP") ) * 1000 );

							myProcess2.Dispose();
						}

						strRetorno = "Forçando a exclusão do serviço FTP TimeOut";
					}
					myProcess.Dispose();
				}

				foreach( FileInfo fli in new DirectoryInfo( strPathXML_MGI ).GetFiles( "IMPORTA.TXT" ) )
				{
					try
					{
						fli.Delete();
					}
					catch( Exception ex )
					{
						throw new Exception( ex.ToString() + "O Timeout para conclusão do DownLoad é de " 
							+ CLS.clsParametro.getParametro("TimeoutFTP")
							+ " segundo, favor aumentá-lo, ou verificar se existem problemas no acesso FTP" );
					}
				}
			}
			catch( Exception ex )
			{
				throw new Exception ( "clsImportacao.spuFTP() " + ex.Message.ToString() );
			}

			return ( strRetorno );
		}

		#endregion

		#region Carregar

		private void sprCarregar()
		{
			SqlConnection cnn;
			try
			{
				cnn = new SqlConnection();
 
				cnn = CLS.clsConexao.ObterConexao();
				cnn.Open();

				oCarac_Veiculacao		= new CLS.clsCarac_Veiculacao();
				oContato				= new CLS.clsContato();
				oContrato				= new CLS.clsContrato();
				oContrato_Duplicata		= new CLS.clsContrato_Duplicata();
				oDuplicata				= new CLS.clsDuplicata();
				oEmpresa				= new CLS.clsEmpresa();
				oGenero					= new CLS.clsGenero();
				oGrade					= new CLS.clsGrade();
				oMercado				= new CLS.clsMercado();
				oMotivo_Cancelamento	= new CLS.clsMotivo_Cancelamento();
				oMotivo_Falha			= new CLS.clsMotivo_Falha();
				oNatureza_Servico		= new CLS.clsNatureza_Servico(); 
				oNucleo					= new CLS.clsNucleo();
				oProduto				= new CLS.clsProduto();
				oPrograma				= new CLS.clsPrograma();
				oRoteiro				= new CLS.clsRoteiro();
				oTabela_Preco			= new CLS.clsTabela_Preco();
				oTerceiro				= new CLS.clsTerceiro();
				oTipo_Comercial			= new CLS.clsTipo_Comercial();
				oTipo_Midia				= new CLS.clsTipo_Midia();
				oVeiculacao				= new CLS.clsVeiculacao();
				oVeiculo				= new CLS.clsVeiculo();

				oCarac_Veiculacao.spuCarregar( cnn, null );
				oContato.spuCarregar( cnn, null );
				oEmpresa.spuCarregar( cnn, null );
				oGenero.spuCarregar( cnn, null );
				oMercado.spuCarregar( cnn, null );
				oMotivo_Cancelamento.spuCarregar( cnn, null );
				oMotivo_Falha.spuCarregar( cnn, null );
				oNucleo.spuCarregar( cnn, null );
				oNatureza_Servico.spuCarregar( cnn, null );
				oProduto.spuCarregar( cnn, null );
				oPrograma.spuCarregar( cnn, null );
				oTerceiro.spuCarregar( cnn, null );
				oTipo_Comercial.spuCarregar( cnn, null );
				oTipo_Midia.spuCarregar( cnn, null );
				oVeiculo.spuCarregar( cnn, null );
				
				cnn.Close();
				cnn.Dispose();
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsImportacao.sprCarregar() " + ex.Message.ToString() );
			}			
		}

		#endregion

		#region Rar DesCompactação
		public string spuRARDesCompacta()
		{
			return ( this.spuRARDesCompacta( "*.rar" ) );
		}

		public string spuRARDesCompacta( string strNomeArquivoRAR )
		{
			Process myProcess;
			string	strCmd,
					strArguments,
					strNewPath,
					strRetorno = "";

			try
			{
				dirXML.Refresh();
				
				if ( ! new FileInfo( strPathRAR_MGI + "RAR.EXE" ).Exists )
				{
					throw new Exception ( "clsImportacao.spuRARDesCompacta() 'Não foi possivel encontrar o arquivo " + strPathRAR_MGI + "RAR.EXE'" );
				}

				if ( dirXML.GetFiles( strNomeArquivoRAR ).Length > 0 )
				{
					strNewPath = "Tmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

					if ( ! new DirectoryInfo( strPathXML_MGI + strNewPath ).Exists )
						dirXML.CreateSubdirectory( strNewPath );

					foreach( FileInfo fli in dirXML.GetFiles( strNomeArquivoRAR ) )
					{
						strRetorno += fli.Name;
						strRetorno += "; ";

						if ( System.IO.File.Exists( fli.FullName.Replace( fli.Name, strNewPath + "\\" + fli.Name ).ToString() ) )
							System.IO.File.Delete( fli.FullName.Replace( fli.Name, strNewPath + "\\" + fli.Name ).ToString() );

						fli.MoveTo( fli.FullName.Replace( fli.Name, strNewPath + "\\" + fli.Name ) );

						strCmd = "CMD.EXE";

						strArguments = " /c ";
						strArguments += strPathRAR_MGI + "RAR.EXE e -y ";
						strArguments += fli.FullName;

						myProcess = new Process();
						myProcess.StartInfo.FileName = strCmd;
						myProcess.StartInfo.Arguments = strArguments;  
						myProcess.StartInfo.CreateNoWindow = false;
						myProcess.StartInfo.WorkingDirectory = strPathXML_MGI;
						myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						myProcess.Start();

						myProcess.WaitForExit(60000);
						myProcess.Dispose();
					}
				}
			}
			catch ( Exception ex )
			{
				throw new Exception ( "clsImportacao.spuRARDesCompacta() " + ex.Message.ToString() );
			}
			return( strRetorno );
		}

		#endregion
		
	}
}
