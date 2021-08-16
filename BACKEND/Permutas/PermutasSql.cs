using CLASSDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace PROPOSTA
{
    public partial class Permutas

    {

        public List<PermutasModel> CarregarPermutas(PermutasFiltroModel Param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<PermutasModel> PermutasLista = new List<PermutasModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Permutas_List");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", Param.Numero_Negociacao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Contrato", Param.Numero_Contrato);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Venda", Param.Cod_Empresa_Venda);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Empresa_Faturamento", Param.Cod_Empresa_Faturamento);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia_Inicio", clsLib.CompetenciaInt(Param.Competencia_Inicio));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Competencia_Fim", clsLib.CompetenciaInt(Param.Competencia_Fim));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Agencia", Param.Agencia);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cliente", Param.Cliente);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Contato", Param.Contato);
                Adp.Fill(dtb);

                foreach (DataRow drw in dtb.Rows)
                {
                    PermutasLista.Add(new PermutasModel
                    {
                        Id_Permuta          = drw["Id_Permuta"].ToString().ConvertToInt32(),
                        Numero_Negociacao   = drw["Numero_Negociacao"].ToString().ConvertToInt32(),
                        Numero_Contrato     = drw["Numero_Contrato"].ToString().ConvertToInt32(),
                        Competencia_Inicial = clsLib.CompetenciaString(drw["Competencia_Inicial"].ToString().ConvertToInt32()),
                        Competencia_Final   = clsLib.CompetenciaString(drw["Competencia_Final"].ToString().ConvertToInt32()),
                        Nome_Agencia        = drw["Nome_Agencia"].ToString(),
                        Nome_Cliente        = drw["Nome_Cliente"].ToString(),
                        Nome_Contato        = drw["Nome_Contato"].ToString(),
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
            return PermutasLista;
        }


        public PermutasModel PermutasGetPermuta(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();
            PermutasModel permuta = new PermutasModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permuta_Get_Contrato]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", pIdPermuta);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    permuta.Numero_Contrato         = dtb.Rows[0]["Numero_Contrato"].ToString().ConvertToInt32();
                    permuta.Numero_Negociacao       = dtb.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32();
                    permuta.Valor_Verba             = dtb.Rows[0]["Valor_Verba"].ToString().ConvertToMoney();
                    permuta.Cod_Empresa_Venda       = dtb.Rows[0]["Cod_Empresa_Venda"].ToString();
                    permuta.Nome_Empresa_Venda      = dtb.Rows[0]["Nome_Empresa_Venda"].ToString();
                    permuta.Cod_Empresa_Faturamento = dtb.Rows[0]["Cod_Empresa_Faturamento"].ToString();
                    permuta.Nome_Empresa_Faturamento= dtb.Rows[0]["Nome_Empresa_Faturamento"].ToString();
                    permuta.Cod_Contato             = dtb.Rows[0]["Cod_Contato"].ToString();
                    permuta.Nome_Contato            = dtb.Rows[0]["Nome_Contato"].ToString();
                    permuta.Cod_Cliente             = dtb.Rows[0]["Cod_Cliente"].ToString();
                    permuta.Nome_Cliente            = dtb.Rows[0]["Nome_Cliente"].ToString();
                    permuta.Cod_Agencia             = dtb.Rows[0]["Cod_Agencia"].ToString();
                    permuta.Nome_Agencia            = dtb.Rows[0]["Nome_Agencia"].ToString();
                    permuta.Cod_Nucleo              = dtb.Rows[0]["Cod_Nucleo"].ToString();
                    permuta.Nome_Nucleo             = dtb.Rows[0]["Nome_Nucleo"].ToString();
                    permuta.Cod_Tipo_Midia          = dtb.Rows[0]["Cod_Tipo_Midia"].ToString();
                    permuta.Data_Autorizacao        = dtb.Rows[0]["Data_Autorizacao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    permuta.Competencia_Inicial     = clsLib.CompetenciaString(dtb.Rows[0]["Competencia_Inicial"].ToString().ConvertToInt32());
                    permuta.Competencia_Final     = clsLib.CompetenciaString(dtb.Rows[0]["Competencia_Final"].ToString().ConvertToInt32());
                    permuta.Id_Permuta              = dtb.Rows[0]["Id_Permuta"].ToString().ConvertToInt32();
                    permuta.Editar_Negociacao = true;
                    permuta.Editar_Cliente = true;
                    permuta.Editar_Agencia = true;
                    permuta.Editar_Contato = true;
                    permuta.Editar_Nucleo = true;
                    permuta.Editar_Empresa_Venda = true;
                    permuta.Editar_Empresa_Faturamento = true;
                    permuta.Editar_Tipo_Midia = true;
                    permuta.ItensPermuta            = AddItensPermuta(pIdPermuta);
                    permuta.ItensProdutosVeiculosModel = AddItensProdutosVeiculosModel(dtb.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32());
                    permuta.ItensResumoCliente= AddResumoCliente(pIdPermuta);
                    permuta.ItensResumoVeiculo = AddResumoVeiculo(pIdPermuta);

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
            return permuta;
        }
        private List<ResumoClienteModel> AddResumoCliente(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ResumoClienteModel> ItensResumoCliente = new List<ResumoClienteModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permuta_Get_ResumoCliente]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", pIdPermuta);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    ItensResumoCliente.Add(new ResumoClienteModel()
                    {
                        Tipo_Linha               = drw["Tipo_Linha"].ToString().ConvertToByte(),
                        Verba_Negociada          = drw["Verba_Negociada"].ToString().ConvertToDouble(),
                        Cod_Cliente              = drw["Cod_Cliente"].ToString(),
                        Nome_Cliente             = drw["Nome_Cliente"].ToString(),
                        Id_Item_Permuta          = drw["Id_Item_Permuta"].ToString().ConvertToInt32(),
                        Descricao_Item           = drw["Descricao_Item"].ToString(),
                        Valor_Tabela_Item        = drw["Valor_Tabela_Item"].ToString().ConvertToDouble(),
                        Quantidade               = drw["Quantidade"].ToString().ConvertToDouble(),
                        Desconto                 = drw["Desconto"].ToString().ConvertToDouble(),
                        Valor_Negociado_Unitario = drw["Valor_Negociado_Unitario"].ToString().ConvertToDouble(),
                        Valor_Tabela_Total       = drw["Valor_Tabela_Total"].ToString().ConvertToDouble(),
                        Valor_Negociado_Total    = drw["Valor_Negociado_Total"].ToString().ConvertToDouble(),
                        Saldo_Cliente            = drw["Saldo_Cliente"].ToString().ConvertToDouble(),
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
            return ItensResumoCliente;
        }

        // Aqui define adicionamento de Resumo do Veículo
        private List<ResumoVeiculoModel> AddResumoVeiculo(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ResumoVeiculoModel> ItensResumoVeiculo = new List<ResumoVeiculoModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permuta_Get_ResumoVeiculo]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", pIdPermuta);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    ItensResumoVeiculo.Add(new ResumoVeiculoModel()
                    {
                        Tipo_Linha                 = drw["Tipo_Linha"].ToString().ConvertToByte(),
                        Competencia                = drw["Competencia"].ToString(),
                        Verba_Negociada            = drw["Verba_Negociada"].ToString().ConvertToDouble(),
                        Cod_Cliente                = drw["Cod_Cliente"].ToString(),
                        Nome_Cliente               = drw["Nome_Cliente"].ToString(),
                        Valor_Tabela_Exibido       = drw["Valor_Tabela_Exibido"].ToString().ConvertToDouble(),
                        Desconto                   = drw["Desconto"].ToString().ConvertToDouble(),
                        Valor_Negociado_Exibido    = drw["Valor_Negociado_Exibido"].ToString().ConvertToDouble(),
                        Valor_Tabela_Financeiro    = drw["Valor_Tabela_Financeiro"].ToString().ConvertToDouble(),
                        Valor_Negociado_Financeiro = drw["Valor_Negociado_Financeiro"].ToString().ConvertToDouble(),
                        Saldo_Veiculo              = drw["Saldo_Veiculo"].ToString().ConvertToDouble(),
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
            return ItensResumoVeiculo;
        }






        private List<ItensPermutaModel> AddItensPermuta(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ItensPermutaModel> ItensPermuta = new List<ItensPermutaModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permuta_Get_ItensPermuta]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", pIdPermuta);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    ItensPermuta.Add(new ItensPermutaModel()
                    {
                        Id_Item_Permuta = drw["Id_Item_Permuta"].ToString().ConvertToInt32(),
                        Descricao       = drw["Descricao"].ToString(),
                        Quantidade      = drw["Quantidade"].ToString().ConvertToInt32(),
                        Valor_Unitario  = drw["Valor_Unitario"].ToString().ConvertToMoney(),
                        Desconto        = drw["Desconto"].ToString().ConvertToPercent(),
                        Vlr_Liq_Unit    = drw["Vlr_Liq_Unit"].ToString().ConvertToMoney(),
                        Valor_Tabela    = drw["Valor_Tabela"].ToString().ConvertToMoney(),
                        Valor_Liquido   = drw["Valor_Liquido"].ToString().ConvertToMoney(),
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
            return ItensPermuta;
        }

        private List<ItensProdutosVeiculosModel> AddItensProdutosVeiculosModel(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ItensProdutosVeiculosModel> ItensPermuta = new List<ItensProdutosVeiculosModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Produtos_Veiculos_Permuta]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", pIdPermuta);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    ItensPermuta.Add(new ItensProdutosVeiculosModel()
                    {
                        Contrato = drw["Contrato"].ToString(),
                        Numero_Negociacao = drw["Numero_Negociacao"].ToString().ConvertToInt32(),
                        Periodo_Campanha  = drw["Periodo_Campanha"].ToString(),
                        Nome_Agencia      = drw["Nome_Agencia"].ToString(),
                        Nome_Cliente      = drw["Nome_Cliente"].ToString(),
                        Nome_Contato      = drw["Nome_Contato"].ToString(),
                        Cod_Tipo_Midia    = drw["Cod_Tipo_Midia"].ToString(),

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
            return ItensPermuta;
        }

        public PermutasModel PermutasGetEntregaPermuta(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable();
            SimLib clsLib = new SimLib();
            PermutasModel permuta = new PermutasModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permuta_Get_Contrato]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", pIdPermuta);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    permuta.Numero_Contrato = dtb.Rows[0]["Numero_Contrato"].ToString().ConvertToInt32();
                    permuta.Numero_Negociacao = dtb.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32();
                    permuta.Valor_Verba = dtb.Rows[0]["Valor_Verba"].ToString().ConvertToMoney();
                    permuta.Cod_Empresa_Venda = dtb.Rows[0]["Cod_Empresa_Venda"].ToString();
                    permuta.Nome_Empresa_Venda = dtb.Rows[0]["Nome_Empresa_Venda"].ToString();
                    permuta.Cod_Empresa_Faturamento = dtb.Rows[0]["Cod_Empresa_Faturamento"].ToString();
                    permuta.Nome_Empresa_Faturamento = dtb.Rows[0]["Nome_Empresa_Faturamento"].ToString();
                    permuta.Cod_Contato = dtb.Rows[0]["Cod_Contato"].ToString();
                    permuta.Nome_Contato = dtb.Rows[0]["Nome_Contato"].ToString();
                    permuta.Cod_Cliente = dtb.Rows[0]["Cod_Cliente"].ToString();
                    permuta.Nome_Cliente = dtb.Rows[0]["Nome_Cliente"].ToString();
                    permuta.Cod_Agencia = dtb.Rows[0]["Cod_Agencia"].ToString();
                    permuta.Nome_Agencia = dtb.Rows[0]["Nome_Agencia"].ToString();
                    permuta.Cod_Nucleo = dtb.Rows[0]["Cod_Nucleo"].ToString();
                    permuta.Nome_Nucleo = dtb.Rows[0]["Nome_Nucleo"].ToString();
                    permuta.Cod_Tipo_Midia = dtb.Rows[0]["Cod_Tipo_Midia"].ToString();
                    permuta.Data_Autorizacao = dtb.Rows[0]["Data_Autorizacao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy");
                    permuta.Competencia_Inicial = clsLib.CompetenciaString(dtb.Rows[0]["Competencia_Inicial"].ToString().ConvertToInt32());
                    permuta.Competencia_Final = clsLib.CompetenciaString(dtb.Rows[0]["Competencia_Final"].ToString().ConvertToInt32());
                    permuta.Id_Permuta = dtb.Rows[0]["Id_Permuta"].ToString().ConvertToInt32();
                    permuta.ItensEntregaPermuta = AddItensEntregaPermuta(pIdPermuta);

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
            return permuta;
        }

        private List<ItensEntregaPermutaModel> AddItensEntregaPermuta(Int32 pIdPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            List<ItensEntregaPermutaModel> ItensEntregaPermuta = new List<ItensEntregaPermutaModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permuta_GetEntrega_ItensPermuta]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", pIdPermuta);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    ItensEntregaPermuta.Add(new ItensEntregaPermutaModel()
                    {
                        Id_Entrega_Permuta  = drw["Id_Entrega_Permuta"].ToString().ConvertToInt32(),
                        Id_Permuta          = drw["Id_Permuta"].ToString().ConvertToInt32(),
                        Id_Item_Permuta     = drw["Id_Item_Permuta"].ToString().ConvertToInt32(),
                        Numero_Nota_Fiscal  = drw["Numero_Nota_Fiscal"].ToString(),
                        Serie               = drw["Serie"].ToString(),
                        Descricao           = drw["Descricao"].ToString(),
                        Quantidade          = drw["Quantidade"].ToString().ConvertToInt32(),
                        Valor_Liq_Unitario  = drw["Valor_Liq_Unitario"].ToString().ConvertToMoney(),
                        Vlr_Liq_Total       = drw["Vlr_Liq_Total"].ToString().ConvertToMoney(),
                        Data_Emissao        = drw["Data_Emissao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy"),
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
            return ItensEntregaPermuta;
        }


        public DataTable PermutasValidarNegociacao(PermutasModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Permutas_Validar_Negociacao");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", param.Numero_Negociacao);
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

        

        public DataTable PermutasSalvar(PermutasModel pPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            String xmlItensPermutas = null;

            if (pPermuta.ItensPermuta.Count > 0)
            {
                xmlItensPermutas = clsLib.SerializeToString(pPermuta.ItensPermuta);
            }

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Permutas_Salvar]");
                Adp.SelectCommand = cmd;

                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Operacao", pPermuta.Operacao);
                clsLib.NewParameter(Adp, "@Par_Numero_Contrato", pPermuta.Numero_Contrato);
                clsLib.NewParameter(Adp, "@Par_Numero_Negociacao", pPermuta.Numero_Negociacao);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Venda", pPermuta.Cod_Empresa_Venda);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Faturamento", pPermuta.Cod_Empresa_Faturamento);
                clsLib.NewParameter(Adp, "@Par_Cod_Cliente", pPermuta.Cod_Cliente);
                clsLib.NewParameter(Adp, "@Par_Cod_Agencia", pPermuta.Cod_Agencia);
                clsLib.NewParameter(Adp, "@Par_Cod_Nucleo", pPermuta.Cod_Nucleo);
                clsLib.NewParameter(Adp, "@Par_Cod_Contato", pPermuta.Cod_Contato);
                clsLib.NewParameter(Adp, "@Par_Data_Autorizacao", pPermuta.Data_Autorizacao.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Id_Permuta", pPermuta.Id_Permuta);
                clsLib.NewParameter(Adp, "@Par_ItensPermutas", xmlItensPermutas);

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


        public DataTable PermutasEntregaSalvar(PermutasModel pPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();

            String xmlItensEntregaPermutas = null;

            if (pPermuta.ItensEntregaPermuta.Count > 0)
            {
                xmlItensEntregaPermutas = clsLib.SerializeToString(pPermuta.ItensEntregaPermuta);
            }

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_PermutasEntrega_Salvar]");
                Adp.SelectCommand = cmd;

                clsLib.NewParameter(Adp, "@Par_Login", this.CurrentUser);
                clsLib.NewParameter(Adp, "@Par_Operacao", pPermuta.Operacao);
                clsLib.NewParameter(Adp, "@Par_Numero_Contrato", pPermuta.Numero_Contrato);
                clsLib.NewParameter(Adp, "@Par_Numero_Negociacao", pPermuta.Numero_Negociacao);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Venda", pPermuta.Cod_Empresa_Venda);
                clsLib.NewParameter(Adp, "@Par_Cod_Empresa_Faturamento", pPermuta.Cod_Empresa_Faturamento);
                clsLib.NewParameter(Adp, "@Par_Cod_Cliente", pPermuta.Cod_Cliente);
                clsLib.NewParameter(Adp, "@Par_Cod_Agencia", pPermuta.Cod_Agencia);
                clsLib.NewParameter(Adp, "@Par_Cod_Nucleo", pPermuta.Cod_Nucleo);
                clsLib.NewParameter(Adp, "@Par_Data_Autorizacao", pPermuta.Data_Autorizacao.ConvertToDatetime());
                clsLib.NewParameter(Adp, "@Par_Cod_Contato", pPermuta.Cod_Contato);
                clsLib.NewParameter(Adp, "@Par_Id_Permuta", pPermuta.Id_Permuta);
                clsLib.NewParameter(Adp, "@Par_ItensEntregaPermutas", xmlItensEntregaPermutas);

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



        public DataTable ExcluirItensPermuta(ItensPermutaModel pExcluirIntesPermuta)
        {
            
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {

                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Excluir_ItensPermuta");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Item_Permuta", pExcluirIntesPermuta.Id_Item_Permuta) ;
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

        public DataTable ExcluirItensEntregaPermuta(ItensEntregaPermutaModel pExcluirIntesEntregaPermuta)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {

                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Excluir_ItensEntregaPermuta");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Entrega_Permuta", pExcluirIntesEntregaPermuta.Id_Entrega_Permuta);
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

        public DataTable PermutasValidarItemPermuta(PermutasModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_ValidarItemPermuta]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", param.Id_Permuta);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Item_Permuta", param.Id_Item_Permuta);
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

        public DataTable PermutasPesquisarItemPermuta(PermutasModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_PesquisarItemPermuta]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Permuta", param.Id_Permuta);
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

 

        public DataTable ProdutosVeiculos(PermutasModel param)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[Pr_Proposta_Produtos_Veiculos_Permuta]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Numero_Negociacao", param.Numero_Negociacao);
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
