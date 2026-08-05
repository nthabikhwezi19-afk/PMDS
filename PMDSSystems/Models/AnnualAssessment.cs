using System;
using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class AnnualAssessment
    {
        public int Id { get; set; }

        public string? PersalNumber { get; set; }

        // ✅ PART D1
        public int KRA1Weight { get; set; }
        public int KRA2Weight { get; set; }
        public int KRA3Weight { get; set; }
        public int KRA4Weight { get; set; }

        public string? KRA1Achievement { get; set; }
        public string? KRA2Achievement { get; set; }
        public string? KRA3Achievement { get; set; }
        public string? KRA4Achievement { get; set; }

        public string? KRA1Comment { get; set; }
        public string? KRA2Comment { get; set; }
        public string? KRA3Comment { get; set; }
        public string? KRA4Comment { get; set; }

        // ✅ PART D2
        public int KRA1_OR { get; set; }
        public int KRA1_SR { get; set; }
        public int KRA1_AR { get; set; }

        public int KRA2_OR { get; set; }
        public int KRA2_SR { get; set; }
        public int KRA2_AR { get; set; }

        public int KRA3_OR { get; set; }
        public int KRA3_SR { get; set; }
        public int KRA3_AR { get; set; }

        public int KRA4_OR { get; set; }
        public int KRA4_SR { get; set; }
        public int KRA4_AR { get; set; }

        // ✅ Dispute
        public bool HasDispute { get; set; }
        public string? DisputeKRA { get; set; }

        // ✅ Signatures
        public string? EmployeeSignature { get; set; }
        public string? SupervisorSignature { get; set; }
        public DateTime? DateSigned { get; set; }

        public string ModerationCategory { get; set; }
        public string ModerationPercentage { get; set; }
        public string ChairpersonSignature { get; set; }

        public string FinalModerationPercentage { get; set; }
        public string FinalModerationCategory { get; set; }

        public string ChairpersonName { get; set; }
        public DateTime? ModerationDate { get; set; }
        public string CommitteeMembers { get; set; }
    }
}