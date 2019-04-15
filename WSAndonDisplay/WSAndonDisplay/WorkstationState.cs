// File: WorkstationState.cs
// Project: Final Project - Advanced SQL
// Programmers: Philip Kempton & Andrii Dushkevych
// First Version: 2019-04-11
// Description: This file contains a class that represents a state of a workstation
//              This state includes information relevant to the current progress of the worksation

namespace WSAndonDisplay
{
    public class WorkstationState
    {
        /// <summary>
        /// The employee type of the WS
        /// (example: "Rookie")
        /// </summary>
        public string EmpType { get; set; }

        /// <summary>
        /// The ID of the current tray for this WS
        /// </summary>
        public int TrayID { get; set; }

        /// <summary>
        /// The amount of parts in current tray for this WS
        /// 0/max  (example: "25/60")
        /// </summary>
        public string TrayVolume { get; set; }

        /// <summary>
        /// The amount of parts completed by this WS
        /// </summary>
        public int CompletedParts { get; set; }

        /// <summary>
        /// The amount of parts defected for this WS
        /// </summary>
        public int DefectParts { get; set; }

        /// <summary>
        /// The amount of good parts made by this WS
        /// </summary>
        public int GoodParts { get; set; }

        /// <summary>
        /// The amount of bezels in current bin
        /// 0/max (example: "15/35")
        /// </summary>
        public string BezelVolume { get; set; }

        /// <summary>
        /// The amount of bulbs in current bin
        /// 0/max (example: "15/35")
        /// </summary>
        public string BulbVolume { get; set; }

        /// <summary>
        /// The amount of harnesses in current bin
        /// 0/max (example: "15/35")
        /// </summary>
        public string HarnessVolume { get; set; }

        /// <summary>
        /// The amount of housings in current bin
        /// 0/max (example: "15/35")
        /// </summary>
        public string HousingVolume { get; set; }

        /// <summary>
        /// The amount of lenses in current bin
        /// 0/max (example: "15/35")
        /// </summary>
        public string LensVolume { get; set; }

        /// <summary>
        /// The amount of reflectors in current bin
        /// 0/max (example: "15/35")
        /// </summary>
        public string ReflectorVolume { get; set; }

        /// <summary>
        /// True if runner is currently running, false otherwise
        /// </summary>
        public bool RunnerIsOut { get; set; }
    }
}