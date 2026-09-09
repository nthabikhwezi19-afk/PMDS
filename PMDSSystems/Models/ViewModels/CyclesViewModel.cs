using PMDSSystems.Models;
using System.Collections.Generic;

namespace PMDSSystems.ViewModels
{
    public class CyclesViewModel
    {
        public Employee Employee { get; set; }

        public List<Employee> MyEmployees { get; set; } = new List<Employee>();
    }
}