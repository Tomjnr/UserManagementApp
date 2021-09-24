using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserManagementApp.General;

namespace UserManagementApp.Forms
{
    public partial class RoleFormcs : TemplateForm
    {
        public RoleFormcs()
        {
            InitializeComponent();
        }
        //handling update proces
        public int RoleId { get; set; }
        public bool IsUpdate { get; set; }

        private void RoleFormcs_Load(object sender, EventArgs e)
        {
            if (this.IsUpdate == true)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("ups_Roles_ReloadDataForUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", RoleId);
                       
                    
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        DataTable dtRoles = new DataTable();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        dtRoles.Load(sdr);
                        DataRow row = dtRoles.Rows[0];
                        TittleTxt.Text = row["RoleTitle"].ToString();
                        DescripTxt.Text = row["Description"].ToString();

                        //Change Controls
                        SaveBtn.Text = "Update Role Information";
                        DeleteBtn.Enabled = true;
                    }
                }
            }
                
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                if (this.IsUpdate)
                {
                    //update
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("ups_Roles_UpdateRoleByRoleid", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RoleId", this.RoleId);
                            cmd.Parameters.AddWithValue("@RoleTitle", TittleTxt.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescripTxt.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role is Successfully Updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    //insert
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Roles_InsertNewRole", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RoleTitle", TittleTxt.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescripTxt.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role is Successfully Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }

        private void ResetFormControl()
        {
            TittleTxt.Clear();
            DescripTxt.Clear();

            TittleTxt.Focus();
            //check if form is loaded for update process
            if (IsUpdate)
            {
                this.RoleId = 0;
                this.IsUpdate = false;
                DeleteBtn.Enabled = false;
                SaveBtn.Text = "Save Information";

            }
        }

        private bool IsFormValid()
        {
            if (TittleTxt.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Role Title is Required.","validation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                TittleTxt.Focus();
                return false;  
            }
            if (TittleTxt.Text.Length >=50)
            {
                MessageBox.Show("Role Title should be less or equals to 50 characters.", "validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TittleTxt.Focus();
                return false;
            }
            return true;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (this.IsUpdate)
            {
                DialogResult result = MessageBox.Show("Are You Sure You want to delete this Role?","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //Delete
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("ups_Roles_DeleteByRoleId", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RoleId", this.RoleId);


                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role is Successfully Removed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }
    }
}
