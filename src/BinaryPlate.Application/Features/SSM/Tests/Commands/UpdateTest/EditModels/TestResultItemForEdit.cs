using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest.EditModels
{
    public class TestResultItemForEdit
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int TestId { get; set; }
        public DateTime DateTaken { get; set; }
        public decimal Score { get; set; }
    }
}
