using System;
using System.Data.SqlClient;

namespace ImportacaoMGI.CLS
{
	/// <summary>
	/// Summary description for clsConexao.
	/// </summary>
	public class clsConexao
	{
		/// <summary>
		/// Devolve uma conexão para o banco de dados da aplicação
		/// </summary>
		/// <returns></returns>
		/// 

		#region Conexão
        
		static public SqlConnection ObterConexao()
		{
			try
			{
				SqlConnection conBD = new SqlConnection();
				//SQL Server Autentication
				string StringConexao = "Integrated Security=False;";

				if ( clsParametro.getParametro("TCPIP") == "true" )
				{   //Conexao TCPIP 
					StringConexao += "Network Library=dbmssocn;";
				}
				else
				{
					//Conexao NAME PIPES
					StringConexao += "Network Library=dbnmpntw;";
				}
			                
				//Nome Aplicacao
				if (clsParametro.getParametro("ApplicationName") != "" )
				{
					StringConexao += "Application Name=";
					StringConexao += clsParametro.getParametro("ApplicationName");
					StringConexao += ";";
				}

				//Nome Servidor SQL
				if ( clsParametro.getParametro("ComputerMGI") != "" )
				{
					StringConexao += "Workstation ID=";
					StringConexao += clsParametro.getParametro("ComputerMGI");
					StringConexao += ";";
				}

				//Nome da Instancia SQL
				StringConexao += "Data Source=";
				StringConexao += clsParametro.getParametro("ServerMGI");
				StringConexao += ";";

				//Nome do Banco de Dados
				if ( clsParametro.getParametro("DatabaseMGI") != "" )
				{
					StringConexao += "Initial Catalog=";
					StringConexao += clsParametro.getParametro("DatabaseMGI");
					StringConexao += ";";
				}

				//User
				StringConexao += "user id=" + clsParametro.getParametro("UsrMGI").ToString();
				StringConexao += ";";

				//Password
				if ( clsParametro.getParametro("PwdMGI").ToString() != "" )
				{
					StringConexao += "password=" + clsParametro.getParametro("PwdMGI").ToString();
					StringConexao += ";";
				}

				//Timeout
				//if ( clsParametro.getParametro("Timeout") != "" )
				//{
                //	    StringConexao += "Connect Timeout=";
	            //	    StringConexao += clsParametro.getParametro("Timeout");
				//      StringConexao += ";";
				//}

				conBD.ConnectionString = StringConexao;

				return conBD;
			}
			catch( Exception ex )
			{
				throw (new Exception( ex.Message.ToString() + "Erro na Conexão com Banco de Dados MGI"));
			}
		}

		#endregion

	}
}
