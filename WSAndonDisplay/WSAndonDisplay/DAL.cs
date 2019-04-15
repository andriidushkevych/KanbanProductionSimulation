// File: DAL.cs
// Project: Final Project - Advanced SQL
// Programmers: Philip Kempton & Andrii Dushkevych
// First Version: 2019-04-11
// Description: This file contains a class that represents a data access layer for this app.
//              This DAL connects to a database using a connection string in App.Config and 
//              allows for the searching of all WS IDs and the state of a WS.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WSAndonDisplay
{
    public class DAL
    {
        private string connString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

        /// <summary>
        /// This method returns an array of workstation IDs
        /// </summary>
        /// <returns>workstation IDs</returns>
        public List<int> FindWorkstationIDs()
        {
            DataTable dt = GetDataTable("SELECT Id FROM Workstation");
            List<int> ints = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                ints.Add(row.Field<int>("Id"));
            }
            return ints;
        }


        /// <summary>
        /// This method gets and return the current state of a Workstation
        /// </summary>
        /// <param name="wsID">The workstation whose state to find</param>
        /// <returns>The state of the workstation</returns>
        public WorkstationState GetWorkstationState(int wsID)
        {
            // create a WS state to return
            WorkstationState state = new WorkstationState();

            // get states of all properties of the WorkstationState
            state.EmpType = GetEmpType(wsID);
            state.TrayID = GetTrayNum(wsID);
            state.TrayVolume = GetTrayVol(wsID);
            state.CompletedParts = GetCompletedParts(wsID); ;
            state.DefectParts = GetBadParts(wsID);
            state.GoodParts = GetGoodParts(wsID);
            state.BezelVolume = GetBezelVol(wsID);
            state.BulbVolume = GetBulbVol(wsID);
            state.HarnessVolume = GetHarnessVol(wsID);
            state.HousingVolume = GetHousingVol(wsID);
            state.LensVolume = GetLensVol(wsID);
            state.ReflectorVolume = GetReflectorVol(wsID);
            state.RunnerIsOut = IsRunnerOut(wsID);

            return state;
        }

        /// <summary>
        /// This method gets the current amployee type of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>employee type</returns>
        private string GetEmpType(int wsID)
        {
            string empTypeQuery = "SELECT EmployeeType FROM KanbanInfo WITH (NOLOCK) WHERE WorkstationId = " + wsID;
            DataTable dt = GetDataTable(empTypeQuery);
            return dt.Rows[0].Field<string>("EmployeeType");
        }


        /// <summary>
        /// This method get the current tray number of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>trayid</returns>
        private int GetTrayNum(int wsID)
        {
            string trayIdQuery = "SELECT COUNT(Id) AS TrayNum FROM Tray WITH (NOLOCK) WHERE WorkstationId = " + wsID;
            DataTable dt = GetDataTable(trayIdQuery);
            return dt.Rows[0].Field<int>("TrayNum");
        }


        /// <summary>
        /// This method gets the current tray volume of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>tray amount</returns>
        private string GetTrayVol(int wsID)
        {
            string trayVolQuery = "SELECT Number FROM Tray WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND IsFull = 0";
            DataTable dt = GetDataTable(trayVolQuery);
            int trayVol = dt.Rows[0].Field<int>("Number");

            string trayMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 15";
            dt = GetDataTable(trayMaxQuery);
            int trayMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));

            return trayVol + "/" + trayMax;
        }


        /// <summary>
        /// This method gets the current number of good parts of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>good part count</returns>
        private int GetGoodParts(int wsID)
        {
            string goodPartsQuery = "SELECT COUNT(Id) AS TotalGoodParts FROM lamp WITH (NOLOCK) WHERE TestPassed = 1 AND TrayId IN (SELECT Id FROM Tray WITH (NOLOCK) WHERE WorkstationId = " + wsID + ");";
            DataTable dt = GetDataTable(goodPartsQuery);
            return dt.Rows[0].Field<int>("TotalGoodParts");
        }


        /// <summary>
        /// This method gets the current number of bad parts of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>bad part count</returns>
        private int GetBadParts(int wsID)
        {
            string badPartsQuery = "SELECT COUNT(Id) AS TotalBadParts FROM lamp WITH (NOLOCK) WHERE TestPassed = 0 AND TrayId IN (SELECT Id FROM Tray WITH (NOLOCK) WHERE WorkstationId = " + wsID + ");";
            DataTable dt = GetDataTable(badPartsQuery);
            return dt.Rows[0].Field<int>("TotalBadParts");
        }


        /// <summary>
        /// This method gets the current number of completed parts
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>completed part count</returns>
        private int GetCompletedParts(int wsID)
        {
            string completedPartsQuery = "SELECT COUNT(Id) AS CompletedLamps FROM lamp WITH (NOLOCK) WHERE TrayId IN (SELECT Id FROM Tray WITH (NOLOCK) WHERE WorkstationId = " + wsID + ");";
            DataTable dt = GetDataTable(completedPartsQuery);
            return dt.Rows[0].Field<int>("CompletedLamps");
        }


        /// <summary>
        /// This method gets the amount of bezels of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>bezel amount</returns>
        private string GetBezelVol(int wsID)
        {
            string bezelVolQuery = "SELECT PartCount FROM Bin WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND PartId = 6";
            DataTable dt = GetDataTable(bezelVolQuery);
            int bezelVol = dt.Rows[0].Field<int>("PartCount");

            string bezelMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 6";
            dt = GetDataTable(bezelMaxQuery);
            int bezelMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));
            return bezelVol + "/" + bezelMax;
        }


        /// <summary>
        /// This method gets the amount of bulbs of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>bulb amount</returns>
        private string GetBulbVol(int wsID)
        {
            string bulbVolQuery = "SELECT PartCount FROM Bin WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND PartId = 5";
            DataTable dt = GetDataTable(bulbVolQuery);
            int bulbVol = dt.Rows[0].Field<int>("PartCount");

            string bulbMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 5";
            dt = GetDataTable(bulbMaxQuery);
            int bulbMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));
            return bulbVol + "/" + bulbMax;
        }


        /// <summary>
        /// This method gets the amount of harnesses of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>harness amount</returns>
        private string GetHarnessVol(int wsID)
        {
            string harnessVolQuery = "SELECT PartCount FROM Bin WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND PartId = 1";
            DataTable dt = GetDataTable(harnessVolQuery);
            int harnessVol = dt.Rows[0].Field<int>("PartCount");

            string harnessMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 1";
            dt = GetDataTable(harnessMaxQuery);
            int harnessMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));
            return harnessVol + "/" + harnessMax;
        }


        /// <summary>
        /// This method gets the amount of housings of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>housing amount</returns>
        private string GetHousingVol(int wsID)
        {
            string housingVolQuery = "SELECT PartCount FROM Bin WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND PartId = 3";
            DataTable dt = GetDataTable(housingVolQuery);
            int housingVol = dt.Rows[0].Field<int>("PartCount");

            string housingMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 3";
            dt = GetDataTable(housingMaxQuery);
            int housingMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));
            return housingVol + "/" + housingMax;
        }


        /// <summary>
        /// This method gets the amount of lenses of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>lens amount</returns>
        private string GetLensVol(int wsID)
        {
            string lensVolQuery = "SELECT PartCount FROM Bin WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND PartId = 4";
            DataTable dt = GetDataTable(lensVolQuery);
            int lensVol = dt.Rows[0].Field<int>("PartCount");

            string lensMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 4";
            dt = GetDataTable(lensMaxQuery);
            int lensMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));
            return lensVol + "/" + lensMax;
        }


        /// <summary>
        /// This method gets the amount of reflectors of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>reflector amount</returns>
        private string GetReflectorVol(int wsID)
        { 
            string reflectorVolQuery = "SELECT PartCount FROM Bin WITH (NOLOCK) WHERE WorkstationId = " + wsID + " AND PartId = 2";
            DataTable dt = GetDataTable(reflectorVolQuery);
            int reflectorVol = dt.Rows[0].Field<int>("PartCount");

            string reflectorMaxQuery = "SELECT Value FROM ConfigInfo WITH (NOLOCK) WHERE Id = 2";
            dt = GetDataTable(reflectorMaxQuery);
            int reflectorMax = Int32.Parse(dt.Rows[0].Field<string>("Value"));
            return reflectorVol + "/" + reflectorMax;
        }


        /// <summary>
        /// This mkethod gets the status of a runner of a WS
        /// </summary>
        /// <param name="wsID">The workstationId</param>
        /// <returns>true if running is out, false otherwise</returns>
        private bool IsRunnerOut(int wsID)
        {
            string runnerQuery = "SELECT isRunning FROM Runner WITH (NOLOCK) WHERE WorkstationId = " + wsID;
            DataTable dt = GetDataTable(runnerQuery);
            bool IsOut = dt.Rows[0].Field<bool>("isRunning");
            return IsOut;
        }


        /// <summary>
        /// This method gets a datatable based on a query string
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>the datatable</returns>
        private DataTable GetDataTable(string query)
        {
            using (SqlConnection con = new SqlConnection(connString))
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