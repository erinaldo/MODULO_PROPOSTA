using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class AssociacaoContatos
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public AssociacaoContatos(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class AssociacaoContatosModel
        {
            public String Operacao          { get; set; }
            public String Cod_Contato       { get; set; }
            public String Nome              { get; set; }
            public Int32  Cod_Representante { get; set; }
        }


    }
}