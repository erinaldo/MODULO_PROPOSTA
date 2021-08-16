using System;
using System.Collections.Generic;


namespace PROPOSTA
{
    public partial class Permutas
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public Permutas(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class PermutasModel
        {
            public String Operacao { get; set; }
            public Int32  Numero_Contrato { get; set; }
            public Int32  Numero_Negociacao { get; set; }
            public String Valor_Verba { get; set; }
            public Int32  Id_Item_Permuta { get; set; }
            public Int32  Id_Permuta { get; set; }
            public String Cod_Contato { get; set; }
            public String Nome_Contato { get; set; }
            public String Competencia_Inicial { get; set; }
            public String Competencia_Final { get; set; }
            public String Cod_Cliente { get; set; }
            public String Nome_Cliente { get; set; }
            public String Cod_Agencia { get; set; }
            public String Nome_Agencia { get; set; }
            public String Data_Autorizacao { get; set; }
            public String Cod_Empresa_Venda { get; set; }
            public String Nome_Empresa_Venda { get; set; }
            public String Cod_Empresa_Faturamento { get; set; }
            public String Nome_Empresa_Faturamento { get; set; }
            public String Cod_Nucleo { get; set; }
            public String Nome_Nucleo { get; set; }
            public String Cod_Tipo_Midia { get; set; }
            public Boolean Editar_Negociacao { get; set; } = true;
            public Boolean Editar_Cliente { get; set; } = true;
            public Boolean Editar_Agencia { get; set; } = true;
            public Boolean Editar_Contato { get; set; } = true;
            public Boolean Editar_Nucleo { get; set; } = true;
            public Boolean Editar_Empresa_Venda { get; set; } = true;
            public Boolean Editar_Empresa_Faturamento { get; set; } = true;
            public Boolean Editar_Tipo_Midia { get; set; } = true;
            public List<ItensPermutaModel> ItensPermuta { get; set; }
            public List<ItensEntregaPermutaModel> ItensEntregaPermuta { get; set; }
            public List<ItensProdutosVeiculosModel> ItensProdutosVeiculosModel { get; set; }
            //public List<ResumoClienteModel> ResumoCLiente { get; set; }
            public List<ResumoClienteModel> ItensResumoCliente { get; set; }
            public List<ResumoVeiculoModel> ItensResumoVeiculo { get; set; }

        }
        public class ItensPermutaModel
        {
            public Int32 Id_Item_Permuta { get; set; }
            //public Int32 Id_Permuta { get; set; }
            public String Descricao { get; set; }
            public Int32 Quantidade { get; set; }
            public String Valor_Unitario { get; set; }
            public String Desconto { get; set; }
            public String Vlr_Liq_Unit { get; set; }
            public String Valor_Tabela { get; set; }
            public String Valor_Liquido { get; set; }
        }

        public class ItensEntregaPermutaModel
        {
            public Int32  Id_Entrega_Permuta { get; set; }
            public Int32  Id_Permuta { get; set; }
            public Int32 Id_Item_Permuta { get; set; }
            public String Numero_Nota_Fiscal { get; set; }
            public String Numero_Nota { get; set; }
            public String Serie { get; set; }
            public Int32  Quantidade { get; set; }
            public String Descricao { get; set; }
            public String Valor_Liq_Unitario { get; set; }
            public String Vlr_Liq_Total { get; set; }
            public String Data_Emissao { get; set; }

        }


        public class PermutasFiltroModel
        {
            public Int32 Numero_Negociacao { get; set; }
            public Int32 Numero_Contrato { get; set; }
            public String Numero_Pi { get; set; }
            public String Competencia_Inicio { get; set; }
            public String Competencia_Fim { get; set; }
            public String Agencia { get; set; }
            public String Cliente { get; set; }
            public String Cod_Empresa_Venda { get; set; }
            public String Cod_Empresa_Faturamento { get; set; }
            public String Contato { get; set; }
            
        }


        public class ItensProdutosVeiculosModel
        {

            public Int32 Numero_Negociacao { get; set; }
            public String Contrato { get; set; }
            public String Periodo_Campanha{ get; set; }
            public String Nome_Cliente { get; set; }
            public String Nome_Agencia { get; set; }
            public String Nome_Contato { get; set; }
            public String Cod_Tipo_Midia { get; set; }
        }



        public class GetTerceirosNegociacaoModel
        {
            public Int32 Numero_Negociacao { get; set; }
            public String Tabela { get; set; }
            public String Codigo { get; set; }
        }

        public class ResumoClienteModel
        {
            public Byte Tipo_Linha { get; set; }
            public Double Verba_Negociada           { get; set; }
            public String Cod_Cliente               { get; set; }
            public String Nome_Cliente              { get; set; }
            public Int32  Id_Item_Permuta           { get; set; }
            public String Descricao_Item            { get; set; }
            public Double Valor_Tabela_Item         { get; set; }
            public Double Quantidade                { get; set; }
            public Double Desconto                  { get; set; }
            public Double Valor_Negociado_Unitario  { get; set; }
            public Double Valor_Tabela_Total        { get; set; }
            public Double Valor_Negociado_Total     { get; set; }
            public Double Saldo_Cliente             { get; set; }
        }



        public class ResumoVeiculoModel
        {
            public Byte Tipo_Linha { get; set; }
            public String Competencia { get; set; }
            public Double Verba_Negociada { get; set; }
            public String Cod_Cliente { get; set; }
            public String Nome_Cliente { get; set; }
            public Double Valor_Tabela_Exibido { get; set; }
            public Double Desconto { get; set; }
            public Double Valor_Negociado_Exibido{ get; set; }
            public Double Valor_Tabela_Financeiro { get; set; }
            public Double Valor_Negociado_Financeiro { get; set; }
            public Double Saldo_Veiculo { get; set; }
        }



    }
}