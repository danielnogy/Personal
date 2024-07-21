using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryPlate.Domain.Entities.SSM
{
    public class Question : IAuditable, ISoftDeletable, IMayHaveTenant, IConcurrencyStamp
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public QuestionCategory Category { get; set; }
        public ICollection<TestQuestion> TestQuestions { get; set; }
        public ICollection<Answer> Answers { get; set; }

        #region Inherited Properties
        public string ConcurrencyStamp { get; set; }
        public Guid? TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        #endregion

    }
}
