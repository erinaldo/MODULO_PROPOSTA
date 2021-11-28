using CLASSDB;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace PROPOSTA
{
    public partial class RetornarEMS
    {




        //-------------------------Processa Integrações de Retorno do EMS---------------------------------
        public void ProcessarRetorno(RetornarEMSModel Param)
        {
            DeletaRetorno();    //--base CARTV

            List<RetornarEMSModel> ListaNegra = new List<RetornarEMSModel>();
            List<RetornarEMSModel> NumeracaoFatura = new List<RetornarEMSModel>();
            List<RetornarEMSModel> BaixaDuplicata = new List<RetornarEMSModel>();

            if (Param.Indica_Lista_Negra)
            {
                //ListaNegra = CarregaListaNegra();   //--base EMS
                ListaNegra = API_CarregaListaNegra();   //--base EMS
                InsereListaNegra(ListaNegra);       //--base CARTV
                //MarcaFlagListaNegra();              //--base EMS
                API_MarcaFlagListaNegra();              //--base EMS
            };

            if (Param.Indica_Numeracao_Fatura)
            {
                //NumeracaoFatura = CarregaNumeracaoFatura(Param);    //--base EMS
                NumeracaoFatura = API_CarregaNumeracaoFatura(Param);    //--base EMS
                InsereNumeracaoFatura(NumeracaoFatura);             //--base CARTV
            };

            if (Param.Indica_Baixa_Duplicata)
            {
                //BaixaDuplicata = CarregaBaixaDuplicata();   //--base EMS
                BaixaDuplicata = API_CarregaBaixaDuplicata();   //--base EMS
                InsereBaixaDuplicata(BaixaDuplicata);       //--base CARTV
                //MarcaFlagBaixaDuplicata();                  //--base EMS
                API_MarcaFlagBaixaDuplicata();                  //--base EMS

            };


            ProcessaIntegracoes(Param); //--base CARTV

            return;
        }




        //------------------------ Deleta a tabela Retorno_EMS na base CARTV -----------------------------
        private void DeletaRetorno()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Text(cnn.Connection, "Delete From RETORNO_EMS");
                //Adp.SelectCommand = cmd;
                //Adp.SelectCommand.Parameters.AddWithValue("@TABELA", "RETORNO_EMS");
                //Adp.SelectCommand.Parameters.AddWithValue("@QUERY", "");
                //Adp.Fill(dtb);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
        }



        //------------------------ Carrega Lista Negra lendo da base EMS -----------------------------
        //private List<RetornarEMSModel> CarregaListaNegra()
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    List<RetornarEMSModel> ListaNegra = new List<RetornarEMSModel>();
        //    String _STR_CGCCPF = "";
        //    try
        //    {
        //        String sSql = "SELECT STR_CGCCPF FROM CLIENTE_NEG with (Nolock)";
        //        sSql += " WHERE FLA_DISPON = 1 AND FLA_DISPON3 = 1 AND (DESTINO = 3 OR DESTINO = 5)";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //        foreach (DataRow drw in dtb.Rows)
        //        {
        //            _STR_CGCCPF = drw["STR_CGCCPF"].ToString().Trim();
        //            _STR_CGCCPF = _STR_CGCCPF.Replace("/","");
        //            _STR_CGCCPF = _STR_CGCCPF.Replace(".", "");
        //            _STR_CGCCPF = _STR_CGCCPF.Replace("-", "");
        //            ListaNegra.Add(new RetornarEMSModel
        //            {
        //                STR_CGCCPF = _STR_CGCCPF   

        //            }); ;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return ListaNegra;
        //}
        private List<RetornarEMSModel> API_CarregaListaNegra()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            List<RetornarEMSModel> ListaNegra = new List<RetornarEMSModel>();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/blacklist";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (!response.IsSuccessful || response.StatusCode != HttpStatusCode.OK)
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_CarregaListaNegra:" + message);
                }

                //GuardaSEQ_WT_DOCTO = clsLib.GetJsonItem(response.Content, "SEQ_WT_DOCTO").ConvertToInt32();
                //object jsonObject = JsonConvert.DeserializeObject(response.Content);
                List<objListaNegraModel> obj = JsonConvert.DeserializeObject<List<objListaNegraModel>>(response.Content);
                for (int i = 0; i < obj.Count; i++)
                {
                    ListaNegra.Add(new RetornarEMSModel
                    {
                        STR_CGCCPF = obj[i].STR_CGCCPF
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ListaNegra;
        }
        //---------------------Insere Lista Negra na base CARTV---------------------------
        private void InsereListaNegra(List<RetornarEMSModel> pLista)
        {
            for (int i = 0; i < pLista.Count; i++)
            {
                clsConexao cnn = new clsConexao(this.Credential);
                cnn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter();
                DataTable dtb = new DataTable("dtb");
                SimLib clsLib = new SimLib();
                try
                {
                    SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Insere_ListaNegra");
                    Adp.SelectCommand = cmd;
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Retorno", 0);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Campo_Chave", pLista[i].STR_CGCCPF);
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
            }
        }


        //------------------------ Marca Flag de Lista Negra na base EMS -----------------------------
        //private void MarcaFlagListaNegra()
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    try
        //    {
        //        String sSql = "UPDATE CLIENTE_NEG SET FLA_DISPON3 = 2";
        //        sSql += " WHERE FLA_DISPON = 1 AND FLA_DISPON3 = 1 AND (DESTINO = 3 or DESTINO = 5)";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //}

        private void API_MarcaFlagListaNegra()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/blacklist/atualizar";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || response.StatusCode != HttpStatusCode.OK)
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("MarcaFlagListaNegra:" + message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //------------------------ Carrega Numeracao Fatura lendo da base EMS -----------------------------
        //private List<RetornarEMSModel> CarregaNumeracaoFatura(RetornarEMSModel pParam)
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    List<RetornarEMSModel> NumeracaoFatura = new List<RetornarEMSModel>();
        //    try
        //    {
        //        String sSql = "SELECT NR_FATURA, NR_NOTA_ENT_FUT, COD_ESTABEL, REC_ORIGEM";
        //        sSql += " FROM WT_DOCTO WITH (NOLOCK)";
        //        sSql += " WHERE REC_ORIGEM = 3";
        //        sSql += " AND FLA_DISPON = 2";
        //        sSql += " AND dbo.faixa_datas(" + pParam.Data_Inicial.ConvertToDatetime().ToString("yyyy-MM-dd") + "," + pParam.Data_Final.ConvertToDatetime().ToString("yyyy-MM/dd") + ") like '%'" + "DAT_TRANSM" + "'%'";
        //        sSql += " AND NR_NOTA_ENT_FUT is not null";
        //        sSql += " AND COD_ESTABEL = '" + pParam.Cod_Empresa_Faturamento + "'";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //        foreach (DataRow drw in dtb.Rows)
        //        {
        //            NumeracaoFatura.Add(new RetornarEMSModel
        //            {
        //                NR_FATURA = drw["NR_FATURA"].ToString().Trim(),
        //                NR_NOTA_ENT_FUT = drw["NR_NOTA_ENT_FUT"].ToString().Trim(),
        //                COD_ESTABEL = drw["COD_ESTABEL"].ToString().Trim(),
        //                REC_ORIGEM = drw["REC_ORIGEM"].ToString().Trim()
        //            });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return NumeracaoFatura;
        //}
        private List<RetornarEMSModel> API_CarregaNumeracaoFatura(RetornarEMSModel pParam)
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            List<RetornarEMSModel> NumeracaoFatura = new List<RetornarEMSModel>();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

            



            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/selectnumeracao";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            ParamNumeracaoFaturaModel Parametros = new ParamNumeracaoFaturaModel();
            Parametros.cod_estabel = pParam.Cod_Empresa_Faturamento;
            Parametros.dat_transm_inicial = pParam.Data_Inicial.ConvertToDatetime().ToString("dd/MM/yyyy");
            Parametros.dat_transm_final = pParam.Data_Final.ConvertToDatetime().ToString("dd/MM/yyyy");
            String json = JsonConvert.SerializeObject(Parametros);

            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (!response.IsSuccessful || response.StatusCode != HttpStatusCode.OK)
                {
                    message = clsLib.GetJsonItem(response.Content, "MESSENGER");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_CarregaNumeracaoFatura:" + message);
                }

                //GuardaSEQ_WT_DOCTO = clsLib.GetJsonItem(response.Content, "SEQ_WT_DOCTO").ConvertToInt32();
                //object jsonObject = JsonConvert.DeserializeObject(response.Content);
                List<objNumeracaoFaturaModel> obj = JsonConvert.DeserializeObject<List<objNumeracaoFaturaModel>>(response.Content);
                for (int i = 0; i < obj.Count; i++)
                {
                    NumeracaoFatura.Add(new RetornarEMSModel()
                    {
                        NR_FATURA = obj[i].NR_FATURA,
                        NR_NOTA_ENT_FUT = obj[i].NR_NOTA_ENT_FUT,
                        COD_ESTABEL = obj[i].COD_ESTABEL,
                        REC_ORIGEM = obj[i].REC_ORIGEM,
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NumeracaoFatura;
        }

        //---------------------Insere Numeracao Fatura na base CARTV---------------------------
        private void InsereNumeracaoFatura(List<RetornarEMSModel> pNumeracao)
        {
            for (int i = 0; i < pNumeracao.Count; i++)
            {
                clsConexao cnn = new clsConexao(this.Credential);
                cnn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter();
                DataTable dtb = new DataTable("dtb");
                SimLib clsLib = new SimLib();
                try
                {
                    SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Insere_NumeracaoFatura");
                    Adp.SelectCommand = cmd;
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Retorno", 1);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Campo_Chave", pNumeracao[i].NR_FATURA);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Campo_Chave2", pNumeracao[i].NR_NOTA_ENT_FUT);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Estabelecimento", pNumeracao[i].COD_ESTABEL);
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
            }
        }


        //------------------------ Carrega Baixa Duplicata lendo da base EMS -----------------------------
        //private List<RetornarEMSModel> CarregaBaixaDuplicata()
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    List<RetornarEMSModel> BaixaDuplicata = new List<RetornarEMSModel>();
        //    try
        //    {
        //        String sSql = "SELECT NUM_NOTA, SEQ_PARCELA, CONVERT(CHAR(10),DAT_PAGAM,103) AS DAT_PAGAM, VLR_PAGO, VLR_JUROS";
        //        sSql += " FROM BAIXA_FAT WITH (NOLOCK)";
        //        sSql += " WHERE ORIGEM = 1 AND DESTINO = 3 AND FLA_DISPON = 1";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //        foreach (DataRow drw in dtb.Rows)
        //        {
        //            BaixaDuplicata.Add(new RetornarEMSModel
        //            {
        //                NUM_NOTA = drw["NUM_NOTA"].ToString().Trim(),
        //                SEQ_PARCELA = drw["SEQ_PARCELA"].ToString().Trim(),
        //                DAT_PAGAM = drw["DAT_PAGAM"].ToString().Trim(),
        //                VLR_PAGO = drw["VLR_PAGO"].ToString().Trim(),
        //                VLR_JUROS = drw["VLR_JUROS"].ToString().Trim()
        //            });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return BaixaDuplicata;
        //}

        private List<RetornarEMSModel> API_CarregaBaixaDuplicata()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            List<RetornarEMSModel> BaixaDuplicata = new List<RetornarEMSModel>();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/selectbaixas";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (!response.IsSuccessful || response.StatusCode != HttpStatusCode.OK)
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_CarregaBaixaDuplicata:" + message);
                }

                List<objBaixaDuplicataModel> obj = JsonConvert.DeserializeObject<List<objBaixaDuplicataModel>>(response.Content);
                for (int i = 0; i < obj.Count; i++)
                {
                    BaixaDuplicata.Add(new RetornarEMSModel()
                    {
                        NUM_NOTA = obj[i].NUM_NOTA,
                        SEQ_PARCELA = obj[i].SEQ_PARCELA,
                        DAT_PAGAM = obj[i].DAT_PAGAM,
                        VLR_PAGO = obj[i].VLR_PAGO,
                        VLR_JUROS = obj[i].VLR_JUROS
                    });
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return BaixaDuplicata;
        }

        //---------------------Insere Baixa Duplicata na base CARTV---------------------------
        private void InsereBaixaDuplicata(List<RetornarEMSModel> pBaixa)
        {
            for (int i = 0; i < pBaixa.Count; i++)
            {
                clsConexao cnn = new clsConexao(this.Credential);
                cnn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter();
                DataTable dtb = new DataTable("dtb");
                SimLib clsLib = new SimLib();
                try
                {
                    SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Insere_BaixaDuplicata");
                    Adp.SelectCommand = cmd;
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Retorno", 2);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Campo_Chave", pBaixa[i].NUM_NOTA);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Campo_Chave2", pBaixa[i].SEQ_PARCELA);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Pagamento", pBaixa[i].DAT_PAGAM.ConvertToDatetime());
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Valor_Pago", pBaixa[i].VLR_PAGO);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Valor_Juros", pBaixa[i].VLR_JUROS);
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
            }
        }


        //------------------------ Marca Flag da Baixa Duplicata na base EMS -----------------------------
        //private void MarcaFlagBaixaDuplicata()
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    try
        //    {
        //        String sSql = "UPDATE BAIXA_FAT SET FLA_DISPON = 2";
        //        sSql += " WHERE ORIGEM = 1 AND DESTINO = 3 AND FLA_DISPON = 1";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //}

        private void API_MarcaFlagBaixaDuplicata()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/updateflagbaixas";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || response.StatusCode != HttpStatusCode.OK)
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_MarcaFlagBaixaDuplicata:" + message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //------------------------ Processa Integrações na base CARTV -----------------------------
        private void ProcessaIntegracoes(RetornarEMSModel pParam)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Retorno_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", pParam.Cod_Empresa_Faturamento);
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
        }


    }
}
