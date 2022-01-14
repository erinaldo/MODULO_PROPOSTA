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
    public partial class IntegrarEMS
    {
        //-----------------------------Carregar Faturas/Boletos ----------------------------------
        public DataTable CarregarFaturasBoletos(IntegrarEMSModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                if (Param.Indica_Faturas)
                {
                    SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Filtra_Faturas_Ems");
                    Adp.SelectCommand = cmd;
                }
                else
                {
                    SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Filtra_Boletos_Ems");
                    Adp.SelectCommand = cmd;
                }
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Usuarioapp", this.CurrentUser);

                if (Param.Indica_Boletos && Param.Indica_Mensal)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Tipo", 1);
                }
                //-- manda 0 para Boletos-Diaria e para Faturas
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Tipo", 0);
                }
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", Param.Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia", clsLib.CompetenciaInt(Param.Competencia));
                if (String.IsNullOrEmpty(Param.Numero_Negociacao))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Negociacao", DBNull.Value);
                }
                else
                { 
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Negociacao", Param.Numero_Negociacao);
                }
                if (String.IsNullOrEmpty(Param.Nota_Fiscal))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Fatura", DBNull.Value);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Fatura", Param.Nota_Fiscal);
                }
                if (Param.Indica_Todos)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Todos", 1);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Todos", 0);
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

        //-------------------------Carregar Criticas--------------------------------------
        public List<CriticaEMSModel> CriticasEMSCarregar(CriticaEMSModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<CriticaEMSModel> Retorno = new List<CriticaEMSModel>();
            try
            {
                String sSql = "Select distinct Cod_EmpFat, Id_Usuario, Data_Consistencia, Num_Fatura, Negociacao, Descricao From Log_Consistencias_EMS with (Nolock)";
                sSql += " Where Cod_EmpFat = '" + Param.Cod_Emp_Fat_Crit + "'";
                sSql += " order by Num_Fatura, Descricao";
                SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Retorno.Add(new CriticaEMSModel
                    {
                        Cod_Emp_Fat_Crit = drw["Cod_EmpFat"].ToString().Trim(),
                        Usuario = drw["Id_Usuario"].ToString().Trim(),
                        Data_Integracao = drw["Data_Consistencia"].ToString().Trim(),
                        Numero_Fatura = drw["Num_Fatura"].ToString().Trim(),
                        Numero_Negociacao = drw["Negociacao"].ToString().Trim(),
                        Msg_Critica = drw["Descricao"].ToString().Trim()
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
            return Retorno;
        }

        //------------------------ Verificar se Emp.Fat está parametrizada-----------------------------
        public Int32 VerificarEmpresaParametrizada(String pCodEmpresaFaturamento)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Int32 GuardaOrigem = 0;
            try
            {
                String sSql = "Select Origem From Parametros_Ems with (Nolock)";
                sSql += " Where Cod_Empresa_Sctv = '" + pCodEmpresaFaturamento + "'";
                SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    GuardaOrigem = dtb.Rows[0]["Origem"].ToString().ConvertToInt32();
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
            return GuardaOrigem;
        }


        //------------------------ Obtém o sequencial SEQ_WT_DOCTO de Faturas na base EMS -----------------------------
        private Int32 ObterSequencialSEQ_WT_DOCTO(String pCodEmpresa)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Int32  GuardaSEQ_WT_DOCTO = 0;
            Int32 Guarda_Origem = this.VerificarEmpresaParametrizada(pCodEmpresa);
            try
            {
                String sSql = "Select MAX(SEQ_WT_DOCTO) AS SEQ_WT_DOCTO From WT_DOCTO with (Nolock)";
                sSql += " Where REC_ORIGEM = '" + Guarda_Origem + "'";
                SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    GuardaSEQ_WT_DOCTO = dtb.Rows[0]["SEQ_WT_DOCTO"].ToString().ConvertToInt32();  
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
            return GuardaSEQ_WT_DOCTO;
        }
        private Int32 API_ObterSequencialSEQ_WT_DOCTO(String pCodEmpresa)
        {
            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            Int32 GuardaSEQ_WT_DOCTO = 0;

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/selectseqfatura";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_ObterSequencialSEQ_WT_DOCTO:" + message);
                }
                GuardaSEQ_WT_DOCTO = clsLib.GetJsonItem(response.Content, "SEQ_WT_DOCTO").ConvertToInt32();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return GuardaSEQ_WT_DOCTO;
        }
        public Int32 TESTEAPIEMS()
        {
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            Int32 GuardaSEQ_WT_DOCTO = 0;
            try
            {
                var client = new RestClient("https://api.datasul.sjcc.dev/api/v2/selectseqfatura");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJkYXRhIjoiMjAyMS0xMS0xMCAxNTo1OTowMSIsIm9yaWdlbSI6MywiY2xpZW50ZSI6ImNhcnR2In0.JiDt0z6I7wc0etQ0Icd_iFRYJXn2iIdXLlB4F_SgBxQ");
                request.AddHeader("Accept", "*/*");
                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                GuardaSEQ_WT_DOCTO = clsLib.GetJsonItem(response.Content, "SEQ_WT_DOCTO").ConvertToInt32();



                //Console.WriteLine(response.Content);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return GuardaSEQ_WT_DOCTO;
        }


        //--------------------------------Processa Faturas -----------------------------------------
        public Boolean ProcessarFaturas(List<IntegrarEMSModel> Param)
        {
            Boolean Indica_Critica = false;
            List<IntegrarFaturasModel> Faturas = CarregarFaturas(Param);
            List<IntegrarItensModel> Itens = CarregarItens(Param[0].Cod_Empresa_Faturamento);

            List<IntegrarDuplicatasModel> Duplicatas = new List<IntegrarDuplicatasModel>();
            List<IntegrarRepresentantesModel> Representantes = new List<IntegrarRepresentantesModel>();
            List<IntegrarClientesModel> Clientes = new List<IntegrarClientesModel>();

            if (Faturas[0].indica_critica || Itens[0].indica_critica)
            {
                Indica_Critica = true;
            }
            if (! Indica_Critica)
            {
                Duplicatas = CarregarDuplicatas(Param[0].Cod_Empresa_Faturamento);
                Representantes = CarregarRepresentantes(Param[0].Cod_Empresa_Faturamento);
                Clientes = CarregarClientes(Param[0].Cod_Empresa_Faturamento, Param[0].Indica_Chamada);

                //--para os testes
                InsereFatura(Faturas);
                InsereItem(Itens);
                InsereDuplicata(Duplicatas);
                InsereRepresentante(Representantes);
                InsereCliente(Clientes);


            }


            return Indica_Critica;
        }


        //--------------------------------Carregar Faturas -----------------------------------------
        private List<IntegrarFaturasModel> CarregarFaturas(List<IntegrarEMSModel> Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarFaturasModel> Faturas = new List<IntegrarFaturasModel>();
            string xmlFaturas = null;
            if (Param.Count>0)
            {
                xmlFaturas = clsLib.SerializeToString(Param);
            }
            //Int32 Guarda_SEQ_WT_DOCTO = this.ObterSequencialSEQ_WT_DOCTO(Param[0].Cod_Empresa_Faturamento);
           Int32 Guarda_SEQ_WT_DOCTO = this.API_ObterSequencialSEQ_WT_DOCTO(Param[0].Cod_Empresa_Faturamento);
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Integracao_Fatura_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", Param[0].Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_FATURAS_xml", xmlFaturas);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero", Guarda_SEQ_WT_DOCTO);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Faturas.Add(new IntegrarFaturasModel
                    {
                        rec_origem = drw["REC_ORIGEM"].ToString().Trim(),
                        des_destino = drw["DES_DESTINO"].ToString().Trim(),
                        dat_transm = drw["DAT_TRANSM"].ToString().Trim(),
                        usu_transm = drw["USU_TRANSM"].ToString().Trim(),
                        fla_dispon = drw["FLA_DISPON"].ToString().Trim(),
                        cod_estabel = drw["COD_ESTABEL"].ToString().Trim(),
                        serie = drw["SERIE"].ToString().Trim(),
                        nr_nota = drw["NR_NOTA"].ToString().Trim(),
                        nome_abrev = drw["NOME_ABREV"].ToString().Trim(),
                        dt_emis_nota = drw["DT_EMIS_NOTA"].ToString().Trim(),
                        cod_cond_pag = drw["COD_COND_PAG"].ToString().Trim(),
                        endereco = drw["ENDERECO"].ToString().Trim(),
                        bairro = drw["BAIRRO"].ToString().Trim(),
                        cidade = drw["CIDADE"].ToString().Trim(),
                        estado = drw["ESTADO"].ToString().Trim(),
                        cep = drw["CEP"].ToString().Trim(),
                        pais = drw["PAIS"].ToString().Trim(),
                        ins_estadual = drw["INS_ESTADUAL"].ToString().Trim(),
                        nr_fatura = drw["NR_FATURA"].ToString().Trim(),
                        nat_operacao = drw["NAT_OPERACAO"].ToString().Trim(),
                        cod_portador = drw["COD_PORTADOR"].ToString().Trim(),
                        dt_prvenc = drw["DT_PRVENC"].ToString().Trim(),
                        cd_vendedor = drw["CD_VENDEDOR"].ToString().Trim(),
                        modalidade = drw["MODALIDADE"].ToString().Trim(),
                        observ_nota = drw["OBSERV_NOTA"].ToString().Trim(),
                        vl_acum_dup = drw["VL_ACUM_DUP"].ToString().ConvertToDouble(),
                        no_ab_reppri = drw["NO_AB_REPPRI"].ToString().Trim(),
                        dt_trans = drw["DT_TRANS"].ToString().Trim(),
                        mo_codigo = drw["MO_CODIGO"].ToString().Trim(),
                        cgccpf_cli = drw["CGCCPF_CLI"].ToString().Trim(),
                        nr_nota_ext = drw["NR_NOTA_EXT"].ToString().Trim(),
                        cidade_cob = drw["CIDADE_COB"].ToString().Trim(),
                        cep_cob = drw["CEP_COB"].ToString().Trim(),
                        endereco_cob = drw["ENDERECO_COB"].ToString().Trim(),
                        bairro_cob = drw["BAIRRO_COB"].ToString().Trim(),
                        estado_cob = drw["ESTADO_COB"].ToString().Trim(),
                        nome_cob = drw["NOME_COB"].ToString().Trim(),
                        nr_tab_finan = drw["NR_TAB_FINAN"].ToString().Trim(),
                        seq_wt_docto = drw["SEQ_WT_DOCTO"].ToString().Trim().ConvertToInt32(),
                        indica_critica = drw["INDICA_CRITICA"].ToString().ConvertToBoolean()
                    }) ;
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
            return Faturas;
        }
        //--------------------------------Carregar Itens -----------------------------------------
        private List<IntegrarItensModel> CarregarItens(String pCod_Empresa_Faturamento)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarItensModel> Itens = new List<IntegrarItensModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Integracao_Item_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", pCod_Empresa_Faturamento);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Itens.Add(new IntegrarItensModel
                    {
                        rec_origem = drw["REC_ORIGEM"].ToString().Trim(),
                        des_destino = drw["DES_DESTINO"].ToString().Trim(),
                        dat_transm = drw["DAT_TRANSM"].ToString().Trim(),
                        usu_transm = drw["USU_TRANSM"].ToString().Trim(),
                        fla_dispon = drw["FLA_DISPON"].ToString().Trim(),
                        nr_sequencia = drw["NR_SEQUENCIA"].ToString().Trim(),
                        it_codigo = drw["IT_CODIGO"].ToString().Trim(),
                        quantidade = drw["QUANTIDADE"].ToString().ConvertToInt32(),
                        per_des_item = drw["PER_DES_ITEM"].ToString().ConvertToDouble(),
                        narrativa = drw["NARRATIVA"].ToString().Trim(),
                        vl_preori_ped = drw["VL_PREORI_PED"].ToString().ConvertToDouble(),
                        val_pct_desconto_tab_preco = drw["VAL_PCT_DESCONTO_TAB_PRECO"].ToString().ConvertToDouble(),
                        seq_wt_it_docto = drw["SEQ_WT_IT_DOCTO"].ToString().Trim(),
                        seq_wt_docto = drw["SEQ_WT_DOCTO"].ToString().Trim(),
                        indica_critica = drw["INDICA_CRITICA"].ToString().ConvertToBoolean()
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
            return Itens;
        }

        //--------------------------------Carregar Duplicatas -----------------------------------------
        public List<IntegrarDuplicatasModel> CarregarDuplicatas(String pCod_Empresa_Faturamento)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarDuplicatasModel> Retorno = new List<IntegrarDuplicatasModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Integracao_Duplicata_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", pCod_Empresa_Faturamento);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Retorno.Add(new IntegrarDuplicatasModel
                    {
                        rec_origem = drw["REC_ORIGEM"].ToString().Trim(),
                        des_destino = drw["DES_DESTINO"].ToString().Trim(),
                        dat_transm = drw["DAT_TRANSM"].ToString().Trim(),
                        usu_transm = drw["USU_TRANSM"].ToString().Trim(),
                        fla_dispon = drw["FLA_DISPON"].ToString().Trim(),
                        parcela = drw["PARCELA"].ToString().Trim(),
                        dt_venciment = drw["DT_VENCIMENT"].ToString().Trim(),
                        vl_parcela = drw["VL_PARCELA"].ToString().ConvertToDouble(),
                        vl_desconto = drw["VL_DESCONTO"].ToString().ConvertToDouble(),
                        vl_comis = drw["VL_COMIS"].ToString().ConvertToDouble(),
                        vl_acum_dup = drw["VL_ACUM_DUP"].ToString().ConvertToDouble(),
                        cod_vencto = drw["COD_VENCTO"].ToString().Trim(),
                        cod_esp = drw["COD_ESP"].ToString().Trim(),
                        seq_wt_docto = drw["SEQ_WT_DOCTO"].ToString().Trim(),
                        nr_seq_nota = drw["NR_SEQ_NOTA"].ToString().Trim(),
                        vl_base_comis = drw["VL_BASE_COMIS"].ToString().ConvertToDouble(),
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
            return Retorno;
        }


        //--------------------------------Carregar Representantes -----------------------------------------
        public List<IntegrarRepresentantesModel> CarregarRepresentantes(String pCod_Empresa_Faturamento)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarRepresentantesModel> Retorno = new List<IntegrarRepresentantesModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Integracao_Representante_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", pCod_Empresa_Faturamento);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Retorno.Add(new IntegrarRepresentantesModel
                    {
                        rec_origem = drw["REC_ORIGEM"].ToString().Trim(),
                        des_destino = drw["DES_DESTINO"].ToString().Trim(),
                        dat_transm = drw["DAT_TRANSM"].ToString().Trim(),
                        usu_transm = drw["USU_TRANSM"].ToString().Trim(),
                        fla_dispon = drw["FLA_DISPON"].ToString().Trim(),
                        cod_rep = drw["COD_REP"].ToString().Trim(),
                        perc_comis = drw["PERC_COMIS"].ToString().ConvertToDouble(),
                        comis_emis = drw["COMIS_EMIS"].ToString().Trim(),
                        seq_wt_docto = drw["SEQ_WT_DOCTO"].ToString().Trim(),
                        nr_seq_nota = drw["NR_SEQ_NOTA"].ToString().Trim(),
                        sequencia = drw["SEQUENCIA"].ToString().Trim(),
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
            return Retorno;
        }


        //------------------------ Obtém o sequencial NR_SEQUENCIA de Clientes na base EMS -----------------------------
        public Int32 ObterSequencialNR_SEQUENCIA(String pCodEmpresa)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Int32 Guarda_NR_SEQUENCIA = 0;
            Int32 Guarda_Origem = this.VerificarEmpresaParametrizada(pCodEmpresa);
            try
            {
                //String sSql = "Select MAX(NR_SEQUENCIA) AS NR_SEQUENCIA From CLIENTE_INT with (Nolock)";
                //sSql += " Where ORIGEM = '" + Guarda_Origem + "'";
                //SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
                //SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                //Adp.Fill(dtb);
                //if (dtb.Rows.Count > 0)
                //{
                //    Guarda_NR_SEQUENCIA = dtb.Rows[0]["NR_SEQUENCIA"].ToString().ConvertToInt32();
                //}
                Guarda_NR_SEQUENCIA = API_ObterSequencialNR_SEQUENCIA();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Guarda_NR_SEQUENCIA;
        }
        
        private Int32 API_ObterSequencialNR_SEQUENCIA()
        {
            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            Int32 Guarda_NR_SEQUENCIA = 0;

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/selectseqcliente";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_ObterSequencialNR_SEQUENCIA:"+ message);
                }
                Guarda_NR_SEQUENCIA = clsLib.GetJsonItem(response.Content, "NR_SEQUENCIA").ConvertToInt32();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Guarda_NR_SEQUENCIA;
        }
        //--------------------------------Carregar Clientes -----------------------------------------
        public List<IntegrarClientesModel> CarregarClientes(String pCod_Empresa_Faturamento, Byte pIndica_Chamada)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarClientesModel> Retorno = new List<IntegrarClientesModel>();
            Int32 Guarda_NR_SEQUENCIA = ObterSequencialNR_SEQUENCIA(pCod_Empresa_Faturamento);
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Integracao_Terceiro_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", pCod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Chamada", pIndica_Chamada); //--0=Faturas e 1=Boletos
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_SEQUENCIA", Guarda_NR_SEQUENCIA);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Retorno.Add(new IntegrarClientesModel
                    {
                        origem = drw["ORIGEM"].ToString().Trim(),
                        destino = drw["DESTINO"].ToString().Trim(),
                        dat_transm = drw["DAT_TRANSM"].ToString().Trim(),
                        usu_transm = drw["USU_TRANSM"].ToString().Trim(),
                        fla_dispon = drw["FLA_DISPON"].ToString().Trim(),
                        str_nom_abrev = drw["STR_NOM_ABREV"].ToString().Trim(),
                        str_cgccpf = drw["STR_CGCCPF"].ToString().Trim(),
                        ind_identific = drw["IND_IDENTIFIC"].ToString().Trim(),
                        ind_natureza = drw["IND_NATUREZA"].ToString().Trim(),
                        str_nome = drw["STR_NOME"].ToString().Trim(),
                        str_endereco = drw["STR_ENDERECO"].ToString().Trim(),
                        str_bairro = drw["STR_BAIRRO"].ToString().Trim(),
                        str_cidade = drw["STR_CIDADE"].ToString().Trim(),
                        str_uf = drw["STR_UF"].ToString().Trim(),
                        str_cep = drw["STR_CEP"].ToString().Trim(),
                        str_cx_postal = drw["STR_CX_POSTAL"].ToString().Trim(),
                        str_pais = drw["STR_PAIS"].ToString().Trim(),
                        str_inscr_est = drw["STR_INSCR_EST"].ToString().Trim(),
                        str_ramo_ativ = drw["STR_RAMO_ATIV"].ToString().Trim(),
                        str_telefax = drw["STR_TELEFAX"].ToString().Trim(),
                        str_ramal_tel = drw["STR_RAMAL_TEL"].ToString().Trim(),
                        str_telex = drw["STR_TELEX"].ToString().Trim(),
                        dat_implanta = drw["DAT_IMPLANTA"].ToString().Trim(),
                        cod_represent = drw["COD_REPRESENT"].ToString().Trim(),
                        ind_loc_av_cr = drw["IND_LOC_AV_CR"].ToString().Trim(),
                        cod_grupo_cli = drw["COD_GRUPO_CLI"].ToString().Trim(),
                        vlr_limite_cr = drw["VLR_LIMITE_CR"].ToString().ConvertToDouble(),
                        cod_portador = drw["COD_PORTADOR"].ToString().Trim(),
                        ind_modal = drw["IND_MODAL"].ToString().Trim(),
                        ind_fat_parc = drw["IND_FAT_PARC"].ToString().Trim(),
                        ind_credito = drw["IND_CREDITO"].ToString().Trim(),
                        ind_aval_cred = drw["IND_AVAL_CRED"].ToString().Trim(),
                        str_nat_oper = drw["STR_NAT_OPER"].ToString().Trim(),
                        ind_meio_ped = drw["IND_MEIO_PED"].ToString().Trim(),
                        str_nome_fant = drw["STR_NOME_FANT"].ToString().Trim(),
                        str_modem = drw["STR_MODEM"].ToString().Trim(),
                        str_ramal_mod = drw["STR_RAMAL_MOD"].ToString().Trim(),
                        str_agencia = drw["STR_AGENCIA"].ToString().Trim(),
                        ind_bloqueto = drw["IND_BLOQUETO"].ToString().Trim(),
                        ind_etiqueta = drw["IND_ETIQUETA"].ToString().Trim(),
                        ind_valores = drw["IND_VALORES"].ToString().Trim(),
                        ind_aviso_deb = drw["IND_AVISO_DEB"].ToString().Trim(),
                        ind_moda_pref = drw["IND_MODA_PREF"].ToString().Trim(),
                        ind_avaliacao = drw["IND_AVALIACAO"].ToString().Trim(),
                        ind_venc_dom = drw["IND_VENC_DOM"].ToString().Trim(),
                        ind_venc_sab = drw["IND_VENC_SAB"].ToString().Trim(),
                        str_cgc_cob = drw["STR_CGC_COB"].ToString().Trim(),
                        str_cep_cob = drw["STR_CEP_COB"].ToString().Trim(),
                        str_uf_cob = drw["STR_UF_COB"].ToString().Trim(),
                        str_cidad_cob = drw["STR_CIDAD_COB"].ToString().Trim(),
                        str_bairr_cob = drw["STR_BAIRR_COB"].ToString().Trim(),
                        str_ender_cob = drw["STR_ENDER_COB"].ToString().Trim(),
                        str_cx_po_cob = drw["STR_CX_PO_COB"].ToString().Trim(),
                        str_in_es_cob = drw["STR_IN_ES_COB"].ToString().Trim(),
                        str_banco = drw["STR_BANCO"].ToString().Trim(),
                        ind_tipo_reg = drw["IND_TIPO_REG"].ToString().Trim(),
                        ind_venc_fer = drw["IND_VENC_FER"].ToString().Trim(),
                        ind_pagamento = drw["IND_PAGAMENTO"].ToString().Trim(),
                        ind_cobr_desp = drw["IND_COBR_DESP"].ToString().Trim(),
                        str_insc_mun = drw["STR_INSC_MUN"].ToString().Trim(),
                        cod_cond_pag = drw["COD_COND_PAG"].ToString().Trim(),
                        str_fone_1 = drw["STR_FONE_1"].ToString().Trim(),
                        str_fone_2 = drw["STR_FONE_2"].ToString().Trim(),
                        num_mes_inat = drw["NUM_MES_INAT"].ToString().Trim(),
                        str_e_mail = drw["STR_E_MAIL"].ToString().Trim(),
                        ind_aval_emb = drw["IND_AVAL_EMB"].ToString().Trim(),
                        str_conta_cor = drw["STR_CONTA_COR"].ToString().Trim(),
                        nr_sequencia = drw["NR_SEQUENCIA"].ToString().Trim(),
                        fla_dispon_1 = drw["FLA_DISPON_1"].ToString().Trim(),
                        fla_dispon_2 = drw["FLA_DISPON_2"].ToString().Trim(),
                        fla_dispon_3 = drw["FLA_DISPON_3"].ToString().Trim(),
                        fla_dispon_4 = drw["FLA_DISPON_4"].ToString().Trim(),
                        fla_dispon_5 = drw["FLA_DISPON_5"].ToString().Trim(),
                        fla_dispon_6 = drw["FLA_DISPON_6"].ToString().Trim(),
                        fla_dispon_7 = drw["FLA_DISPON_7"].ToString().Trim(),
                        fla_dispon_8 = drw["FLA_DISPON_8"].ToString().Trim(),
                        fla_dispon_9 = drw["FLA_DISPON_9"].ToString().Trim(),
                        fla_dispon_10 = drw["FLA_DISPON_10"].ToString().Trim(),
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
            return Retorno;
        }




        //--------------------------------Processa Boletos -----------------------------------------
        public Boolean ProcessarBoletos(List<IntegrarEMSModel> Param)
        {
            Boolean Indica_Critica = false;
            String Guarda_Estabelecimento;
            String Guarda_Sequencial_Estabelecimento=null;

            Boolean CriticaBoletos = CarregarBoletos(Param);

            List<MatrizEstabelecimentoModel> MatrizEstabelecimentos = new List<MatrizEstabelecimentoModel>();

            List<IntegrarLotesModel> Lotes = new List<IntegrarLotesModel>();
            List<IntegrarItemLotesModel> ItemLotes = new List<IntegrarItemLotesModel>();
            List<IntegrarApropsModel> Aprops = new List<IntegrarApropsModel>();
            List<IntegrarRepresModel> Repres = new List<IntegrarRepresModel>();
            List<IntegrarClientesModel> Clientes = new List<IntegrarClientesModel>();

            if (CriticaBoletos)
            {
                Indica_Critica = true;
            };

            if (!Indica_Critica)
            {
                Lotes = CarregarLotes();

                Guarda_Estabelecimento = Lotes[0].tta_cod_estab;
                //MatrizEstabelecimentos = Carrega_Matriz_Estabelecimento();
                MatrizEstabelecimentos = API_Carrega_Matriz_Estabelecimento();
                for (int i = 0; i < MatrizEstabelecimentos.Count; i++)
                {
                    if (MatrizEstabelecimentos[i].Cod_Estabelecimento == Guarda_Estabelecimento)
                    {
                        Guarda_Sequencial_Estabelecimento = MatrizEstabelecimentos[i].Seq_Estabelecimento;
                        break;
                    }
                }

                ItemLotes = CarregarItemLotes(Guarda_Sequencial_Estabelecimento);
                Aprops = CarregarAprops();
                Repres = CarregarRepres();
                Clientes = CarregarClientes(Param[0].Cod_Empresa_Faturamento, Param[0].Indica_Chamada);

                //--Para os testes foi necessário criar novas tabelas pois a proc Pr_Proposta_Integracao_Antecipado_Ems
                //--utiliza as tabelas já existentes
                InsereLote(Lotes);
                InsereItemLote(ItemLotes);
                InsereAprop(Aprops);
                InsereRepre(Repres);
                InsereCliente(Clientes);
            }
            return Indica_Critica;
        }



        //------------------------ Obtém o número de lote COD_LOTE de Boletos na base EMS -----------------------------
        //private Int32 ObterNumeroLoteCOD_LOTE(String pCodEmpresa)
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    Int32 Guarda_COD_LOTE = 0;
        //    Int32 Guarda_Origem = this.VerificarEmpresaParametrizada(pCodEmpresa);
        //    try
        //    {
        //        String sSql = "Select MAX(COD_LOTE) AS COD_LOTE From INTEGR_ACR_LOTE_IMPL with (Nolock)";
        //        sSql += " Where TTV_REC_ORIGEM = '" + Guarda_Origem + "'";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //        if (dtb.Rows.Count > 0)
        //        {
        //            Guarda_COD_LOTE = dtb.Rows[0]["COD_LOTE"].ToString().ConvertToInt32();
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
        //    return Guarda_COD_LOTE;
        //}
        private Int32 API_ObterNumeroLoteCOD_LOTE(String pCodEmpresa)
        {
            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            Int32 Guarda_COD_LOTE = 0;
            //Int32 Guarda_Origem = this.VerificarEmpresaParametrizada(pCodEmpresa);

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/selectloteboleto";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_ObterNumeroLoteCOD_LOTE:" + message);
                }
                Guarda_COD_LOTE = clsLib.GetJsonItem(response.Content, "COD_LOTE").ConvertToInt32();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Guarda_COD_LOTE;
        }


        //------------------------ Carrega Matriz Estabelecimento pegando dados na base EMS -----------------------------
        //private List<MatrizEstabelecimentoModel> Carrega_Matriz_Estabelecimento()
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    List<MatrizEstabelecimentoModel> Estabelecimentos = new List<MatrizEstabelecimentoModel>();
        //    try
        //    {
        //        String sSql = "Select l.TTA_COD_ESTAB, MAX(i.TTA_COD_TIT_ACR) as TTA_COD_TIT_ACR";
        //        sSql += " From INTEGR_ACR_LOTE_IMPL l with (Nolock), INTEGR_ACR_ITEM_LOTE_IMPL i with (Nolock)";
        //        sSql += " Where l.TTV_REC_ORIGEM = i.TTV_REC_ORIGEM And l.COD_LOTE = i.COD_LOTE And l.TTV_REC_ORIGEM in (3, 7, 10)";
        //        sSql += " Group By l.TTA_COD_ESTAB";
        //        SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
        //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //        Adp.Fill(dtb);
        //        foreach (DataRow drw in dtb.Rows)
        //        {
        //            Estabelecimentos.Add(new MatrizEstabelecimentoModel
        //            {
        //                Cod_Estabelecimento = drw["TTA_COD_ESTAB"].ToString().Trim(),
        //                Seq_Estabelecimento = drw["TTA_COD_TIT_ACR"].ToString().Trim()
        //        });
        //    }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return Estabelecimentos;
        //}
        private List<MatrizEstabelecimentoModel> API_Carrega_Matriz_Estabelecimento()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            List<MatrizEstabelecimentoModel> Estabelecimentos = new List<MatrizEstabelecimentoModel>();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/selectestabelecimentos";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", Token);
                var body = @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_Carrega_Matriz_Estabelecimento:" + message);
                }

                //GuardaSEQ_WT_DOCTO = clsLib.GetJsonItem(response.Content, "SEQ_WT_DOCTO").ConvertToInt32();
                //object jsonObject = JsonConvert.DeserializeObject(response.Content);
                List<objEstabelecimentoModel> obj = JsonConvert.DeserializeObject<List<objEstabelecimentoModel>>(response.Content);
                for (int i = 0; i < obj.Count; i++)
                {
                    Estabelecimentos.Add(new MatrizEstabelecimentoModel
                    {
                        Cod_Estabelecimento = obj[i].TTA_COD_ESTAB,
                        Seq_Estabelecimento = obj[i].SEQUENCIA
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Estabelecimentos;
        }

        //--------------------------------Carregar Boletos -----------------------------------------
        private Boolean CarregarBoletos(List<IntegrarEMSModel> Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Boolean CriticaBoletos = false;
            string xmlBoletos = null;
            if (Param.Count > 0)
            {
                xmlBoletos = clsLib.SerializeToString(Param);
            }
            //Int32 Guarda_COD_LOTE = this.ObterNumeroLoteCOD_LOTE(Param[0].Cod_Empresa_Faturamento);
            Int32 Guarda_COD_LOTE = this.API_ObterNumeroLoteCOD_LOTE(Param[0].Cod_Empresa_Faturamento);
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Integracao_Antecipado_Ems");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_UsuarioApp", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", Param[0].Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia", Param[0].Competencia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Tipo", Param[0].Tipo);      //--0=Diária 1=Mensal
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Boletos_xml", xmlBoletos);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Lote", Guarda_COD_LOTE);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    CriticaBoletos = dtb.Rows[0]["Retorno"].ToString().ConvertToBoolean();
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
            return CriticaBoletos;
        }

        //--------------------------------Carregar Lotes -----------------------------------------
        private List<IntegrarLotesModel> CarregarLotes()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarLotesModel> Lotes = new List<IntegrarLotesModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_INTEGR_ACR_LOTE_IMPL");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Usuario_App", this.CurrentUser);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Lotes.Add(new IntegrarLotesModel
                    {
                        ttv_rec_origem = drw["TTV_REC_ORIGEM"].ToString().Trim(),
                        ttv_des_destino = drw["TTV_DES_DESTINO"].ToString().Trim(),
                        ttv_dat_transm = drw["TTV_DAT_TRANSM"].ToString().Trim(),
                        ttv_usu_transm = drw["TTV_USU_TRANSM"].ToString().Trim(),
                        ttv_fla_dispon = drw["TTV_FLA_DISPON"].ToString().Trim(),
                        tta_cod_empresa = drw["TTA_COD_EMPRESA"].ToString().Trim(),
                        tta_cod_estab = drw["TTA_COD_ESTAB"].ToString().Trim(),
                        tta_dat_transacao = drw["TTA_DAT_TRANSACAO"].ToString().Trim(),
                        tta_cod_refer = drw["TTA_COD_REFER"].ToString().Trim(),
                        tta_val_tot_lote_infor_tit_acr = drw["TTA_VAL_TOT_LOTE_INFOR_TIT_ACR"].ToString().ConvertToDouble(),
                        tta_ind_tip_cobr_acr = drw["TTA_IND_TIP_COBR_ACR"].ToString().Trim(),
                        ttv_log_lote_impl_ok = drw["TTV_LOG_LOTE_IMPL_OK"].ToString().Trim(),
                        cod_lote = drw["COD_LOTE"].ToString().Trim(),
                        ttv_cod_erro = drw["TTV_COD_ERRO"].ToString().Trim(),
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
            return Lotes;
        }
        //--------------------------------Carregar ItemLotes -----------------------------------------
        private List<IntegrarItemLotesModel> CarregarItemLotes(String pSequencial)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarItemLotesModel> ItemLotes = new List<IntegrarItemLotesModel>();
            String str_TTA_COD_TIT_ACR = "";
            Int32 int_TTA_COD_TIT_ACR = 0;
            String novo_TTA_COD_TIT_ACR = "";
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_INTEGR_ACR_ITEM_LOTE_IMPL");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Usuario_App", this.CurrentUser);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    str_TTA_COD_TIT_ACR = drw["TTA_COD_TIT_ACR"].ToString().Substring(1, 9);
                    int_TTA_COD_TIT_ACR = str_TTA_COD_TIT_ACR.ConvertToInt32() + pSequencial.Substring(1,9).ConvertToInt32();
                    novo_TTA_COD_TIT_ACR = "S" + int_TTA_COD_TIT_ACR.ToString().PadLeft(9,'0');            // formato S000000000

                    ItemLotes.Add(new IntegrarItemLotesModel
                    {
                        ttv_rec_origem = drw["TTV_REC_ORIGEM"].ToString().Trim(),
                        ttv_des_destino = drw["TTV_DES_DESTINO"].ToString().Trim(),
                        ttv_dat_transm = drw["TTV_DAT_TRANSM"].ToString().Trim(),
                        ttv_usu_transm = drw["TTV_USU_TRANSM"].ToString().Trim(),
                        ttv_fla_dispon = drw["TTV_FLA_DISPON"].ToString().Trim(),
                        tta_num_seq_refer = drw["TTA_NUM_SEQ_REFER"].ToString().Trim(),
                        tta_cod_ser_docto = drw["TTA_COD_SER_DOCTO"].ToString().Trim(),
                        tta_cod_espec_docto = drw["TTA_COD_ESPEC_DOCTO"].ToString().Trim(),
                        tta_cod_tit_acr = novo_TTA_COD_TIT_ACR,
                        tta_cod_parcela = drw["TTA_COD_PARCELA"].ToString().Trim(),
                        tta_cod_indic_econ = drw["TTA_COD_INDIC_ECON"].ToString().Trim(),
                        tta_cod_portador = drw["TTA_COD_PORTADOR"].ToString().Trim(),
                        tta_cod_cart_bcia = drw["TTA_COD_CART_BCIA"].ToString().Trim(),
                        tta_cdn_repres = drw["TTA_CDN_REPRES"].ToString().Trim(),
                        tta_dat_vencto_tit_acr = drw["TTA_DAT_VENCTO_TIT_ACR"].ToString().Trim(),
                        tta_dat_prev_liquidac = drw["TTA_DAT_PREV_LIQUIDAC"].ToString().Trim(),
                        tta_dat_emis_docto = drw["TTA_DAT_EMIS_DOCTO"].ToString().Trim(),
                        tta_val_tit_acr = drw["TTA_VAL_TIT_ACR"].ToString().ConvertToDouble(),
                        tta_val_perc_juros_dia_atr = drw["TTA_VAL_PERC_JUROS_DIA_ATR"].ToString().ConvertToDouble(),
                        tta_val_perc_multa_atr = drw["TTA_VAL_PERC_MULTA_ATR"].ToString().ConvertToDouble(),
                        tta_val_liq_tit_acr = drw["TTA_VAL_LIQ_TIT_ACR"].ToString().ConvertToDouble(),
                        tta_ind_tip_espec_docto = drw["TTA_IND_TIP_ESPEC_DOCTO"].ToString().Trim(),
                        cod_item_lote = drw["COD_ITEM_LOTE"].ToString().Trim(),
                        cod_lote = drw["COD_LOTE"].ToString().Trim(),
                        ttv_cod_erro = drw["TTV_COD_ERRO"].ToString().Trim(),
                        str_cgccpf = drw["STR_CGCCPF"].ToString().Trim(),
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
            return ItemLotes;
        }
        //--------------------------------Carregar Aprops -----------------------------------------
        private List<IntegrarApropsModel> CarregarAprops()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarApropsModel> Aprops = new List<IntegrarApropsModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_INTEGR_ACR_APROP_CTBL_PEND");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_UsuarioApp", this.CurrentUser);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Aprops.Add(new IntegrarApropsModel
                    {
                        ttv_rec_origem = drw["TTV_REC_ORIGEM"].ToString().Trim(),
                        ttv_des_destino = drw["TTV_DES_DESTINO"].ToString().Trim(),
                        ttv_dat_transm = drw["TTV_DAT_TRANSM"].ToString().Trim(),
                        ttv_usu_trans = drw["TTV_USU_TRANS"].ToString().Trim(),
                        ttv_fla_dispon = drw["TTV_FLA_DISPON"].ToString().Trim(),
                        cod_item_lote = drw["COD_ITEM_LOTE"].ToString().Trim(),
                        tta_cod_plano_cta_ctbl = drw["TTA_COD_PLANO_CTA_CTBL"].ToString().Trim(),
                        tta_cod_cta_ctbl = drw["TTA_COD_CTA_CTBL"].ToString().Trim(),
                        tta_cod_unid_negoc = drw["TTA_COD_UNID_NEGOC"].ToString().Trim(),
                        tta_cod_tip_fluxo_financ = drw["TTA_COD_TIP_FLUXO_FINANC"].ToString().Trim(),
                        tta_val_aprop_ctbl = drw["TTA_VAL_APROP_CTBL"].ToString().ConvertToDouble(),
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
            return Aprops;
        }
        //--------------------------------Carregar Repres -----------------------------------------
        private List<IntegrarRepresModel> CarregarRepres()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<IntegrarRepresModel> Repres = new List<IntegrarRepresModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_INTEGR_ACR_REPRES_PEND");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_UsuarioApp", this.CurrentUser);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Repres.Add(new IntegrarRepresModel
                    {
                        ttv_rec_origem = drw["TTV_REC_ORIGEM"].ToString().Trim(),
                        ttv_des_destino = drw["TTV_DES_DESTINO"].ToString().Trim(),
                        ttv_dat_transm = drw["TTV_DAT_TRANSM"].ToString().Trim(),
                        ttv_usu_transm = drw["TTV_USU_TRANSM"].ToString().Trim(),
                        ttv_fla_dispon = drw["TTV_FLA_DISPON"].ToString().Trim(),
                        cod_item_lote = drw["COD_ITEM_LOTE"].ToString().Trim(),
                        tta_val_perc_comis_repres = drw["TTA_VAL_PERC_COMIS_REPRES"].ToString().ConvertToDouble(),
                        tta_val_perc_comis_repres_emis = drw["TTA_VAL_PERC_COMIS_REPRES_EMIS"].ToString().ConvertToDouble(),
                        tta_val_perc_comis_abat = drw["TTA_VAL_PERC_COMIS_ABAT"].ToString().ConvertToDouble(),
                        tta_val_perc_comis_desc = drw["TTA_VAL_PERC_COMIS_DESC"].ToString().ConvertToDouble(),
                        tta_val_perc_comis_juros = drw["TTA_VAL_PERC_COMIS_JUROS"].ToString().ConvertToDouble(),
                        tta_val_perc_comis_multa = drw["TTA_VAL_PERC_COMIS_MULTA"].ToString().ConvertToDouble(),
                        tta_val_perc_comis_acerto_val = drw["TTA_VAL_PERC_COMIS_ACERTO_VAL"].ToString().ConvertToDouble(),
                        tta_log_comis_repres_proporc = drw["TTA_LOG_COMIS_REPRES_PROPORC"].ToString().Trim(),
                        tta_ind_tip_comis = drw["TTA_IND_TIP_COMIS"].ToString().Trim(),
                        tta_cdn_repres = drw["TTA_CDN_REPRES"].ToString().Trim(),
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
            return Repres;
        }

        //---Insere cada FATURA retornada na nossa tabela EMS
        //private void InsereFatura(List<IntegrarFaturasModel> pFaturas)
        //{
        //    for (int i = 0; i < pFaturas.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Fatura");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_REC_ORIGEM", pFaturas[i].REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_DES_DESTINO", pFaturas[i].DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_DAT_TRANSM", pFaturas[i].DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_USU_TRANSM", pFaturas[i].USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_FLA_DISPON", pFaturas[i].FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_COD_ESTABEL", pFaturas[i].COD_ESTABEL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_SERIE", pFaturas[i].SERIE);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NR_NOTA", pFaturas[i].NR_NOTA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NOME_ABREV", pFaturas[i].NOME_ABREV);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_DT_EMIS_NOTA", pFaturas[i].DT_EMIS_NOTA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_COD_COND_PAG", pFaturas[i].COD_COND_PAG.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_ENDERECO", pFaturas[i].ENDERECO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_BAIRRO", pFaturas[i].BAIRRO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_CIDADE", pFaturas[i].CIDADE);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_ESTADO", pFaturas[i].ESTADO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_CEP", pFaturas[i].CEP);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_PAIS", pFaturas[i].PAIS);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_INS_ESTADUAL", pFaturas[i].INS_ESTADUAL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NR_FATURA", pFaturas[i].NR_FATURA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NAT_OPERACAO", pFaturas[i].NAT_OPERACAO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_COD_PORTADOR", pFaturas[i].COD_PORTADOR.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_DT_PRVENC", pFaturas[i].DT_PRVENC);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_CD_VENDEDOR", pFaturas[i].CD_VENDEDOR);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_MODALIDADE", pFaturas[i].MODALIDADE.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_OBSERV_NOTA", pFaturas[i].OBSERV_NOTA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_VL_ACUM_DUP", pFaturas[i].VL_ACUM_DUP.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NO_AB_REPPRI", pFaturas[i].NO_AB_REPPRI);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_DT_TRANS", pFaturas[i].DT_TRANS);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_MO_CODIGO", pFaturas[i].MO_CODIGO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_CGCCPF_CLI", pFaturas[i].CGCCPF_CLI);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NR_NOTA_EXT", pFaturas[i].NR_NOTA_EXT);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_CIDADE_COB", pFaturas[i].CIDADE_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_CEP_COB", pFaturas[i].CEP_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_ENDERECO_COB", pFaturas[i].ENDERECO_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_BAIRRO_COB", pFaturas[i].BAIRRO_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_ESTADO_COB", pFaturas[i].ESTADO_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NOME_COB", pFaturas[i].NOME_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_NR_TAB_FINAN", pFaturas[i].NR_TAB_FINAN.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@Par_SEQ_WT_DOCTO", pFaturas[i].SEQ_WT_DOCTO.ConvertToInt32());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}
        private void InsereFatura(List<IntegrarFaturasModel> pFaturas)
        {
            for (int i = 0; i < pFaturas.Count; i++)
            {
                    Api_InsereFatura(pFaturas[i]);
            }
        }
        private void  Api_InsereFatura(IntegrarFaturasModel pFatura)
        {

            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            

            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertwtdocto";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";
         
            String json = JsonConvert.SerializeObject(pFatura);
            
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "message");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("Api_InsereFatura:"+ message);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }



        //---Insere cada ITEM retornado na nossa tabela EMS
        //private void InsereItem(List<IntegrarItensModel> pItens)
        //{
        //    for (int i = 0; i < pItens.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Item");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_REC_ORIGEM", pItens[i].REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DES_DESTINO", pItens[i].DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DAT_TRANSM", pItens[i].DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_USU_TRANSM", pItens[i].USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON", pItens[i].FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_NR_SEQUENCIA", pItens[i].NR_SEQUENCIA.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IT_CODIGO", pItens[i].IT_CODIGO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_QUANTIDADE", pItens[i].QUANTIDADE.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_PER_DES_ITEM", pItens[i].PER_DES_ITEM.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_NARRATIVA", pItens[i].NARRATIVA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VL_PREORI_PED", pItens[i].VL_PREORI_PED.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VAL_PCT_DESCONTO_TAB_PRECO", pItens[i].VAL_PCT_DESCONTO_TAB_PRECO.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_SEQ_WT_IT_DOCTO", pItens[i].SEQ_WT_IT_DOCTO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_SEQ_WT_DOCTO", pItens[i].SEQ_WT_DOCTO.ConvertToInt32());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}

        //---Insere cada DUPLICATA retornado na nossa tabela EMS
        private void InsereItem(List<IntegrarItensModel> pItens)
        {
            for (int i = 0; i < pItens.Count; i++)
            {
                Api_InsereItem(pItens[i]);
            }
        }
        private void Api_InsereItem(IntegrarItensModel pItem)
        {
            
            
            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertwtitdocto";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pItem);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("Api_InsereItem" + message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        //private void InsereDuplicata(List<IntegrarDuplicatasModel> pDuplicatas)
        //{
        //    for (int i = 0; i < pDuplicatas.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Duplicata");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_REC_ORIGEM", pDuplicatas[i].REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DES_DESTINO", pDuplicatas[i].DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DAT_TRANSM", pDuplicatas[i].DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_USU_TRANSM", pDuplicatas[i].USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON", pDuplicatas[i].FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_PARCELA", pDuplicatas[i].PARCELA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DT_VENCIMENT", pDuplicatas[i].DT_VENCIMENT);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VL_PARCELA", pDuplicatas[i].VL_PARCELA.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VL_DESCONTO", pDuplicatas[i].VL_DESCONTO.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VL_COMIS", pDuplicatas[i].VL_COMIS.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VL_ACUM_DUP", pDuplicatas[i].VL_ACUM_DUP.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_VENCTO", pDuplicatas[i].COD_VENCTO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_ESP", pDuplicatas[i].COD_ESP);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_SEQ_WT_DOCTO", pDuplicatas[i].SEQ_WT_DOCTO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_NR_SEQ_NOTA", pDuplicatas[i].NR_SEQ_NOTA.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VL_BASE_COMIS", pDuplicatas[i].VL_BASE_COMIS.ConvertToDecimal());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}

        
        private void InsereDuplicata(List<IntegrarDuplicatasModel> pDuplicata)
        {
            for (int i = 0; i < pDuplicata.Count; i++)
            {
                Api_InsereDuplicata(pDuplicata[i]);
            }
        }
        private void Api_InsereDuplicata(IntegrarDuplicatasModel pDuplicata)
        {


            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertwtfatduplic";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pDuplicata);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("Api_InsereDuplicata"+message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        //---Insere cada REPRESENTANTE retornado na nossa tabela EMS
        //private void InsereRepresentante(List<IntegrarRepresentantesModel> pRepresentantes)
        //{
        //    for (int i = 0; i < pRepresentantes.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Representante");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_REC_ORIGEM", pRepresentantes[i].REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DES_DESTINO", pRepresentantes[i].DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DAT_TRANSM", pRepresentantes[i].DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_USU_TRANSM", pRepresentantes[i].USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON", pRepresentantes[i].FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_REP", pRepresentantes[i].COD_REP.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_PERC_COMIS", pRepresentantes[i].PERC_COMIS.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COMIS_EMIS", pRepresentantes[i].COMIS_EMIS.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_SEQ_WT_DOCTO", pRepresentantes[i].SEQ_WT_DOCTO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_NR_SEQ_NOTA", pRepresentantes[i].NR_SEQ_NOTA.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_SEQUENCIA", pRepresentantes[i].SEQUENCIA.ConvertToDecimal());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}
        private void InsereRepresentante(List<IntegrarRepresentantesModel> pRepresentante)
        {
            for (int i = 0; i < pRepresentante.Count; i++)
            {
                Api_InsereRepresentante(pRepresentante[i]);
            }
        }
        private void Api_InsereRepresentante(IntegrarRepresentantesModel pRepresentante)
        {


            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertwtfatrepre";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pRepresentante);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("Api_InsereRepresentante"+message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        //---Insere cada CLIENTE retornado na nossa tabela EMS
        //private void InsereCliente(List<IntegrarClientesModel> pClientes)
        //{
        //    for (int i = 0; i < pClientes.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Cliente");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_ORIGEM", pClientes[i].ORIGEM.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DESTINO", pClientes[i].DESTINO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DAT_TRANSM", pClientes[i].DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_USU_TRANSM", pClientes[i].USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON", pClientes[i].FLA_DISPON.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_NOM_ABREV", pClientes[i].STR_NOM_ABREV);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CGCCPF", pClientes[i].STR_CGCCPF);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_IDENTIFIC", pClientes[i].IND_IDENTIFIC.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_NATUREZA", pClientes[i].IND_NATUREZA.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_NOME", pClientes[i].STR_NOME);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_ENDERECO", pClientes[i].STR_ENDERECO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_BAIRRO", pClientes[i].STR_BAIRRO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CIDADE", pClientes[i].STR_CIDADE);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_UF", pClientes[i].STR_UF);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CEP", pClientes[i].STR_CEP);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CX_POSTAL", pClientes[i].STR_CX_POSTAL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_PAIS", pClientes[i].STR_PAIS);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_INSCR_EST", pClientes[i].STR_INSCR_EST);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_RAMO_ATIV", pClientes[i].STR_RAMO_ATIV);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_TELEFAX", pClientes[i].STR_TELEFAX);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_RAMAL_TEL", pClientes[i].STR_RAMAL_TEL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_TELEX", pClientes[i].STR_TELEX);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_DAT_IMPLANTA", pClientes[i].DAT_IMPLANTA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_REPRESENT", pClientes[i].COD_REPRESENT.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_LOC_AV_CR", pClientes[i].IND_LOC_AV_CR.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_GRUPO_CLI", pClientes[i].COD_GRUPO_CLI.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_VLR_LIMITE_CR", pClientes[i].VLR_LIMITE_CR.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_PORTADOR", pClientes[i].COD_PORTADOR.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_MODAL", pClientes[i].IND_MODAL.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_FAT_PARC", pClientes[i].IND_FAT_PARC.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_CREDITO", pClientes[i].IND_CREDITO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_AVAL_CRED", pClientes[i].IND_AVAL_CRED.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_NAT_OPER", pClientes[i].STR_NAT_OPER);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_MEIO_PED", pClientes[i].IND_MEIO_PED.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_NOME_FANT", pClientes[i].STR_NOME_FANT);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_MODEM", pClientes[i].STR_MODEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_RAMAL_MOD", pClientes[i].STR_RAMAL_MOD);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_AGENCIA", pClientes[i].STR_AGENCIA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_BLOQUETO", pClientes[i].IND_BLOQUETO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_ETIQUETA", pClientes[i].IND_ETIQUETA.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_VALORES", pClientes[i].IND_VALORES.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_AVISO_DEB", pClientes[i].IND_AVISO_DEB.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_MODA_PREF", pClientes[i].IND_MODA_PREF.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_AVALIACAO", pClientes[i].IND_AVALIACAO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_VENC_DOM", pClientes[i].IND_VENC_DOM.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_VENC_SAB", pClientes[i].IND_VENC_SAB.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CGC_COB", pClientes[i].STR_CGC_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CEP_COB", pClientes[i].STR_CEP_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_UF_COB", pClientes[i].STR_UF_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CIDAD_COB", pClientes[i].STR_CIDAD_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_BAIRR_COB", pClientes[i].STR_BAIRR_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_ENDER_COB", pClientes[i].STR_ENDER_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CX_PO_COB", pClientes[i].STR_CX_PO_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_IN_ES_COB", pClientes[i].STR_IN_ES_COB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_BANCO", pClientes[i].STR_BANCO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_TIPO_REG", pClientes[i].IND_TIPO_REG.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_VENC_FER", pClientes[i].IND_VENC_FER.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_PAGAMENTO", pClientes[i].IND_PAGAMENTO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_COBR_DESP", pClientes[i].IND_COBR_DESP.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_INSC_MUN", pClientes[i].STR_INSC_MUN);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_COND_PAG", pClientes[i].COD_COND_PAG.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_FONE_1", pClientes[i].STR_FONE_1);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_FONE_2", pClientes[i].STR_FONE_2);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_NUM_MES_INAT", pClientes[i].NUM_MES_INAT.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_LOG_VER_PUB", pClientes[i].LOG_VER_PUB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_E_MAIL", pClientes[i].STR_E_MAIL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_IND_AVAL_EMB", pClientes[i].IND_AVAL_EMB.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CONTA_COR", pClientes[i].STR_CONTA_COR);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_NR_SEQUENCIA", pClientes[i].NR_SEQUENCIA.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_1", pClientes[i].FLA_DISPON_1.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_2", pClientes[i].FLA_DISPON_2.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_3", pClientes[i].FLA_DISPON_3.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_4", pClientes[i].FLA_DISPON_4.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_5", pClientes[i].FLA_DISPON_5.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_6", pClientes[i].FLA_DISPON_6.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_7", pClientes[i].FLA_DISPON_7.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_8", pClientes[i].FLA_DISPON_8.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_9", pClientes[i].FLA_DISPON_9.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_FLA_DISPON_10", pClientes[i].FLA_DISPON_10.ConvertToInt32());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}




        //---Insere cada LOTE retornado na nossa tabela EMS

        private void InsereCliente(List<IntegrarClientesModel> pCliente)
        {
            for (int i = 0; i < pCliente.Count; i++)
            {
                Api_InsereCliente(pCliente[i]);
            }
        }
        private void Api_InsereCliente(IntegrarClientesModel pCliente)
        {


            AppSettingsReader AppRead = new AppSettingsReader();

            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertclienteint";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pCliente);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("Api_InsereCliente"+message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //private void InsereLote(List<IntegrarLotesModel> pLotes)
        //{
        //    for (int i = 0; i < pLotes.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Lote");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_REC_ORIGEM", pLotes[i].TTV_REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DES_DESTINO", pLotes[i].TTV_DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DAT_TRANSM", pLotes[i].TTV_DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_USU_TRANSM", pLotes[i].TTV_USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_FLA_DISPON", pLotes[i].TTV_FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_EMPRESA", pLotes[i].TTA_COD_EMPRESA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_ESTAB", pLotes[i].TTA_COD_ESTAB);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_REFER", pLotes[i].TTA_COD_REFER);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_DAT_TRANSACAO", pLotes[i].TTA_DAT_TRANSACAO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_TOT_LOTE_INFOR_TIT_ACR", pLotes[i].TTA_VAL_TOT_LOTE_INFOR_TIT_ACR.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_IND_TIP_COBR_ACR", pLotes[i].TTA_IND_TIP_COBR_ACR);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_LOG_LOTE_IMPL_OK", pLotes[i].TTV_LOG_LOTE_IMPL_OK);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_LOTE", pLotes[i].COD_LOTE.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_COD_ERRO", pLotes[i].TTV_COD_ERRO.ConvertToInt32());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}
        private void InsereLote(List<IntegrarLotesModel> pLotes)
        {
            for (int i = 0; i < pLotes.Count; i++)
            {
                API_InsereLote(pLotes[i]);
            }
        }
        private void API_InsereLote(IntegrarLotesModel pLote)
        {

            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertloteimpl";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pLote);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_InsereLote:" + message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //---Insere cada ITEM LOTE retornado na nossa tabela EMS
        //private void InsereItemLote(List<IntegrarItemLotesModel> pItemLotes)
        //{
        //    for (int i = 0; i < pItemLotes.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_ItemLote");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_REC_ORIGEM", pItemLotes[i].TTV_REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DES_DESTINO", pItemLotes[i].TTV_DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DAT_TRANSM", pItemLotes[i].TTV_DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_USU_TRANSM", pItemLotes[i].TTV_USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_FLA_DISPON", pItemLotes[i].TTV_FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_NUM_SEQ_REFER", pItemLotes[i].TTA_NUM_SEQ_REFER.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_SER_DOCTO", pItemLotes[i].TTA_COD_SER_DOCTO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_ESPEC_DOCTO", pItemLotes[i].TTA_COD_ESPEC_DOCTO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_TIT_ACR", pItemLotes[i].TTA_COD_TIT_ACR);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_PARCELA", pItemLotes[i].TTA_COD_PARCELA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_INDIC_ECON", pItemLotes[i].TTA_COD_INDIC_ECON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_PORTADOR", pItemLotes[i].TTA_COD_PORTADOR);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_CART_BCIA", pItemLotes[i].TTA_COD_CART_BCIA);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_CDN_REPRES", pItemLotes[i].TTA_CDN_REPRES.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_DAT_VENCTO_TIT_ACR", pItemLotes[i].TTA_DAT_VENCTO_TIT_ACR);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_DAT_PREV_LIQUIDAC", pItemLotes[i].TTA_DAT_PREV_LIQUIDAC);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_DAT_EMIS_DOCTO", pItemLotes[i].TTA_DAT_EMIS_DOCTO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_TIT_ACR", pItemLotes[i].TTA_VAL_TIT_ACR.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_JUROS_DIA_ATR", pItemLotes[i].TTA_VAL_PERC_JUROS_DIA_ATR.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_MULTA_ATR", pItemLotes[i].TTA_VAL_PERC_MULTA_ATR.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_LIQ_TIT_ACR", pItemLotes[i].TTA_VAL_LIQ_TIT_ACR.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_IND_TIP_ESPEC_DOCTO", pItemLotes[i].TTA_IND_TIP_ESPEC_DOCTO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_ITEM_LOTE", pItemLotes[i].COD_ITEM_LOTE.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_LOTE", pItemLotes[i].COD_LOTE.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_COD_ERRO", pItemLotes[i].TTV_COD_ERRO.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_STR_CGCCPF", pItemLotes[i].STR_CGCCPF);
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}
        //---Insere cada APROP retornado na nossa tabela EMS
        private void InsereItemLote(List<IntegrarItemLotesModel> pItens)
        {
            for (int i = 0; i < pItens.Count; i++)
            {
                API_InsereItemLote(pItens[i]);
            }
        }
        private void API_InsereItemLote(IntegrarItemLotesModel pItem)
        {

            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertitemloteimpl";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pItem);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_InsereItemLote:" + message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        //private void InsereAprop(List<IntegrarApropsModel> pAprops)
        //{
        //    for (int i = 0; i < pAprops.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Aprop");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_REC_ORIGEM", pAprops[i].TTV_REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DES_DESTINO", pAprops[i].TTV_DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DAT_TRANSM", pAprops[i].TTV_DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_USU_TRANS", pAprops[i].TTV_USU_TRANS);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_FLA_DISPON", pAprops[i].TTV_FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_ITEM_LOTE", pAprops[i].COD_ITEM_LOTE.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_PLANO_CTA_CTBL", pAprops[i].TTA_COD_PLANO_CTA_CTBL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_CTA_CTBL", pAprops[i].TTA_COD_CTA_CTBL);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_UNID_NEGOC", pAprops[i].TTA_COD_UNID_NEGOC);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_COD_TIP_FLUXO_FINANC", pAprops[i].TTA_COD_TIP_FLUXO_FINANC);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_APROP_CTBL", pAprops[i].TTA_VAL_APROP_CTBL.ConvertToDecimal());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}
        //---Insere cada REPRE retornado na nossa tabela EMS
        private void InsereAprop(List<IntegrarApropsModel> pAprops)
        {
            for (int i = 0; i < pAprops.Count; i++)
            {
                API_InsereAprop(pAprops[i]);
            }
        }
        private void API_InsereAprop(IntegrarApropsModel pAprop)
        {

            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertapropctblpend";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pAprop);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "MESSENGER");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_InsereAprop:" + message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //private void InsereRepre(List<IntegrarRepresModel> pRepres)
        //{
        //    for (int i = 0; i < pRepres.Count; i++)
        //    {
        //        clsConexao cnn = new clsConexao(this.Credential);
        //        cnn.Open();
        //        SqlDataAdapter Adp = new SqlDataAdapter();
        //        DataTable dtb = new DataTable("dtb");
        //        SimLib clsLib = new SimLib();
        //        try
        //        {
        //            SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Teste_Insere_Repre");
        //            Adp.SelectCommand = cmd;
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_REC_ORIGEM", pRepres[i].TTV_REC_ORIGEM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DES_DESTINO", pRepres[i].TTV_DES_DESTINO);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_DAT_TRANSM", pRepres[i].TTV_DAT_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_USU_TRANSM", pRepres[i].TTV_USU_TRANSM);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTV_FLA_DISPON", pRepres[i].TTV_FLA_DISPON);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_ITEM_LOTE", pRepres[i].COD_ITEM_LOTE.ConvertToInt32());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_REPRES", pRepres[i].TTA_VAL_PERC_COMIS_REPRES.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_REPRES_EMIS", pRepres[i].TTA_VAL_PERC_COMIS_REPRES_EMIS.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_ABAT", pRepres[i].TTA_VAL_PERC_COMIS_ABAT.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_DESC", pRepres[i].TTA_VAL_PERC_COMIS_DESC.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_JUROS", pRepres[i].TTA_VAL_PERC_COMIS_JUROS.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_MULTA", pRepres[i].TTA_VAL_PERC_COMIS_MULTA.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_VAL_PERC_COMIS_ACERTO_VAL", pRepres[i].TTA_VAL_PERC_COMIS_ACERTO_VAL.ConvertToDecimal());
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_LOG_COMIS_REPRES_PROPORC", pRepres[i].TTA_LOG_COMIS_REPRES_PROPORC);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_IND_TIP_COMIS", pRepres[i].TTA_IND_TIP_COMIS);
        //            Adp.SelectCommand.Parameters.AddWithValue("@PAR_TTA_CDN_REPRES", pRepres[i].TTA_CDN_REPRES.ConvertToInt32());
        //            Adp.Fill(dtb);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}

        private void InsereRepre(List<IntegrarRepresModel> pRepres)
        {
            for (int i = 0; i < pRepres.Count; i++)
            {
                API_InsereRepre(pRepres[i]);
            }
        }
        private void API_InsereRepre(IntegrarRepresModel pRepre)
        {

            AppSettingsReader AppRead = new AppSettingsReader();
            SimLib clsLib = new SimLib();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            String url = AppRead.GetValue("UrlApiEms", typeof(string)).ToString() + "api/v2/insertreprespend";
            String Token = AppRead.GetValue("TokenApiEms", typeof(string)).ToString();
            String message = "";

            String json = JsonConvert.SerializeObject(pRepre);
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                var body = @"" + json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful || (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
                {
                    message = clsLib.GetJsonItem(response.Content, "messenger");
                    if (String.IsNullOrEmpty(message))
                    {
                        message = response.ErrorMessage;
                    }
                    throw new Exception("API_InsereRepre:" + message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //--------------------------------Atualiza o Log de Controle de Integrações -----------------------------------------
        public Boolean GravarLogControle(IntegrarEMSModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Boolean Retorno = true;
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Grava_Log_Integracoes_EMS");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_USUARIOAPP", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@PAR_COD_EMPRESA_FATURAMENTO", Param.Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia", clsLib.CompetenciaInt(Param.Competencia));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Fat_Boleto", Param.LogFatBoleto);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Tipo", Param.LogTipo);
                Adp.Fill(dtb);
                {
                    Retorno = dtb.Rows[0]["Retorno"].ToString().ConvertToBoolean();
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
            return Retorno;
        }




    }
}
