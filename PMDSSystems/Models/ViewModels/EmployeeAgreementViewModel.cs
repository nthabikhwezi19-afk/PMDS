
using PMDSSystems.Models;

namespace PMDSSystems.ViewModels
{
    public class EmployeeAgreementViewModel
    {
        public Employee Employee { get; set; } = new();

        public PMDSForm? PartA { get; set; }

        public PerformanceAgreement? Agreement { get; set; }

        public PDPModel? PDP { get; set; }
    }
}
