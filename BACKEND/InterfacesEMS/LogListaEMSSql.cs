using CLASSDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace PROPOSTA
{
    public partial class LogListaEMS

    {

        public List<LogListaEMSModel> CarregarLogListaEMS(LogListaEMSModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<LogListaEMSModel> LogLista = new List<LogListaEMSModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_LogListaEMS_List");
                Adp.SelectCommand = cmd;
                if (!String.IsNullOrEmpty(Param.Data_Inicial))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Inicial", Param.Data_Inicial.ConvertToDatetime());
                  
                }
                if (!String.IsNullOrEmpty(Param.Data_Final))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Final", Param.Data_Final.ConvertToDatetime());
                }
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Cliente", Param.Cod_Cliente);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_CGC_CLIENTE", Param.CGC_Cliente);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_chkCom_Programacao", Param.chkCom_Programacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_chkSem_Programacao", Param.chkSem_Programacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_chkDesativacao", Param.chkDesativacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_chkReativacao", Param.chkReativacao);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    LogLista.Add(new LogListaEMSModel
                    {
                        Cod_Usuario          = drw["Cod_Usuario"].ToString(),
                        Data_Processamento   = drw["Data_Processamento"].ToString(),
                        Operacao             = drw["Operacao"].ToString(),
                        Cod_Empresa          = drw["Cod_Empresa"].ToString(),
                        Numero_Mr            = drw["Numero_Mr"].ToString().ConvertToInt32(),
                        Sequencia_Mr         = drw["Sequencia_Mr"].ToString().ConvertToInt32(),
                        Cod_Cliente          = drw["Cod_Cliente"].ToString(),
                        Razao_Social_Cliente = drw["Razao_Social_Cliente"].ToString(),
                        CGC_Cliente          = drw["CGC_Cliente"].ToString(),
                        Razao_Social_Agencia = drw["Razao_Social_Agencia"].ToString(),
                        Periodo_Veiculacao   = drw["Periodo_Veiculacao"].ToString(),
                        Qtd                  = drw["Qtd"].ToString().ConvertToInt32(),

                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return LogLista;
        }

    }
}
