using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class TipoComercial
    {

        public DataTable TipoComercialListar(TipoComercialFiltro Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_TipoComercial_Listar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Merchandising", Param.Indica_Merchandising);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_OnLine", Param.Indica_OnLine);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_OffLine", Param.Indica_OffLine);

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

        public TipoComercialModel GetTipoComercialData(String pCod_Tipo_Comercial)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            TipoComercialModel TipoComercial = new TipoComercialModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_Proposta_TipoComercial_Get");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Tipo_Comercial", pCod_Tipo_Comercial);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    TipoComercial.Cod_Tipo_Comercial        = dtb.Rows[0]["Cod_Tipo_Comercial"].ToString();
                    TipoComercial.Descricao                 = dtb.Rows[0]["Descricao"].ToString();
                    TipoComercial.Absorcao                  = dtb.Rows[0]["Absorcao"].ToString().ConvertToBoolean();
                    TipoComercial.INDICA_ABSORCAO_MERCHA    = dtb.Rows[0]["INDICA_ABSORCAO_MERCHA"].ToString().ConvertToBoolean();
                    TipoComercial.Roteiro_Tecnico           = dtb.Rows[0]["Roteiro_Tecnico"].ToString().ConvertToBoolean();
                    TipoComercial.Merchandising             = dtb.Rows[0]["Merchandising"].ToString().ConvertToBoolean();
                    TipoComercial.Tipo_Valoracao            = dtb.Rows[0]["Tipo_Valoracao"].ToString().ConvertToBoolean();
                    TipoComercial.Indica_Midia_On_Line      = dtb.Rows[0]["Indica_Midia_On_Line"].ToString().ConvertToBoolean();
                    TipoComercial.Exibidora_DAD             = dtb.Rows[0]["Exibidora_DAD"].ToString();
                    TipoComercial.Cod_Tipo_Comercializacao  = dtb.Rows[0]["Cod_Tipo_Comercializacao"].ToString();
                    TipoComercial.Desc_Tipo_Comercializacao  = dtb.Rows[0]["Desc_Tipo_Comercializacao"].ToString();
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
            return TipoComercial;
        }

        public DataTable SalvarTipoComercial(TipoComercialModel pTipoComercial)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_TipoComercial_Salvar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Operacao", pTipoComercial.id_operacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Tipo_Comercial", pTipoComercial.Cod_Tipo_Comercial);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Descricao", pTipoComercial.Descricao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Absorcao", pTipoComercial.Absorcao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_INDICA_ABSORCAO_MERCHA", pTipoComercial.INDICA_ABSORCAO_MERCHA);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Roteiro_Tecnico", pTipoComercial.Roteiro_Tecnico);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Merchandising", pTipoComercial.Merchandising);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Tipo_Valoracao", pTipoComercial.Tipo_Valoracao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Midia_On_Line", pTipoComercial.Indica_Midia_On_Line);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Exibidora_DAD", pTipoComercial.Exibidora_DAD);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Tipo_Comercializacao", pTipoComercial.Cod_Tipo_Comercializacao);


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
                        
        public DataTable excluirtipocomercial(TipoComercialModel pTipoComercial)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_TipoComercial_Excluir");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Tipo_Comercial", pTipoComercial.Cod_Tipo_Comercial);
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
