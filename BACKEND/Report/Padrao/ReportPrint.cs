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
    public partial class Report
    {
        private Int32 PageNumber = 0;
        private float CurrentPosition = 0;
        PdfLib clsPdf = new PdfLib();
        public String ImprimirRpt(Report.ReportFilterModel Rpt)
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
                //=========================Gera pdf -- Modelos Padrão 


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
            PdfContentByte pc;
            String _text = "";
            try
            {
                //===================================Le a definicao do report e Configura o Documento
                ReportDefModel Def = ReadDef(Filter.RptName);
                if (Def.PageOrientation.ToUpper() == "PORTRAIT")
                {
                    doc.SetPageSize(PageSize.A4.Rotate());
                }
                else
                {
                    doc.SetPageSize(PageSize.A4);
                }
                doc.SetMargins(10, 10, 10, 10);
                doc.Open();
                //==================Configura o Grid do Relatorio
                List<float> tbColumns = new List<float>();
                for (int i = 0; i < Def.Fields.Count; i++)
                {
                    if (!Def.Fields[i].Fixed)
                    {
                        tbColumns.Add(float.Parse(Def.Fields[i].width));
                    }
                }

                PdfPTable tbModelo = clsPdf.CreateTable(tbColumns.ToArray());
                PdfPCell cell = new PdfPCell();

                //==================Executa a Procedure do Relatorio e preenche datatable
                //DataTable dtbPdf = ExecuteProc(Filter);
                DataTable dtbPdf = this.ReportLoadData(Filter);

                if (dtbPdf.Rows.Count == 0)
                {
                    return false;
                }
                //==================Imprime todas as Linhas que retornaram da procedure
                int _Aligin = 0;
                int _FontSize = 0;

                System.Drawing.Color _BackGroundColor;
                System.Drawing.Color _ForeColor;


                foreach (DataRow drw in dtbPdf.Rows)
                {
                    //PdfPTable tbGrid = tbModelo;
                    PdfPTable tbGrid = clsPdf.CreateTable(tbColumns.ToArray());
                    for (int i = 0; i < Def.Fields.Count; i++)
                    {
                        if (!Def.Fields[i].Fixed)
                        {
                            switch (Def.Fields[i].Align.ToUpper())
                            {
                                case "LEFT":
                                    _Aligin = PdfPCell.ALIGN_LEFT;
                                    break;
                                case "CENTER":
                                    _Aligin = PdfPCell.ALIGN_CENTER;
                                    break;
                                case "RIGHT":
                                    _Aligin = PdfPCell.ALIGN_RIGHT;
                                    break;
                                default:
                                    break;
                            }
                            if (string.IsNullOrEmpty(Def.Fields[i].FontSize))
                            {
                                _FontSize = 10;
                            }
                            else
                            {
                                _FontSize = int.Parse(Def.Fields[i].FontSize);
                            }
                            if (string.IsNullOrEmpty(Def.Fields[i].Backcolor))
                            {
                                _BackGroundColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                _BackGroundColor = System.Drawing.Color.FromName(Def.Fields[i].Backcolor);
                            }

                            if (string.IsNullOrEmpty(Def.Fields[i].Color))
                            {
                                _ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                _ForeColor = System.Drawing.Color.FromName(Def.Fields[i].Color);
                            }

                            if (!String.IsNullOrEmpty(Def.Fields[i].SqlField))
                            {
                                _text = drw[Def.Fields[i].SqlField].ToString();
                            }
                            else
                            {
                                _text = drw[i].ToString();
                            }

                            clsPdf.addCell(tbGrid, new pdfLibCell()
                            {
                                Text = _text,
                                FontName = Def.Fields[i].FontName,
                                FontSize = _FontSize,
                                FontStyle = (Def.Fields[i].FontBold) ? iTextSharp.text.Font.BOLD : iTextSharp.text.Font.NORMAL,
                                Align = _Aligin,
                                Background = _BackGroundColor,
                                FontColor = _ForeColor

                            });
                        }
                    }
                    if (CurrentPosition - tbGrid.TotalHeight < 20)
                    {
                        this.ImprimeCabecalho(write, doc, Filter, Def, drw);
                    }
                    pc = write.DirectContent;
                    tbGrid.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
                    CurrentPosition = CurrentPosition - tbGrid.TotalHeight;
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
        private ReportDefModel ReadDef(String RptName)
        {
            String sPath = HttpContext.Current.Server.MapPath("~/REPORT/JSON");
            if (sPath.Right(1) != @"\")
            {
                sPath += @"\";
            }
            string sFile = sPath + RptName + "_DEF.json";
            string allText = System.IO.File.ReadAllText(sFile);
            JObject json = JObject.Parse(allText);
            var jsonFields = json["Fields"].Value<JArray>();
            var _fields = JsonConvert.DeserializeObject<List<FieldsModel>>(jsonFields.ToString());

            ReportDefModel Def = new ReportDefModel();
            List<FieldsModel> Fields = new List<FieldsModel>();
            for (int i = 0; i < _fields.Count; i++)
            {

                Fields.Add(new FieldsModel()
                {
                    SqlField = _fields[i].SqlField,
                    FontName = _fields[i].FontName,
                    FontSize = _fields[i].FontSize,
                    FontStyle = _fields[i].FontStyle,
                    FontBold = _fields[i].FontBold,
                    Backcolor = _fields[i].Backcolor,
                    Color = _fields[i].Color,
                    HeaderText = _fields[i].HeaderText,
                    HeaderFontName = _fields[i].HeaderFontName,
                    HeaderFontSize = _fields[i].HeaderFontSize,
                    HeaderFontStyle = _fields[i].HeaderFontStyle,
                    HeaderFontBold = _fields[i].HeaderFontBold,
                    width = _fields[i].width,
                    Align = _fields[i].Align,
                    PositionX = _fields[i].PositionX,
                    PositionY = _fields[i].PositionY,
                    Fixed = _fields[i].Fixed,
                });

            }
            Def.PageSize = json["PageSize"].Value<string>();
            Def.PageOrientation = json["PageOrientation"].Value<string>();
            Def.ReportName = json["ReportName"].Value<string>();
            Def.Fields = Fields;
            return Def;
        }
        //private DataTable ExecuteProc(ReportFilterModel Param)
        //{
        //    clsConexao cnn = new clsConexao(this.Credential);
        //    cnn.Open();
        //    SqlDataAdapter Adp = new SqlDataAdapter();
        //    DataTable dtb = new DataTable("dtb");
        //    SimLib clsLib = new SimLib();
        //    try
        //    {

        //        SqlCommand cmd = cnn.Procedure(cnn.Connection, Param.ProcName);
        //        Adp.SelectCommand = cmd;
        //        Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
        //        Adp.SelectCommand.Parameters.AddWithValue("@Par_Output", Param.Output);
        //        var strXml = "";
        //        for (int i = 0; i < Param.Filters.Count; i++)
        //        {
        //            if (!String.IsNullOrEmpty(Param.Filters[i].ParameterName))
        //            {


        //            switch (Param.Filters[i].Type)
        //            {
        //                case null:
        //                    break;
        //                case "Date":
        //                    if (!String.IsNullOrEmpty(Param.Filters[i].Value.ToString()))
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, Param.Filters[i].Value.ConvertToDatetime());
        //                    }
        //                    else
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
        //                    }
        //                    break;
        //                case "Boolean":
        //                    Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, Param.Filters[i].Value.ConvertToBoolean());
        //                    break;
        //                case "Text":
        //                case "Options":

        //                    if (!String.IsNullOrEmpty(Param.Filters[i].Value.ToString()))
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, Param.Filters[i].Value);
        //                    }
        //                    else
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
        //                    }
        //                    break;
        //                case "Month":
        //                    if (!String.IsNullOrEmpty(Param.Filters[i].Value.ToString()))
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, clsLib.CompetenciaInt(Param.Filters[i].Value));
        //                    }
        //                    else
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
        //                    }
        //                    break;
        //                case "List":
        //                    if (Param.Filters[i].ArrayValue.Count > 0)
        //                    {

        //                        strXml = clsLib.SerializeToString(Param.Filters[i].ArrayValue);
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, strXml);
        //                    }
        //                    else
        //                    {
        //                        Adp.SelectCommand.Parameters.AddWithValue(Param.Filters[i].ParameterName, DBNull.Value);
        //                    }
        //                    break;
        //                default:
        //                    throw new Exception("Tipo de dado não definido no Filtro");
        //            }
        //        }
        //        }

        //        Adp.Fill(dtb);

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return dtb;
        //}
        private void ImprimeCabecalho(PdfWriter ww, Document dd, ReportFilterModel Filter, ReportDefModel Def, DataRow drw)
        {
            CurrentPosition = dd.PageSize.Height;
            int _Aligin = 0;
            int _FontSize = 0;
            Boolean hasFixedTop = false;
            System.Drawing.Color _BackGroundColor;
            System.Drawing.Color _ForeColor;
            PdfPTable tbGrid;

            PageNumber++;
            if (PageNumber > 1)
            {
                dd.NewPage();
            }
            String sPathLogo = HttpContext.Current.Server.MapPath("~/logos/" + "logo_Padrao.png");
            PdfContentByte pc;
            CurrentPosition = dd.PageSize.Height;

            clsPdf.addLogo(dd, new pdfLibLogo() { X = 20, Y = dd.PageSize.Height - 50, Path = sPathLogo, Scale = 70 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = 200, Y = dd.PageSize.Height - 20, Text = Filter.Title, FontSize = 14 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 50, Y = dd.PageSize.Height - 20, Text = "Pag." + PageNumber.ToString(), FontSize = 7 });
            clsPdf.AddTexto(ww, new pdfLibText() { X = dd.PageSize.Width - 132, Y = dd.PageSize.Height - 35, Text = "Emitido em." + DateTime.Now, FontSize = 7 });

            for (int i = 0; i < Def.Fields.Count; i++)
            {
                if (Def.Fields[i].Fixed)
                {
                    hasFixedTop = true;
                    clsPdf.AddTexto(ww, new pdfLibText()
                    {
                        X = float.Parse(Def.Fields[i].PositionX),
                        Y = float.Parse(Def.Fields[i].PositionY),
                        Text = drw[i].ToString(),
                        FontSize = float.Parse(Def.Fields[i].FontSize),
                        FontStyle = (Def.Fields[i].FontStyle.ToUpper() == "BOLD") ? Font.BOLD : Font.NORMAL,
                    });
                    CurrentPosition = float.Parse(Def.Fields[i].PositionY) - 20;
                }
            }



            List<float> tbColumns = new List<float>();
            for (int i = 0; i < Def.Fields.Count; i++)
            {
                if (!Def.Fields[i].Fixed)
                {
                    tbColumns.Add(float.Parse(Def.Fields[i].width));
                }
            }

            PdfPTable tbModelo = clsPdf.CreateTable(tbColumns.ToArray());
            PdfPCell cell = new PdfPCell();
            tbGrid = new PdfPTable(tbModelo);


            for (int i = 0; i < Def.Fields.Count; i++)
            {
                if (!Def.Fields[i].Fixed)
                {
                    switch (Def.Fields[i].Align.ToUpper())
                    {
                        case "LEFT":
                            _Aligin = PdfPCell.ALIGN_LEFT;
                            break;
                        case "CENTER":
                            _Aligin = PdfPCell.ALIGN_CENTER;
                            break;
                        case "RIGHT":
                            _Aligin = PdfPCell.ALIGN_RIGHT;
                            break;
                        default:
                            break;
                    }
                    if (string.IsNullOrEmpty(Def.Fields[i].HeaderFontSize))
                    {
                        _FontSize = 10;
                    }
                    else
                    {
                        _FontSize = int.Parse(Def.Fields[i].HeaderFontSize);
                    }

                    _BackGroundColor = System.Drawing.Color.White;
                    _ForeColor = System.Drawing.Color.Black;



                    clsPdf.addCell(tbGrid, new pdfLibCell()
                    {
                        Text = (String.IsNullOrEmpty(Def.Fields[i].HeaderText) ? drw.Table.Columns[i].ColumnName : Def.Fields[i].HeaderText),
                        FontName = Def.Fields[i].HeaderFontName,
                        FontSize = _FontSize,
                        FontStyle = (Def.Fields[i].HeaderFontBold) ? iTextSharp.text.Font.BOLD : iTextSharp.text.Font.NORMAL,
                        Align = _Aligin,
                        Background = _BackGroundColor,
                        FontColor = _ForeColor

                    });
                }
            }
            pc = ww.DirectContent;
            if (!hasFixedTop)
            {
                CurrentPosition = dd.PageSize.Height - 70;
            }

            tbGrid.WriteSelectedRows(0, -1, 10, CurrentPosition, pc);
            CurrentPosition = CurrentPosition - tbGrid.TotalHeight;
        }
    }
}
