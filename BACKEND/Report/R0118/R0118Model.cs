using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class R0118
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public R0118(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class ReportFilterModel
        {

            public String Title { get; set; }
            public String ProcName { get; set; }
            public String RptName { get; set; }
            public String Output{ get; set; }
            public Boolean Custom{ get; set; }
            public List<FilterModel> Filters { get; set; }

        }
        public class FilterModel
        {
            
            public String ParameterName { get; set; }
            public String Type { get; set; }
            public String Label { get; set; }
            public String Search { get; set; }
            public Boolean BigQuery { get; set; }
            public String Dictionary { get; set; }
            public String Value { get; set; }
            public Boolean Multiple { get; set; }
            public String Class { get; set; }
            public List<ArrayValueModel> ArrayValue { get; set; }
        }
        public class ArrayValueModel
        {
            public String Codigo { get; set; }
            public String Descricao { get; set; }
            public Boolean Selected { get; set; }
        }

        public class TotalizadorModel
        {
            public int Qtde_Exibida { get; set; }
            public Double Vlr_Tabela_Bruto { get; set; } = 0;
            public Double Vlr_Gerencial_Bruto { get; set; } = 0;
            public Double Vlr_Comissao_Agencia { get; set; } = 0;
            public Double Vlr_Gerencial_Liquido { get; set; } = 0;
            public Double Vlr_Comissao_Intermediario { get; set; } = 0;
            public Double Vlr_Varejo { get; set; } = 0;
            public Double Vlr_Receita_Liquida { get; set; } = 0;
        }
    }
}