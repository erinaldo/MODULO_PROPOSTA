using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class R0050
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public R0050(String pCredential)
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
            public Double Vlr_Amort_Normal { get; set; }
            public Double Vlr_Amort_Merchandising { get; set; }
            public Double Vlr_Amort_Patrocinio { get; set; }
            public Double Vlr_Amort_Apoio { get; set; }
            public Double Vlr_Amort_Evento { get; set; }
            public Double Vlr_BAV_Normal { get; set; }
            public Double Vlr_BAV_Merchandising { get; set; }
            public Double Vlr_BAV_Patrocinio { get; set; }
            public Double Vlr_BAV_Apoio { get; set; }
            public Double Vlr_BAV_Evento { get; set; }
            public Double Vlr_BAV_Correcao { get; set; }
            public Double Vlr_Venda_Liquida { get; set; }
            public Double Sub_Total { get; set; }
            public Double Total_Bav { get; set; }


        }
    }
}