namespace PMDSSystems.Models
{
    public class PDPModel
    {
        public int Id { get; set; }

        // Employee information
     
        public string? Surname { get; set; }
        public string? PersalNo { get; set; }
        public string? Directorate { get; set; }
        public string? IdNumber { get; set; }
        public string? Branch { get; set; }
        public string? SalaryLevel { get; set; }
        public string? Gender { get; set; }
        public string? Race { get; set; }
        public string? Disabled { get; set; }

        // Supervisor information
        public string? SupervisorPosition { get; set; }
        public string? Supervisor { get; set; }

        // PDP information
        public string? Goal { get; set; }
        public string? ActionPlan { get; set; }

    }
}
