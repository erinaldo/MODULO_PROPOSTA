using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace WinService_ImportacaoMGI2
{
	/// <summary>
	/// Summary description for ProjectInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		private System.ServiceProcess.ServiceProcessInstaller ImportacaoMGIProcessInstaller;
		private System.ServiceProcess.ServiceInstaller ImportacaoMGIInstaller;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region INIT
		public ProjectInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Install / UnInstall
		public override void Install(IDictionary stateSaver)
		{
			Microsoft.Win32.RegistryKey system, currentControlSet, services, service, config;

			try
			{
				base.Install(stateSaver);
				system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");
				currentControlSet = system.OpenSubKey("CurrentControlSet");
				services = currentControlSet.OpenSubKey("Services");
				service = services.OpenSubKey(this.ImportacaoMGIInstaller.ServiceName, true);
				service.SetValue("Description", "Importação de arquivos XML para aplicação MGI.");
				config = service.CreateSubKey("Parameters");
			}
			catch ( Exception ex )
			{
				this.RegisterLog(ex.Message);
			}
		}

		public override void Uninstall(IDictionary savedState)
		{
			Microsoft.Win32.RegistryKey system, currentControlSet, services, service; //, config;

			try
			{
				base.Uninstall (savedState);
				system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");
				currentControlSet = system.OpenSubKey("CurrentControlSet");
				services = currentControlSet.OpenSubKey("Services");
				service = services.OpenSubKey(this.ImportacaoMGIInstaller.ServiceName, true);
				service.DeleteSubKey("Parameters");
			}
			catch ( Exception ex )
			{
				this.RegisterLog(ex.Message);
			}
		}

		private void RegisterLog(string message)
		{
			System.Diagnostics.EventLog el = new System.Diagnostics.EventLog("Application", ".", "ImportacaoMGI Windows Service Installation");
			el.WriteEntry(message);
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ImportacaoMGIProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.ImportacaoMGIInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// ImportacaoMGIProcessInstaller
			// 
			this.ImportacaoMGIProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.ImportacaoMGIProcessInstaller.Password = null;
			this.ImportacaoMGIProcessInstaller.Username = null;
			// 
			// ImportacaoMGIInstaller
			// 
			this.ImportacaoMGIInstaller.ServiceName = "WinService_ImportacaoMGI2";
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
																					  this.ImportacaoMGIProcessInstaller,
																					  this.ImportacaoMGIInstaller});

		}
		#endregion

	}
}
