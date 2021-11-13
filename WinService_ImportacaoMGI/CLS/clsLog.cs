using System;
using System.IO;

namespace ImportacaoMGI.CLS
{
	public class clsLOG
	{
		#region LOG
		internal static void psuLog(string strLog, Boolean blnStart)
		{
			Boolean blnExistente;
			string strArquivo;
			try
			{
				strArquivo = clsLOG.GetPhysicalPath() + "WinService_ImportacaoMGI2" + DateTime.Now.ToString( ImportacaoMGI.CLS.clsParametro.getParametro("FormatoLog") ) + ".log";
				blnExistente = ( new FileInfo( strArquivo ).Exists );

				using (StreamWriter ofile = new StreamWriter( strArquivo, true) ) 
				{
					if ( blnStart )
						ofile.WriteLine( DateTime.Now.ToString( "dd/MM/yy HH:mm:ss" ) + " - Inicializando Serviço;" );

					if ( blnStart || ! blnExistente )
					{
						Version vss = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

						ofile.WriteLine( "--------- --------- --------- --------- --------- --------- --------- --------- --------- ");
						ofile.WriteLine( " Versão .........>" + vss.Major.ToString() + "." + vss.Minor.ToString() + "." + vss.Build.ToString() + "." + vss.Revision.ToString() );
						ofile.WriteLine( " Timeout ........>" + ImportacaoMGI.CLS.clsParametro.getParametro("Timeout") );
						ofile.WriteLine( " TCPIP ..........>" + ImportacaoMGI.CLS.clsParametro.getParametro("TCPIP") );
						ofile.WriteLine( " ApplicationName >" + ImportacaoMGI.CLS.clsParametro.getParametro("ApplicationName") );
						ofile.WriteLine( " Computer .......>" + ImportacaoMGI.CLS.clsParametro.getParametro("ComputerMGI") );
						ofile.WriteLine( " Server .........>" + ImportacaoMGI.CLS.clsParametro.getParametro("ServerMGI") );
						ofile.WriteLine( " Database .......>" + ImportacaoMGI.CLS.clsParametro.getParametro("DatabaseMGI") );
						ofile.WriteLine( " Usr ............>" + ImportacaoMGI.CLS.clsParametro.getParametro("UsrMGI") );
						ofile.WriteLine( " Pwd ............>" + ImportacaoMGI.CLS.clsParametro.getParametro("PwdMGI") );
						ofile.WriteLine( " Path RAR MGI ...>" + ImportacaoMGI.CLS.clsParametro.getParametro("PathRAR_MGI") );
						ofile.WriteLine( " Path XML MGI ...>" + ImportacaoMGI.CLS.clsParametro.getParametro("PathXML_MGI") );
						ofile.WriteLine( " Path FTP MGI ...>" + ImportacaoMGI.CLS.clsParametro.getParametro("PathFTP_MGI") );
						//						ofile.WriteLine( "---");
						//						ofile.WriteLine( " Path EXEC FPT ..>" + ImportacaoMGI.CLS.clsParametro.getParametro("EXEFTP") );
						//						ofile.WriteLine( " IP FPT .........>" + ImportacaoMGI.CLS.clsParametro.getParametro("EndFTP") );
						//						ofile.WriteLine( " Path FPT .......>" + ImportacaoMGI.CLS.clsParametro.getParametro("PathFTP") );
						//						ofile.WriteLine( " Usr FPT ........>" + ImportacaoMGI.CLS.clsParametro.getParametro("UsrFTP") );
						//						ofile.WriteLine( " Pws FPT ........>" + ImportacaoMGI.CLS.clsParametro.getParametro("PwsFTP") );
						//						ofile.WriteLine( " Timeout FPT ....>" + ImportacaoMGI.CLS.clsParametro.getParametro("TimeoutFTP") );
						ofile.WriteLine( "---");
						ofile.WriteLine( " SmtpServer .....>" + ImportacaoMGI.CLS.clsParametro.getParametro("SmtpServer") );
						ofile.WriteLine( " MailMessageFrom >" + ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageFrom") );
						ofile.WriteLine( " MailPriority ...>" + ImportacaoMGI.CLS.clsParametro.getParametro("MailPriority") );
						ofile.WriteLine( " MailMessageTo ..>" + ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageTo") );
						ofile.WriteLine( " MailMessageCc ..>" + ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageCc") );
						ofile.WriteLine( " MailMessageCco .>" + ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageCco") );
						ofile.WriteLine( " MailSubject ....>" + ImportacaoMGI.CLS.clsParametro.getParametro("MailSubject") );
						ofile.WriteLine( " MailBody .......>" + ImportacaoMGI.CLS.clsParametro.getParametro("MailBody") );
						ofile.WriteLine( " FormatoLog .....>" + ImportacaoMGI.CLS.clsParametro.getParametro("FormatoLog") );
						ofile.WriteLine( "---");
						ofile.WriteLine( " StartFTPChecking>" + ImportacaoMGI.CLS.clsParametro.getParametro("StartFTPChecking") );
						ofile.WriteLine( " EndFTPChecking>" + ImportacaoMGI.CLS.clsParametro.getParametro("EndFTPChecking") );
						ofile.WriteLine( " TimerFTPChecking>" + ImportacaoMGI.CLS.clsParametro.getParametro("TimerFTPChecking") );

						ofile.WriteLine( "--------- --------- --------- --------- --------- --------- --------- --------- --------- ");
					}
					ofile.WriteLine( DateTime.Now.ToString( "dd/MM/yy HH:mm:ss" ) + " - " + strLog + ";" );
					ofile.Close();
				}
			}
			catch ( Exception ex )
			{
				throw new Exception(" WinService_ImportacaoMGI2.psuLog " +  ex.Message.ToString() );
			}
		}

		protected static string GetPhysicalPath()
		{
			string diretorio;
			// Obtêm a chave de registro. 
			//Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\WinService_ImportacaoMGI2", false);
			//diretorio = Key.GetValue("ImagePath").ToString();

			// Remove diretórios de executáveis e debug, no caso de querer o path padrão
			//diretorio = diretorio.Replace("\\bin","");
			//diretorio = diretorio.Replace("\\Debug","");
            diretorio = System.Reflection.Assembly.GetEntryAssembly().Location.ToString();
			// Retorna o diretório sem o arquivo executável 
			// Obtêm a última barra do diretório e seleciona a string até este ponto. 
			int UltBarra = diretorio.LastIndexOf("\\");
			return diretorio.Substring(0, UltBarra + 1);
		}
		#endregion
	}
	}

	
  
