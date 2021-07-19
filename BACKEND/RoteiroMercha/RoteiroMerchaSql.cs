using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class RoteiroMercha
    {
        private Int32 Contador_Programa = 0;
        private Int32 Contador_Item = -1;
        private Int32 Contador_Posicao = 0;
        private Int32 Contador_Break = 0;
        private Int32 Contador_Intervalo = 0;

        public DataTable CarregarGuiaProgramacao(RoteiroMerchaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Roteiro_Mercha_Programa_List]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime().ToString("yyyy-MM-dd"));
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

        public List<RoteiroMerchaModel> RoteiroMerchaCarregar(RoteiroMerchaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<RoteiroMerchaModel> RoteiroMercha = new List<RoteiroMerchaModel>();
            try
            {
                String xmlProgramas = "";
                if (Param.Programas.Count>0)
                {
                    xmlProgramas = clsLib.SerializeToString(Param.Programas);
                }

                
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Roteiro_Mercha_Get");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime().ToString("yyyy-MM-dd"));
                if (xmlProgramas == "")
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Programas", DBNull.Value);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Programas", xmlProgramas);
                }
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    String Cod_Programa_Ant = "";
                    Int32 Break_Ant = -1;
                    foreach (DataRow drw in dtb.Rows)
                    {
                        if (Cod_Programa_Ant != drw["Cod_Programa"].ToString())
                        {
                            Contador_Programa++;
                            Contador_Item++;
                            RoteiroMercha.Add(new RoteiroMerchaModel()
                            {
                                Cod_Programa = drw["Cod_Programa"].ToString(),
                                Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                                Data_Exibicao = Param.Data_Exibicao.ConvertToDatetime(),
                                Titulo_Programa = drw["Titulo_Programa"].ToString(),
                                Hora_Inicio_Programa = drw["Inicio_Programa"].ToString().ConvertToDatetime(),
                                Hora_Fim_Programa = drw["Fim_Programa"].ToString().ConvertToDatetime(),
                                Versao = drw["Inicio_Programa"].ToString().ConvertToInt32(),
                                Id_Programa = Contador_Programa,
                                Id_Item = Contador_Item,
                                Indica_Titulo_Programa = true,
                                Indica_Titulo_Break = false,
                                Id_Break = Contador_Break,
                                Id_Intervalo = Contador_Intervalo,
                                Permite_Ordenacao = true,

                                Show = true,
                            });
                            Cod_Programa_Ant = drw["Cod_Programa"].ToString();
                            Break_Ant = -1;
                        }
                        if (Break_Ant != drw["Breaks"].ToString().ConvertToInt32())
                        {
                            Contador_Posicao = 0;
                            Break_Ant = drw["Breaks"].ToString().ConvertToInt32();
                        }
                        AddItem(RoteiroMercha, drw, Param.Data_Exibicao.ConvertToDatetime(), Param.Cod_Veiculo);
                    }
                    //RoteiroMercha[0].Contador_Item = Contador_Item;
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
            return RoteiroMercha;
        }
        public List<RoteiroMerchaComercialModel> RoteiroMerchaCarregarComerciais(RoteiroMerchaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            SimLib clsLib = new SimLib();
            List<RoteiroMerchaComercialModel> Comerciais = new List<RoteiroMerchaComercialModel>();
            try
            {
                AddComercial(Param, Comerciais);
                //AddRotativo(Param, Comerciais);
                //AddAvulso(Param, Comerciais);
                //AddArtistico(Param, Comerciais);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Comerciais;
        }

        private void AddItem(List<RoteiroMerchaModel> RoteiroMercha, DataRow drw, DateTime Data_Exibicao, String Cod_Veiculo)
        {
            
            Boolean bolIndica_Comercial = false;
            try
            {
               
                if (String.IsNullOrEmpty(drw["Indica_Titulo_Break"].ToString()))
                {
                    bolIndica_Comercial = true;
                    Contador_Posicao++;
                }

                String strOrigem = "";
                if (drw["Indica_Midia"].ToString() == "1")
                {
                    strOrigem = "Midia";
                }
                if (drw["Indica_Avulso"].ToString() == "1")
                {
                    strOrigem = "Avulso";
                }
                if (drw["Indica_Artistico"].ToString() == "1")
                {
                    strOrigem = "Artistico";
                }
                if (drw["Indica_Rotativo"].ToString() == "1")
                {
                    strOrigem = "Rotativo";
                }
                Contador_Item++;
                if (drw["Indica_Titulo_Break"].ToString().ConvertToInt32() == 1)
                {
                    Contador_Break++;
                }

                RoteiroMercha.Add(new RoteiroMerchaModel()
                {
                    Cod_Programa = drw["Cod_Programa"].ToString(),
                    Cod_Veiculo = Cod_Veiculo,
                    Data_Exibicao = Data_Exibicao,
                    Titulo_Programa = drw["Titulo_Programa"].ToString(),
                    Hora_Inicio_Programa = drw["Inicio_Programa"].ToString().ConvertToDatetime(),
                    Hora_Fim_Programa = drw["Fim_Programa"].ToString().ConvertToDatetime(),
                    Id_Programa = Contador_Programa,
                    Id_Item = Contador_Item,
                    Id_Break = Contador_Break,
                    Id_Intervalo = Contador_Intervalo,
                    Break = drw["Breaks"].ToString().ConvertToInt32(),
                    Titulo_Break = drw["Titulo_Break"].ToString(),
                    Sequencia_Intervalo = Contador_Posicao,
                    Hora_Inicio_Break = drw["Hora_Inicio_Break"].ToString().ConvertToDatetime().ToString("HH:mm"),
                    Indica_Titulo_Programa = false,
                    Indica_Titulo_Break = drw["Indica_Titulo_Break"].ToString().ConvertToInt32() == 1,
                    Indica_Comercial = bolIndica_Comercial,
                    Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                    Cod_Comercial = drw["Cod_Comercial"].ToString(),
                    Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                    Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                    Cod_Produto = drw["Cod_Produto"].ToString(),
                    Nome_Produto = drw["Descricao_Produto"].ToString(),
                    Encaixe = drw["Encaixe"].ToString().ConvertToInt32(),
                    Cod_Empresa = drw["Cod_Empresa"].ToString(),
                    Numero_Mr = drw["Numero_Mr"].ToString().ConvertToInt32(),
                    Sequencia_Mr = drw["Sequencia_Mr"].ToString().ConvertToInt32(),
                    Cod_Programa_Origem = drw["Cod_Programa_Origem"].ToString(),
                    Cod_Veiculo_Origem = drw["Cod_Veiculo_Origem"].ToString(),
                    Chave_Acesso = drw["Chave_Acesso"].ToString().ConvertToInt32(),
                    Versao = drw["Inicio_Programa"].ToString().ConvertToInt32(),
                    Origem = strOrigem,
                    Permite_Ordenacao = true,
                    Show = true
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void AddComercial(RoteiroMerchaFiltroModel Param, List<RoteiroMerchaComercialModel> Comerciais)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();
            string strPasta = "";

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_RoteiroMercha_Carrega_Comerciais");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime().ToString("yyyy-MM-dd"));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", DBNull.Value);
                Adp.Fill(dtb);

                String strPrograma_Anterior = "";

                foreach (DataRow drw in dtb.Rows)
                {

                    if (strPrograma_Anterior != drw["Cod_Programa"].ToString())
                    {
                        strPasta = "Outros";
                        for (int i = 0; i < Param.Programas.Count; i++)
                        {
                            if (drw["Cod_Programa"].ToString() == Param.Programas[i].Cod_Programa && Param.Programas[i].Selected)
                            {
                                strPasta = "Midia";
                            }
                        }
                        Contador_Item++;
                        Comerciais.Add(new RoteiroMerchaComercialModel()
                        {
                            Id_Item = Contador_Item,
                            Origem = "Midia",
                            Indica_Titulo_Programa = true,
                            Cod_Programa = drw["Cod_Programa"].ToString(),
                            Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                            Data_Exibicao = Param.Data_Exibicao.ConvertToDatetime(),
                            Titulo_Programa = drw["Titulo_Programa"].ToString(),
                            Hora_Inicio_Programa = drw["Inicio_Programa"].ToString().ConvertToDatetime(),
                            Hora_Fim_Programa = drw["Fim_Programa"].ToString().ConvertToDatetime(),
                            Pasta = strPasta,
                        });
                        strPrograma_Anterior = drw["Cod_Programa"].ToString();
                    }
                    Contador_Item++;
                    Comerciais.Add(new RoteiroMerchaComercialModel()
                    {
                        Id_Item = Contador_Item,
                        Origem = "Midia",
                        Indica_Titulo_Programa = false,
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                        Data_Exibicao = Param.Data_Exibicao.ConvertToDatetime(),
                        Titulo_Programa = drw["Titulo_Programa"].ToString(),
                        Hora_Inicio_Programa = drw["Inicio_Programa"].ToString().ConvertToDatetime(),
                        Hora_Fim_Programa = drw["FIm_Programa"].ToString().ConvertToDatetime(),
                        Cod_Empresa = drw["Cod_Empresa"].ToString(),
                        Numero_Mr = drw["Numero_Mr"].ToString().ConvertToInt32(),
                        Sequencia_Mr = drw["Sequencia_Mr"].ToString().ConvertToInt32(),
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Indica_Titulo_Determinar = drw["Indica_Titulo_Determinar"].ToString().ConvertToBoolean(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Cod_Produto = drw["Cod_Produto"].ToString(),
                        Nome_Produto = drw["Descricao"].ToString(),
                        Indica_Grade = drw["Indica_Grade"].ToString().ConvertToByte(),
                        Chave_Acesso = drw["Chave_Acesso"].ToString().ConvertToInt32(),
                        Obs_Roteiro = drw["Obs_Roteiro"].ToString(),
                        Indica_Ordenado = drw["Indica_Ordenado"].ToString().ConvertToBoolean(),
                        Pasta = strPasta,
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
        }
        private void AddRotativo(RoteiroMerchaFiltroModel Param, List<RoteiroMerchaComercialModel> Comerciais)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_RoteiroMercha_Carrega_Rotativo");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime().ToString("yyyy-MM-dd"));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", DBNull.Value);
                Adp.Fill(dtb);

                String strPrograma_Anterior = "";

                foreach (DataRow drw in dtb.Rows)
                {
                    if (strPrograma_Anterior != drw["Cod_Programa"].ToString())
                    {
                        Contador_Item++;
                        Comerciais.Add(new RoteiroMerchaComercialModel()
                        {
                            Id_Item = Contador_Item,
                            Origem = "Midia",
                            Indica_Titulo_Programa = true,
                            Cod_Programa = drw["Cod_Programa"].ToString(),
                            Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                            Titulo_Programa = drw["Titulo_Programa"].ToString(),
                            Hora_Inicio_Programa = drw["Hora_Inicio_Programa"].ToString().ConvertToDatetime(),
                            Hora_Fim_Programa = drw["Hora_Fim_Programa"].ToString().ConvertToDatetime(),
                            Pasta = "Rotativo",
                        });
                        strPrograma_Anterior = drw["Cod_Programa"].ToString();
                    }
                    Contador_Item++;
                    Comerciais.Add(new RoteiroMerchaComercialModel()
                    {
                        Id_Item = Contador_Item,
                        Origem = "Midia",
                        Indica_Titulo_Programa = false,
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                        Data_Exibicao = Param.Data_Exibicao.ConvertToDatetime(),
                        Titulo_Programa = drw["Titulo_Programa"].ToString(),
                        Hora_Inicio_Programa = drw["Hora_Inicio_Programa"].ToString().ConvertToDatetime(),
                        Hora_Fim_Programa = drw["Hora_Fim_Programa"].ToString().ConvertToDatetime(),
                        Cod_Empresa = drw["Cod_Empresa"].ToString(),
                        Numero_Mr = drw["Numero_Mr"].ToString().ConvertToInt32(),
                        Sequencia_Mr = drw["Sequencia_Mr"].ToString().ConvertToInt32(),
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Indica_Titulo_Determinar = drw["Indica_Titulo_Determinar"].ToString().ConvertToBoolean(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Cod_Produto = drw["Cod_Produto"].ToString(),
                        Nome_Produto = drw["Descricao"].ToString(),
                        Indica_Grade = drw["Indica_Grade"].ToString().ConvertToByte(),
                        Chave_Acesso = drw["Chave_Acesso"].ToString().ConvertToInt32(),
                        Obs_Roteiro= drw["Obs_Roteiro"].ToString(),
                        Indica_Ordenado = drw["Indica_Ordenado"].ToString().ConvertToBoolean(),
                        Pasta = "Rotativo",
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
        }
        private void AddAvulso(RoteiroMerchaFiltroModel Param, List<RoteiroMerchaComercialModel> Comerciais)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Roteiro_Mercha_Carrega_Depositorio");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime().ToString("yyyy-MM-dd"));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", DBNull.Value);
                Adp.Fill(dtb);



                foreach (DataRow drw in dtb.Rows)
                {

                    Contador_Item++;
                    Comerciais.Add(new RoteiroMerchaComercialModel()
                    {
                        Id_Item = Contador_Item,
                        Origem = "Avulso",
                        Indica_Titulo_Programa = false,
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Cod_Veiculo = Param.Cod_Veiculo,
                        Data_Exibicao = Param.Data_Exibicao.ConvertToDatetime(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Id_Fita = drw["IdChave"].ToString().ConvertToInt32(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Cod_Produto = drw["Cod_Produto"].ToString(),
                        Nome_Produto = drw["Descricao"].ToString(),
                        Pasta = "Avulso",
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
        }
        private void AddArtistico(RoteiroMerchaFiltroModel Param, List<RoteiroMerchaComercialModel> Comerciais)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Sp_RU_Carrega_Depositorio");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime().ToString("yyyy-MM-dd"));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", DBNull.Value);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Tipo", 1);
                Adp.Fill(dtb);



                foreach (DataRow drw in dtb.Rows)
                {

                    Contador_Item++;
                    Comerciais.Add(new RoteiroMerchaComercialModel()
                    {
                        Id_Item = Contador_Item,
                        Origem = "Artistico",
                        Indica_Titulo_Programa = false,
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Cod_Veiculo = Param.Cod_Veiculo,
                        Data_Exibicao = Param.Data_Exibicao.ConvertToDatetime(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Cod_Produto = drw["Cod_Produto"].ToString(),
                        Nome_Produto = drw["Descricao"].ToString(),
                        //Tipo_Break = null,
                        Pasta = "Artistico",
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
        }
        public DataTable RoteiroMerchaBaixarVeiculacao(RoteiroMerchaModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try { 
           
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Roteiro_Baixar_Veiculacao");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", Param.Cod_Programa);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Chave_Acesso", Param.Chave_Acesso);
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
        public void RoteiroMerchaExcluir(RoteiroMerchaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                String strsql = "Delete from Merchandising_Roteiro Where Cod_Veiculo = '" + Param.Cod_Veiculo + "'";
                strsql += " And Data_Exibicao = '" + Param.Data_Exibicao.ToString().ConvertToDatetime().ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = cnn.Text(cnn.Connection, strsql);
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
        public DataTable RoteiroMerchaSalvar(List<RoteiroMerchaModel> Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            String xmlRoteiroMercha = clsLib.SerializeToString(Param);
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_RoteiroMercha_Salvar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param[0].Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param[0].Data_Exibicao.ToString().ConvertToDatetime());
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Roteiro", xmlRoteiroMercha);
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
        public BreakModel RoteiroMerchaListarBreak(RoteiroMerchaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            BreakModel Breaks = new BreakModel();
            List<ComposicaoBreakModel> Composicao = new List<ComposicaoBreakModel>();
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Break_Listar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime());
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", Param.Cod_Programa);
                Adp.Fill(dtb);
                Int32 Id_Composicao = 0;
                if (dtb.Rows.Count > 0)
                {
                    Breaks.Cod_Programa = dtb.Rows[0]["Cod_Programa"].ToString();   
                    Breaks.Nome_Programa = dtb.Rows[0]["Nome_Programa"].ToString();
                    Breaks.Cod_Veiculo = dtb.Rows[0]["Cod_Veiculo"].ToString();
                    Breaks.Data_Exibicao = dtb.Rows[0]["Data_Exibicao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy"); 
                    Breaks.Ultimo_Dia_Break = dtb.Rows[0]["Ultimo_Dia_Break"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Breaks.Grade_Domingo = dtb.Rows[0]["Grade_Domingo"].ToString().ConvertToBoolean();
                    Breaks.Grade_Segunda = dtb.Rows[0]["Grade_Segunda"].ToString().ConvertToBoolean();
                    Breaks.Grade_Terca = dtb.Rows[0]["Grade_Terca"].ToString().ConvertToBoolean();
                    Breaks.Grade_Quarta = dtb.Rows[0]["Grade_Quarta"].ToString().ConvertToBoolean();
                    Breaks.Grade_Quinta = dtb.Rows[0]["Grade_Quinta"].ToString().ConvertToBoolean();
                    Breaks.Grade_Sexta = dtb.Rows[0]["Grade_Sexta"].ToString().ConvertToBoolean();
                    Breaks.Grade_Sabado = dtb.Rows[0]["Grade_Sabado"].ToString().ConvertToBoolean();
                    Breaks.Hora_Inicio_Programa = dtb.Rows[0]["Hora_Inicio_Programa"].ToString().ConvertToDatetime().ToString("HH:mm");
                    Breaks.Dispo_Net = dtb.Rows[0]["Dispo_Net"].ToString().ConvertToInt32();
                    Breaks.Dispo_Local= dtb.Rows[0]["Dispo_Local"].ToString().ConvertToInt32();
                    foreach (DataRow drw in dtb.Rows)
                    {
                        Id_Composicao++;
                        if (!String.IsNullOrEmpty(drw["Breaks"].ToString()))
                        {
                            Composicao.Add(new ComposicaoBreakModel()
                            {
                                Id_Composicao = Id_Composicao,
                                Breaks = drw["Breaks"].ToString().ConvertToInt32(),
                                Sequencia = drw["Sequencia"].ToString().ConvertToInt32(),
                                Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                                //Tipo_Break = new TipoBreakModel() { Codigo = drw["Tipo_Break"].ToString().ConvertToInt32(), Descricao = drw["Nome_Tipo_Break"].ToString() },
                                Titulo_Break = drw["Titulo_Break"].ToString().Trim(),
                                Sequencia_Break = drw["Sequencia_Break"].ToString().ConvertToInt32(),
                                Observacao = drw["Observacao"].ToString().Trim(),
                                Hora_Inicio = drw["Hora_Inicio"].ToString(),
                            });
                        };
                    }
                }
                Breaks.Composicao = Composicao;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Breaks;
        }
        public DataTable RoteiroMerchaGravarBreak(BreakModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            String DiaSemana = "";
            DiaSemana += (Param.Grade_Domingo ? "S" : "N");
            DiaSemana += (Param.Grade_Segunda ? "S" : "N");
            DiaSemana += (Param.Grade_Terca ? "S" : "N");
            DiaSemana += (Param.Grade_Quarta ? "S" : "N");
            DiaSemana += (Param.Grade_Quinta ? "S" : "N");
            DiaSemana += (Param.Grade_Sexta ? "S" : "N");
            DiaSemana += (Param.Grade_Sabado ? "S" : "N");
            String xmlComposicao = clsLib.SerializeToString(Param.Composicao);

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Breaks_Gravar");

                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                cmd.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime());
                cmd.Parameters.AddWithValue("@Par_Cod_Programa", Param.Cod_Programa);
                cmd.Parameters.AddWithValue("@Par_Data_Inicio", Param.Data_Inicio_Propagacao.ConvertToDatetime());
                cmd.Parameters.AddWithValue("@Par_Data_Final", Param.Data_Fim_Propagacao.ConvertToDatetime());
                cmd.Parameters.AddWithValue("@Par_Dia_Semana", DiaSemana);
                cmd.Parameters.AddWithValue("@Par_Composicao", xmlComposicao);
                Adp.SelectCommand = cmd;
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
        public DataTable RoteiroMerchaProgramasBreak(RoteiroMerchaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Grade_Get");

                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                cmd.Parameters.AddWithValue("@Par_Data_Exibicao", Param.Data_Exibicao.ConvertToDatetime());
                if (!String.IsNullOrEmpty(Param.Cod_Programa))
                {
                    cmd.Parameters.AddWithValue("@Par_Cod_Programa", Param.Cod_Programa);
                }
                Adp.SelectCommand = cmd;
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
        public DataTable PreOrdenar(FiltroPreOrdModel pFiltro)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_PreOrdenacao_Consistencias");   // Faz consistencias e chama a proc Sp_Ru_Pre_Ordenacao
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", pFiltro.Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", pFiltro.Data.ConvertToDatetime());
                if (String.IsNullOrEmpty(pFiltro.Programa))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", DBNull.Value);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", pFiltro.Programa);
                }
                if (pFiltro.Indica_PreOrdenar_Rotativos)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Rotativo", 1);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Rotativo", 0);
                }
                if (pFiltro.Indica_PreOrdenar_Vinhetas)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Vinheta", 1);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Vinheta", 0);
                }
                if (pFiltro.Indica_Evitar_Choque_Produtos)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Concorrente_Produto", 1);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Concorrente_Produto", 0);
                }
                if (pFiltro.Indica_Evitar_Choque_Apresent)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Concorrente_Apresentador", 1);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Concorrente_Apresentador", 0);
                }
                if (pFiltro.Indica_Nao_Colar_Comerciais)
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Separar_Apresentador", 1);
                }
                else
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Separar_Apresentador", 0);
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
        public Boolean ExisteRoteiroMerchaOrdenado(FiltroPreOrdModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            Boolean Retorno = false;
            SimLib clsLib = new SimLib();
            try
            {
                String sSql = "Select top 1 'x' From RoteiroMercha_Tecnico with (nolock)";
                sSql += " Where Cod_Veiculo = '" + Param.Veiculo + "'";
                sSql += " And Data_Exibicao = '" + Param.Data.ConvertToDatetime().ToString("yyyy-MM-dd") + "'";
                if (!String.IsNullOrEmpty(Param.Programa))
                {
                    sSql += " And Cod_Programa  = '" + Param.Programa + "'";
                };

                SqlCommand cmd = cnn.Text(cnn.Connection, sSql);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    Retorno = true;
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

        public DataTable EncerrarRoteiroMercha(EncerramentoRoteiroMerchaModel pFiltro)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_EncerramentoRoteiroMercha");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", pFiltro.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", pFiltro.Data.ConvertToDatetime());
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
