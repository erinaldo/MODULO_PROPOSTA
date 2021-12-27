using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROPOSTA
{
    public partial class Determinacao
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public Determinacao(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class ContratoModel
        {
            public Int32 Id_Contrato { get; set; }
            public String Cod_Empresa { get; set; }
            public Int32 Numero_Mr { get; set; }
            public Int32 Sequencia_Mr { get; set; }
            public String Cod_Agencia { get; set; }
            public String Nome_Agencia { get; set; }
            public String Cod_Cliente { get; set; }
            public String Nome_Cliente { get; set; }
            public String Data_Inicio { get; set; }
            public String Data_Fim { get; set; }
            public Int32 Competencia { get; set; }
            public String Competencia_String { get; set; }
            public String Operacao{ get; set; }
            public DeParaComercialModel De_Para { get; set; }
            public List<ComercialModel> Comerciais { get; set; }
            public List<VeiculoModel> Veiculos { get; set; }
            public List<ProgramaModel> Programas { get; set; }
            public List<VeiculacaoModel> Veiculacoes { get; set; }

        }
        public class DeParaComercialModel /*---A primeira pasta de determinacao*/
        {
            public String Data_Inicio { get; set; }
            public String Data_Fim { get; set; }
            public String Cod_Programa { get; set; }
            public String Nome_Programa { get; set; }
            public String Cod_Comercial { get; set; }
            public String Titulo_Comercial { get; set; }
            public Int32 Duracao { get; set; }
            public String Cod_Tipo_Comercial { get; set; }
            public String Nome_Tipo_Comercial { get; set; }
            public Int32 Qtd_Trocar { get; set; }
            public List<DeParaComercialParaModel> ComercialPara { get; set; }
            
        }

        public class DeParaComercialParaModel
        {
            public String Cod_Comercial{ get; set; }
            public Int32 Duracao{ get; set; }
        }
        public class VeiculacaoModel
        {
            public Int32 Id_Veiculacao { get; set; }
            public String Cod_Veiculo { get; set; }
            public String Cod_Programa { get; set; }
            public String Cod_Comercial { get; set; }
            public Int32 Duracao { get; set; }
            public String Cod_Caracteristica{ get; set; }
            public Int32 Qtd_Total{ get; set; }
            public Int32 Tipo_Linha{ get; set; }
            public List<InsercaoModel> Insercoes { get; set; }
            public List<InsercaoParaModel> ComercialPara { get; set; }
            public Boolean Status { get; set; }
            public String Mensagem{ get; set; }
            
    }
        public class InsercaoModel
        {
            public Int32 Id_Veiculacao { get; set; }
            public Int32 Dia { get; set; }
            public DateTime Data_Exibicao { get; set; }
            public String Dia_Semana{ get; set; }
            public Int32 Numero_Semana{ get; set; }
            public Int32 Qtd{ get; set; }
            public Boolean Selected { get; set; }
            public String Classe { get; set; }

        }
        public class InsercaoParaModel
        {
            public Int32 Id_Veiculacao { get; set; }
            public String Cod_Comercial { get; set; }
        }
        public class FiltroModel
        {
            public String Cod_Empresa { get; set; }
            public Int32 Numero_Mr { get; set; }
            public Int32 Sequencia_Mr { get; set; }
            public String Cod_Veiculo { get; set; }
        }
        public class ComercialModel
        {
            public String Cod_Empresa { get; set; }
            public Int32 Numero_Mr { get; set; }
            public Int32 Sequencia_Mr { get; set; }
            public String Cod_Comercial { get; set; }
            public String Titulo_Comercial { get; set; }
            public Int32 Duracao { get; set; }
            public String Cod_Tipo_Comercial { get; set; }
            public String Nome_Tipo_Comercial { get; set; }
            public Int32 Cod_Red_Produto { get; set; }
            public String Nome_Produto { get; set; }
            public Boolean Indica_Titulo_Determinar { get; set; }
        }
        public class VeiculoModel
        {
            public String Codigo{ get; set; }
            public String Descricao{ get; set; }
            public Boolean Selected { get; set; }
        }
        public class ProgramaModel
        {
            public String Codigo { get; set; }
            public String Descricao { get; set; }
            public Boolean Selected { get; set; }
        }
    }
}