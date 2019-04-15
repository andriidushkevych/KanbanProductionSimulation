// File: Form1.cs
// Project: Final Project - Advanced SQL
// Programmers: Philip Kempton & Andrii Dushkevych
// First Version: 2019-04-11
// Description: This file contains a class that represents the main window of this app.
//              This window acts as an andon display for a selectable workstation.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WSAndonDisplay
{
    public partial class Andon : Form
    {
        private int wsID = 1;
        /// <summary>
        /// Base contructor for the main form
        /// This constructor populates the list of workstations to select from
        /// </summary>
        public Andon()
        {
            InitializeComponent();
            PopulateWSList();
        }


        /// <summary>
        /// This method populates the list of workstations to select from
        /// </summary>
        private void PopulateWSList()
        {
            List<int> wsIDs = new DAL().FindWorkstationIDs();
            WorkstationList.DataSource = wsIDs;
        }


        /// <summary>
        /// This method up[dates the selected Wokrstation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectWSButton_Click(object sender, EventArgs e)
        {
            Int32.TryParse(WorkstationList.SelectedValue.ToString(), out wsID); 
        }


        /// <summary>
        /// This method populates the andon display with the current state of the WS
        /// </summary>
        /// <param name="state"></param>
        private void PopulateAndonDisplay(WorkstationState state)
        {
            // Populate all fields of andon display with current state of WS
            EmployeeTypeLabel.Text = state.EmpType;
            TrayIDLabel.Text = state.TrayID.ToString();
            TrayAmountLabel.Text = state.TrayVolume;
            TotalPartsLabel.Text = state.CompletedParts.ToString();
            DefectPartsLabel.Text = state.DefectParts.ToString();
            GoodPartsLabel.Text = state.GoodParts.ToString();
            BezelAmountLabel.Text = state.BezelVolume;
            BulbAmountLabel.Text = state.BulbVolume;
            HarnessAmountLabel.Text = state.HarnessVolume;
            LensAmountLabel.Text = state.LensVolume;
            HousingAmountLabel.Text = state.HousingVolume;
            ReflectorAmountLabel.Text = state.ReflectorVolume;
            if (state.RunnerIsOut)
            {
                RunnerLabel.Text = "Yes";
            }
            else
            {
                RunnerLabel.Text = "No";
            }
        }

        // FUNCTION : Timer_Tick
        // DESCRIPTION : Event handler for every timer tick
        // PARAMETERS : object sender - sender
        //              EventArgs e - args
        // RETURNS : nothng
        private void Timer_Tick(object sender, EventArgs e)
        {
            // get current state of the workstation
            WorkstationState state = new DAL().GetWorkstationState(wsID);
            PopulateAndonDisplay(state);
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

        // FUNCTION : Form1_Load
        // DESCRIPTION : Initializes timer
        // PARAMETERS : object sender - sender
        //              EventArgs e - args
        // RETURNS : nothng

        private void Andon_Load(object sender, EventArgs e)
        {
            InitTimer();
        }
    }
}
