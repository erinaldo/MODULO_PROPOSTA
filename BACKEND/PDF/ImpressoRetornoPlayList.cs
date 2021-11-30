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

namespace PROPOSTA
{
    public class impressaoRetornoPlayList
    {
        private String Credential;
        private String CurrentUser;
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        private SimLib clsLib = new SimLib();
        private String[] NomeStatus= { "Previso/Exibido", "Previso/Não exibido", "Não Previsto/Exibido" };
        PdfLib clsPdf = new PdfLib(); 
        public impressaoRetornoPlayList(String pUser)
        {
            this.Credential = pUser;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }
        public String ImprimirRetornoPlayList(List<RetornoPlayList.RetornoPlayListBaixaModel> Filtro)
        {
            String strFilePdf = string.Empty;
            String PdfFinal = string.Empty;
            DataTable dtbEsquema = new DataTable("");
            SqlDataAdapter dtaEsquema = new SqlDataAdapter();
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            try
            {
                String xmlRetorno = null;
                if (Filtro.Count > 0)
                {
                    xmlRetorno = clsLib.SerializeToString(Filtro);
                }
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Retorno_Playlist_Print");
                SqlDataAdapter Adp = new SqlDataAdapter();
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Retorno", xmlRetorno);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Order_Column", Filtro[0].SortOrder);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Order_Type", Filtro[0].SortType );
                
                Adp.Fill(dtbEsquema);

                //'------------------------Diretorio Temporario para geracao dos PDF
                String sPath = HttpContext.Current.Server.MapPath("~/PDFFILES/RETORNOPLAYLIST");
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
                var list = System.IO.Directory.GetFiles(sPath, "*.pdf");
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

                //=========================Gera pdf 
                strFilePdf = "" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".PDF";
                if (GerarPdf(dtbEsquema, sPath, strFilePdf))
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
                cnn.Close();
            }
            return PdfFinal;
        }
        private Boolean GerarPdf(DataTable Filtro, String Path, String FileName)
        {
            Boolean bolRetorno = true;
            String strFile = Path + FileName;
            FileStream strea = new FileStream(strFile, FileMode.Create);
            Document doc = new Document(PageSize.A4.Rotate());

            PdfWriter write = PdfWriter.GetInstance(doc, strea);
            clsConexao cnn = new clsConexao(this.Credential);
            PdfContentByte pc;
            Boolean hasRow = false;
            try
            {
                doc.SetMargins(10, 10, 10, 10);
                doc.Open();
                PdfPTable TbRetorno = clsPdf.CreateTable(new float[] { 100, 50, 70, 50, 50, 70, 200, 50, 75, 70 });
                //for (int i = 0; i < Filtro.Count; i++)
                foreach (DataRow drw  in Filtro.Rows)
                { 
                    //------------------------Quebrou o tamanho da pagina
                    if (CurrentPosition - TbRetorno.TotalHeight < 50)
                    {
                        if (hasRow)
                        {
                            pc = write.DirectContent;
                            TbRetorno.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                            TbRetorno = clsPdf.CreateTable(new float[] { 100, 50, 70, 50, 50, 70, 200, 50, 75, 70 });
                        }
                        this.ImprimeCabecalho(write, doc,drw);
                    }

                    //------------------------Imprime Linha
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = NomeStatus[drw["Status"].ToString().ConvertToInt32()] });
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Cod_Veiculo"].ToString() });
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Data_Exibicao"].ToString().ConvertToDatetime().ToString("dd/MM/yyyy")});
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Cod_Programa"].ToString() });
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Chave_Acesso"].ToString()});
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Numero_Fita"].ToString() });
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Titulo_Comercial"].ToString(), Align = PdfPCell.ALIGN_LEFT } );
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Cod_Qualidade"].ToString() });
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Horario_Exibicao"].ToString() });
                    clsPdf.addCell(TbRetorno, new pdfLibCell() { Text = drw["Numero_Ce"].ToString() });

                    hasRow = true;
                }
                //------------------------Imprime ultimo bloco
                if (hasRow)
                {
                    pc = write.DirectContent;
                    TbRetorno.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                }

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
            PageNumber++;
            if (PageNumber > 1)
            {
                dd.NewPage();
            }

            String LogoName = "logo_Roteiro_" + drw["Cod_Veiculo"].ToString();
            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + LogoName);
            if (!File.Exists(sPathLogo))
            {

                sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Roteiro_Padrao.png");
            }

            PdfContentByte pc;
            PdfPTable tbDados = clsPdf.CreateTable(new float[] { 120, 450 });

            CurrentPosition = 560;
            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = 540, Path = sPathLogo, Scale = 100 });
            
            clsPdf.AddTexto(ww, new pdfLibText() { Text = "Listagem de Checking - Retorno PlayList ", X = 200, Y = CurrentPosition, FontSize = 14 });
            

            CurrentPosition -= 30;
            pc = ww.DirectContent;
            tbDados.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbDados.TotalHeight;

            tbDados = clsPdf.CreateTable(new float[] { 100, 50, 70, 50, 50, 70, 200,50,75,70 });


            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Status", Background = System.Drawing.Color.WhiteSmoke, Align = PdfPCell.ALIGN_LEFT });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Veiculo", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Dt.Exibição", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Programa", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Ch.Ac", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Fita", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Titulo", Background = System.Drawing.Color.WhiteSmoke,Align= PdfPCell.ALIGN_LEFT});
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Qual.", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Horário", Background = System.Drawing.Color.WhiteSmoke });
            clsPdf.addCell(tbDados, new pdfLibCell() { Text = "Numero CE", Background = System.Drawing.Color.WhiteSmoke });

            pc = ww.DirectContent;
            CurrentPosition -= 10;
            tbDados.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition -= tbDados.TotalHeight;

        }
        



    }
}
