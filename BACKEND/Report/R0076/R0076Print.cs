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
    public partial class R0076
    {
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        private PdfPTable tbGrid;
        private String Periodo = "";
        Boolean First = true;
        PdfLib clsPdf = new PdfLib();
        TotalizadorModel TotalSituacao = new TotalizadorModel();
        TotalizadorModel TotalVeiculo = new TotalizadorModel();
        public String ImprimirRpt(R0076.ReportFilterModel Rpt)
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

            Periodo = "Periodo: ";
            Periodo += Filter.Filters.Find(item => item.ParameterName == "@Par_Dt_Inicial").Value;
            Periodo += " a ";
            Periodo += Filter.Filters.Find(item => item.ParameterName == "@Par_Dt_Final").Value;
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

                String strVeiculo_Ant = dtbPdf.Rows[0]["Cod_Veiculo"].ToString();
                byte Indica_Complementado_Ant = dtbPdf.Rows[0]["Indica_Complementado"].ToString().ConvertToByte();
                this.ImprimeCabecalho(write, doc, dtbPdf.Rows[0]);

                foreach (DataRow drw in dtbPdf.Rows)
                {
                    //-------------Quebra do Indica_Complementado
                    if (Indica_Complementado_Ant != drw["Indica_Complementado"].ToString().ConvertToByte())
                    {
                        this.ImprimeTotalVeiculo(write, doc, strVeiculo_Ant);
                        this.ImprimeTotalSituacao(write, doc, Indica_Complementado_Ant);
                        this.ImprimeCabecalho(write, doc, drw);
                        strVeiculo_Ant = drw["Cod_Veiculo"].ToString();
                    }
                    //-------------Quebra do Veiculo
                    if (strVeiculo_Ant != drw["Cod_Veiculo"].ToString())
                    {
                        this.ImprimeTotalVeiculo(write, doc, strVeiculo_Ant);
                        First = true;
                    }

                    //-------------Linha Detalhe
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Numero_Negociacao"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Contrato"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Cliente"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Cod_Agencia"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Bruto"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Liquido"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Data_Vencimento"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Numero_Ce"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    this.ImprimeGrid(write, doc, drw, tbGrid);

                    TotalSituacao.Vlr_Bruto += drw["Vlr_Bruto"].ToString().ConvertToDouble();
                    TotalSituacao.Vlr_Comissao_Agencia += drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble();
                    TotalSituacao.Vlr_Comissao_Intermediario += drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble();
                    TotalSituacao.Vlr_Liquido += drw["Vlr_Liquido"].ToString().ConvertToDouble();

                    TotalVeiculo.Vlr_Bruto += drw["Vlr_Bruto"].ToString().ConvertToDouble();
                    TotalVeiculo.Vlr_Comissao_Agencia += drw["Vlr_Comissao_Agencia"].ToString().ConvertToDouble();
                    TotalVeiculo.Vlr_Comissao_Intermediario += drw["Vlr_Comissao_Intermediario"].ToString().ConvertToDouble();
                    TotalVeiculo.Vlr_Liquido += drw["Vlr_Liquido"].ToString().ConvertToDouble();

                    strVeiculo_Ant = drw["Cod_Veiculo"].ToString();
                    Indica_Complementado_Ant = drw["Indica_Complementado"].ToString().ConvertToByte();
                }
                //=================Imprime Ultimo Total
                this.ImprimeTotalVeiculo(write, doc, strVeiculo_Ant);
                this.ImprimeTotalSituacao(write, doc, Indica_Complementado_Ant);

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

            clsPdf.AddRectangle(ww, 10, 525, 820, 60);


            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Padrao.png");
            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = CurrentPosition - 50, Path = sPathLogo, Scale = 50 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 30, Text = "Contratos " + drw["Situacao"].ToString(), FontSize = 14 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 45, Text = Periodo, FontSize = 10 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 50, Y = dd.PageSize.Height - 20, Text = "Pag." + PageNumber.ToString(), FontSize = 7 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 132, Y = dd.PageSize.Height - 35, Text = "Emitido em." + DateTime.Now, FontSize = 7 });
            First = true;
            CurrentPosition -= 70;

            //PdfPTable tbcabecalho = NewGrid();
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Negociação", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Contrato", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Cliente", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Agência", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Valor Bruto", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Com. Agência", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Com. Interm.", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Valor Liquido", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vencimento", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "C.E.", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            //pc = ww.DirectContent;
            //tbcabecalho.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            //CurrentPosition -= tbcabecalho.TotalHeight;
        }
        private void ImprimeSubCabecalho(PdfWriter ww, Document dd, DataRow drw)
        {
            PdfContentByte pc;
            CurrentPosition -= 30;

            PdfPTable tbcabecalho = NewGrid();

            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Veiculo:" +drw["Nome_Veiculo"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD ,colspan=10});
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Negociação", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Contrato", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Cliente", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Agência", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Valor Bruto", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Com. Agência", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Com. Interm.", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Valor Liquido", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Vencimento", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "C.E.", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 25, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            pc = ww.DirectContent;
            tbcabecalho.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbcabecalho.TotalHeight;
        }

        //======================Cria num novo grid vazio
        private PdfPTable NewGrid()
        {
            PdfPTable tb = new PdfPTable(10);
            float[] CellWidths = new float[] { 70f, 70f, 260f, 70f, 70f, 70f, 70f, 70f, 70f, 70f };
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
            if (First)
            {
                this.ImprimeSubCabecalho(ww, dd, drw);
            }
            //this.ImprimeSubCabecalho(ww,dd,drw);
            PdfContentByte pc = ww.DirectContent;
            grd.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbGrid.TotalHeight;
            First = false;

            tbGrid = NewGrid();

        }
        private void ImprimeTotalVeiculo(PdfWriter ww, Document dd, String Veiculo)
        {
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total do Veiculo " + Veiculo, FontSize = 7, Align = PdfPCell.ALIGN_CENTER, colspan = 4, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalVeiculo.Vlr_Bruto), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalVeiculo.Vlr_Comissao_Agencia), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalVeiculo.Vlr_Comissao_Intermediario), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalVeiculo.Vlr_Liquido), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbTotal.TotalHeight;

            TotalVeiculo.Vlr_Bruto = 0;
            TotalVeiculo.Vlr_Comissao_Agencia = 0;
            TotalVeiculo.Vlr_Comissao_Intermediario = 0;
            TotalVeiculo.Vlr_Liquido = 0;
        }
        private void ImprimeTotalSituacao(PdfWriter ww, Document dd, Byte Situacao)
        {
            String strSituacao = (Situacao == 0) ? "a Complementar" : "Complementados";
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total " + strSituacao, FontSize = 7, Align = PdfPCell.ALIGN_CENTER, colspan = 4, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalSituacao.Vlr_Bruto), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalSituacao.Vlr_Comissao_Agencia), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalSituacao.Vlr_Comissao_Intermediario), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(TotalSituacao.Vlr_Liquido), FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD });
            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbTotal.TotalHeight;

            TotalSituacao.Vlr_Bruto = 0;
            TotalSituacao.Vlr_Comissao_Agencia = 0;
            TotalSituacao.Vlr_Comissao_Intermediario = 0;
            TotalSituacao.Vlr_Liquido = 0;
        }
        private String FormatValor(double pValue)
        {
            if (pValue == 0)
            {
                return String.Empty;
            }
            else
            {
                return pValue.ToString("#,##0.00");
            }
        }
    }
}
