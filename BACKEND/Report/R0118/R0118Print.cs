using CLASSDB;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using webapi.SIMLIB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PROPOSTA
{
    public partial class R0118
    {
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        private PdfPTable tbGrid;
        private String Periodo = "";
        private String optReceita = "";
        TotalizadorModel TotalPrograma = new TotalizadorModel();
        TotalizadorModel TotalReceita = new TotalizadorModel();
        TotalizadorModel TotalAbrangencia = new TotalizadorModel();
        PdfLib clsPdf = new PdfLib();
        Boolean first = false;
        Boolean forceSubCab= false;
        public String ImprimirRpt(R0118.ReportFilterModel Rpt)
        {
            String strFilePdf = string.Empty;
            String PdfFinal = string.Empty;
            try
            {
                //===================Nome do Relatorio
                strFilePdf = Rpt.RptName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".PDF";
                String sPath = HttpContext.Current.Server.MapPath("~/PDFFILES/REPORT");
                if (sPath.Right(1) != @"\")
                {
                    sPath += @"\";
                }
                sPath += this.CurrentUser;
                sPath += @"\";
                if (!System.IO.Directory.Exists(sPath))
                {
                    System.IO.Directory.CreateDirectory(sPath);
                }
                //=========================Apaga todos os arquivos da pasta antes da geracao do strFilePDf 
                var list = System.IO.Directory.GetFiles(sPath, Rpt.RptName + "*.pdf");
                try
                {
                    foreach (var item in list)
                    {
                        System.IO.File.Delete(item);
                    }
                }
                catch (Exception)
                {
                }

                if (GerarPdf(Rpt, sPath, strFilePdf))
                {
                    PdfFinal = strFilePdf;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            finally
            {
            }
            return PdfFinal;
        }
        private Boolean GerarPdf(ReportFilterModel Filter, String Path, String FileName)
        {
            Boolean bolRetorno = true;
            String strFile = Path + FileName;
            FileStream strea = new FileStream(strFile, FileMode.Create);
            Document doc = new Document();
            PdfWriter write = PdfWriter.GetInstance(doc, strea);
            clsConexao cnn = new clsConexao(this.Credential);

            Periodo = "Periodo de Exibição: ";
            Periodo += Filter.Filters.Find(item => item.ParameterName == "@Par_Dt_Inicial").Value;
            Periodo += " a ";
            Periodo += Filter.Filters.Find(item => item.ParameterName == "@Par_Dt_Final").Value;
            switch (Filter.Filters.Find(item => item.ParameterName == "@Par_Indica_Midia_Paga").Value)
            {
                case "1":
                    optReceita = "Mídias pagas";
                    break;
                case "2":
                    optReceita = "Mídias não pagas";
                    break;
                case "3":
                    optReceita = "Mídias pagas/Mídias não pagas";
                    break;
                default:
                    break;
            }

            try
            {
                //===================================Configura o Documento
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(10, 10, 10, 10);
                doc.Open();
                tbGrid = NewGrid();
                CurrentPosition = doc.PageSize.Height;
                //==================Executa a Procedure do Relatorio e preenche datatable
                DataTable dtbPdf = this.ReportLoadData(Filter);

                if (dtbPdf.Rows.Count == 0)
                {
                    return false;
                }
                Int32 Competencia_Ant = dtbPdf.Rows[0]["Competencia"].ToString().ConvertToInt32();
                String strPrograma_Ant = dtbPdf.Rows[0]["Cod_Programa"].ToString();
                String strTipoReceita_Ant = dtbPdf.Rows[0]["Tipo_Receita"].ToString();
                String strAbrangencia_Ant = dtbPdf.Rows[0]["Abrangencia"].ToString();
                this.ImprimeCabecalho(write, doc, dtbPdf.Rows[0]);
                this.ImprimeSubCabecalho(write, doc, dtbPdf.Rows[0]);

                foreach (DataRow drw in dtbPdf.Rows)
                {
                    //-------------Quebra da Competencia
                    if (Competencia_Ant != drw["Competencia"].ToString().ConvertToInt32())
                    {
                        this.ImprimeTotalAbrangencia(write, doc, strAbrangencia_Ant);
                        this.ImprimeTotalReceita(write, doc, strTipoReceita_Ant);
                        this.ImprimeTotalPrograma(write, doc, strPrograma_Ant);
                        this.ImprimeCabecalho(write, doc, drw);
                        strPrograma_Ant = drw["Cod_Programa"].ToString();
                        strTipoReceita_Ant = drw["Tipo_Receita"].ToString();
                        strAbrangencia_Ant = drw["Abrangencia"].ToString();
                        Competencia_Ant = drw["Competencia"].ToString().ConvertToInt32();
                        this.ImprimeSubCabecalho(write, doc, drw);
                    }
                    //-------------Quebra do Programa
                    if (strPrograma_Ant != drw["Cod_Programa"].ToString())
                    {
                        this.ImprimeTotalAbrangencia(write, doc, strAbrangencia_Ant);
                        this.ImprimeTotalReceita(write, doc, strTipoReceita_Ant);
                        this.ImprimeTotalPrograma(write, doc, strPrograma_Ant);
                        strPrograma_Ant = drw["Cod_Programa"].ToString();
                        strTipoReceita_Ant = drw["Tipo_Receita"].ToString();
                        strAbrangencia_Ant = drw["Abrangencia"].ToString();
                        this.ImprimeSubCabecalho(write, doc, drw);
                    }
                    //-------------Quebra Tipo da Receita
                    if (strTipoReceita_Ant != drw["Tipo_Receita"].ToString())
                    {
                        this.ImprimeTotalAbrangencia(write, doc, strAbrangencia_Ant);
                        this.ImprimeTotalReceita(write, doc, strTipoReceita_Ant);
                        strTipoReceita_Ant = drw["Tipo_Receita"].ToString();
                        strAbrangencia_Ant = drw["Abrangencia"].ToString();
                        this.ImprimeSubCabecalho(write, doc, drw);
                    }
                    //-------------Quebra da Abrangencia
                    if (strAbrangencia_Ant != drw["Abrangencia"].ToString())
                    {
                        this.ImprimeTotalAbrangencia(write, doc, strAbrangencia_Ant);
                        strAbrangencia_Ant = drw["Abrangencia"].ToString();
                        this.ImprimeSubCabecalho(write, doc, drw);
                    }

                    //-------------Linha Detalhe
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Cliente"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT});
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Contato"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Tipo_Comercial"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Duracao"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Cod_Veiculo"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Qtde_Exibida"].ToString().ConvertToDouble().ToString("#.###"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Tabela_Unitario"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Tabela_Bruto"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Gerencial_Bruto"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Gerencial_Liquido"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Varejo"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Vlr_Receita_Liquida"].ToString().ConvertToDouble().ToString("#,##0.00"), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    this.ImprimeGrid(write, doc, drw, tbGrid);

                    TotalAbrangencia.Qtde_Exibida += drw["Qtde_Exibida"].ToString().ConvertToInt32();
                    TotalAbrangencia.Vlr_Tabela_Bruto += drw["Vlr_Tabela_Bruto"].ToString().ConvertToDouble();
                    TotalAbrangencia.Vlr_Gerencial_Bruto += drw["Vlr_Gerencial_Bruto"].ToString().ConvertToDouble();
                    TotalAbrangencia.Vlr_Comissao_Agencia += drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble();
                    TotalAbrangencia.Vlr_Gerencial_Liquido += drw["Vlr_Gerencial_Liquido"].ToString().ConvertToDouble();
                    TotalAbrangencia.Vlr_Comissao_Intermediario += drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble();
                    TotalAbrangencia.Vlr_Varejo += drw["Vlr_Varejo"].ToString().ConvertToDouble();
                    TotalAbrangencia.Vlr_Receita_Liquida += drw["Vlr_Receita_Liquida"].ToString().ConvertToDouble();

                    TotalReceita.Qtde_Exibida += drw["Qtde_Exibida"].ToString().ConvertToInt32();
                    TotalReceita.Vlr_Tabela_Bruto += drw["Vlr_Tabela_Bruto"].ToString().ConvertToDouble();
                    TotalReceita.Vlr_Gerencial_Bruto += drw["Vlr_Gerencial_Bruto"].ToString().ConvertToDouble();
                    TotalReceita.Vlr_Comissao_Agencia += drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble();
                    TotalReceita.Vlr_Gerencial_Liquido += drw["Vlr_Gerencial_Liquido"].ToString().ConvertToDouble();
                    TotalReceita.Vlr_Comissao_Intermediario += drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble();
                    TotalReceita.Vlr_Varejo += drw["Vlr_Varejo"].ToString().ConvertToDouble();
                    TotalReceita.Vlr_Receita_Liquida += drw["Vlr_Receita_Liquida"].ToString().ConvertToDouble();

                    TotalPrograma.Qtde_Exibida += drw["Qtde_Exibida"].ToString().ConvertToInt32();
                    TotalPrograma.Vlr_Tabela_Bruto += drw["Vlr_Tabela_Bruto"].ToString().ConvertToDouble();
                    TotalPrograma.Vlr_Gerencial_Bruto += drw["Vlr_Gerencial_Bruto"].ToString().ConvertToDouble();
                    TotalPrograma.Vlr_Comissao_Agencia += drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble();
                    TotalPrograma.Vlr_Gerencial_Liquido += drw["Vlr_Gerencial_Liquido"].ToString().ConvertToDouble();
                    TotalPrograma.Vlr_Comissao_Intermediario += drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble();
                    TotalPrograma.Vlr_Varejo += drw["Vlr_Varejo"].ToString().ConvertToDouble();
                    TotalPrograma.Vlr_Receita_Liquida += drw["Vlr_Receita_Liquida"].ToString().ConvertToDouble();
                }
                //=================Imprime Ultimo Total
                this.ImprimeTotalAbrangencia(write, doc, strAbrangencia_Ant);
                this.ImprimeTotalReceita(write, doc, strTipoReceita_Ant);
                this.ImprimeTotalPrograma(write, doc, strPrograma_Ant);

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            finally
            {
                cnn.Close();
                try
                {
                    doc.Close();
                    doc.Dispose();
                    strea.Dispose();
                    write.Dispose();
                }
                catch (Exception)
                {
                    bolRetorno = false;
                }
            }
            return bolRetorno;
        }
        private void ImprimeCabecalho(PdfWriter ww, Document dd, DataRow drw)
        {
            CurrentPosition = dd.PageSize.Height;
            PageNumber++;
            if (PageNumber > 1)
            {
                dd.NewPage();
            }
            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Padrao.png");
            //PdfContentByte pc;


            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = CurrentPosition - 50, Path = sPathLogo, Scale = 70 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 30, Text = "Receita por Programa Detalhado", FontSize = 14 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 45, Text = Periodo, FontSize = 10 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 57, Text = optReceita, FontSize = 10 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 50, Y = dd.PageSize.Height - 20, Text = "Pag." + PageNumber.ToString(), FontSize = 7 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 132, Y = dd.PageSize.Height - 35, Text = "Emitido em." + DateTime.Now, FontSize = 7 });
            CurrentPosition -= 80;
            first = true;
            
        }
        private void ImprimeSubCabecalho(PdfWriter ww, Document dd, DataRow drw)
        {
            PdfContentByte pc;
            PdfPTable tbSub = new PdfPTable(3);
            float position1 = 0;
            float position2 = 0;
            if (!first)
            {
                CurrentPosition -= 30;
            }
            first = false;

            //float[] CellWidths = new float[] { 400, 390 };
            float[] CellWidths = new float[] { 350, 220,220 };
            float aTotalWidth = 0;
            for (int x = 0; x < tbSub.NumberOfColumns; x++)
            {
                aTotalWidth += CellWidths[x];
            }
            tbSub.TotalWidth = aTotalWidth;
            tbSub.SetWidths(CellWidths);
            clsPdf.addCell(tbSub, new pdfLibCell() { Text = "Emp.Faturamento: " + drw["Nome_Empresa"].ToString(), Height = 20, FontStyle = Font.BOLD, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbSub, new pdfLibCell() { Text = "Tipo de Receita:" + drw["Tipo_Receita"].ToString(), Height = 20, FontStyle = Font.BOLD, Align = PdfPCell.ALIGN_CENTER, colspan=2});

            clsPdf.addCell(tbSub, new pdfLibCell() { Text = "Programa:" + drw["Nome_Programa"].ToString(), Height = 20, FontStyle = Font.BOLD, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbSub, new pdfLibCell() { Text = "Abrangência:" + drw["Abrangencia"].ToString(), Height = 20, FontStyle = Font.BOLD, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbSub, new pdfLibCell() { Text = "Competência:" + drw["Competencia_Text"].ToString(), Height = 20, FontStyle = Font.BOLD, Align = PdfPCell.ALIGN_CENTER });
            position1 = CurrentPosition;
            CurrentPosition = CurrentPosition - tbSub.TotalHeight;
            forceSubCab = false;

            PdfPTable tbcabecalho = NewGrid();
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Cliente", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Contato", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Tp Comercial", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Dur", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Veic", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Qtd Ins", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Unit.Tabela", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Bruto Tabela", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Negociado    ", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Com Agencia", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Venda Liquida", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Com Interm", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Varejo   ", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vlr Rec Liquida", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            position2 = CurrentPosition;
            CurrentPosition = CurrentPosition - tbcabecalho.TotalHeight;

            if (CurrentPosition-50 < 50)//nao imprime sub cabecalho por nao cabe a linha e vai ficar o sub cabecalho sozinha na pagina
            {
                forceSubCab = true;
                return;
            }
            pc = ww.DirectContent;
            tbSub.WriteSelectedRows(0, -1, 10, position1, pc);

            pc = ww.DirectContent;
            tbcabecalho.WriteSelectedRows(0, -1, 10, position2, pc);
        }
        //======================Cria num novo grid vazio
        private PdfPTable NewGrid()
        {
            PdfPTable tb = new PdfPTable(14);
            float[] CellWidths = new float[] { 150F, 100f, 100f, 30F, 30F, 30F, 50F, 50F, 50F, 50F, 50F, 50F, 50F, 50F };
            float aTotalWidth = 0;
            for (int x = 0; x < tb.NumberOfColumns - 1; x++)
            {
                aTotalWidth += CellWidths[x];
            }
            tb.TotalWidth = aTotalWidth; ;
            tb.SetWidths(CellWidths);
            return tb;

        }
        private void ImprimeGrid(PdfWriter ww, Document dd, DataRow drw, PdfPTable grd)
        {

            if (CurrentPosition - grd.TotalHeight < 50 || forceSubCab)
            {
                this.ImprimeCabecalho(ww, dd, drw);
                this.ImprimeSubCabecalho(ww, dd, drw);
                forceSubCab = false;
            }
            
            //this.ImprimeSubCabecalho(ww,dd,drw);
            PdfContentByte pc = ww.DirectContent;
            grd.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbGrid.TotalHeight;


            tbGrid = NewGrid();
           
        }
        private void ImprimeTotalAbrangencia(PdfWriter ww, Document dd, String Abrangencia)
        {
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total da Abrangencia " + Abrangencia, FontSize = 7, Align = PdfPCell.ALIGN_CENTER, colspan = 5, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Qtde_Exibida.ToString("#.###"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Tabela_Bruto.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Gerencial_Bruto.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Comissao_Agencia.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Gerencial_Liquido.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Comissao_Intermediario.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Varejo.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalAbrangencia.Vlr_Receita_Liquida.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });

            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tbTotal.TotalHeight;
            //CurrentPosition = CurrentPosition -= 20;

            TotalAbrangencia.Qtde_Exibida = 0;
            TotalAbrangencia.Vlr_Tabela_Bruto = 0;
            TotalAbrangencia.Vlr_Gerencial_Bruto = 0;
            TotalAbrangencia.Vlr_Comissao_Agencia = 0;
            TotalAbrangencia.Vlr_Gerencial_Liquido = 0;
            TotalAbrangencia.Vlr_Comissao_Intermediario = 0;
            TotalAbrangencia.Vlr_Varejo = 0;
            TotalAbrangencia.Vlr_Receita_Liquida = 0;
        }
        private void ImprimeTotalReceita(PdfWriter ww, Document dd, String Receita)
        {
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total da Receita " + Receita, FontSize = 7, Align = PdfPCell.ALIGN_CENTER, colspan = 5, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Qtde_Exibida.ToString("#.###"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Tabela_Bruto.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Gerencial_Bruto.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Comissao_Agencia.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Gerencial_Liquido.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Comissao_Intermediario.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Varejo.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalReceita.Vlr_Receita_Liquida.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });

            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tbTotal.TotalHeight;
            //CurrentPosition = CurrentPosition -= 20;

            TotalReceita.Qtde_Exibida = 0;
            TotalReceita.Vlr_Tabela_Bruto = 0;
            TotalReceita.Vlr_Gerencial_Bruto = 0;
            TotalReceita.Vlr_Comissao_Agencia = 0;
            TotalReceita.Vlr_Gerencial_Liquido = 0;
            TotalReceita.Vlr_Comissao_Intermediario = 0;
            TotalReceita.Vlr_Varejo = 0;
            TotalReceita.Vlr_Receita_Liquida = 0;
        }
        private void ImprimeTotalPrograma(PdfWriter ww, Document dd, String Programa)
        {
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total do  Programa " + Programa, FontSize = 7, Align = PdfPCell.ALIGN_CENTER, colspan = 5, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Qtde_Exibida.ToString("#.###"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Tabela_Bruto.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Gerencial_Bruto.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Comissao_Agencia.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Gerencial_Liquido.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Comissao_Intermediario.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Varejo.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = TotalPrograma.Vlr_Receita_Liquida.ToString("#,##0.00"), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });

            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tbTotal.TotalHeight;

            TotalPrograma.Qtde_Exibida = 0;
            TotalPrograma.Vlr_Tabela_Bruto = 0;
            TotalPrograma.Vlr_Gerencial_Bruto = 0;
            TotalPrograma.Vlr_Comissao_Agencia = 0;
            TotalPrograma.Vlr_Gerencial_Liquido = 0;
            TotalPrograma.Vlr_Comissao_Intermediario = 0;
            TotalPrograma.Vlr_Varejo = 0;
            TotalPrograma.Vlr_Receita_Liquida = 0;
        }
    }
}
