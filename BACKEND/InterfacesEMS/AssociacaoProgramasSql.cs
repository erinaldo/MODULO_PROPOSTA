using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class AssociacaoProgramas
    {
        public static string Cod_Contato { get; private set; }

        public DataTable AssociacaoProgramasListar(AssociacaoProgramasModel pCodEmpresaFat)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_AssociacaoProgramas_Listar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", pCodEmpresaFat.Cod_Empresa_Faturamento);
                Adp.Fill(dtb);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }

        public AssociacaoProgramasModel GetAssociacaoProgramas(String Cod_Empresa_Faturamento, String Cod_Programa)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            AssociacaoProgramasModel associacaoprograma = new AssociacaoProgramasModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_AssociacaoProgramas_Get]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", Cod_Programa);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", Cod_Empresa_Faturamento);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    associacaoprograma.Cod_Empresa_Faturamento  = dtb.Rows[0]["Cod_Empresa_Faturamento"].ToString();
                    associacaoprograma.Nome_Empresa_Faturamento = dtb.Rows[0]["Nome_Empresa_Faturamento"].ToString();
                    associacaoprograma.Cod_Programa             = dtb.Rows[0]["Cod_Programa"].ToString();
                    associacaoprograma.Titulo                   = dtb.Rows[0]["Titulo"].ToString();
                    associacaoprograma.Cod_Item                 = dtb.Rows[0]["Cod_Item"].ToString();
                    associacaoprograma.Cod_Item_Antecipado      = dtb.Rows[0]["Cod_Item_Antecipado"].ToString();

                };

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return associacaoprograma;
        }


        public DataTable SalvarAssociacaoProgramas(AssociacaoProgramasModel pAssociacaoPrograma)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                   SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_AssociacaoProgramas_Salvar]");
                Adp.SelectCommand = cmd;

                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Operacao", pAssociacaoPrograma.Operacao);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Faturamento", pAssociacaoPrograma.Cod_Empresa_Faturamento);
                clsLib.NewParameter(Adp, "@Par_Cod_Programa", pAssociacaoPrograma.Cod_Programa);
                clsLib.NewParameter(Adp, "@Par_Cod_Item", pAssociacaoPrograma.Cod_Item);
                clsLib.NewParameter(Adp, "@Par_Cod_Item_Antecipado", pAssociacaoPrograma.Cod_Item_Antecipado);


                Adp.Fill(dtb);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }



    }
}
