using System;

namespace PMDSSystems.Models
{
    public class AnnualAssessment
    {
        public int Id { get; set; }

        public string? PersalNumber { get; set; }

        // ============================================================
        // KRA NAMES - AUTO POPULATED FROM PERFORMANCE AGREEMENT
        // ============================================================

        public string? KRA1Name { get; set; }
        public string? KRA2Name { get; set; }
        public string? KRA3Name { get; set; }
        public string? KRA4Name { get; set; }


        // ============================================================
        // PART D1 - MOTIVATION FOR KRAs
        // ============================================================

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


        // ============================================================
        // PART D2 - RATINGS
        // ============================================================

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


        // ============================================================
        // DISPUTE
        // ============================================================

        public bool HasDispute { get; set; }

        public string? DisputeKRA { get; set; }


        // ============================================================
        // SIGNATURES
        // ============================================================

        public string? EmployeeSignature { get; set; }

        public string? SupervisorSignature { get; set; }

        public DateTime? DateSigned { get; set; }


        // ============================================================
        // MODERATION
        // ============================================================

        public string? ModerationCategory { get; set; }

        public string? ModerationPercentage { get; set; }

        public string? ChairpersonSignature { get; set; }

        public string? FinalModerationPercentage { get; set; }

        public string? FinalModerationCategory { get; set; }

        public string? ChairpersonName { get; set; }

        public DateTime? ModerationDate { get; set; }

        public string? CommitteeMembers { get; set; }


        // ============================================================
        // CHAIRPERSON
        // ============================================================

        public string? ChairpersonSurname { get; set; }

        public string? ChairpersonInitials { get; set; }


        // ============================================================
        // COMMITTEE MEMBER 1
        // ============================================================

        public string? CommitteeMember1Surname { get; set; }

        public string? CommitteeMember1Initials { get; set; }

        public string? CommitteeMember1Signature { get; set; }


        // ============================================================
        // COMMITTEE MEMBER 2
        // ============================================================

        public string? CommitteeMember2Surname { get; set; }

        public string? CommitteeMember2Initials { get; set; }

        public string? CommitteeMember2Signature { get; set; }


        // ============================================================
        // COMMITTEE MEMBER 3
        // ============================================================

        public string? CommitteeMember3Surname { get; set; }

        public string? CommitteeMember3Initials { get; set; }

        public string? CommitteeMember3Signature { get; set; }


        // ============================================================
        // COMMITTEE MEMBER 4
        // ============================================================

        public string? CommitteeMember4Surname { get; set; }

        public string? CommitteeMember4Initials { get; set; }

        public string? CommitteeMember4Signature { get; set; }


        // ============================================================
        // COMMITTEE MEMBER 5
        // ============================================================

        public string? CommitteeMember5Surname { get; set; }

        public string? CommitteeMember5Initials { get; set; }

        public string? CommitteeMember5Signature { get; set; }


        // ============================================================
        // HIGHER LINE MANAGER
        // ============================================================

        public string? HigherLineManagerDecision { get; set; }

        public string? HigherLineManagerSignature { get; set; }

        public string? HigherLineManagerSurnameInitials { get; set; }

        public DateTime? HigherLineManagerDate { get; set; }
    }
}