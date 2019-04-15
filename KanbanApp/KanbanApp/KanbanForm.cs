using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace KanbanApp
{
    public partial class KanbanForm : Form
    {
        private string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        private DataTable kanbanDT;

        public KanbanForm()
        {
            InitializeComponent();
            UpdateKanbanInfo();
        }

        // FUNCTION : UpdateKanbanTable
        // DESCRIPTION : Redraws Kanban table
        // PARAMETERS : no params
        // RETURNS : nothing
        private void UpdateKanbanTable()
        {
            kanbanTable.Controls.Clear();
            kanbanTable.RowStyles.Clear();
            int rowCount = 1;
            kanbanTable.RowCount = rowCount;
            kanbanTable.Controls.Add(new Label() { Text = "Workstation #" }, 1, 0);
            kanbanTable.Controls.Add(new Label() { Text = "Employee type" }, 2, 0);
            kanbanTable.Controls.Add(new Label() { Text = "Completed trays" }, 3, 0);
            kanbanTable.Controls.Add(new Label() { Text = "Completed lamps" }, 4, 0);
            kanbanTable.Controls.Add(new Label() { Text = "Tested lamps" }, 5, 0);
            kanbanTable.Controls.Add(new Label() { Text = "Defect rate" }, 6, 0);
            for(int i = 0; i < kanbanDT.Rows.Count; i++)
            {
                rowCount++;
                kanbanTable.RowCount = rowCount;
                kanbanTable.Controls.Add(new Label() { Text = kanbanDT.Rows[i].Field<int>("WorkstationId").ToString() }, 1, rowCount - 1);
                kanbanTable.Controls.Add(new Label() { Text = kanbanDT.Rows[i].Field<string>("EmployeeType") }, 2, rowCount - 1);
                kanbanTable.Controls.Add(new Label() { Text = kanbanDT.Rows[i].Field<int>("CompletedTrays").ToString() }, 3, rowCount - 1);
                kanbanTable.Controls.Add(new Label() { Text = kanbanDT.Rows[i].Field<int>("CompletedLamps").ToString() }, 4, rowCount - 1);
                kanbanTable.Controls.Add(new Label() { Text = kanbanDT.Rows[i].Field<int>("TestedLamps").ToString() }, 5, rowCount - 1);
                kanbanTable.Controls.Add(new Label() { Text = kanbanDT.Rows[i].Field<object>("DefectRate").ToString() }, 6, rowCount - 1);
            }

        }

        // FUNCTION : UpdateKanbanInfo
        // DESCRIPTION : Executes stored procedure to update stats in DB and gets fresh data
        // PARAMETERS : no params
        // RETURNS : nothing
        private void UpdateKanbanInfo()
        {
            ExecuteStoredProcedure("sp_updateKanbanInfo");
            string query = "SELECT WorkstationId, EmployeeType, CompletedTrays, CompletedLamps, TestedLamps, DefectRate FROM KanbanInfo WITH (NOLOCK)";
            kanbanDT = GetDataTable(query);
        }

        // FUNCTION : GetDataTable
        // DESCRIPTION : Loads contents into DataTable from database.
        // PARAMETERS : string query - select query
        // RETURNS : query execution result DataTable
        private DataTable GetDataTable(string query)
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

        // FUNCTION : ExecuteStoredProcedure
        // DESCRIPTION : Executes SQL stored procedure
        // PARAMETERS : string spName - stored procedute name
        //              List<KeyValuePair<string, string>> spParams - optional stored procedure params
        // RETURNS : nothing
        private void ExecuteStoredProcedure(string spName, List<KeyValuePair<string, string>> spParams = null)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (spParams != null)
                    {
                        foreach (var param in spParams)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        // FUNCTION : Timer_Tick
        // DESCRIPTION : Event handler for every timer tick
        // PARAMETERS : object sender - sender
        //              EventArgs e - args
        // RETURNS : nothng
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateKanbanInfo();
            UpdateKanbanTable();
        }

        // FUNCTION : InitTimer
        // DESCRIPTION : Starts refresh application data timer
        // PARAMETERS : no params
        // RETURNS : nothng
        private void InitTimer()
        {
            Timer timer1 = new Timer();
            timer1.Tick += new EventHandler(Timer_Tick);
            timer1.Interval = 2000; // in miliseconds
            timer1.Start();
        }

        // FUNCTION : KanbanForm_Load
        // DESCRIPTION : Initializes timer
        // PARAMETERS : object sender - sender
        //              EventArgs e - args
        // RETURNS : nothng
        private void KanbanForm_Load(object sender, EventArgs e)
        {
            InitTimer();
        }
    }
}
