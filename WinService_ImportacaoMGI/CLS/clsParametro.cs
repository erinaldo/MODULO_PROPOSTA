using System;
using System.Configuration;

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsParametro.
	/// </summary>
	public class clsParametro
	{
		public clsParametro()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string getParametro( string strParametro )
		{
			if ( ConfigurationSettings.AppSettings[strParametro] != "" )
				return ( ConfigurationSettings.AppSettings[strParametro] );
			else
				return "";
		}
	}
}
