namespace PMDSSystems.Models
{
    public class PDPJobRequirement
    {
        public int Id { get; set; }

        public int PDPModelId { get; set; }

        public string? Task { get; set; }

        public string? Training { get; set; }

        public string? LearningType { get; set; }

        public string? NQFLevel { get; set; }

        public string? Cost { get; set; }

        public string? Impact { get; set; }

        public PDPModel? PDPModel { get; set; }
    }
}