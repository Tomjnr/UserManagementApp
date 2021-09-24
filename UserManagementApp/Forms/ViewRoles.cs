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
    public partial class ViewRoles : TemplateForm
    {
        public ViewRoles()
        {
            InitializeComponent();
            LoadDataIntoDatagridview();
        }

        private void LoadDataIntoDatagridview()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Roles_LoadDataInDatagridview", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                   

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    DataTable dtRoles = new DataTable();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    dtRoles.Load(sdr);

                    RolesDataGridView.DataSource = dtRoles;


                }

            }


            }

        private void ViewRoles_Load(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            if (SearchTxt.Text != string.Empty)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Roles_SearchByTitle", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleTitle", SearchTxt.Text.Trim());


                        if (con.State != ConnectionState.Open)
                            con.Open();
                        DataTable dtRoles = new DataTable();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            dtRoles.Load(sdr);

                            RolesDataGridView.DataSource = dtRoles;
                        }
                        else
                        {
                            MessageBox.Show("No matching Role is Found", "No Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
             }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataIntoDatagridview();
            SearchTxt.Clear();
            SearchTxt.Focus();
        }

        private void newRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleFormcs rf = new RoleFormcs();
            rf.ShowDialog();
            LoadDataIntoDatagridview();
        }

        private void RolesDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RolesDataGridView.Rows.Count > 0)
            {
                //it gets the role id double clicked in the data gridview
                int roleId = Convert.ToInt32(RolesDataGridView.SelectedRows[0].Cells[0].Value);

                RoleFormcs rolesForm = new RoleFormcs();
                rolesForm.RoleId = roleId;
                rolesForm.IsUpdate = true;
                rolesForm.ShowDialog();
                LoadDataIntoDatagridview();
            }
        }
    }
}
