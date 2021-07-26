using CLASSDB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace PROPOSTA
{

    public partial class GrupoUsuario
    {
        public DataTable GrupoUsuarioListar(Int32 pIdGrupoUsuario)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "[PR_PROPOSTA_Grupo_Usuario_Listar]");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Grupo", pIdGrupoUsuario);
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
        public GrupoUsuarioModel GetGrupoUsuario(Int32 pIdGrupoUsuario)
        {
            GrupoUsuarioModel GrupoUsuario = new GrupoUsuarioModel();
            List<PerfilModel> Perfil = new List<PerfilModel>();

            clsConexao cnn = new clsConexao(this.Credential);
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            cnn.Open();
            SimLib clsLib = new SimLib();
            try
            {
                //=======================Dados do GrupoUsuario

                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Grupo_Usuario_Listar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Grupo", pIdGrupoUsuario);
                Adp.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    GrupoUsuario.Id_Grupo= dtb.Rows[0]["Id_Grupo"].ToString().ConvertToInt32();
                    GrupoUsuario.Cod_Grupo= dtb.Rows[0]["Cod_Grupo"].ToString();
                    GrupoUsuario.Descricao= dtb.Rows[0]["Descricao"].ToString();
                    GrupoUsuario.Status = dtb.Rows[0]["Status"].ToString().ConvertToBoolean();
                    GrupoUsuario.Descricao_Status = dtb.Rows[0]["Descricao_Status"].ToString();
                    GrupoUsuario.Perfil = this.addPerfil(pIdGrupoUsuario);
                    GrupoUsuario.Usuarios= this.addUsuarios(pIdGrupoUsuario);
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
            return GrupoUsuario;
        }
        public List<PerfilModel> addPerfil(Int32 pIdGrupoUsuario)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            List<PerfilModel> Perfil = new List<PerfilModel>();
            try
            {
                SqlDataAdapter Adp = new SqlDataAdapter();
                DataTable dtb = new DataTable("dtb");
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Grupo_Usuario_Perfil_List");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Grupo", pIdGrupoUsuario);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Perfil.Add(new PerfilModel()
                    {
                        Id_Funcao = drw["Id_Funcao"].ToString().ConvertToInt32(),
                        Id_Funcao_Root = drw["Id_Funcao_Root"].ToString().ConvertToInt32(),
                        Descricao_Funcao = drw["Descricao_Funcao"].ToString(),
                        Path = drw["Path"].ToString(),
                        Selected = drw["Selected"].ToString().ConvertToBoolean(),
                        Nivel = drw["Nivel"].ToString().ConvertToInt32()
                    }
                    );
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

            return Perfil;
        }
        public List<UsuarioModel> addUsuarios(Int32 pIdGrupoUsuario)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            List<UsuarioModel> Usuarios = new List<UsuarioModel>();
            try
            {
                SqlDataAdapter Adp = new SqlDataAdapter();
                DataTable dtb = new DataTable("dtb");
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "Pr_Proposta_Composicao_Grupo_Usuario_List");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Grupo", pIdGrupoUsuario);
                Adp.Fill(dtb);
                foreach (DataRow drw in dtb.Rows)
                {
                    Usuarios.Add(new UsuarioModel()
                    {
                        Id_Usuario= drw["Id_Usuario"].ToString().ConvertToInt32(),
                        Login = drw["Login"].ToString(),
                        Nome=  drw["Nome"].ToString(),
                        Selected = drw["Selected"].ToString().ConvertToBoolean(),
                    }
                    );
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

            return Usuarios;
        }
        public DataTable SalvarGrupoUsuario(GrupoUsuarioModel GrupoUsuario)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable dtb = new DataTable("dtb");
            SimLib clsLib = new SimLib();
            String xmlPerfil = null;
            String xmlUsuarios= null;
            if (GrupoUsuario.Perfil.Count > 0)
            {
                xmlPerfil = clsLib.SerializeToString(GrupoUsuario.Perfil);
            }
            if (GrupoUsuario.Usuarios.Count > 0)
            {
                xmlUsuarios= clsLib.SerializeToString(GrupoUsuario.Usuarios);
            }
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Grupo_Usuario_Salvar");
                Adp.SelectCommand = cmd;
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Id_Grupo", GrupoUsuario.Id_Grupo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Cod_Grupo", GrupoUsuario.Cod_Grupo);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Descricao", GrupoUsuario.Descricao);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Perfil", xmlPerfil);
                Adp.SelectCommand.Parameters.AddWithValue("@Par_Usuarios", xmlUsuarios);
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
        public void DesativarReativar(GrupoUsuarioModel GrupoUsuario)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Grupo_Usuario_Update_Status");
                cmd.Parameters.AddWithValue("@Par_Id_Grupo", GrupoUsuario.Id_Grupo);
                cmd.Parameters.AddWithValue("@Par_Status", GrupoUsuario.Status);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }

        }
        public void ExcluirGrupoUsuario(GrupoUsuarioModel GrupoUsuario)
        {
            clsConexao cnn = new clsConexao(this.Credential);
            cnn.Open();
            SimLib clsLib = new SimLib();
            try
            {
                SqlCommand cmd = cnn.Procedure(cnn.Connection, "PR_PROPOSTA_Excluir_Grupo_Usuario");
                cmd.Parameters.AddWithValue("@Par_Id_Grupo", GrupoUsuario.Id_Grupo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }

        }

    }
}
