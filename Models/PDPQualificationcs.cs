namespace PMDSSystems.Models
{
    public class PDPEducation
    {
        public int Id { get; set; }

        public int PDPModelId { get; set; }

        public string? Qualification { get; set; }

        public string? NQF { get; set; }

        public string? Year { get; set; }

        public PDPModel? PDPModel { get; set; }
    }
}