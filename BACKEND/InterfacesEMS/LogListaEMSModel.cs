using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class LogListaEMS
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public LogListaEMS(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class LogListaEMSModel
        {
            public String Operacao { get; set; }
            public String Data_Inicial { get; set; }
            public String Data_Final { get; set; }
            public String Cod_Cliente { get; set; }
            public String Razao_Social_Cliente { get; set; }
            public String CGC_Cliente { get; set; }
            public String Razao_Social_Agencia { get; set; }
            public String CGC { get; set; }
            public String Data_Processamento { get; set; }
            public String Cod_Usuario { get; set; }
            public Int32 Numero_Mr { get; set; }
            public Int32 Sequencia_Mr { get; set; }
            public String Cod_Empresa { get; set; }
            public String Periodo_Veiculacao { get; set; }
            public Int32 Qtd { get; set; }
            public Int32 Status { get; set; }
            public Boolean chkCom_Programacao { get; set; }
            public Boolean chkSem_Programacao { get; set; }
            public Boolean chkDesativacao { get; set; }
            public Boolean chkReativacao { get; set; }
        }
    }
}