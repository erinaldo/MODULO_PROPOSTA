using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class Nucleo
    {
        private string Credential;
        private string CurrentUser;
        private SimLib clsLib = new SimLib();
        public Nucleo(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class NucleoModel
        {
            public String Cod_Nucleo { get; set; }
            public String Descricao { get; set; }
            public String Cod_Empresa { get; set; }
            public String Cod_Empresa_Faturamento { get; set; }
            public String Cod_Centro_Custo { get; set; }
            public string Id_operacao { get; set; }
        }
        public class FiltroModel
        {
            public String Cod_Nucleo { get; set; }
            public String Descricao { get; set; }
            public String Cod_Empresa { get; set; }
            public String Desc_Empresa { get; set; }
            public String Cod_Empresa_Fatura { get; set; }
            public String Desc_Empresa_Fatura { get; set; }
            public String Cod_Centro_Custo { get; set; }
            public String Desc_Centro_Custo { get; set; }
            public string Id_operacao { get; set; }
        }
    }
} 
  