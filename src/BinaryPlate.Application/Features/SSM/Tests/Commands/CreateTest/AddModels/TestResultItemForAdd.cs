using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest.AddModels
{
    public class TestResultItemForAdd
    {
        public int EmployeeId { get; set; }
        public int TestId { get; set; }
        public DateTime DateTaken { get; set; }
        public decimal Score { get; set; }
    }
}
