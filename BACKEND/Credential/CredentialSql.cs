using CLASSDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PROPOSTA
{
    public partial class apiCredential
    {
        public DataTable Login(String pCredential,Boolean pValidar_Token)
        {
            SimLib clsLib = new SimLib();
            String User = clsLib.Decriptografa(clsLib.GetJsonItem(pCredential, "Name"));
            String Password = clsLib.GetJsonItem(pCredential, "Password");
            String Token = clsLib.GetJsonItem(pCredential, "Token");
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(pCredential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Login");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", User);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Senha", Password);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Token",Token);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Validar_Token", pValidar_Token);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }
        public UserDataModel GetUserData()
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            String pUser = this.CurrentUser;
             AppSettingsReader AppRead = new AppSettingsReader();
             String Register = AppRead.GetValue("Register", typeof(string)).ToString();
            UserDataModel Userdata = new UserDataModel();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_Proposta_User_Data_S");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", pUser);
                Adp.Fill(dtb);
                if (dtb.Rows.Count>0)
                {
                    Userdata.Login = dtb.Rows[0]["Login"].ToString();
                    Userdata.Nome = dtb.Rows[0]["Nome"].ToString();
                    Userdata.Email = dtb.Rows[0]["Email"].ToString();
                    Userdata.Usuario_Id = dtb.Rows[0]["Usuario_Id"].ToString().ConvertToInt32();
                    Userdata.Cod_Empresa = dtb.Rows[0]["Cod_Empresa"].ToString();
                    Userdata.Nome_Empresa = dtb.Rows[0]["Nome_Empresa"].ToString();
                    Userdata.Register = Register;
                }
                //DataColumn newColumn = new DataColumn("Register", typeof(String));
                //newColumn.DefaultValue = Register;
                //dtb.Columns.Add(newColumn);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Userdata;
        }

        public  List<UserModulosModel> AddUserModulos(String pLogin)
        {
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            SimLib clsLib = new SimLib();
            List<UserModulosModel> Modulos = new List<UserModulosModel>();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Get_User_Modulos");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Modulos.Add(new UserModulosModel()
                    {
                        Id_Modulo = drw["Id_Modulo"].ToString().ConvertToInt32(),
                        Descricao = drw["Descricao"].ToString(),
                        Chave = drw["Chave"].ToString(),
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Modulos;
        }

        public Boolean Permissao(String pRota)
        {
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            Boolean bolRetorno = false;
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Permissao_Validar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Route", pRota);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                bolRetorno = dtb.Rows[0]["Permissao"].ToString().ConvertToBoolean();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return bolRetorno;
        }

        public String EsqueceuSenha(RememberPassword Param)
        {

            String Retorno = "";
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            SimLib clsLib = new SimLib();
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Esqueceu_Senha");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Email", Param.Email);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", Param.Login);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Esqueceu_Login", Param.EsqueceuLogin);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Url", Param.Url);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);

                if (dtb.Rows.Count > 0)
                {
                    if (int.Parse(dtb.Rows[0]["Status"].ToString()) == 1)
                    {
                        clsLib.EnviaEmail(Param.Email, "", "", "Solicitação de Alteração de Senha", dtb.Rows[0]["Email"].ToString(), "");
                    }
                    Retorno = dtb.Rows[0]["Mensagem"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Retorno;
        }

        public String AlterarSenha(RememberPassword Param)
        {

            String Retorno = "";
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            SimLib clsLib = new SimLib();
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Alterar_Senha");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Token", Param.Token);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Senha", Param.Senha);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);

                if (dtb.Rows.Count > 0)
                {
                    Retorno = dtb.Rows[0]["Mensagem"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Retorno;
        }

        //public DataTable CheckLogin(CheckLoginModel pCredential)
        //{
        //    SimLib clsLib = new SimLib();
        //    String User = clsLib.Criptografa( pCredential.login);
        //    String Password = clsLib.Criptografa(pCredential.password);

        //    DataTable dtb = new DataTable("dtb");
        //    clsConexao cnn = new clsConexao(User, Password);
        //    cnn.Open();
        //    SqlDataAdapter Adp = new SqlDataAdapter();
        //    try
        //    {


        //        SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Login");
        //        Adp.SelectCommand = cmd;
        //        Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", pCredential.login);
        //        Adp.SelectCommand.Parameters.AddWithValue("@Par_Senha", Password);
        //        Adp.SelectCommand = cmd;
        //        Adp.Fill(dtb);

        //        if (dtb.Rows[0]["Indica_Token"].ToString().ConvertToBoolean() && dtb.Rows[0]["Forma_EnvioToken"].ToString().ConvertToByte()==2)
        //        {
        //            String strBody = "";
        //            strBody += "<style>";
        //            strBody += "p {font-family:verdana;font-size:12;font-style:italic}";
        //            strBody += "</style>";
        //            strBody += "</head>";
        //            strBody += "<body>";
        //            strBody += "<p>Prezado(a) Sr(a) " + dtb.Rows[0]["Nome"].ToString() + "</p>";
        //            strBody += "<br>";
        //            strBody += "<p>Seu Token de Acesso ao Sim-Vendas é " + dtb.Rows[0]["Token"].ToString() + "</p>";
        //            strBody += "<br>";
        //            strBody += "<br>";
        //            strBody += "<p style='font-size=9'>" + "Email enviado automáticamente,favor não responder." + "</p>";
        //            clsLib.EnviaEmail(dtb.Rows[0]["Email"].ToString(), "", "", "Sim Vendas - Token de Acesso", strBody,"");
        //        }
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
        public DataTable CheckLogin(CheckLoginModel pCredential)
        {
            SimLib clsLib = new SimLib();
            //String User = clsLib.Criptografa(pCredential.login);
            //String Password = clsLib.Criptografa(pCredential.password);

            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Login");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", clsLib.Decriptografa(pCredential.login));
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Senha", pCredential.password);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }

        public DataTable GetToken(CheckLoginModel pCredential)
        {
            SimLib clsLib = new SimLib();
            //String User = clsLib.Criptografa(pCredential.login);
            //String Password = clsLib.Criptografa(pCredential.password);

            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_GetToken");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", pCredential.login);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
                if (dtb.Rows[0]["Status"].ToString().ConvertToBoolean()) 
                {
                    String strBody = "";
                    strBody += "<style>";
                    strBody += "p {font-family:verdana;font-size:12;font-style:italic}";
                    strBody += "</style>";
                    strBody += "</head>";
                    strBody += "<body>";
                    strBody += "<p>Prezado(a) Sr(a) " + dtb.Rows[0]["Nome"].ToString() + "</p>";
                    strBody += "<br>";
                    strBody += "<p>Seu Token de Acesso ao Sim-Vendas é " + dtb.Rows[0]["Token"].ToString() + "</p>";
                    strBody += "<br>";
                    strBody += "<br>";
                    strBody += "<p style='font-size=9'>" + "Email enviado automáticamente,favor não responder." + "</p>";
                    clsLib.EnviaEmail(dtb.Rows[0]["Email"].ToString(), "", "", "Sim Vendas - Token de Acesso", strBody, "");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }
        public DataTable AppGetToken(String pLogin)
        {
            SimLib clsLib = new SimLib();

            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_GetToken");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", pLogin);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }

        public DataTable GetUserModulos()
        {
            SimLib clsLib = new SimLib();
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Get_User_Modulos");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return dtb;
        }
        public List<MenuModel> GetUserMenu()
        {
            SimLib clsLib = new SimLib();
            DataTable dtb = new DataTable("dtb");
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            List<MenuModel> Menu = new List<MenuModel>();
            SqlDataAdapter Adp = new SqlDataAdapter();

            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Get_User_Menu");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Login", this.CurrentUser);
                Adp.SelectCommand = cmd;
                Adp.Fill(dtb);

                DataView viewMenu = new DataView(dtb);
                DataTable dtbMenu = viewMenu.ToTable(true, "Id_Modulo", "Descricao_Modulo");


                foreach (DataRow drw in dtbMenu.Rows)
                {
                    Menu.Add(new MenuModel()
                    {
                        Id = drw["Id_Modulo"].ToString().ConvertToInt32(),
                        Title = drw["Descricao_Modulo"].ToString(),
                        SubItens = AddItens(dtb, drw["Id_Modulo"].ToString().ConvertToInt32())
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
            return Menu;
        }

        private List<MenuItemModel> AddItens(DataTable dtb, Int32 Id)
        {
            List<MenuItemModel> Itens = new List<MenuItemModel>();
            DataView viewItem = new DataView(dtb);
            viewItem.RowFilter = "Id_Modulo = " + Id.ToString();
            DataTable dtbItem = viewItem.ToTable();
            foreach (DataRow drw in dtbItem.Rows)
            {
                Itens.Add(new MenuItemModel()
                {
                    Id = drw["Id_Menu"].ToString().ConvertToInt32(),
                    Title = drw["Descricao_Menu"].ToString(),
                    Url = drw["Url"].ToString(),
                });
            }
            return Itens;
        }

    }

}