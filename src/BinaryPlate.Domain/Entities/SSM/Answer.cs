using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryPlate.Domain.Entities.SSM
{
    public class Answer : IAuditable, ISoftDeletable, IMayHaveTenant, IConcurrencyStamp
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        #region Inherited Properties
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? TenantId { get; set; }
        public string ConcurrencyStamp { get; set; }
        #endregion

    }
}
