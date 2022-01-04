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
    public partial class R0050
    {
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        private PdfPTable tbGrid;
        private TotalizadorModel TotalGeral = new TotalizadorModel();
        String Periodo = "";
        PdfLib clsPdf = new PdfLib();
        public String ImprimirRpt(R0050.ReportFilterModel Rpt)
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
            Periodo += Filter.Filters.Find(item => item.ParameterName == "@PAR_COMPETENCIA_INICIAL").Value;
            Periodo += " a ";
            Periodo += Filter.Filters.Find(item => item.ParameterName == "@PAR_COMPETENCIA_FINAL").Value;

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
                this.ImprimeCabecalho(write, doc, dtbPdf.Rows[0]);

                foreach (DataRow drw in dtbPdf.Rows)
                {
                    //-------------Linha Detalhe
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Numero_Negociacao"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Cod_Agencia"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Cliente"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Amort_Normal"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Amort_Merchandising"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Amort_Patrocinio"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Amort_Apoio"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Amort_Evento"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_BAV_Normal"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_BAV_Merchandising"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_BAV_Patrocinio"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_BAV_Apoio"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_BAV_Evento"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    //clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Sub_Total"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Total_Bav"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Venda_Liquida"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });


                    this.ImprimeGrid(write, doc, drw, tbGrid);

                    TotalGeral.Vlr_Amort_Normal += drw["Vlr_Amort_Normal"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_Amort_Merchandising += drw["Vlr_Amort_Merchandising"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_Amort_Patrocinio += drw["Vlr_Amort_Patrocinio"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_Amort_Apoio += drw["Vlr_Amort_Apoio"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_Amort_Evento += drw["Vlr_Amort_Evento"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_BAV_Normal += drw["Vlr_BAV_Normal"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_BAV_Merchandising += drw["Vlr_BAV_Merchandising"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_BAV_Patrocinio += drw["Vlr_BAV_Patrocinio"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_BAV_Apoio += drw["Vlr_BAV_Apoio"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_BAV_Evento += drw["Vlr_BAV_Evento"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_BAV_Correcao += drw["Vlr_BAV_Correcao"].ToString().ConvertToDouble();
                    TotalGeral.Vlr_Venda_Liquida += drw["Vlr_Venda_Liquida"].ToString().ConvertToDouble();
                    TotalGeral.Total_Bav += drw["Vlr_Total_Bav"].ToString().ConvertToDouble();

                }
                //=================Imprime Ultimo Total
                this.ImprimeTotalGeral(write, doc);
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
            PdfContentByte pc;
            CurrentPosition = dd.PageSize.Height;
            PageNumber++;
            if (PageNumber > 1)
            {
                dd.NewPage();
            }
            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Padrao.png");
            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = CurrentPosition - 50, Path = sPathLogo, Scale = 70 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 30, Text = "Relatório de Negociações Antecipadas", FontSize = 14 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 45, Text = Periodo, FontSize = 10 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 50, Y = dd.PageSize.Height - 20, Text = "Pag." + PageNumber.ToString(), FontSize = 7 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 132, Y = dd.PageSize.Height - 35, Text = "Emitido em." + DateTime.Now, FontSize = 7 });
            CurrentPosition -= 80;

            PdfPTable tbcabecalho = NewGrid();
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "", FontSize = 7, Background = System.Drawing.Color.White, Height = 25, colspan=3});
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "AMORTIZAÇÃO", FontSize = 7, Background = System.Drawing.Color.White, Height = 25, colspan = 5, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "BAV", FontSize = 7, Background = System.Drawing.Color.White, Height = 25, colspan = 6, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "", FontSize = 7, Background = System.Drawing.Color.White, Height = 25, colspan = 2 });
            pc = ww.DirectContent;
            tbcabecalho.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbcabecalho.TotalHeight;

            tbcabecalho = NewGrid();
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Negociação", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Agência", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Cliente", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Normal", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Merch", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Patroc.", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "M.Apoio", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Evento", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Normal", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Merch", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Patrocínio", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "M.Apoio", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Evento", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Sub Total", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Total BAV", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Venda Liquida", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });

            
            pc = ww.DirectContent;
            tbcabecalho.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbcabecalho.TotalHeight;
        }
    //======================Cria num novo grid vazio
    private PdfPTable NewGrid()
        {
            PdfPTable tb = new PdfPTable(15);
            float[] CellWidths = new float[] {50f,50f,250f,40f, 40f, 40f, 40, 40F, 40F, 40F, 40F, 40F, 40F, 40F, 40F };
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

            if (CurrentPosition - grd.TotalHeight < 50)
            {
                this.ImprimeCabecalho(ww, dd, drw);
            }
            
            //this.ImprimeSubCabecalho(ww,dd,drw);
            PdfContentByte pc = ww.DirectContent;
            grd.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbGrid.TotalHeight;


            tbGrid = NewGrid();
           
        }
        private void ImprimeTotalGeral(PdfWriter ww, Document dd)
        {
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total Geral" , FontSize = 7, Align = PdfPCell.ALIGN_CENTER, colspan = 3, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_Amort_Normal), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_Amort_Merchandising), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_Amort_Patrocinio), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_Amort_Apoio), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_Amort_Evento), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_BAV_Normal), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_BAV_Merchandising), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_BAV_Patrocinio), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_BAV_Apoio), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_BAV_Evento), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Sub_Total), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Total_Bav), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalGeral.Vlr_Venda_Liquida), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tbTotal.TotalHeight;
        }
        private String FormatValor(double pValue)
        {
            if (pValue==0)
            {
                return String.Empty;
            }
            else
            {
                return pValue.ToString("#,##0");
            }
        }
    }
}
