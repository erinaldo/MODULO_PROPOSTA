using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class Nucleo
    {
        public static String Cod_Nucleo { get; private set; }

        public DataTable NucleoListar()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Nucleo_Listar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
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

        public DataTable SalvarNucleo(FiltroModel pFiltro)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Nucleo_Salvar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Operacao", pFiltro.Id_operacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Nucleo", pFiltro.Cod_Nucleo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Descricao", pFiltro.Descricao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa", pFiltro.Cod_Empresa);
                if (String.IsNullOrEmpty(pFiltro.Cod_Empresa_Fatura))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", DBNull.Value);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", pFiltro.Cod_Empresa_Fatura);
                }
                if (String.IsNullOrEmpty(pFiltro.Cod_Centro_Custo))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Centro_Custo", DBNull.Value);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Centro_Custo", pFiltro.Cod_Centro_Custo);
                }

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

        public FiltroModel GetNucleoData(String pCodNucleo)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            FiltroModel Filtro = new FiltroModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Nucleo_Get");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Nucleo", pCodNucleo);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    Filtro.Cod_Nucleo = dtb.Rows[0]["Cod_Nucleo"].ToString();
                    Filtro.Descricao = dtb.Rows[0]["Descricao"].ToString();
                    Filtro.Cod_Empresa = dtb.Rows[0]["Cod_Empresa"].ToString();
                    Filtro.Desc_Empresa = dtb.Rows[0]["Desc_Empresa_Venda"].ToString();
                    Filtro.Cod_Empresa_Fatura = dtb.Rows[0]["Cod_Empresa_Faturamento"].ToString();
                    Filtro.Desc_Empresa_Fatura = dtb.Rows[0]["Desc_Empresa_Fatura"].ToString();
                    Filtro.Cod_Centro_Custo = dtb.Rows[0]["Cod_Centro_Custo"].ToString();
                    Filtro.Desc_Centro_Custo = dtb.Rows[0]["Desc_Cento_Custo"].ToString();
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
            return Filtro;
        }

        public DataTable ExcluirNucleo(FiltroModel pFiltro)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Nucleo_Excluir");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Nucleo", pFiltro.Cod_Nucleo);
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
