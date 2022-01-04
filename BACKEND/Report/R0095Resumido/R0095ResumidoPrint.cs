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
    public partial class R0095RESUMIDO
    {
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        private PdfPTable tbGrid;
        private TotalizadorModel TotalGeral = new TotalizadorModel();
        PdfLib clsPdf = new PdfLib();
        TotalizadorModel Totalizador = new TotalizadorModel();
        public String ImprimirRpt(R0095RESUMIDO.ReportFilterModel Rpt)
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
                double Total_Detalhe_Bruto = 0;
                double Total_Detalhe_Liquido= 0;
                Int32 Negociacao_Ant = dtbPdf.Rows[0]["Numero_Negociacao"].ToString().ConvertToInt32();
                String Cod_Nucleo_Ant = dtbPdf.Rows[0]["Cod_Nucleo"].ToString();
                String Cod_Agencia_Ant = dtbPdf.Rows[0]["Cod_Agencia"].ToString();
                String Cod_Cliente_Ant = dtbPdf.Rows[0]["Cod_Cliente"].ToString();
                String Cod_Contato_Ant = dtbPdf.Rows[0]["Cod_Contato"].ToString();
                String Cod_Veiculo_Ant = dtbPdf.Rows[0]["Cod_Veiculo"].ToString();
                Boolean Indica_Consolidado = false;
                foreach (DataRow drw in dtbPdf.Rows)
                {
                    //-------------Quebra da Negociacao
                    if (Negociacao_Ant != drw["Numero_Negociacao"].ToString().ConvertToInt32())
                    {
                        ImprimeTotal(write, doc,drw,Indica_Consolidado);
                        ImprimeCabecalho(write, doc, drw);
                        Negociacao_Ant = drw["Numero_Negociacao"].ToString().ConvertToInt32();
                        Cod_Nucleo_Ant = drw["Cod_Nucleo"].ToString();
                        Cod_Agencia_Ant = drw["Cod_Agencia"].ToString();
                        Cod_Cliente_Ant = drw["Cod_Cliente"].ToString();
                        Cod_Contato_Ant = drw["Cod_Contato"].ToString();
                        Cod_Veiculo_Ant = drw["Cod_Veiculo"].ToString();
                    }
                    //-------------Quebra da Nucleo,Agencia,Cliente,Contato,Veiculo
                    if (Cod_Nucleo_Ant != drw["Cod_Nucleo"].ToString() ||
                        Cod_Agencia_Ant != drw["Cod_Agencia"].ToString() ||
                        Cod_Cliente_Ant != drw["Cod_Cliente"].ToString() ||
                        Cod_Contato_Ant != drw["Cod_Contato"].ToString() ||
                        Cod_Veiculo_Ant != drw["Cod_Veiculo"].ToString() )
                    {
                        ImprimeTotal(write, doc,drw, Indica_Consolidado);
                        ImprimeCabecalho(write, doc, drw);
                    }


                    //-------------Linha Detalhe

                    Total_Detalhe_Bruto = drw["Vlr_Principal_Bruto"].ToString().ConvertToDouble();
                    Total_Detalhe_Bruto += drw["Vlr_Reaplicacao_Bruto"].ToString().ConvertToDouble();

                    Total_Detalhe_Liquido = drw["Vlr_Principal_Liquido"].ToString().ConvertToDouble();
                    Total_Detalhe_Liquido += drw["Vlr_Reaplicacao_Liquido"].ToString().ConvertToDouble();


                    



                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Competencia_Text"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Faturado_Bruto"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Faturado_Liquido"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Faturado_Credito"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Principal_Bruto"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Principal_Liquido"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Reaplicacao_Bruto"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Reaplicacao_Liquido"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(Total_Detalhe_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(Total_Detalhe_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Venda_Liquido"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Venda_Liquido_Ajuste"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Vlr_Venda_Liquido_Total"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });


                    this.ImprimeGrid(write, doc, drw, tbGrid);


                    Totalizador.Vlr_Faturado_Bruto += drw["Vlr_Faturado_Bruto"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Faturado_Liquido += drw["Vlr_Faturado_Liquido"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Faturado_Credito += drw["Vlr_Faturado_Credito"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Principal_Bruto += drw["Vlr_Principal_Bruto"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Principal_Liquido += drw["Vlr_Principal_Liquido"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Reaplicacao_Bruto += drw["Vlr_Reaplicacao_Bruto"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Reaplicacao_Liquido += drw["Vlr_Reaplicacao_Liquido"].ToString().ConvertToDouble();
                    Totalizador.Total_Detalhe_Bruto += Total_Detalhe_Bruto;
                    Totalizador.Total_Detalhe_Liquido += Total_Detalhe_Liquido;
                    Totalizador.Vlr_Venda_Liquido += drw["Vlr_Venda_Liquido"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Venda_Liquido_Ajuste += drw["Vlr_Venda_Liquido_Ajuste"].ToString().ConvertToDouble();
                    Totalizador.Vlr_Venda_Liquido_Total += drw["Vlr_Venda_Liquido_Total"].ToString().ConvertToDouble();

                    Negociacao_Ant = drw["Numero_Negociacao"].ToString().ConvertToInt32();
                    Cod_Nucleo_Ant = drw["Cod_Nucleo"].ToString();
                    Cod_Agencia_Ant = drw["Cod_Agencia"].ToString();
                    Cod_Cliente_Ant = drw["Cod_Cliente"].ToString();
                    Cod_Contato_Ant = drw["Cod_Contato"].ToString();
                    Cod_Veiculo_Ant = drw["Cod_Veiculo"].ToString();

                    Indica_Consolidado = drw["Indica_Consolidado"].ToString().ConvertToByte() == 1;

                }
                ////=================Imprime Ultimo Total
                ImprimeTotal(write, doc,dtbPdf.Rows[dtbPdf.Rows.Count-1],Indica_Consolidado);
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
            //clsPdf.AddRectangle(ww, 10, 600, 1000, 50);

            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Padrao.png");
            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = CurrentPosition - 50, Path = sPathLogo, Scale = 50 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 30, Text = "Conta Corrente da Negociação (Resumido)", FontSize = 14 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 45, Text = "Negociação:" + drw["Numero_Negociacao"].ToString(), FontSize = 10 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 50, Y = dd.PageSize.Height - 20, Text = "Pag." + PageNumber.ToString(), FontSize = 7 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 132, Y = dd.PageSize.Height - 35, Text = "Emitido em." + DateTime.Now, FontSize = 7 });
            CurrentPosition -= 70;




            PdfPTable tbcabecalho = new PdfPTable(5);
            float[] CellWidths = new float[] { 100f, 200f, 260f, 260f, 130f };
            float aTotalWidth = 0;
            for (int x = 0; x < tbcabecalho.NumberOfColumns - 1; x++)
            {   
                aTotalWidth += CellWidths[x];
            }
            tbcabecalho.TotalWidth = aTotalWidth; ;
            tbcabecalho.SetWidths(CellWidths);

            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "VIGÊNCIA\n\n" +" " + drw["Vigencia"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "NÚCLEO\n\n" + " " + drw["Nome_Nucleo"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "AGÊNCIA\n\n" + " " + drw["Nome_Agencia"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "CLIENTE\n\n" + " " + drw["Nome_Cliente"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, colspan = 2, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "TABELA NEGOCIADA\n\n" + " " + drw["Tabela_Negociada"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "CONTATO\n\n" + " " + drw["Nome_Contato"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "MERCADO\n\n" + " " + drw["Nome_Veiculo"].ToString(), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "VLR NEG BRUTO\n\n" + " " + FormatValor(drw["Vlr_Negociado_Bruto"].ToString().ConvertToDouble()), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "VLR NEG LIQUIDO\n\n" + " " + FormatValor(drw["Vlr_Negociado_Liquido"].ToString().ConvertToDouble()), FontSize = 7, Background = System.Drawing.Color.White, Height = 30, Align = PdfPCell.ALIGN_LEFT });
            pc = ww.DirectContent;
            tbcabecalho.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbcabecalho.TotalHeight;


            tbcabecalho = NewGrid();

            double Vlr_Saldo_Total_Bruto = drw["Vlr_Saldo_Midia_Bruto"].ToString().ConvertToDouble();
            Vlr_Saldo_Total_Bruto += drw["Vlr_Saldo_Reaplicacao_Bruto"].ToString().ConvertToDouble();
            Vlr_Saldo_Total_Bruto += drw["Vlr_Saldo_Evento_Bruto"].ToString().ConvertToDouble();

            //double Vlr_Saldo_Total_Liquido = drw["Vlr_Saldo_Midia_Liquido"].ToString().ConvertToDouble();
            //Vlr_Saldo_Total_Liquido += drw["Vlr_Saldo_Reaplicacao_Liquido"].ToString().ConvertToDouble();
            //Vlr_Saldo_Total_Liquido += drw["Vlr_Saldo_Evento_Liquido"].ToString().ConvertToDouble();

            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Faturado", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 20, Align = PdfPCell.ALIGN_CENTER,colspan=4 });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Amortizado", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 20, Align = PdfPCell.ALIGN_CENTER, colspan = 6 });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Venda Líquida", FontSize = 7, Background = System.Drawing.Color.LightGray, Height = 20, Align = PdfPCell.ALIGN_CENTER, colspan = 3 });

            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Descrição", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER});
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Valores Faturado", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER ,colspan=3});
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Mídia Principal", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER, colspan = 2 });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Mídia Apoio / Reaplicação", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER, colspan = 2 });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Total", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER, colspan = 2 });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Valores Venda Líquida", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER, colspan = 3 });

            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER});
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Bruto", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Líquido", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Crédito Espécie", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Bruto", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Líquido", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Bruto", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Líquido", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Bruto", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Líquido", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Venda Líquida", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Ajustes", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            clsPdf.addCell(tbcabecalho, new pdfLibCell() { Text = "Total", FontSize = 7, Height = 20, Align = PdfPCell.ALIGN_CENTER });
            pc = ww.DirectContent;
            tbcabecalho.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbcabecalho.TotalHeight;


        }
        //======================Cria num novo grid vazio
        private PdfPTable NewGrid()
        {
            PdfPTable tb = new PdfPTable(13);
            float[] CellWidths = new float[] { 160f,60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f};
            float aTotalWidth = 0;
            for (int x = 0; x < tb.NumberOfColumns - 1; x++)
            {
                aTotalWidth += CellWidths[x];
            }
            tb.TotalWidth = aTotalWidth;  
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
        private void ImprimeTotal(PdfWriter ww, Document dd,DataRow drw,Boolean Indica_Consolidado)
        {
            PdfContentByte pc;
            PdfPTable tbTotal = NewGrid();

         
            double Saldo_Final_Bruto = drw["Vlr_Negociado_Bruto"].ToString().ConvertToDouble();
            Saldo_Final_Bruto -= Totalizador.Vlr_Principal_Bruto;
            double Saldo_Final_Liquido = drw["Vlr_Negociado_Liquido"].ToString().ConvertToDouble();
            Saldo_Final_Liquido -= Totalizador.Vlr_Principal_Liquido;

            double Saldo_Final_Reaplicacao_Bruto = drw["Vlr_Reaplicacao_Bruto"].ToString().ConvertToDouble();
            Saldo_Final_Reaplicacao_Bruto -= Totalizador.Vlr_Reaplicacao_Bruto;
            double Saldo_Final_Reaplicacao_Liquido = drw["Vlr_Reaplicacao_Liquido"].ToString().ConvertToDouble();
            Saldo_Final_Reaplicacao_Liquido -= Totalizador.Vlr_Reaplicacao_Liquido;

            double Saldo_Final_Total_Bruto = Saldo_Final_Bruto + Saldo_Final_Reaplicacao_Bruto;
            double Saldo_Final_Total_Liquido = Saldo_Final_Liquido + Saldo_Final_Reaplicacao_Liquido;


            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Total", FontSize = 7, Align = PdfPCell.ALIGN_CENTER,FontStyle=Font.BOLD});
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Faturado_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Faturado_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Faturado_Credito), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Principal_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Principal_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Reaplicacao_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT ,FontStyle= Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Reaplicacao_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Total_Detalhe_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Total_Detalhe_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Venda_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Venda_Liquido_Ajuste), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Totalizador.Vlr_Venda_Liquido_Total), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
            if (Indica_Consolidado)
            {
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "Saldo Final", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, FontStyle = Font.BOLD ,colspan=4});
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Saldo_Final_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD });
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Saldo_Final_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD});
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Saldo_Final_Reaplicacao_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD});
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Saldo_Final_Reaplicacao_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD});
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Saldo_Final_Total_Bruto), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD});
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = FormatValor(Saldo_Final_Total_Liquido), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD});
                clsPdf.addCell(tbTotal, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, FontStyle = Font.BOLD ,colspan=3});

            }
            pc = ww.DirectContent;
            tbTotal.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tbTotal.TotalHeight;

            Totalizador.Vlr_Faturado_Bruto = 0;
            Totalizador.Vlr_Faturado_Liquido = 0;
            Totalizador.Vlr_Faturado_Credito = 0;
            Totalizador.Vlr_Principal_Bruto = 0;
            Totalizador.Vlr_Principal_Liquido = 0;
            Totalizador.Vlr_Reaplicacao_Bruto = 0;
            Totalizador.Vlr_Reaplicacao_Liquido = 0;
            Totalizador.Total_Detalhe_Bruto = 0;
            Totalizador.Total_Detalhe_Liquido = 0;
            Totalizador.Vlr_Venda_Liquido = 0;
            Totalizador.Vlr_Venda_Liquido_Ajuste = 0;
            Totalizador.Vlr_Venda_Liquido_Total = 0;
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
