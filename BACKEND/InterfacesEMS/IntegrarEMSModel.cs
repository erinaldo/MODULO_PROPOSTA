using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class IntegrarEMS
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public IntegrarEMS(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class IntegrarEMSModel
        {
            //----Filtro
            public Boolean Indica_Faturas { get; set; }
            public Boolean Indica_Boletos { get; set; }
            public Boolean Indica_Mensal { get; set; }
            public Boolean Indica_Todos { get; set; }
            public String Cod_Empresa_Faturamento { get; set; }
            public String Competencia { get; set; }
            public String Nota_Fiscal { get; set; }
                //----Grid
            public Boolean Indica_Marcado { get; set; }
            public String Numero_Fatura { get; set; }
            public String Fatura_Anterior { get; set; }
            public String Numero_Negociacao { get; set; }
            public String Qtde_Linha_por_Negoc { get; set; }
            public String Cod_Cliente { get; set; }
            public String Produto { get; set; }
            public String Cod_Agencia { get; set; }
            public String Cod_Contato { get; set; }
            public String Cod_Tipo_Midia { get; set; }
            public String Valor { get; set; }
            public String Integrada { get; set; }
            //--Processamento
            public String Guarda_Origem { get; set; }
            public String Guarda_SEQ_WT_DOCTO { get; set; }
            public String Guarda_Faturas { get; set; }
            public String Guarda_NR_SEQUENCIA { get; set; }
            public Byte Indica_Chamada { get; set; }
            public String Guarda_COD_LOTE { get; set; }
            public Byte Tipo { get; set; }
            public String LogFatBoleto { get; set; }
            public String LogTipo { get; set; }
        }

        public class CriticaEMSModel
        {
            public String Cod_Emp_Fat_Crit { get; set; }
            public String Usuario { get; set; }
            public String Data_Integracao { get; set; }
            public String Numero_Fatura { get; set; }
            public String Numero_Negociacao { get; set; }
            public String Msg_Critica { get; set; }
        }

        public class IntegrarFaturasModel
        {
            public String rec_origem { get; set; }
            public String des_destino { get; set; }
            public String dat_transm { get; set; }
            public String usu_transm { get; set; }
            public String fla_dispon { get; set; }
            public String cod_estabel { get; set; }
            public String serie { get; set; }
            public String nr_nota { get; set; }
            public String nome_abrev { get; set; }
            public String dt_emis_nota { get; set; }
            public String cod_cond_pag { get; set; }
            public String endereco { get; set; }
            public String bairro { get; set; }
            public String cidade { get; set; }
            public String estado { get; set; }
            public String cep { get; set; }
            public String pais { get; set; }
            public String ins_estadual { get; set; }
            public String nr_fatura { get; set; }
            public String nat_operacao { get; set; }
            public String cod_portador { get; set; }
            public String dt_prvenc { get; set; }
            public String cd_vendedor { get; set; }
            public String modalidade { get; set; }
            public String observ_nota { get; set; }
            public Double vl_acum_dup { get; set; }
            public String no_ab_reppri { get; set; }
            public String dt_trans { get; set; }
            public String mo_codigo { get; set; }
            public String cgccpf_cli { get; set; }
            public String nr_nota_ext { get; set; }
            public String cidade_cob { get; set; }
            public String cep_cob { get; set; }
            public String endereco_cob { get; set; }
            public String bairro_cob { get; set; }
            public String estado_cob { get; set; }
            public String nome_cob { get; set; }
            public String nr_tab_finan { get; set; }
            public Int32 seq_wt_docto { get; set; }
            public Boolean indica_critica{ get; set; }
        }

        public class IntegrarItensModel
        {
            public String rec_origem { get; set; }
            public String des_destino { get; set; }
            public String dat_transm { get; set; }
            public String usu_transm { get; set; }
            public String fla_dispon { get; set; }
            public String nr_sequencia { get; set; }
            public String it_codigo { get; set; }
            public Int32 quantidade { get; set; }
            public Double per_des_item { get; set; }
            public String narrativa { get; set; }
            public Double vl_preori_ped { get; set; }
            public Double val_pct_desconto_tab_preco { get; set; }
            public String seq_wt_it_docto { get; set; }
            public String seq_wt_docto { get; set; }
            public Boolean indica_critica { get; set; }
        }

        public class IntegrarDuplicatasModel
        {
            public String rec_origem { get; set; }
            public String des_destino { get; set; }
            public String dat_transm { get; set; }
            public String usu_transm { get; set; }
            public String fla_dispon { get; set; }
            public String parcela { get; set; }
            public String dt_venciment { get; set; }
            public Double vl_parcela { get; set; }
            public Double vl_desconto { get; set; }
            public Double vl_comis { get; set; }
            public Double vl_acum_dup { get; set; }
            public String cod_vencto { get; set; }
            public String cod_esp { get; set; }
            public String seq_wt_docto { get; set; }
            public String nr_seq_nota { get; set; }
            public Double vl_base_comis { get; set; }
        }

        public class IntegrarRepresentantesModel
        {
            public String rec_origem { get; set; }
            public String des_destino { get; set; }
            public String dat_transm { get; set; }
            public String usu_transm { get; set; }
            public String fla_dispon { get; set; }
            public String cod_rep { get; set; }
            public Double perc_comis { get; set; }
            public String comis_emis { get; set; }
            public String seq_wt_docto { get; set; }
            public String nr_seq_nota { get; set; }
            public String sequencia { get; set; }
        }

        public class IntegrarClientesModel
        {
            public String origem { get; set; }
            public String destino { get; set; }
            public String dat_transm { get; set; }
            public String usu_transm { get; set; }
            public String fla_dispon { get; set; }
            public String str_nom_abrev { get; set; }
            public String str_cgccpf { get; set; }
            public String ind_identific { get; set; }
            public String ind_natureza { get; set; }
            public String str_nome { get; set; }
            public String str_endereco { get; set; }
            public String str_bairro { get; set; }
            public String str_cidade { get; set; }
            public String str_uf { get; set; }
            public String str_cep { get; set; }
            public String str_cx_postal { get; set; }
            public String str_pais { get; set; }
            public String str_inscr_est { get; set; }
            public String str_ramo_ativ { get; set; }
            public String str_telefax { get; set; }
            public String str_ramal_tel { get; set; }
            public String str_telex { get; set; }
            public String dat_implanta { get; set; }
            public String cod_represent { get; set; }
            public String ind_loc_av_cr { get; set; }
            public String cod_grupo_cli { get; set; }
            public Double vlr_limite_cr { get; set; }
            public String cod_portador { get; set; }
            public String ind_modal { get; set; }
            public String ind_fat_parc { get; set; }
            public String ind_credito { get; set; }
            public String ind_aval_cred { get; set; }
            public String str_nat_oper { get; set; }
            public String ind_meio_ped { get; set; }
            public String str_nome_fant { get; set; }
            public String str_modem { get; set; }
            public String str_ramal_mod { get; set; }
            public String str_agencia { get; set; }
            public String ind_bloqueto { get; set; }
            public String ind_etiqueta { get; set; }
            public String ind_valores { get; set; }
            public String ind_aviso_deb { get; set; }
            public String ind_moda_pref { get; set; }
            public String ind_avaliacao { get; set; }
            public String ind_venc_dom { get; set; }
            public String ind_venc_sab { get; set; }
            public String str_cgc_cob { get; set; }
            public String str_cep_cob { get; set; }
            public String str_uf_cob { get; set; }
            public String str_cidad_cob { get; set; }
            public String str_bairr_cob { get; set; }
            public String str_ender_cob { get; set; }
            public String str_cx_po_cob { get; set; }
            public String str_in_es_cob { get; set; }
            public String str_banco { get; set; }
            public String ind_tipo_reg { get; set; }
            public String ind_venc_fer { get; set; }
            public String ind_pagamento { get; set; }
            public String ind_cobr_desp { get; set; }
            public String str_insc_mun { get; set; }
            public String cod_cond_pag { get; set; }
            public String str_fone_1 { get; set; }
            public String str_fone_2 { get; set; }
            public String num_mes_inat { get; set; }
            public String log_ver_pub { get; set; }
            public String str_e_mail { get; set; }
            public String ind_aval_emb { get; set; }
            public String str_conta_cor { get; set; }
            public String nr_sequencia { get; set; }
            public String fla_dispon_1 { get; set; }
            public String fla_dispon_2 { get; set; }
            public String fla_dispon_3 { get; set; }
            public String fla_dispon_4 { get; set; }
            public String fla_dispon_5 { get; set; }
            public String fla_dispon_6 { get; set; }
            public String fla_dispon_7 { get; set; }
            public String fla_dispon_8 { get; set; }
            public String fla_dispon_9 { get; set; }
            public String fla_dispon_10 { get; set; }
        }


        public class MatrizEstabelecimentoModel
        {
            public String Cod_Estabelecimento  { get; set; }
            public String Seq_Estabelecimento { get; set; }
        }

        public class objEstabelecimentoModel
        {
            public string TTA_COD_ESTAB { get; set; }
            public string SEQUENCIA { get; set; }
        }
        public class IntegrarLotesModel
        {
            public String ttv_rec_origem { get; set; }
            public String ttv_des_destino { get; set; }
            public String ttv_dat_transm { get; set; }
            public String ttv_usu_transm { get; set; }
            public String ttv_fla_dispon { get; set; }
            public String tta_cod_empresa { get; set; }
            public String tta_cod_estab { get; set; }
            public String tta_dat_transacao { get; set; }
            public String tta_cod_refer { get; set; }
            public Double tta_val_tot_lote_infor_tit_acr { get; set; }
            public String tta_ind_tip_cobr_acr { get; set; }
            public String ttv_log_lote_impl_ok { get; set; }
            public String cod_lote { get; set; }
            public String ttv_cod_erro { get; set; }
        }
        public class IntegrarItemLotesModel
        {
            public String ttv_rec_origem { get; set; }
            public String ttv_des_destino { get; set; }
            public String ttv_dat_transm { get; set; }
            public String ttv_usu_transm { get; set; }
            public String ttv_fla_dispon { get; set; }
            public String tta_num_seq_refer { get; set; }
            public String tta_cod_ser_docto { get; set; }
            public String tta_cod_espec_docto { get; set; }
            public String tta_cod_tit_acr { get; set; }
            public String str_tta_cod_tit_acr { get; set; }
            public Int32 int_tta_cod_tit_acr { get; set; }
            public String novo_tta_cod_tit_acr { get; set; }
            public String tta_cod_parcela { get; set; }
            public String tta_cod_indic_econ { get; set; }
            public String tta_cod_portador { get; set; }
            public String tta_cod_cart_bcia { get; set; }
            public String tta_cdn_repres { get; set; }
            public String tta_dat_vencto_tit_acr { get; set; }
            public String tta_dat_prev_liquidac { get; set; }
            public String tta_dat_emis_docto { get; set; }
            public Double tta_val_tit_acr { get; set; }
            public Double tta_val_perc_juros_dia_atr { get; set; }
            public Double tta_val_perc_multa_atr { get; set; }
            public Double tta_val_liq_tit_acr { get; set; }
            public String tta_ind_tip_espec_docto { get; set; }
            public String cod_item_lote { get; set; }
            public String cod_lote { get; set; }
            public String ttv_cod_erro { get; set; }
            public String str_cgccpf { get; set; }
        }
        public class IntegrarApropsModel
        {
            public String ttv_rec_origem { get; set; }
            public String ttv_des_destino { get; set; }
            public String ttv_dat_transm { get; set; }
            public String ttv_usu_trans { get; set; }
            public String ttv_fla_dispon { get; set; }
            public String cod_item_lote { get; set; }
            public String tta_cod_plano_cta_ctbl { get; set; }
            public String tta_cod_cta_ctbl { get; set; }
            public String tta_cod_unid_negoc { get; set; }
            public String tta_cod_tip_fluxo_financ { get; set; }
            public Double tta_val_aprop_ctbl { get; set; }
        }
        public class IntegrarRepresModel
        {
            public String ttv_rec_origem { get; set; }
            public String ttv_des_destino { get; set; }
            public String ttv_dat_transm { get; set; }
            public String ttv_usu_transm { get; set; }
            public String ttv_fla_dispon { get; set; }
            public String cod_item_lote { get; set; }
            public Double tta_val_perc_comis_repres { get; set; }
            public Double tta_val_perc_comis_repres_emis { get; set; }
            public Double tta_val_perc_comis_abat { get; set; }
            public Double tta_val_perc_comis_desc { get; set; }
            public Double tta_val_perc_comis_juros { get; set; }
            public Double tta_val_perc_comis_multa { get; set; }
            public Double tta_val_perc_comis_acerto_val { get; set; }
            public String tta_log_comis_repres_proporc { get; set; }
            public String tta_ind_tip_comis { get; set; }
            public String tta_cdn_repres { get; set; }
        }


        public class ResponseModel
        {
            public Boolean IsSuccessful { get; set; }
            public String Message { get; set; }
            public Boolean Indica_Critica { get; set; }
        }


    }
}