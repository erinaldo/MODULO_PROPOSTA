using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class Report
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public Report(String pCredential)
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
        public class ReportDefModel
        {
            public String ReportName { get; set; }
            public String PageSize { get; set; }
            public String PageOrientation { get; set; }
            public List<FieldsModel> Fields { get; set; }
        }
        public class FieldsModel
        {
            public String SqlField { get; set; }
            public String PositionX { get; set; }
            public String PositionY { get; set; }
            public Boolean Fixed { get; set; }
            public String FontName { get; set; }
            public String FontSize { get; set; }
            public String FontStyle { get; set; }
            public Boolean FontBold { get; set; }
            public String Backcolor { get; set; }
            public String Color { get; set; }
            public String Align { get; set; }
            public String HeaderText { get; set; }
            public String HeaderFontName { get; set; }
            public String HeaderFontSize { get; set; }
            public String HeaderFontStyle { get; set; }
            public Boolean HeaderFontBold { get; set; }
            public String width { get; set; }
        }
    }
}