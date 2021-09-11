using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class AssociacaoContatos
    {
        public static string Cod_Contato { get; private set; }

        public DataTable AssociacaoContatosListar(Int32 pIdContato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_AssociacaoContatos_Listar");
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
                                       
        public AssociacaoContatosModel GetAssociacaoContatos(String pIdAssociacaoContatos)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();
            AssociacaoContatosModel associacaocontato = new AssociacaoContatosModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_AssociacaoContatos_Get]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Contato", pIdAssociacaoContatos);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    associacaocontato.Cod_Contato       = dtb.Rows[0]["Cod_Contato"].ToString();
                    associacaocontato.Nome              = dtb.Rows[0]["Nome"].ToString();
                    associacaocontato.Cod_Representante = dtb.Rows[0]["Cod_Representante"].ToString().ConvertToInt32();

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
            return associacaocontato;
        }

        public DataTable SalvarAssociacaoContatos(AssociacaoContatosModel pAssociacaoContato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_AssociacaoContato_Salvar]");
                Adp.SelectCommand = cmd;

                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Operacao", pAssociacaoContato.Operacao);
                clsLib.NewParameter(Adp, "@Par_Cod_Contato", pAssociacaoContato.Cod_Contato);
                clsLib.NewParameter(Adp, "@Par_Cod_Representante", pAssociacaoContato.Cod_Representante);


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
