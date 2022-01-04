using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class R0118Diaria
    {
        public DataTable ReportLoadData(ReportFilterModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {

                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_R0118_Diaria");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Output", Param.Output);
                var strXml = "";
                for (int i = 0; i < Param.Filters.Count; i++)
                {
                    if (!String.IsNullOrEmpty(Param.Filters[i].ParameterName))
                    {


                        switch (Param.Filters[i].Type)
                        {
                            case "Date":
                                if (!String.IsNullOrEmpty(Param.Filters[i].Value.ToString()))
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, Param.Filters[i].Value.ConvertToDatetime());
                                }
                                else
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
                                }
                                break;
                            case "Boolean":
                                Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, Param.Filters[i].Value.ConvertToBoolean());
                                break;
                            case "Text":
                            case "Options":

                                if (!String.IsNullOrEmpty(Param.Filters[i].Value.ToString()))
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, Param.Filters[i].Value);
                                }
                                else
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
                                }
                                break;
                            case "Month":
                                if (!String.IsNullOrEmpty(Param.Filters[i].Value.ToString()))
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, clsLib.CompetenciaInt(Param.Filters[i].Value));
                                }
                                else
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
                                }
                                break;
                            case "List":
                                if (Param.Filters[i].ArrayValue.Count > 0)
                                {

                                    strXml = clsLib.SerializeToString(Param.Filters[i].ArrayValue);
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, strXml);
                                }
                                else
                                {
                                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
                                }
                                break;
                            default:
                                break;
                        }
                    }
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

    }
}
