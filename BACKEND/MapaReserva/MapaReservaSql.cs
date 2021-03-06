using CLASSDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace PROPOSTA
{
    public partial class MapaReserva

    {
        Int32 Sequenciador_Veiculacao = 0;
        Int32 Sequenciador_Comercial = 0;
        public DataTable MapaReservaList(MapaReservaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_List");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", Param.Numero_Negociacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Mr", Param.Numero_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Sequencia_Mr", Param.Sequencia_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Pi", Param.Numero_Pi);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Venda", Param.Cod_Empresa_Venda);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", Param.Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia_Inicio", clsLib.CompetenciaInt(Param.Competencia_Inicio));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia_Termino", clsLib.CompetenciaInt(Param.Competencia_Fim));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Agencia", Param.Agencia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cliente", Param.Cliente);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Contato", Param.Contato);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_TipoVenda", Param.TipoVenda);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", Param.Cod_Programa);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Agencia", Param.Cod_Agencia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Cliente", Param.Cod_Cliente);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Tipo_Midia", Param.Cod_Tipo_Midia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Nucleo", Param.Cod_Nucleo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Situacao", Param.Situacao);

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
        public DataTable MapaReservaDetalheContrato(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Contrato");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
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
        public DataTable MapaReservaDetalheComercial(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Comercial");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
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
        public DataTable MapaReservaDetalheCompetencia(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Competencia");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
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
        public DataTable MapaReservaDetalheVeiculo(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Veiculo");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
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
        public List<MapaReservaMidiaModel> MapaReservaDetalheMidia(MapaReservaMidiaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<MapaReservaMidiaModel> Midias = new List<MapaReservaMidiaModel>();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Midia");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", Param.Id_Contrato);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia", Param.Competencia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Display", Param.Display);
                Adp.Fill(dtb);


                DataView viewMidia = new DataView(dtb);
                DataTable dtbMidia = viewMidia.ToTable(true, "Cod_Veiculo", "Tipo_Linha", "Cod_Programa", "Cod_Caracteristica", "Cod_Comercial", "Indica_Exibido");
                Int32 Qtd_Total = 0;
                Double Valor_Negociado = 0;
                Double Valor_Tabela = 0;
                foreach (DataRow drw in dtbMidia.Rows)
                {
                    Qtd_Total = 0;
                    Valor_Negociado = 0;
                    Valor_Tabela = 0;
                    MapaReservaMidiaModel MidiaTemp = new MapaReservaMidiaModel();
                    MidiaTemp.Tipo_Linha = drw["Tipo_Linha"].ToString().ConvertToInt32();
                    MidiaTemp.Cod_Veiculo = drw["Cod_Veiculo"].ToString();
                    MidiaTemp.Cod_Programa = drw["Cod_Programa"].ToString();
                    MidiaTemp.Cod_Caracteristica = drw["Cod_Caracteristica"].ToString();
                    MidiaTemp.Cod_Comercial = drw["Cod_Comercial"].ToString();
                    MidiaTemp.Indica_Exibido = drw["Indica_Exibido"].ToString().ConvertToBoolean();
                    MidiaTemp.Insercoes = new List<MapaReservaInsercoesModel>();
                    String strSql = "Cod_Veiculo = '" + drw["Cod_Veiculo"].ToString() + "'";
                    strSql += " And Tipo_Linha= '" + drw["Tipo_Linha"].ToString() + "'";
                    strSql += " And Cod_Programa = '" + drw["Cod_Programa"].ToString() + "'";
                    strSql += " And Cod_Caracteristica = '" + drw["Cod_Caracteristica"].ToString() + "'";
                    strSql += " And Cod_Comercial= '" + drw["Cod_Comercial"].ToString() + "'";
                    strSql += " And Indica_Exibido= " + drw["Indica_Exibido"].ToString();

                    DataRow[] rows = dtb.Select(strSql, "Data_Exibicao");
                    for (int i = 0; i < rows.Length; i++)
                    {
                        Valor_Tabela += rows[i]["Valor_Tabela"].ToString().ConvertToDouble();
                        Valor_Negociado += rows[i]["Valor_Negociado"].ToString().ConvertToDouble();
                        Qtd_Total += rows[i]["Qtd"].ToString().ConvertToInt32();

                        MidiaTemp.Insercoes.Add(new MapaReservaInsercoesModel()
                        {
                            Data_Exibicao = rows[i]["Data_Exibicao"].ToString(),
                            Dia = rows[i]["Dia"].ToString(),
                            Dia_Semana = rows[i]["Dia_Semana"].ToString(),
                            Qtd = rows[i]["Qtd"].ToString().ConvertToInt32(),
                        });

                    }
                    MidiaTemp.Valor_Negociado = Valor_Negociado;
                    MidiaTemp.Valor_Tabela = Valor_Tabela;
                    if (Valor_Tabela > 0)
                    {
                        MidiaTemp.Desconto = (1 - (Valor_Negociado / Valor_Tabela)) * 100;
                    }
                    else
                    {
                        MidiaTemp.Desconto = 0;
                    }
                    MidiaTemp.Qtd_Total = Qtd_Total;


                    Midias.Add(MidiaTemp);
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
            return Midias;
        }
        public DataTable MapaReservaDetalheVeiculacoes(MapaReservaMidiaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Veiculacoes");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", Param.Id_Contrato);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", Param.Cod_Veiculo);
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
        public DataTable MapaReservaDetalheResumo(MapaReservaMidiaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Resumo");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", Param.Id_Contrato);
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
        public DataTable MapaReservaImport()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_Import]");
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
        public ContratoModel MapaReservaCarregarEsquema(Int32 pId_Esquema)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            ContratoModel Contrato = new ContratoModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_Importa_Esquema]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Esquema", pId_Esquema);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    Contrato.Numero_Negociacao = dtb.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32();
                    Contrato.Cod_Empresa_Venda = dtb.Rows[0]["Cod_Empresa_Venda"].ToString().Trim();
                    Contrato.Nome_Empresa_Venda = dtb.Rows[0]["Nome_Empresa_Venda"].ToString().Trim();
                    Contrato.Cod_Empresa_Faturamento = dtb.Rows[0]["Cod_Empresa_Faturamento"].ToString().Trim();
                    Contrato.Cod_Tipo_Midia = dtb.Rows[0]["Cod_Tipo_Midia"].ToString().Trim();
                    Contrato.Nome_Empresa_Faturamento = dtb.Rows[0]["Nome_Empresa_Faturamento"].ToString().Trim();
                    Contrato.Cod_Contato = dtb.Rows[0]["Cod_Contato"].ToString().Trim();
                    Contrato.Nome_Contato = dtb.Rows[0]["Nome_Contato"].ToString().Trim();
                    Contrato.Cod_Nucleo = dtb.Rows[0]["Cod_Nucleo"].ToString().Trim();
                    Contrato.Nome_Nucleo= dtb.Rows[0]["Nome_Nucleo"].ToString().Trim();
                    Contrato.Periodo_Campanha_Inicio = dtb.Rows[0]["Periodo_Campanha_Inicio"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Contrato.Periodo_Campanha_Termino = dtb.Rows[0]["Periodo_Campanha_Termino"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Contrato.Competencia = dtb.Rows[0]["Competencia"].ToString().ConvertToInt32();
                    Contrato.Indica_Grade = dtb.Rows[0]["Indica_Grade"].ToString().ConvertToInt32();
                    Contrato.Cod_Cliente = dtb.Rows[0]["Cod_Cliente"].ToString().Trim();
                    Contrato.Nome_Cliente = dtb.Rows[0]["Nome_Cliente"].ToString().Trim();
                    Contrato.Cod_Agencia = dtb.Rows[0]["Cod_Agencia"].ToString().Trim();
                    Contrato.Nome_Agencia = dtb.Rows[0]["Nome_Agencia"].ToString().Trim();
                    Contrato.Vlr_Informado = dtb.Rows[0]["Vlr_Informado"].ToString().ConvertToMoney();
                    Contrato.Id_Simulacao = dtb.Rows[0]["Id_Simulacao"].ToString().ConvertToInt32();
                    Contrato.Id_Esquema = dtb.Rows[0]["Id_Esquema"].ToString().ConvertToInt32();
                    Contrato.Cod_Mercado = dtb.Rows[0]["Cod_Mercado"].ToString().Trim();
                    Contrato.Caracteristica_Contrato =dtb.Rows[0]["Caracteristica_Contrato"].ToString().Trim();
                    Contrato.Cod_Programa= dtb.Rows[0]["Cod_Programa_Patrocinado"].ToString().Trim();
                    Contrato.Editar_Negociacao = true;
                    Contrato.Editar_Empresa_Venda = false;
                    Contrato.Editar_Empresa_Faturamento = false;
                    Contrato.Editar_Tipo_Midia = true;
                    Contrato.Editar_Mercado = false;
                    Contrato.Editar_Abrangencia = false;
                    Contrato.Editar_Periodo_Campanha = false;
                    Contrato.Editar_Valor_Informado = false;
                    Contrato.Editar_Nucleo = true;
                    Contrato.Indica_Midia_Online = dtb.Rows[0]["Indica_Midia_Online"].ToString().ConvertToBoolean();

                    if (String.IsNullOrEmpty(Contrato.Cod_Cliente))
                    {
                        Contrato.Editar_Cliente = true;
                    }
                    if (String.IsNullOrEmpty(Contrato.Cod_Agencia))
                    {
                        Contrato.Editar_Agencia = true;
                    }
                    if (String.IsNullOrEmpty(Contrato.Cod_Contato))
                    {
                        Contrato.Editar_Contato = true;
                    }
                    Contrato.Comerciais = AddComerciaisEsquema(pId_Esquema);
                    Contrato.Veiculacoes = AddVeiculacoesEsquema(pId_Esquema);
                    Contrato.Veiculos = AddVeiculosEsquema(pId_Esquema);
                    Contrato.Sequenciador_Veiculacao = Sequenciador_Veiculacao;
                    Contrato.VeiculacoesOnLine = AddVeiculacoesOnLineEsquema(pId_Esquema);
                    Contrato.Sequenciador_Comercial = Sequenciador_Comercial;

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
            return Contrato;
        }
        private List<ComercialModel> AddComerciaisEsquema(Int32 pId_Esquema)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ComercialModel> Comerciais = new List<ComercialModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_Comercial_Esquema]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Esquema", pId_Esquema);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Comerciais.Add(new ComercialModel()
                    {
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Nome_Tipo_Comercial = drw["Nome_Tipo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Cod_Red_Produto = drw["Cod_Red_Produto"].ToString().ConvertToInt32(),
                        Nome_Produto = drw["Nome_Produto"].ToString(),
                        Indica_Titulo_Determinar = drw["Indica_Titulo_Determinar"].ToString().ConvertToBoolean(),
                        Tem_Veiculacao = true
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
        public List<VeiculacacaoModel> AddVeiculacoesEsquema(Int32 pId_Esquema)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculacacaoModel> Veiculacoes = new List<VeiculacacaoModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Midia_Get]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Esquema", pId_Esquema);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Sequenciador_Veiculacao++;
                    Veiculacoes.Add(new VeiculacacaoModel()
                    {
                        Id_Veiculacao = Sequenciador_Veiculacao,
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Cod_Caracteristica = drw["Cod_Caracteristica"].ToString(),
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Permite_Editar = true,
                        Qtd_Total = drw["Qtd_Total_Insercoes"].ToString().ConvertToInt32(),
                        Insercoes = AddInsercoesEsquema(drw["Id_Midia"].ToString().ConvertToInt32()),
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
            return Veiculacoes;
        }
        public List<VeiculacaoOnLineModel> AddVeiculacoesOnLineEsquema(Int32 pId_Esquema)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculacaoOnLineModel> Veiculacoes = new List<VeiculacaoOnLineModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Add_Veiculacao_OnLine");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Esquema", pId_Esquema);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Sequenciador_Veiculacao++;
                    Veiculacoes.Add(new VeiculacaoOnLineModel()
                    {
                        Id_Veiculacao = Sequenciador_Veiculacao,
                        Data_Inicio = drw["Data_Inicio"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy"),
                        Data_Fim = drw["Data_Fim"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy"),
                        Cod_Caracteristica = drw["Cod_Caracteristica"].ToString(),
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Permite_Editar = true,
                        Nome_Tipo_Comercializacao = drw["Nome_Tipo_Comercializacao"].ToString(),
                        Qtd = drw["Qtd"].ToString().ConvertToInt32(),
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
            return Veiculacoes;
        }
        private List<InsercoesModel> AddInsercoesEsquema(Int32 pId_Midia)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<InsercoesModel> Insercoes = new List<InsercoesModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Insercoes_Get]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Midia", pId_Midia);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Insercoes.Add(new InsercoesModel()
                    {
                        Id_Veiculacao = Sequenciador_Veiculacao,
                        Data_Exibicao = drw["Data_Exibicao"].ToString().ConvertToDatetime(),
                        Dia = drw["Dia"].ToString().ConvertToByte(),
                        Dia_Semana = drw["Dia_Semana"].ToString(),
                        Qtd = drw["Qtd"].ToString().ConvertToInt32(),
                        Tem_Grade = drw["Tem_Grade"].ToString().ConvertToBoolean(),
                        Valido = true,
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
            return Insercoes;
        }
        private List<VeiculoModel> AddVeiculosEsquema(Int32 pId_Esquema)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculoModel> Veiculos = new List<VeiculoModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Esquema_Veiculo_Get]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Esquema", pId_Esquema);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Veiculos.Add(new VeiculoModel()
                    {
                        Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                        Nome_Veiculo = drw["Nome_Veiculo"].ToString()
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
            return Veiculos;
        }
        public DataTable MapaReservaValidarNegociacao(ContratoModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Mapa_Reserva_Validar_Negociacao");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Operacao", param.Operacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", param.Numero_Negociacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Venda", param.Cod_Empresa_Venda);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", param.Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Cliente", param.Cod_Cliente);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Agencia", param.Cod_Agencia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Contato", param.Cod_Contato);
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
        public DataTable MapaReservaLocalizarNegociacao(ContratoModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_Localizar_Negociacao]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Operacao", param.Operacao);
                if (!String.IsNullOrEmpty(param.Periodo_Campanha_Inicio))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Inicio_Campanha", param.Periodo_Campanha_Inicio.ConvertToDatetime());
                }
                if (!String.IsNullOrEmpty(param.Periodo_Campanha_Termino))
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Termino_Campanha", param.Periodo_Campanha_Termino.ConvertToDatetime());
                }
                if (param.Operacao=="Import")
                {
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Venda", param.Cod_Empresa_Venda);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", param.Cod_Empresa_Faturamento);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Cliente", param.Cod_Cliente);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Agencia", param.Cod_Agencia);
                    Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Contato", param.Cod_Contato);
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
        public DataTable MapaReservaListarProdutoCliente(String Cod_Terceiro)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            DataTable dtbProduto = new DataTable();
            dtbProduto.Columns.Add("Codigo", typeof(int));
            dtbProduto.Columns.Add("Descricao", typeof(string));

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[PR_PROPOSTA_Produto_Cliente_List]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Red_Produto", DBNull.Value);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Terceiro", Cod_Terceiro);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    DataRow drProduto = dtbProduto.NewRow();
                    drProduto["Codigo"] = drw["Cod_Red_Produto"].ToString().ConvertToInt32();
                    drProduto["Descricao"] = drw["Nome_Produto"].ToString();
                    dtbProduto.Rows.Add(drProduto);
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
            return dtbProduto;
        }
        public ContratoModel MapaReservaGetContrato(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();
            ContratoModel Contrato = new ContratoModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_MapaReserva_Get_Contrato]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {

                    Contrato.Cod_Empresa_Venda = dtb.Rows[0]["Cod_Empresa"].ToString();
                    Contrato.Nome_Empresa_Venda = dtb.Rows[0]["Nome_Empresa_Venda"].ToString();
                    Contrato.Numero_Mr = dtb.Rows[0]["Numero_Mr"].ToString().ConvertToInt32();
                    Contrato.Numero_Negociacao = dtb.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32();
                    Contrato.Sequencia_Mr = dtb.Rows[0]["Sequencia_Mr"].ToString().ConvertToInt32();
                    Contrato.Cod_Programa = dtb.Rows[0]["Cod_Programa"].ToString();
                    Contrato.Cod_Empresa_Faturamento = dtb.Rows[0]["Cod_Empresa_Faturamento"].ToString();
                    Contrato.Nome_Empresa_Faturamento = dtb.Rows[0]["Nome_Empresa_Faturamento"].ToString();
                    Contrato.Caracteristica_Contrato = dtb.Rows[0]["Caracteristica_Contrato"].ToString();
                    Contrato.Cod_Contato = dtb.Rows[0]["Cod_Contato"].ToString();
                    Contrato.Nome_Contato = dtb.Rows[0]["Nome_Contato"].ToString();
                    Contrato.Cod_Nucleo = dtb.Rows[0]["Cod_Nucleo"].ToString();
                    Contrato.Nome_Nucleo = dtb.Rows[0]["Nome_Nucleo"].ToString();
                    Contrato.Cod_Tipo_Midia = dtb.Rows[0]["Cod_Tipo_Midia"].ToString();
                    Contrato.Data_Recepcao_Reserva = dtb.Rows[0]["Data_Recepcao_Reserva"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Contrato.Numero_PI = dtb.Rows[0]["Numero_PI"].ToString();
                    Contrato.Obs_Roteiro = dtb.Rows[0]["Obs_Roteiro"].ToString();
                    Contrato.Periodo_Campanha_Inicio = dtb.Rows[0]["Periodo_Campanha_Inicio"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Contrato.Periodo_Campanha_Termino = dtb.Rows[0]["Periodo_Campanha_Termino"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    Contrato.Competencia = dtb.Rows[0]["Competencia"].ToString().ConvertToInt32();
                    Contrato.Indica_Grade = dtb.Rows[0]["Indica_Grade"].ToString().ConvertToInt32();
                    Contrato.Cod_Cliente = dtb.Rows[0]["Cod_Cliente"].ToString();
                    Contrato.Nome_Cliente = dtb.Rows[0]["Nome_Cliente"].ToString();
                    Contrato.Cod_Agencia = dtb.Rows[0]["Cod_Agencia"].ToString();
                    Contrato.Nome_Agencia = dtb.Rows[0]["Nome_Agencia"].ToString();
                    Contrato.Indica_Por_Credito = dtb.Rows[0]["Indica_Por_Credito"].ToString().ConvertToBoolean(); ;
                    Contrato.Vlr_Informado = dtb.Rows[0]["Vlr_Informado"].ToString().ConvertToMoney();
                    Contrato.Desconto= dtb.Rows[0]["Desconto"].ToString().ConvertToPercent();
                    Contrato.Indica_Apoio = dtb.Rows[0]["Indica_Apoio"].ToString().ConvertToBoolean();
                    Contrato.Cod_Mercado = dtb.Rows[0]["Cod_Mercado"].ToString();
                    Contrato.Id_Contrato = dtb.Rows[0]["Id_Contrato"].ToString().ConvertToInt32();
                    Contrato.Obs_Contrato = dtb.Rows[0]["Obs_Contrato"].ToString();
                    Contrato.Campanha = dtb.Rows[0]["Campanha"].ToString();
                    Contrato.Codigo_Projeto = dtb.Rows[0]["Codigo_Projeto"].ToString();
                    Contrato.Versao_Projeto = dtb.Rows[0]["Versao_Projeto"].ToString().ConvertToInt32();
                    Contrato.Tem_Fatura = dtb.Rows[0]["Tem_Fatura"].ToString().ConvertToBoolean();
                    Contrato.Comprovado= dtb.Rows[0]["Comprovado"].ToString().ConvertToBoolean();
                    Contrato.Indica_Midia_Online= dtb.Rows[0]["Indica_Midia_Online"].ToString().ConvertToBoolean();
                    Contrato.Editar_Negociacao = true;
                    Contrato.Editar_Cliente = true;
                    Contrato.Editar_Agencia = true;
                    Contrato.Editar_Contato = true;
                    Contrato.Editar_Nucleo = true;
                    Contrato.Editar_Empresa_Venda = true;
                    Contrato.Editar_Empresa_Faturamento = true;
                    Contrato.Editar_Tipo_Midia = true;
                    Contrato.Editar_Mercado = true;
                    Contrato.Editar_Abrangencia = true;
                    Contrato.Editar_Periodo_Campanha = true;
                    Contrato.Editar_Valor_Informado = true;
                    Contrato.Comerciais = AddComerciais(pIdContrato);
                    //Contrato.Veiculacoes = AddVeiculacoes(pIdContrato); // nao trazer veiculacoes para edicao/ somente é permitido adicionar novas veiculacoes
                    Contrato.Veiculos = AddVeiculos(pIdContrato); 
                    Contrato.Sequenciador_Veiculacao = Sequenciador_Veiculacao;
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
            return Contrato;
        }
        private List<ComercialModel> AddComerciais(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ComercialModel> Comerciais = new List<ComercialModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_MapaReserva_Get_Comercial]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Sequenciador_Comercial++;
                    Comerciais.Add(new ComercialModel()
                    {
                        Id_Comercial = Sequenciador_Comercial,
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Cod_Tipo_Comercial = drw["Cod_Tipo_Comercial"].ToString(),
                        Titulo_Comercial = drw["Titulo_Comercial"].ToString(),
                        Nome_Tipo_Comercial = drw["Nome_Tipo_Comercial"].ToString(),
                        Duracao = drw["Duracao"].ToString().ConvertToInt32(),
                        Cod_Red_Produto = drw["Cod_Red_Produto"].ToString().ConvertToInt32(),
                        Nome_Produto = drw["Nome_Produto"].ToString(),
                        Numero_Fita = drw["Numero_Fita"].ToString(),
                        Indica_Titulo_Determinar = drw["Indica_Titulo_Determinar"].ToString().ConvertToBoolean(),
                        Tem_Veiculacao = drw["Tem_Veiculacao"].ToString().ConvertToBoolean(),
                        Cod_Tipo_Comercializacao = drw["Cod_Tipo_Comercializacao"].ToString(),
                        Nome_Tipo_Comercializacao = drw["Nome_Tipo_Comercializacao"].ToString(),
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
        public List<VeiculacacaoModel> AddVeiculacoes(Int32 pIdContrato,String pOperacao)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculacacaoModel> Veiculacoes = new List<VeiculacacaoModel>();
            try
            {
                Boolean bolPermiteEditar = false;
                Boolean SomenteDemanda = false;
                if (pOperacao.ToUpper() == "IMPORTARMAPA")
                {
                    bolPermiteEditar = true;
                    SomenteDemanda = true;
                }

                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_MapaReserva_Get_Midia_Contrato]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Somente_Demanda", SomenteDemanda);
                Adp.Fill(dtb);
                
                foreach (DataRow drw in dtb.Rows)
                {
                    Sequenciador_Veiculacao++;
                    Veiculacoes.Add(new VeiculacacaoModel()
                    {
                        Cod_Programa = drw["Cod_Programa"].ToString(),
                        Cod_Caracteristica = drw["Cod_Caracteristica"].ToString(),
                        Cod_Comercial = drw["Cod_Comercial"].ToString(),
                        Qtd_Total = drw["Qtd_Total"].ToString().ConvertToInt32(),
                        Id_Veiculacao = Sequenciador_Veiculacao,
                        Insercoes = AddInsercoes(pIdContrato, drw,SomenteDemanda),
                        Permite_Editar = bolPermiteEditar,

                        
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
            return Veiculacoes;
        }
        private List<InsercoesModel> AddInsercoes(Int32 pIdContrato, DataRow pRow,Boolean Indica_Somenta_Demanda)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<InsercoesModel> Insercoes = new List<InsercoesModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_MapaReserva_Get_Insercoes]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", pRow["Cod_Programa"].ToString());
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Caracteristica", pRow["Cod_Caracteristica"].ToString());
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Comercial", pRow["Cod_Comercial"].ToString());
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Somente_Demanda", Indica_Somenta_Demanda);

                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Insercoes.Add(new InsercoesModel()
                    {
                        Id_Veiculacao = Sequenciador_Veiculacao,
                        Data_Exibicao = drw["Data_Exibicao"].ToString().ConvertToDatetime(),
                        Dia = drw["Dia"].ToString().ConvertToByte(),
                        Dia_Semana = drw["Dia_Semana"].ToString(),
                        Qtd = drw["Qtd"].ToString().ConvertToInt32(),
                        Tem_Grade = drw["Tem_Grade"].ToString().ConvertToBoolean(),
                        Valido = drw["Tem_Grade"].ToString().ConvertToBoolean(),
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
            return Insercoes;
        }
        private List<VeiculoModel> AddVeiculos(Int32 pIdContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<VeiculoModel> Veiculos = new List<VeiculoModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_MapaReserva_Get_Veiculo]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", pIdContrato);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Veiculos.Add(new VeiculoModel()
                    {
                        Cod_Veiculo = drw["Cod_Veiculo"].ToString(),
                        Nome_Veiculo = drw["Nome_Veiculo"].ToString()
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
            return Veiculos;
        }
        public DataTable GetTerceirosNegociacao(GetTerceirosNegociacaoModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_Get_Terceiro_Negociacao]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", param.Numero_Negociacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Tabela", param.Tabela);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Codigo", param.Codigo);
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
        public DataTable MapaReservaSalvar(ContratoModel pContrato)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            String xmlVeiculos = null;
            String xmlComerciais = null;
            String xmlVeiculacoes = null;
            String xmlVeiculacoes_Online = null;
            if (pContrato.Veiculos.Count > 0)
            {
                xmlVeiculos = clsLib.SerializeToString(pContrato.Veiculos);
            }
            if (pContrato.Comerciais.Count > 0)
            {
                xmlComerciais = clsLib.SerializeToString(pContrato.Comerciais);
            }
            if (pContrato.Veiculacoes.Count > 0)
            {
                xmlVeiculacoes = clsLib.SerializeToString(pContrato.Veiculacoes);
            }
            if (pContrato.VeiculacoesOnLine.Count > 0)
            {
                xmlVeiculacoes_Online = clsLib.SerializeToString(pContrato.VeiculacoesOnLine);
            }

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Mapa_Reserva_Salvar");
                Adp.SelectCommand = cmd;
                //Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                //Adp.SelectCommand.Parameters.AddWithValue("@Par_Operacao", pContrato.Operacao);
                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Operacao", pContrato.Operacao);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Venda", pContrato.Cod_Empresa_Venda);
                clsLib.NewParameter(Adp, "@Par_Numero_Mr", pContrato.Numero_Mr, true);
                clsLib.NewParameter(Adp, "@Par_Numero_Negociacao", pContrato.Numero_Negociacao, true);
                clsLib.NewParameter(Adp, "@Par_Sequencia_Mr", pContrato.Sequencia_Mr, true);
                clsLib.NewParameter(Adp, "@Par_Cod_Programa", pContrato.Cod_Programa);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Faturamento", pContrato.Cod_Empresa_Faturamento);
                clsLib.NewParameter(Adp, "@Par_Caracteristica_Contrato", pContrato.Caracteristica_Contrato);
                clsLib.NewParameter(Adp, "@Par_Cod_Contato", pContrato.Cod_Contato);
                clsLib.NewParameter(Adp, "@Par_Cod_Nucleo", pContrato.Cod_Nucleo);
                clsLib.NewParameter(Adp, "@Par_Cod_Tipo_Midia", pContrato.Cod_Tipo_Midia);
                clsLib.NewParameter(Adp, "@Par_Indica_Midia_On_Line", pContrato.Indica_Midia_Online);
                clsLib.NewParameter(Adp, "@Par_Data_Recepcao_Reserva", pContrato.Data_Recepcao_Reserva.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Numero_PI", pContrato.Numero_PI);
                clsLib.NewParameter(Adp, "@Par_Obs_Roteiro", pContrato.Obs_Roteiro);
                clsLib.NewParameter(Adp, "@Par_Periodo_Campanha_Inicio", pContrato.Periodo_Campanha_Inicio.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Periodo_Campanha_Termino", pContrato.Periodo_Campanha_Termino.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Indica_Grade", pContrato.Indica_Grade);
                clsLib.NewParameter(Adp, "@Par_Cod_Cliente", pContrato.Cod_Cliente);
                clsLib.NewParameter(Adp, "@Par_Cod_Agencia", pContrato.Cod_Agencia);
                clsLib.NewParameter(Adp, "@Par_Indica_Por_Credito", pContrato.Indica_Por_Credito, true);
                clsLib.NewParameter(Adp, "@Par_Vlr_Informado", pContrato.Vlr_Informado);
                clsLib.NewParameter(Adp, "@Par_Desconto", pContrato.Desconto);
                clsLib.NewParameter(Adp, "@Par_Indica_Apoio", pContrato.Indica_Apoio, true);
                clsLib.NewParameter(Adp, "@Par_Cod_Mercado", pContrato.Cod_Mercado);
                clsLib.NewParameter(Adp, "@Par_Id_Contrato", pContrato.Id_Contrato, true);
                clsLib.NewParameter(Adp, "@Par_Obs_Contrato", pContrato.Obs_Contrato);
                clsLib.NewParameter(Adp, "@Par_Campanha", pContrato.Campanha);
                clsLib.NewParameter(Adp, "@Par_Codigo_Projeto", pContrato.Codigo_Projeto);
                clsLib.NewParameter(Adp, "@Par_Versao_Projeto", pContrato.Versao_Projeto, true);
                clsLib.NewParameter(Adp, "@Par_Criar_Negociacao", pContrato.Criar_Negociacao);
                clsLib.NewParameter(Adp, "@Par_Id_Simulacao", pContrato.Id_Simulacao);
                clsLib.NewParameter(Adp, "@Par_Id_Esquema", pContrato.Id_Esquema);
                clsLib.NewParameter(Adp, "@Par_Indica_Tqp", pContrato.Indica_Tqp);
                clsLib.NewParameter(Adp, "@Par_Comerciais", xmlComerciais);
                clsLib.NewParameter(Adp, "@Par_Veiculacoes", xmlVeiculacoes);
                clsLib.NewParameter(Adp, "@Par_Veiculacoes_Online", xmlVeiculacoes_Online);
                clsLib.NewParameter(Adp, "@Par_Veiculos", xmlVeiculos);

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
        public List<InsercoesModel> MapaReservaNewMidia(ParamNewMidiaModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<InsercoesModel> Insercoes = new List<InsercoesModel>();
            String xmlVeiculos = null;
            if (param.Veiculos.Count > 0)
            {
                xmlVeiculos = clsLib.SerializeToString(param.Veiculos);
            }

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_NewMidia]");
                Adp.SelectCommand = cmd;
                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Inicio_Campanha", param.Inicio_Campanha.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Fim_Campanha", param.Fim_Campanha.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Cod_Programa", param.Cod_Programa);
                clsLib.NewParameter(Adp, "@Par_Veiculos", xmlVeiculos);
                Adp.Fill(dtb);
                foreach (DataRow drw  in dtb.Rows)
                {
                    Insercoes.Add(new InsercoesModel()
                    {
                        Data_Exibicao = drw["Data_Exibicao"].ToString().ConvertToDatetime(),
                        Dia = drw["Dia"].ToString().ConvertToByte(),
                        Dia_Semana = drw["Dia_Semana"].ToString(),
                        Tem_Grade = drw["Tem_Grade"].ToString().ConvertToBoolean(),
                        Valido= drw["Valido"].ToString().ConvertToBoolean(),

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
            return Insercoes;
        }
        public String MapaReservaValidarPeriodo(ContratoModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            String Retorno = "";
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Mapa_Reserva_Consiste_Periodo_Campanha]");
                Adp.SelectCommand = cmd;
                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Numero_Negociacao", param.Numero_Negociacao);
                clsLib.NewParameter(Adp, "@Par_Periodo_Campanha_Inicio", param.Periodo_Campanha_Inicio.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Periodo_Campanha_Termino", param.Periodo_Campanha_Termino.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Id_Contrato", param.Id_Contrato,true);
                clsLib.NewParameter(Adp, "@Par_Indica_Midia_On_Line", param.Indica_Midia_Online, true);
                Adp.Fill(dtb);
                Retorno = dtb.Rows[0]["Mensagem"].ToString();
                
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
        public DataTable ValidarGradePeriodo(ParamNewMidiaModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            String xmlVeiculos = null;
            if (param.Veiculos.Count > 0)
            {
                xmlVeiculos = clsLib.SerializeToString(param.Veiculos);
            }
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Validar_Grade_Periodo]");
                Adp.SelectCommand = cmd;
                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Inicio_Campanha", param.Inicio_Campanha.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Fim_Campanha", param.Fim_Campanha.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Cod_Programa", param.Cod_Programa);
                clsLib.NewParameter(Adp, "@Par_Veiculos", xmlVeiculos);
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
        public Int32 GetIdContrato(MapaReservaFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Int32 Retorno = 0;
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Get_Id_Contrato");
                Adp.SelectCommand = cmd;
                clsLib.NewParameter(Adp, "@Par_Numero_Mr", Param.Numero_Mr);
                clsLib.NewParameter(Adp, "@Par_Sequencia_Mr", Param.Sequencia_Mr);
                Adp.Fill(dtb);
                if (dtb.Rows.Count>0)
                {
                    Retorno = dtb.Rows[0]["Id_Contrato"].ToString().ConvertToInt32();
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

        public DataTable DeParaContato(DeParaContatoModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_De_Para_Contato");
                Adp.SelectCommand = cmd;
                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Competencia", clsLib.CompetenciaInt(param.Competencia));
                clsLib.NewParameter(Adp, "@Par_Cod_Agencia", param.Cod_Agencia);
                clsLib.NewParameter(Adp, "@Par_Cod_Contato_De", param.Cod_Contato_De);
                clsLib.NewParameter(Adp, "@Par_Cod_Contato_Para", param.Cod_Contato_Para);
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

        public DeParaNegociacaooModel GetContratoDeParaNegociacao(DeParaNegociacaooModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            DeParaNegociacaooModel Retorno = new DeParaNegociacaooModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_MapaReserva_Get_Contrato");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Contrato", DBNull.Value);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa", param.Cod_Empresa);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Mr", param.Numero_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Sequencia_Mr", param.Sequencia_Mr);
                Adp.Fill(dtb);
                if (dtb.Rows.Count>0)
                {
                   Retorno.Cod_Empresa = dtb.Rows[0]["Cod_Empresa"].ToString();
                    Retorno.Numero_Mr = dtb.Rows[0]["Numero_Mr"].ToString().ConvertToInt32();
                    Retorno.Sequencia_Mr = dtb.Rows[0]["Sequencia_Mr"].ToString().ConvertToInt32();
                    Retorno.Cod_Agencia= dtb.Rows[0]["Cod_Agencia"].ToString();
                    Retorno.Nome_Agencia = dtb.Rows[0]["Nome_Agencia"].ToString();
                    Retorno.Cod_Cliente= dtb.Rows[0]["Cod_Cliente"].ToString();
                    Retorno.Nome_Cliente= dtb.Rows[0]["Nome_Cliente"].ToString();
                    Retorno.Negociacao_De = dtb.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32();
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

        public DataTable ProcessaDeParaNegociacao(DeParaNegociacaooModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "sp_Troca_Negociacao");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Negociacao", param.Negociacao_Para);
                Adp.SelectCommand.Parameters.AddWithValue("@Cod_Empresa", param.Cod_Empresa);
                Adp.SelectCommand.Parameters.AddWithValue("@Numero_Mr", param.Numero_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Sequencia_Mr", param.Sequencia_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Cod_Usuario", this.CurrentUser);
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

        public BaixaVeiculacaoModel MapaBaixarVeiculacao(BaixaVeiculacaoModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Baixa_Veiculacao");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Qualidade", param.Cod_Qualidade);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", param.Cod_Veiculo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Documento_De", param.Documento_De);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Documento_Para", param.Documento_Para);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa", param.Cod_Empresa);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Mr", param.Numero_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Sequencia_Mr", param.Sequencia_Mr);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", param.Data_Exibicao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Horario_Exibicao", param.Horario_Exibicao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Chave_Acesso   ", param.Chave_Acesso);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", param.Cod_Programa);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    param.Cod_Qualidade = dtb.Rows[0]["Cod_Qualidade"].ToString();
                    param.Documento_De= dtb.Rows[0]["Documento_De"].ToString();
                    param.Documento_Para = dtb.Rows[0]["Documento_Para"].ToString();
                    param.Critica= dtb.Rows[0]["Mensagem"].ToString();
                    param.Status = dtb.Rows[0]["Status"].ToString().ConvertToBoolean();
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
            return param;
        }
        public List<AlterarCVModel> AlterarCV(List<AlterarCVModel> param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            Boolean Valido = false;
            try
            {
                for (int i = 0; i < param.Count; i++)
                {
                    Valido = !String.IsNullOrEmpty(param[i].Cod_Empresa) ||
                        param[i].Numero_Mr != 0 ||
                        param[i].Sequencia_Mr != 0 ||
                        param[i].Cod_Comercial != "" ||
                        !String.IsNullOrEmpty(param[i].Cod_Veiculo) ||
                        !String.IsNullOrEmpty(param[i].Data_Exibicao) ||
                        !String.IsNullOrEmpty(param[i].Cod_Programa) ||
                        param[i].Chave_Acesso != 0 ||
                        !String.IsNullOrEmpty(param[i].Cod_Caracteristica_De) ||
                        !String.IsNullOrEmpty(param[i].Cod_Caracteristica_Para);

                    if (Valido)
                    {
                        SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Altera_Caracteristica");
                        Adp.SelectCommand = cmd;
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Usuario", this.CurrentUser);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa", param[i].Cod_Empresa);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_MR", param[i].Numero_Mr);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Sequencia_MR", param[i].Sequencia_Mr);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Comercial", param[i].Cod_Comercial);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Veiculo", param[i].Cod_Veiculo);
                        if (!string.IsNullOrEmpty(param[i].Data_Exibicao))
                        {
                            Adp.SelectCommand.Parameters.AddWithValue("@Par_Data_Exibicao", param[i].Data_Exibicao.ConvertToDatetime());
                        }
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Programa", param[i].Cod_Programa);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Chave_Acesso", param[i].Chave_Acesso);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Caracteristica", param[i].Cod_Caracteristica_De);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Caracteristica_Nova", param[i].Cod_Caracteristica_Para);
                        Adp.SelectCommand.Parameters.AddWithValue("@Par_Indica_Gravacao", 1);

                        Adp.Fill(dtb);
                        param[i].Qtd_Alterada = dtb.Rows[0]["Qtd_Alterada"].ToString().ConvertToInt32();
                        param[i].Qtd_Existente = dtb.Rows[0]["Qtd_Existente"].ToString().ConvertToInt32();
                        param[i].Qtd_Am_Utilizada = dtb.Rows[0]["Qtd_Am_Utilizada"].ToString().ConvertToInt32();
                        param[i].Qtd_Comprovante = dtb.Rows[0]["Qtd_Comprovante"].ToString().ConvertToInt32();
                        param[i].Critica = dtb.Rows[0]["Mensagem"].ToString();
                        cmd.Dispose();
                        dtb.Dispose();
                        Adp.Dispose();
                    }
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
            return param;
        }


    }
}
