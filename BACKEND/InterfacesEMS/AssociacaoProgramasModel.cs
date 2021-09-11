
using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class AssociacaoProgramas
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public AssociacaoProgramas(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class AssociacaoProgramasModel
        {
            public String Operacao { get; set; }
            public String Cod_Empresa_Faturamento { get; set; }
            public String Nome_Empresa_Faturamento { get; set; }
            public Int32  Cod_Representante { get; set; }
            public String Cod_Programa { get; set; }
            public String Titulo { get; set; }
            public String Cod_Item { get; set; }
            public String Cod_Item_Antecipado { get; set; }
        }


    }
}
