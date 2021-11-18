using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class RetornarEMS
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public RetornarEMS(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class RetornarEMSModel
        {
            //--Filtro
            public String Cod_Empresa_Faturamento { get; set; }
            public String Nome_Empresa_Faturamento { get; set; }
            public Boolean Indica_Lista_Negra { get; set; }
            public Boolean Indica_Baixa_Duplicata { get; set; }
            public Boolean Indica_Numeracao_Fatura { get; set; }
            public String Data_Inicial { get; set; }
            public String Data_Final { get; set; }
            //--Lista Negra
            public String STR_CGCCPF { get; set; }
            //--Numeração
            public String NR_FATURA { get; set; }
            public String NR_NOTA_ENT_FUT { get; set; }
            public String COD_ESTABEL { get; set; }
            public String REC_ORIGEM { get; set; }
            //--Baixa
            public String NUM_NOTA { get; set; }
            public String SEQ_PARCELA { get; set; }
            public String DAT_PAGAM { get; set; }
            public Decimal VLR_PAGO { get; set; }
            public Decimal VLR_JUROS { get; set; }
        }

        public class objListaNegraModel
        {
            public String STR_CGCCPF { get; set; }
        }

        public class objNumeracaoFaturaModel
        {
            public String NR_FATURA { get; set; }
            public String NR_NOTA_ENT_FUT { get; set; }
            public String COD_ESTABEL { get; set; }
            public String REC_ORIGEM { get; set; }
        }

        public class ParamNumeracaoFaturaModel
        {
            public String dat_transm_inicial { get; set; }
            public String dat_transm_final { get; set; }
            public String cod_estabel { get; set; }
        }

        public class objBaixaDuplicataModel
        {
            public String NUM_NOTA { get; set; }
            public String SEQ_PARCELA { get; set; }
            public String DAT_PAGAM { get; set; }
            public Decimal VLR_PAGO { get; set; }
            public Decimal VLR_JUROS { get; set; }
        }

        public class ResponseModel
        {
            public Boolean IsSuccessful { get; set; }
            public String Message { get; set; }
        }
    }
}