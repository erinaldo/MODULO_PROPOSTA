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
    public partial class R0183
    {
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        private PdfPTable tbGrid;
        PdfLib clsPdf = new PdfLib();
        TotalizadorModel Totalizador = new TotalizadorModel();
        Boolean First = false;
        public String ImprimirRpt(R0183.ReportFilterModel Rpt)
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
                doc.SetPageSize(PageSize.A4);
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

                String Programa_Ant = "";
                String Veiculo_Ant = "";
                String Data_Exibicao_Ant = "";

                foreach (DataRow drw in dtbPdf.Rows)
                {
                    //-----Quando mudar o veiculo/data/programa
                    if (Programa_Ant != drw["Cod_Programa"].ToString() ||
                        Veiculo_Ant != drw["Cod_Veiculo"].ToString() ||
                        Data_Exibicao_Ant != drw["Data_Exibicao"].ToString() 
                        )
                    {
                        this.ImprimeTotal(write,doc,Programa_Ant);
                        this.ImprimeCabecalho(write,doc,drw);
                        this.ImprimeBreak(write, drw["Cod_Veiculo"].ToString(), drw["Data_Exibicao"].ToString(), drw["Cod_Programa"].ToString());
                    }
                    //-------------Imprimir Linha Detalhe
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Cliente"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Nome_Tipo_Comercial"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT });
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = drw["Duracao"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER});
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Valor"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT});
                    clsPdf.addCell(tbGrid, new pdfLibCell() { Text = FormatValor(drw["Valor_30"].ToString().ConvertToDouble()), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT });
                    this.ImprimeGrid(write, doc, drw, tbGrid);
                    //-------------Guarda os Totais
                    Totalizador.Valor += drw["Valor"].ToString().ConvertToDouble();
                    Totalizador.Valor_30 += drw["Valor_30"].ToString().ConvertToDouble();
                    Totalizador.Qtd++;
                    //-------------Guardar ultima chave de quebra
                    Programa_Ant = drw["Cod_Programa"].ToString();
                    Veiculo_Ant = drw["Cod_Veiculo"].ToString();
                    Data_Exibicao_Ant = drw["Data_Exibicao"].ToString();
                }
                //==============Imprime Total do ultimo registros
                this.ImprimeTotal(write, doc, Programa_Ant);
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
            clsPdf.AddRectangle(ww, 10, 797, 575, 40);

            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Padrao.png");
            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = CurrentPosition - 40, Path = sPathLogo, Scale = 50 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = CurrentPosition - 30, Text = "Relatório de Análise dos Breaks", FontSize = 14 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 50, Y = dd.PageSize.Height - 20, Text = "Pag." + PageNumber.ToString(), FontSize = 7 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 132, Y = dd.PageSize.Height - 35, Text = "Emitido em." + DateTime.Now, FontSize = 7 });
            CurrentPosition -= 45;


            PdfPTable tb = new PdfPTable(3);
            float[] CellWidths = new float[] { 305f, 270f, 300f };
            float aTotalWidth = 0;
            for (int x = 0; x < tb.NumberOfColumns - 1; x++)
            {
                aTotalWidth += CellWidths[x];
            }
            tb.TotalWidth = aTotalWidth;
            tb.SetWidths(CellWidths);
            
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Veiculo\n\n" + drw["Nome_Veiculo"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT, Height = 30 });
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Data Exibição\n\n" + drw["Data_Exibicao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy"), FontSize = 7, Align = PdfPCell.ALIGN_LEFT, Height = 30 });
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Programa\n\n" + drw["Nome_Programa"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT, Height = 30 });

            pc = ww.DirectContent;
            tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tb.TotalHeight;
            CurrentPosition = CurrentPosition - 10;

            First = true;
            //tb = NewGrid();
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Cliente", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Tipo Comercial", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Duração", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Valor", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Valor 30", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            //pc = ww.DirectContent;
            //tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            //CurrentPosition = CurrentPosition - tb.TotalHeight;
        }
        private void ImprimeSubCabecalho(PdfWriter ww, Document dd, DataRow drw)
        {

            PdfContentByte pc;

            PdfPTable tb = new PdfPTable(3);
            float[] CellWidths = new float[] { 305f, 270f, 300f };
            float aTotalWidth = 0;
            for (int x = 0; x < tb.NumberOfColumns - 1; x++)
            {
                aTotalWidth += CellWidths[x];
            }
            tb.TotalWidth = aTotalWidth;
            tb.SetWidths(CellWidths);

            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Veiculo\n\n" + drw["Nome_Veiculo"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT, Height = 30 });
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Data Exibição\n\n" + drw["Data_Exibicao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy"), FontSize = 7, Align = PdfPCell.ALIGN_LEFT, Height = 30 });
            //clsPdf.addCell(tb, new pdfLibCell() { Text = "Programa\n\n" + drw["Nome_Programa"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_LEFT, Height = 30 });

            //pc = ww.DirectContent;
            //tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            //CurrentPosition = CurrentPosition - tb.TotalHeight;
            //CurrentPosition = CurrentPosition - 10;

            tb = NewGrid();
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Cliente", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Tipo Comercial", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Duração", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Valor", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            clsPdf.addCell(tb, new pdfLibCell() { Text = "Valor 30", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, Background = System.Drawing.Color.LightGray });
            pc = ww.DirectContent;
            tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tb.TotalHeight;
        }

        //======================Cria num novo grid vazio
        private PdfPTable NewGrid()
        {
            PdfPTable tb = new PdfPTable(5);
            float[] CellWidths = new float[] { 255f, 200f, 50f, 70f, 70f };
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

            if (CurrentPosition - grd.TotalHeight < 50 )
            {
                this.ImprimeCabecalho(ww, dd, drw);
            }
            if (First)
            {
                this.ImprimeSubCabecalho(ww, dd, drw);
            }
            PdfContentByte pc = ww.DirectContent;
            grd.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbGrid.TotalHeight;
            tbGrid = NewGrid();
            First = false;
        }
        private void ImprimeTotal(PdfWriter ww, Document dd, String Programa)
        {
            if (!String.IsNullOrEmpty(Programa))
            {
                PdfContentByte pc;
                PdfPTable tb = NewGrid();
                Double PrecoMedio_30 = Totalizador.Valor_30 / Totalizador.Qtd;
                Double PrecoMedio = Totalizador.Valor / Totalizador.Qtd;

                clsPdf.addCell(tb, new pdfLibCell() { Text = "Preço Médio do Programa", FontSize = 7, Align = PdfPCell.ALIGN_CENTER, Height = 20, colspan = 3 });
                clsPdf.addCell(tb, new pdfLibCell() { Text = FormatValor(PrecoMedio), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, Height = 20 });
                clsPdf.addCell(tb, new pdfLibCell() { Text = FormatValor(PrecoMedio_30), FontSize = 7, Align = PdfPCell.ALIGN_RIGHT, Height = 20 });
                pc = ww.DirectContent;
                tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                CurrentPosition = CurrentPosition - tb.TotalHeight;

                Totalizador.Valor = 0;
                Totalizador.Valor_30 = 0;
                Totalizador.Qtd = 0;
            }
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
        
        private void ImprimeBreak(PdfWriter ww, String Cod_Veiculo, String Data_Exibicao, String Cod_Programa)
        {
            PdfContentByte pc = ww.DirectContent;
            if (String.IsNullOrEmpty(Cod_Veiculo))
            {
                return;
            }

            DataTable dtbBreak = this.ReportLoadBrk(Cod_Veiculo, Data_Exibicao.ConvertToDatetime(),Cod_Programa);

            if (dtbBreak.Rows.Count==0)
            {
                return;
            }

            PdfPTable tb= NewGridBreak();
            PdfPTable tbLinha1 =  NewGridBreak();
            PdfPTable tbLinha2 = NewGridBreak();
            PdfPTable tbLinha3 = NewGridBreak();
            PdfPTable tbLinha4 = NewGridBreak();


            

            clsPdf.addCell(tb, new pdfLibCell() { Text = "Ocupação dos Breaks", FontSize = 7,Background=System.Drawing.Color.LightGray, Align = PdfPCell.ALIGN_CENTER,colspan=11 });
            pc = ww.DirectContent;
            tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tb.TotalHeight;


            Int32 contador = 0;
            foreach (DataRow drw in dtbBreak.Rows)
            {
                if (contador==0)
                {
                    tbLinha1 = NewGridBreak();
                    tbLinha2 = NewGridBreak();
                    tbLinha3 = NewGridBreak();
                    tbLinha4 = NewGridBreak();
                    clsPdf.addCell(tbLinha1, new pdfLibCell() { Text = "Horário", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbLinha2, new pdfLibCell() { Text = "Duraçao", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbLinha3, new pdfLibCell() { Text = "Ocupação", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbLinha4, new pdfLibCell() { Text = "Saldo", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                }
                
                clsPdf.addCell(tbLinha1, new pdfLibCell() { Text = drw["Hora_Inicio_Break"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                clsPdf.addCell(tbLinha2, new pdfLibCell() { Text = drw["Duracao"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                clsPdf.addCell(tbLinha3, new pdfLibCell() { Text = drw["Ocupacao"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                clsPdf.addCell(tbLinha4, new pdfLibCell() { Text = drw["Saldo"].ToString(), FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                contador++;
                if (contador >= 10)
                {
                    pc = ww.DirectContent;
                    tbLinha1.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                    CurrentPosition -= tbLinha1.TotalHeight;

                    pc = ww.DirectContent;
                    tbLinha2.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                    CurrentPosition -= tbLinha2.TotalHeight;

                    pc = ww.DirectContent;
                    tbLinha3.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                    CurrentPosition -= tbLinha3.TotalHeight;

                    pc = ww.DirectContent;
                    tbLinha4.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                    CurrentPosition -= tbLinha4.TotalHeight;


                    tb = NewGridBreak();
                    clsPdf.addCell(tb, new pdfLibCell() { Text = "", FontSize = 7, Background = System.Drawing.Color.LightGray, Align = PdfPCell.ALIGN_CENTER, colspan = 11, Height = 7 });
                    pc = ww.DirectContent;
                    tb.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                    CurrentPosition -= tb.TotalHeight;
                    contador = 0;
                }
            }
            if (contador<10)
            {
                for (int i =contador+1; i <= 10; i++)
                {
                    clsPdf.addCell(tbLinha1, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbLinha2, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbLinha3, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                    clsPdf.addCell(tbLinha4, new pdfLibCell() { Text = "", FontSize = 7, Align = PdfPCell.ALIGN_CENTER });
                }
                pc = ww.DirectContent;
                tbLinha1.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                CurrentPosition -= tbLinha1.TotalHeight;

                pc = ww.DirectContent;
                tbLinha2.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                CurrentPosition -= tbLinha2.TotalHeight;

                pc = ww.DirectContent;
                tbLinha3.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                CurrentPosition -= tbLinha3.TotalHeight;

                pc = ww.DirectContent;
                tbLinha4.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                CurrentPosition -= tbLinha4.TotalHeight;
                CurrentPosition -= 20;
            }
        }
        private PdfPTable NewGridBreak()
        {
            PdfPTable tb = new PdfPTable(11);
            float[] CellWidths = new float[] { 125f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f };
            float aTotalWidth = 0;
            for (int x = 0; x < tb.NumberOfColumns - 1; x++)
            {
                aTotalWidth += CellWidths[x];
            }
            tb.TotalWidth = aTotalWidth;
            tb.SetWidths(CellWidths);
            return tb;
        }
    }
}
