

using System;
using System.Collections.Generic;


namespace PROPOSTA
{
    public partial class GradeMercha
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public GradeMercha(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }
        public class GradeMerchaModel
        {
            public String Action { get; set; }
            public Int32 RedeId { get; set; }
            public String Cod_Veiculo { get; set; }
            public String Nome_Veiculo { get; set; }
            public String Data_Exibicao { get; set; }
            public String Cod_Programa { get; set; }
            public String Nome_Programa { get; set; }
            public Int32 Disponibilidade{ get; set; }
            public Byte Faixa_Horaria { get; set; }
            public String Horario_Final { get; set; }
            public String Horario_Inicial { get; set; }
            public Boolean Domingo { get; set; } = false;
            public Boolean Segunda { get; set; } = false;
            public Boolean Terca { get; set; } = false;
            public Boolean Quarta { get; set; } = false;
            public Boolean Quinta { get; set; } = false;
            public Boolean Sexta { get; set; } = false;
            public Boolean Sabado { get; set; } = false;
            public String Inicio_Validade { get; set; }
            public String Termino_Validade { get; set; }
            public List<VeiculoModel> Veiculos { get; set; }

        }

        public class GradeMerchaListModel
        {
            public List<GradeMerchaListDiaModel> Dias { get; set; }
        }
        public class GradeMerchaListDiaModel
        {
            public DateTime Data_Exibicao { get; set; }
            public String Dia_Semana { get; set; }
            public List<GradeMerchaListProgramaModel> Programas { get; set; }
        }
        public class GradeMerchaListProgramaModel
        {
            public String Cod_Programa { get; set; }
            public String Nome_Programa { get; set; }
            public String Hora_Inicio { get; set; }
            public String Hora_Termino { get; set; }
            public String Nome_Genero { get; set; }
            public Boolean Indica_Desativado { get; set; }
            public Int32 Disponibilidade{ get; set; }
            public Int32 Absorvido{ get; set; }
            public Int32 Saldo{ get; set; }
            
        }
        public class GradeMerchaFiltroModel
        {
            public String Cod_Veiculo { get; set; }
            public String Competencia { get; set; }
            public String Cod_Programa{ get; set; }
            public Int32 RedeId{ get; set; }
        }
        public class GradeMerchaGetDataModel
        {
            public String Action    { get; set; }
            public String Cod_Veiculo { get; set; }
            public DateTime Data_Exibicao { get; set; }
            public String Cod_Programa { get; set; }
            

        }
        public class VeiculoModel
        {
            public String Cod_Veiculo { get; set; }
            public String Nome_Veiculo { get; set; }
            public Boolean Selected { get; set; }
        }

        //--mmm INICIO 

        public class ListarVeiculoModel
        {
            public String Cod_Veiculo { get; set; }
            public String Nome_Veiculo { get; set; }
            public Boolean Selected { get; set; }
        }
        public class ListarProgramaModel
        {
            public String Cod_Programa { get; set; }
            public String Titulo { get; set; }
            public Boolean Selected { get; set; }
        }
        public class PropagacaoGradeMerchaModel
        {
            public String Competencia_Base { get; set; }
            public String Data_Inicio { get; set; }
            public String Data_Fim { get; set; }
            public String Cod_Veiculo_Origem { get; set; }
            public String Nome_Veiculo_Origem { get; set; }
            public List<ListarVeiculoModel> Veiculos { get; set; }
            public List<ListarProgramaModel> Programas { get; set; }
        }

        //--mmm FIM

    }
}