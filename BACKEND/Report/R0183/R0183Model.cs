using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class R0183
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public R0183(String pCredential)
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
            public Double Valor { get; set; } = 0;
            public Double Valor_30 { get; set; } = 0;
            public Int32 Qtd { get; set; } = 0;
        }
        public class BreakModel
        {
            public String Horario { get; set; }
            public Int32 Duracao { get; set; }
            public Int32 Ocupacao { get; set; }
            public Int32 Saldo { get; set; }
        }
    }
}