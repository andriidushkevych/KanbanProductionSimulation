/*
* FILE : Bin.cs
* PROJECT : PROG3070 - Project Milestone 02
* PROGRAMMER : Andrii Dushkevych, Phil Kempton
* FIRST VERSION : 2019-04-02
* * DESCRIPTION :
* This file contains a model class representing a bin
*/

namespace SimulationTool.Models
{
    public class Bin
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public int WorkstationId { get; set; }
        public int PartCount { get; set; }
        public int AmountConfItemId { get; set; }
    }
}
