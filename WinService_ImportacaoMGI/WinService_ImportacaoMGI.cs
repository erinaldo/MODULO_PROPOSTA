using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using ImportacaoMGI;
using ImportacaoMGI.CLS; 

namespace WinService_ImportacaoMGI2
{
	public class WinService_ImportacaoMGI2 : System.ServiceProcess.ServiceBase
	{
		//private System.Timers.Timer timerImportacaoMGI;
		private string strPathFTP;
		private string strPathXML;
		private ImportacaoMGI.clsImportacao oImportacao;
		private FileSystemWatcher oWatcher;
		private System.Timers.Timer timerFTPChecking;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Serviço
		public WinService_ImportacaoMGI2()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

		// The main entry point for the process
		[STAThread]
		static void Main()
		{
			
			System.ServiceProcess.ServiceBase[] ServicesToRun;
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new WinService_ImportacaoMGI2() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);

			
			
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "WinService_ImportacaoMGI2";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region OnStart / OnStop
		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			try
			{
				this.strPathFTP = ImportacaoMGI.CLS.clsParametro.getParametro("PathFTP_MGI");
				this.strPathXML = ImportacaoMGI.CLS.clsParametro.getParametro("PathXML_MGI");

				this.strPathFTP = this.strPathFTP.Substring( this.strPathFTP.Length - 1, 1) == @"\" ? this.strPathFTP : this.strPathFTP + @"\";
				this.strPathXML = this.strPathXML.Substring( this.strPathXML.Length - 1, 1) == @"\" ? this.strPathXML : this.strPathXML + @"\";

				clsLOG.psuLog("Inicializando Windows Service", true);

				this.timerFTPChecking = new System.Timers.Timer();
				((System.ComponentModel.ISupportInitialize)(this.timerFTPChecking)).BeginInit();
				this.timerFTPChecking.Enabled = true;
                //this.timerFTPChecking.Interval = (double.Parse( ImportacaoMGI.CLS.clsParametro.getParametro("TimerFTPChecking") ) * 60000);
                this.timerFTPChecking.Interval = 60000; //1 minuto apos start e depois vai pelo config
                this.timerFTPChecking.Elapsed += new System.Timers.ElapsedEventHandler(this.timerFTPChecking_Elapsed);
				((System.ComponentModel.ISupportInitialize)(this.timerFTPChecking)).EndInit();

				//this.oImportacao = new clsImportacao();
				//this.sprMoverRAR();
				//this.sprAtualizar();
				
				// Begin watching.
				oWatcher = new System.IO.FileSystemWatcher();
				
				oWatcher.Created += new FileSystemEventHandler(oWatcher_Created);
				oWatcher.Error += new ErrorEventHandler(oWatcher_Error);

				oWatcher.Path = this.strPathFTP;
				oWatcher.NotifyFilter = NotifyFilters.FileName;
				oWatcher.IncludeSubdirectories = false;
				oWatcher.InternalBufferSize = 16384;

				oWatcher.Filter = "*.rar";

				oWatcher.EnableRaisingEvents = true;
                
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.OnStart() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.OnStart() >> " + ex.Message.ToString() );
			}
		}
	
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			try
			{
				this.timerFTPChecking.Enabled = false;
				this.timerFTPChecking.Dispose();

				this.oWatcher.EnableRaisingEvents = false;

				clsLOG.psuLog("Finalizando Windows Service",false);
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.OnStop() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.OnStop() >> " + ex.Message.ToString() );
			}
		}
		#endregion

		#region oWatcher

		#region oWatcher_Created

		private void oWatcher_Created(object source, FileSystemEventArgs e)
		{
			try
			{
				clsLOG.psuLog("NOVO ARQUIVO >> " + e.Name, false);

				FileInfo fa = new FileInfo( e.FullPath );
				Boolean blnLiberado = false;

				while( ! blnLiberado && fa.Exists  )
				{
					try
					{
						FileStream fs = fa.Open( FileMode.Open, FileAccess.Write, FileShare.None );
						fs.Close();
						blnLiberado = true;
					}
					catch
					{
						fa.Refresh();
					}
				}

				this.sprMoverRAR( fa );
				fa.Refresh();
				this.sprAtualizar( fa.Name );
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.OnCreated() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.OnCreated() >> " + ex.Message.ToString() );
			}
		}

		#endregion

		#region oWatcher_Error

		private void oWatcher_Error(object sender, ErrorEventArgs e)
		{
			try
			{
				Exception ex1 = e.GetException();
	
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.oWatcher_Error() >> " + ex1.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.oWatcher_Error() >> " + ex1.Message.ToString() );
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.oWatcher_Error() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.oWatcher_Error() >> " + ex.Message.ToString() );
			}
		}

		#endregion

		#endregion

		#region Atualizar
		private void sprAtualizar()
		{
			this.sprAtualizar( "*.RAR" );
		}

		private void sprAtualizar( string strNomeArquivoRAR )
		{
			string strRetorno = "";
            
			try
			{
				clsLOG.psuLog("Executando Serviço", false);

				strRetorno += oImportacao.spuRARDesCompacta( strNomeArquivoRAR );
				strRetorno += ";";
				strRetorno += oImportacao.spuArquivosXML_Tabela_Apoio(); 
				strRetorno += oImportacao.spuArquivosXML_Movimento();

				//if ( strRetorno.Trim().Length > 0 )
				//{
				//clsLOG.psuLog("Atualizado: " + strRetorno , false );
				//}
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.sprAtualizar() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.sprAtualizar() >> " + ex.Message.ToString() );
			}
			finally
			{
				clsLOG.psuLog("Finalizando Atualizações",false);
			}
		}

		#endregion

		#region mover arquivos .RAR para a pasta XML

		private void sprMoverRAR()
		{
			try
			{

				DirectoryInfo dir = new DirectoryInfo( this.strPathFTP );
			clsLOG.psuLog("Iniciando copias ",false);

				int cont = 0 ;
				foreach( FileInfo fa in dir.GetFiles("*.rar") )
				{		
					cont +=1;
					if(cont == 101)
					{
						return;
					}
					this.sprMoverRAR( fa );
				}
				if(cont==0)
				{
					clsLOG.psuLog("Nenhum Arquivo encontrado no FTP",false);
				}
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.sprMoverRAR() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.sprMoverRAR() >> " + ex.Message.ToString() );
			}
		}

		private void sprMoverRAR( FileInfo fliArquivo )
		{
			try
			{
				if ( File.Exists( fliArquivo.FullName.Replace( this.strPathFTP, this.strPathXML ) ) )
					File.Delete( fliArquivo.FullName.Replace( this.strPathFTP, this.strPathXML ) );

				fliArquivo.MoveTo( fliArquivo.FullName.Replace( this.strPathFTP, this.strPathXML ) );
			}		
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.sprMoverRAR( FileInfo fliArquivo ) >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.sprMoverRAR( FileInfo fliArquivo ) >> " + ex.Message.ToString() );
			}
		}

		#endregion

		#region Timer FTP Checking

		private void timerFTPChecking_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
            Double iIntervalo = (double.Parse(ImportacaoMGI.CLS.clsParametro.getParametro("TimerFTPChecking")) * 60000);
            try
			{
                /* trecho original 
				timerFTPChecking.Enabled = false;

				DateTime dte = DateTime.Now;
				int intStartFTPChecking = int.Parse( ImportacaoMGI.CLS.clsParametro.getParametro("StartFTPChecking").Replace(":","") );
				int intEndFTPChecking = int.Parse( ImportacaoMGI.CLS.clsParametro.getParametro("EndFTPChecking").Replace(":","") );

				if ( int.Parse( dte.ToString("HHmm") ) >= intStartFTPChecking && int.Parse( dte.ToString("HHmm") ) < intEndFTPChecking )
				{
					FileInfo fliFTPChecking = new FileInfo( this.strPathFTP + "FTPChecking.xml");

					clsLOG.psuLog("Verificando o arquivo FTP Checking",false);

					if ( ! fliFTPChecking.Exists )
					{
						//clsLOG.psuLog("Verificar o servico de atualização MGI, não foi recebido o arquivo de Checking de FTP",false);
						clsLOG.psuLog("Nenhum Arquivo a ser processado",false);
						//clsSendMail.SendMail("Verificar o servico de atualização MGI, não foi recebido o arquivo de Checking de FTP" );
					}
					else
					{
						//clsLOG.psuLog("Arquivo recebido com sucesso" + fliFTPChecking.CreationTime.ToString("dd/MMM/yyyy - HH:mm") ,false);
						fliFTPChecking.Delete();
					}
				}
			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.timerFTPChecking_Elapsed() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.timerFTPChecking_Elapsed() >> " + ex.Message.ToString() );
			}
			finally
			{
				clsLOG.psuLog("Habilitando timer",false);
				timerFTPChecking.Enabled = true;
			}
			 fim trecho original*/

                //timerFTPChecking.Enabled = false;
                timerFTPChecking.Stop();
                if (oImportacao==null)
                {
                    oImportacao= new clsImportacao();
                }

				DateTime dte = DateTime.Now;
				int intStartFTPChecking = int.Parse( ImportacaoMGI.CLS.clsParametro.getParametro("StartFTPChecking").Replace(":","") );
				int intEndFTPChecking = int.Parse( ImportacaoMGI.CLS.clsParametro.getParametro("EndFTPChecking").Replace(":","") );
                if ( int.Parse( dte.ToString("HHmm") ) >= intStartFTPChecking && int.Parse( dte.ToString("HHmm") ) < intEndFTPChecking )
				{
					clsLOG.psuLog("Inicio do Processamento dos Arquivos",false);
					this.sprMoverRAR();
					this.sprAtualizar();				
				}
				else
				{
					clsLOG.psuLog("Fora do Horário de Execução do Serviço",false);
				}

			}
			catch( Exception ex )
			{
				clsLOG.psuLog("** Exception ** WinService_ImportacaoMGI2.timerFTPChecking_Elapsed() >> " + ex.Message.ToString(),false);
				clsSendMail.SendMail("** Exception ** WinService_ImportacaoMGI2.timerFTPChecking_Elapsed() >> " + ex.Message.ToString() );
			}
			finally
			{

                timerFTPChecking.Interval = iIntervalo;
                timerFTPChecking.Start();

            }

		}

		#endregion
	}
}
