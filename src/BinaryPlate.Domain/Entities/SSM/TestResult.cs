using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryPlate.Domain.Entities.SSM
{
    public class TestResult : IAuditable, ISoftDeletable, IMayHaveTenant, IConcurrencyStamp
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int TestId { get; set; }
        public DateTime DateTaken { get; set; }
        public decimal Score { get; set; }
        public Employee Employee { get; set; }
        public Test Test { get; set; }

        #region Inherited Properties
        public string ConcurrencyStamp { get; set; }
        public Guid? TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        #endregion Inherited Properties
    }
}
