/*
* FILE : ConfigToolForm.cs
* PROJECT : PROG3070 - Project Milestone 01
* PROGRAMMER : Andrii Dushkevych, Phil Kempton
* FIRST VERSION : 2019-03-18
* DESCRIPTION :
* This file contains logic for ConfigTool WinForms based app for Advanced SQL Project
*/
using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace ADushkevych_PKempton_AdvSqlProject
{
    public partial class ConfigToolForm : Form
    {
        //configurations from DB
        private DataTable configDT;
        //connection string getter
        private string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        public ConfigToolForm()
        {
            InitializeComponent();
            LoadDataGrid();
        }

        // FUNCTION : LoadDataGrid
        // DESCRIPTION : Loads contents for config DataTable from database.
        // PARAMETERS : no params
        // RETURNS : nothing
        private void LoadDataGrid()
        {
            string query = "SELECT [Key], [Value] FROM ConfigInfo";
            configDT = GetData(query);
            ConfigInfoDG.DataSource = configDT;
        }

        // FUNCTION : GetData
        // DESCRIPTION : Loads contents into DataTable from database.
        // PARAMETERS : string query - select query
        // RETURNS : query execution result DataTable
        private DataTable GetData(string query)
        {            
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        // FUNCTION : updateConfigButton_Click
        // DESCRIPTION : UpdateConfig button handler
        // PARAMETERS : object sender - event sender
        //              EventArgs e - event args
        // RETURNS : nothing
        private void updateConfigButton_Click(object sender, EventArgs e)
        {
            SaveToDB();
        }

        // FUNCTION : SaveToDB
        // DESCRIPTION : Writes current state of Config DataTable into database 
        //                  updating fields or adding new if needed
        // PARAMETERS : no params
        // RETURNS : nothing
        private void SaveToDB()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                foreach (DataRow dr in configDT.Rows)
                {
                    bool updated = false;
                    using (SqlCommand cmd = new SqlCommand("UPDATE ConfigInfo SET [Value]=@NewValue WHERE [Key]=@Key", con))
                    {
                        cmd.Parameters.AddWithValue("@Key", dr["Key"]);
                        cmd.Parameters.AddWithValue("@NewValue", dr["Value"]);

                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            updated = true;
                        }
                    }

                    if (!updated)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO ConfigInfo ([Key], [Value]) VALUES (@NewKey, @NewValue)", con))
                        {
                            cmd.Parameters.AddWithValue("@NewKey", dr["Key"]);
                            cmd.Parameters.AddWithValue("@NewValue", dr["Value"]);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                con.Close();
            }
        }
    }
}
