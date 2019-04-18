/*
* FILE : SimulationTool.cs
* PROJECT : PROG3070 - Project Milestone 02
* PROGRAMMER : Andrii Dushkevych, Phil Kempton
* FIRST VERSION : 2019-04-02
* * DESCRIPTION :
* This file contains logic for SimulationTool WinForms based app for Advanced SQL Project
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using SimulationTool.Models;
using SimulationTool.Helper;
using System.Threading;

namespace SimulationTool
{
    public partial class SimulationTool : Form
    {
        private Thread lampMakeThread, runnerThread;
        private string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        private DataTable employeeTypes, confItems;
        private int WSId, EmpId, EmpTypeId, RunnerId;
        private List<Bin> bins;
        private int binCriticalAmount;
        private bool runnerAvailable = true;

        // FUNCTION : SimulationTool
        // DESCRIPTION : Constructor for this class.
        // PARAMETERS : object sender - event sender
        //              EventArgs e - event args
        // RETURNS : nothing
        public SimulationTool()
        {
            InitializeComponent();
            FillEmployeeTypesDT();
            FillConfItemsDT();
            binCriticalAmount = Convert.ToInt32(confItems.Rows[6].Field<string>("Value"));
        }

        // FUNCTION : createWorkstationButton_Click
        // DESCRIPTION : Creates employee and workstation
        // PARAMETERS : object sender - event sender
        //              EventArgs e - event args
        // RETURNS : nothing
        private void createWorkstationButton_Click(object sender, EventArgs e)
        {
            EmpTypeId = empTypeComboBox.SelectedIndex + 1;
            string createEmpQuery = "INSERT INTO [Employee](TypeId) VALUES(" + EmpTypeId + ") SELECT SCOPE_IDENTITY()";
            EmpId = ExecuteScalarQuery(createEmpQuery);
            string createWSQuery = "INSERT INTO [Workstation](EmployeeId) VALUES(" + EmpId + ") SELECT SCOPE_IDENTITY()";
            WSId = ExecuteScalarQuery(createWSQuery);
            string createRunnerQuery = "INSERT INTO [Runner](WorkstationId, isRunning) VALUES(" + WSId + ", 0) SELECT SCOPE_IDENTITY()";
            RunnerId = ExecuteScalarQuery(createRunnerQuery);
            CreateBins();
            bins = GetBins();                
            empTypeComboBox.Enabled = false;
            createWorkstationButton.Enabled = false;
            startLampsButton.Enabled = true;
        }

        // FUNCTION : CreateBins
        // DESCRIPTION : Creates bins for current workstation
        // PARAMETERS : no params
        // RETURNS : nothing
        private void CreateBins()
        {
            int partCount, partId;
            string insertBinsQuery = "INSERT INTO Bin(PartId, WorkstationId, PartCount, AmountConfItemId) VALUES";
            for (int i = 0; i < 6; i++)
            {
                partCount = Convert.ToInt32(confItems.Rows[i].Field<string>("Value"));
                partId = i + 1;
                if (i == 5)
                {
                    insertBinsQuery += "(" + partId + ", " + WSId + ", " + partCount + ", " + partId + ")";
                }
                else
                {
                    insertBinsQuery += "(" + partId + ", " + WSId + ", " + partCount + ", " + partId + "),";
                }
            }
            ExecuteNonQuery(insertBinsQuery);
        }

        // FUNCTION : GetBins
        // DESCRIPTION : Gets current bins state from database
        // PARAMETERS : no params
        // RETURNS : List<Bin> list of bins for current workstation
        private List<Bin> GetBins()
        {            
            string selectBins = "SELECT Id, PartId, WorkstationId, PartCount, AmountConfItemId FROM Bin WHERE WorkstationId = " + WSId;
            DataTable binsDT = GetDataTable(selectBins);
            return binsDT.DataTableToList<Bin>();
        }

        // FUNCTION : FillEmployeeTypesDT
        // DESCRIPTION : Gets employee types from database
        // PARAMETERS : no params
        // RETURNS : nothing
        private void FillEmployeeTypesDT()
        {
            employeeTypes = GetDataTable("SELECT [Id], [Name] FROM [EmployeeType]");
            empTypeComboBox.DataSource = employeeTypes;
            empTypeComboBox.DisplayMember = "Name";
        }

        // FUNCTION : FillConfItemsDT
        // DESCRIPTION : Gets configuration items from database
        // PARAMETERS : no params
        // RETURNS : nothing
        private void FillConfItemsDT()
        {
            confItems = GetDataTable("SELECT [Key], [Value] FROM ConfigInfo");
        }

        // FUNCTION : ExecuteScalarQuery
        // DESCRIPTION : Executes SQL scalar query
        // PARAMETERS : string query - SQL query
        // RETURNS : int inserted row id
        private int ExecuteScalarQuery(string query)
        {
            int resId = 0;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    resId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                conn.Close();
            }
            return resId;
        }

        // FUNCTION : startLampsButton_Click
        // DESCRIPTION : Starts main loop for lamp creation process in a thread
        // PARAMETERS : object sender - event sender
        //              EventArgs e - event args
        // RETURNS : nothing
        private void startLampsButton_Click(object sender, EventArgs e)
        {
            startLampsButton.Enabled = false;
            lampMakeThread = new Thread(MakeLamps);
            lampMakeThread.Start();         
        }

        // FUNCTION : MakeLamps
        // DESCRIPTION : Runs main loop for lamp creation process
        // PARAMETERS : no params
        // RETURNS : nothing
        private void MakeLamps()
        {
            int workerTimeMS = GetWorkerTimeMS();
            List<int> binIdsToRefil = new List<int>();
            List<KeyValuePair<string, string>> spParams = new List<KeyValuePair<string, string>>();
            spParams.Add(new KeyValuePair<string, string>("@StationId", WSId.ToString()));

            while (true)
            {
                foreach (var bin in bins)
                {
                    if (bin.PartCount <= binCriticalAmount && runnerAvailable)
                    {
                        binIdsToRefil.Add(bin.Id);
                    }
                }
                if (binIdsToRefil.Count != 0)
                {
                    runnerThread = new Thread(() =>
                    {
                        // wait while runner gets new bins (5 minutes simulated time)
                        Thread.Sleep(5000);
                        
                        foreach (var binId in binIdsToRefil)
                        {
                            List<KeyValuePair<string, string>> binSpParams = new List<KeyValuePair<string, string>>();
                            binSpParams.Add(new KeyValuePair<string, string>("@BinId", binId.ToString()));
                            ExecuteStoredProcedure("refill_bin", binSpParams);
                        }
                        runnerAvailable = true;
                        UpdateRunner(false);
                        binIdsToRefil.Clear();

                    });
                    if (runnerAvailable)
                    {
                        UpdateRunner(true);
                        runnerAvailable = false;
                        runnerThread.Start();
                    }

                }

                ExecuteStoredProcedure("start_new_lamp", spParams);
                
                // wait
                // finish lamp
                Thread.Sleep(workerTimeMS);
                ExecuteStoredProcedure("finish_lamp", spParams);
                bins = GetBins();
            }
        }

        // FUNCTION : UpdateRunner
        // DESCRIPTION : Updates runner state in DB
        // PARAMETERS : bool busyFlag - runner state flag
        // RETURNS : nothing
        private void UpdateRunner(bool busyFlag)
        {
            int busyBit = 0;
            if(busyFlag)
            {
                busyBit = 1;
            }
            string updateRunnerQuery = "UPDATE Runner SET isRunning = " + busyBit + " WHERE Id = " + RunnerId;
            ExecuteNonQuery(updateRunnerQuery);
        }

        // FUNCTION : stopWSbutton_Click
        // DESCRIPTION : Stops running workstation and closes app
        // PARAMETERS : object sender - event sender
        //              EventArgs e - event args
        // RETURNS : nothing
        private void stopWSbutton_Click(object sender, EventArgs e)
        {
            if (runnerThread.IsAlive)
                runnerThread.Suspend();
            if (lampMakeThread.IsAlive)
                lampMakeThread.Suspend();
            Application.Exit();
        }


        // FUNCTION : GetWorkerTimeMS
        // DESCRIPTION : Calculates time in milliseconds for worker to make one lamp
        // PARAMETERS : no params
        // RETURNS : int time in milliseconds
        private int GetWorkerTimeMS()
        {
            int percentage = 100;
            int calculatedTime;
            int baseTime = Convert.ToInt32(confItems.Rows[7].Field<string>("Value"));
            int timeFactor = Convert.ToInt32(confItems.Rows[17].Field<string>("Value"));
            switch (EmpTypeId)
            {
                //experienced
                case (1):
                    percentage = Convert.ToInt32(confItems.Rows[8].Field<string>("Value"));                    
                    break;
                //rookie
                case (2):
                    percentage = Convert.ToInt32(confItems.Rows[9].Field<string>("Value"));
                    break;
                //super
                case (3):
                    percentage = Convert.ToInt32(confItems.Rows[10].Field<string>("Value"));
                    break;
            }
            calculatedTime = (1000 * (baseTime + (baseTime * percentage / 100)) / timeFactor);
            return calculatedTime;
        }


        // FUNCTION : ExecuteNonQuery
        // DESCRIPTION : Executes SQL query
        // PARAMETERS : string query - SQL query
        // RETURNS : nothing
        private void ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        // FUNCTION : ExecuteStoredProcedure
        // DESCRIPTION : Executes SQL stored procedure
        // PARAMETERS : string spName - stored procedute name
        //              List<KeyValuePair<string, string>> spParams - optional stored procedure params
        // RETURNS : nothing
        private void ExecuteStoredProcedure(string spName, List<KeyValuePair<string, string>> spParams = null)
        {
            using (SqlConnection conn= new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (spParams != null)
                    {
                        foreach(var param in spParams)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
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
    }
}
