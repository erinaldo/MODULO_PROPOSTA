using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class Determinacao
    {
        public ContratoModel CarregarDados(FiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            ContratoModel Determinacao = new ContratoModel();
            DeParaComercialModel DePara = new DeParaComercialModel();
            List<DeParaComercialParaModel> DeParaComercial = new List<DeParaComercialParaModel>();
            DePara.ComercialPara = DeParaComercial;

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Determinacao_Dados");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    Determinacao.Id_Contrato = dtb.Rows[0]["Id_Contrato"].ToString().ConvertToInt32();
                    Determinacao.Cod_Empresa = dtb.Rows[0]["Cod_Empresa"].ToString();
                    Determinacao.Numero_Mr = dtb.Rows[0]["Numero_Mr"].ToString().ConvertToInt32();
                    Determinacao.Sequencia_Mr = dtb.Rows[0]["Sequencia_Mr"].ToString().ConvertToInt32();
                    Determinacao.Cod_Agencia = dtb.Rows[0]["Cod_Agencia"].ToString();
                    Determinacao.Nome_Agencia = dtb.Rows[0]["Nome_Agencia"].ToString();
                    Determinacao.Cod_Cliente = dtb.Rows[0]["Cod_Cliente"].ToString();
                    Determinacao.Nome_Cliente = dtb.Rows[0]["Nome_Cliente"].ToString();
                    Determinacao.Data_Inicio = dtb.Rows[0]["Data_Inicio"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Determinacao.Data_Fim = dtb.Rows[0]["Data_Fim"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Determinacao.Competencia = dtb.Rows[0]["Competencia"].ToString().ConvertToInt32();
                    Determinacao.Competencia_String = dtb.Rows[0]["Data_Inicio"].ToString().ConvertToDatetime().ToString("MM/yyyy");
                    Determinacao.De_Para = DePara;
                    Determinacao.Comerciais = AddComerciais(Param);
                    Determinacao.Veiculos = AddVeiculos(dtb.Rows[0]["Id_Contrato"].ToString().ConvertToInt32());
                    Determinacao.Programas = AddProgramas(Param);
                    Determinacao.Veiculacoes = AddVeiculacao(Param);

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
            return Determinacao;
        }
        public List<ComercialModel> AddComerciais(FiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ComercialModel> Comerciais = new List<ComercialModel>();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Determinacao_Comerciais_List");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);



                foreach (DataRow drw in dtb.Rows)
                {
                    Comerciais.Add(new ComercialModel()
                    {
                        Cod_Empresa = drw["Cod_Empresa"].ToString(),
                        Numero_Mr = drw["Numero_Mr"].ToString().ConvertToInt32(),
                        Sequencia_Mr = drw["Sequencia_Mr"].ToString().ConvertToInt32(),
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Nome_Tipo_Comercial = drw["Nome_Tipo_Comercial"].ToString(),
                        Cod_Red_Produto = drw["Cod_Red_Produto"].ToString().ConvertToInt32(),
                        Nome_Produto = drw["Nome_Produto"].ToString(),
                        Indica_Titulo_Determinar = drw["Indica_Titulo_Determinar"].ToString().ConvertToBoolean(),
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
            return Comerciais;
        }
        public List<VeiculoModel> AddVeiculos(Int32 pId_Contrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculoModel> Veiculos = new List<VeiculoModel>();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Veiculo");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Id_Contrato", pId_Contrato);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Veiculos.Add(new VeiculoModel()
                    {
                        Codigo = drw["Cod_Veiculo"].ToString(),
                        Descricao = drw["Nome_Veiculo"].ToString(),
                        Selected = true,
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
            return Veiculos;
        }
        public List<ProgramaModel> AddProgramas(FiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ProgramaModel> Programas = new List<ProgramaModel>();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Baixa_Contrato_Get_Programa]");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);

                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Programas.Add(new ProgramaModel()
                    {
                        Codigo = drw["Codigo"].ToString(),
                        Descricao = drw["Descricao"].ToString(),
                        Selected = true,
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
            return Programas;
        }
        public List<VeiculacaoModel> AddVeiculacao(FiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculacaoModel> Veiculacao = new List<VeiculacaoModel>();

            String strQuebra = "";
            int Id_Veiculacao = -1;
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Determinacao_Get_Veiculacao");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                cmd.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);


                foreach (DataRow drw in dtb.Rows)
                {
                    if (strQuebra != drw["Cod_Veiculo"].ToString().Trim() +  drw["Cod_Programa"].ToString().Trim() + drw["Cod_Comercial"].ToString().Trim() + drw["Cod_Caracteristica"].ToString().Trim())
                    {
                        Id_Veiculacao++;
                        //----adicina dois comercias para por linha
                        List<InsercaoParaModel> ComercialPara = new List<InsercaoParaModel>();
                        ComercialPara.Add(new InsercaoParaModel() { Id_Veiculacao = Id_Veiculacao, Cod_Comercial = "" });
                        ComercialPara.Add(new InsercaoParaModel() { Id_Veiculacao = Id_Veiculacao, Cod_Comercial = "" });

                        Veiculacao.Add(new VeiculacaoModel()
                        {
                            Id_Veiculacao = Id_Veiculacao,
                            Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                            Cod_Programa = drw["Cod_Programa"].ToString(),
                            Cod_Comercial = drw["Cod_Comercial"].ToString(),
                            Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                            Cod_Caracteristica = drw["Cod_Caracteristica"].ToString(),
                            ComercialPara = ComercialPara,
                            Insercoes = new List<InsercaoModel>()
                        });
                        strQuebra = drw["Cod_Veiculo"].ToString().Trim() + drw["Cod_Programa"].ToString().Trim() + drw["Cod_Comercial"].ToString().Trim() + drw["Cod_Caracteristica"].ToString().Trim();
                    };
                    Veiculacao[Id_Veiculacao].Insercoes.Add(new InsercaoModel()
                    {
                        Id_Veiculacao = Id_Veiculacao,
                        Dia = drw["Dia"].ToString().ConvertToInt32(),
                        Data_Exibicao = drw["Data_Exibicao"].ToString().ConvertToDatetime(),
                        Dia_Semana = drw["Dia_Semana"].ToString(),
                        Numero_Semana = drw["Numero_Semana"].ToString().ConvertToInt32(),
                        Qtd = drw["Qtd"].ToString().ConvertToInt32(),
                    });
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
            return Veiculacao;
        }
        public DataTable SalvarComercial(ComercialModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Comercial_Insert]");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                cmd.Parameters.AddWithValue("@Par_Cod_Comercial", Param.Cod_Comercial);
                cmd.Parameters.AddWithValue("@Par_Titulo_Comercial", Param.Titulo_Comercial);
                cmd.Parameters.AddWithValue("@Par_Duracao", Param.Duracao);
                cmd.Parameters.AddWithValue("@Par_Cod_Tipo_Comercial", Param.Cod_Tipo_Comercial);
                cmd.Parameters.AddWithValue("@Par_Cod_Red_Produto", Param.Cod_Red_Produto);
                cmd.Parameters.AddWithValue("@Par_Nome_Produto", Param.Nome_Produto);
                cmd.Parameters.AddWithValue("@Par_Indica_Titulo_Determinar", Param.Indica_Titulo_Determinar);
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
        public List<VeiculacaoModel> SalvarDeterminacao(ContratoModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            String xmlComerciaisDePara = null;
            String xmlVeiculacoes = null;
            String xmlVeiculos = null;
            Generic clsGeneric = new Generic(this.Credential);

            if (Param.De_Para.ComercialPara.Count > 0)
            {
                xmlComerciaisDePara = clsLib.SerializeToString(Param.De_Para);
            }
            if (Param.Veiculacoes.Count > 0)
            {
                xmlVeiculacoes = clsLib.SerializeToString(Param.Veiculacoes);
            }
            if (Param.Veiculos.Count > 0)
            {
                xmlVeiculos = clsLib.SerializeToString(Param.Veiculos);
            }
            String strQuebra = "X";
            List<VeiculacaoModel> Analise = new List<VeiculacaoModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Determinacao_Simular");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Operacao", Param.Operacao);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                cmd.Parameters.AddWithValue("@Par_Comercial_De_Para", xmlComerciaisDePara);
                cmd.Parameters.AddWithValue("@Par_Veiculacoes", xmlVeiculacoes);
                cmd.Parameters.AddWithValue("@Par_Veiculos", xmlVeiculos);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                String Classe = "";
                int Id_Veiculacao = -1;
                foreach (DataRow drw in dtb.Rows)
                {
                    if (strQuebra != drw["Cod_Veiculo"].ToString().Trim() + drw["Cod_Programa"].ToString().Trim() + drw["Cod_Comercial"].ToString().Trim() + drw["Cod_Caracteristica"].ToString().Trim() + drw["Tipo_Linha"].ToString().Trim())
                    {
                        Id_Veiculacao++;
                        Analise.Add(new VeiculacaoModel()
                        {
                            Cod_Veiculo= drw["Cod_Veiculo"].ToString(),
                            Cod_Programa = drw["Cod_Programa"].ToString(),
                            Cod_Comercial = drw["Cod_Comercial"].ToString(),
                            Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                            Cod_Caracteristica = drw["Cod_Caracteristica"].ToString(),
                            Tipo_Linha = drw["Tipo_Linha"].ToString().ConvertToInt32(),
                            Status = drw["Status"].ToString().ConvertToBoolean(),
                            Mensagem = drw["Mensagem"].ToString(),
                            Insercoes = new List<InsercaoModel>()
                        });
                        strQuebra =  drw["Cod_Veiculo"].ToString().Trim() + drw["Cod_Programa"].ToString().Trim() + drw["Cod_Comercial"].ToString().Trim() + drw["Cod_Caracteristica"].ToString().Trim() + drw["Tipo_Linha"].ToString().Trim() ;
                    };
                    Classe = drw["Classe"].ToString();
                    Analise[Id_Veiculacao].Insercoes.Add(new InsercaoModel()
                    {
                        Id_Veiculacao = Id_Veiculacao,
                        Dia = drw["Dia"].ToString().ConvertToInt32(),
                        Data_Exibicao = drw["Data_Exibicao"].ToString().ConvertToDatetime(),
                        Dia_Semana = drw["Dia_Semana"].ToString(),
                        Numero_Semana = drw["Numero_Semana"].ToString().ConvertToInt32(),
                        Qtd = drw["Qtd"].ToString().ConvertToInt32(),
                        Classe= drw["Classe"].ToString()
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
            return Analise;
        }

        public DataTable ExcluirComercialContrato(ComercialModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_comercial_Delete");
                cmd.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                cmd.Parameters.AddWithValue("@Par_Cod_Empresa", Param.Cod_Empresa);
                cmd.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                cmd.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                cmd.Parameters.AddWithValue("@Par_Cod_Comercial", Param.Cod_Comercial);
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
    }
}