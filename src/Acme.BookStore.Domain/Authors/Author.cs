using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Authors
{
    public class Author : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public DateTime DoB { get; set; }
        public string ShortBio { get; set; }

        public Status Status { get; set; }
        public Author()
        {

        }

        internal Author(Guid id, [NotNull] string name, DateTime dob, [CanBeNull] string shortBio = null): base(id)
        {
            SetName(name);
            DoB = dob;
            ShortBio = shortBio;
        }

        public Author ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AuthorConsts.MaxNameLength
            );
        }
    }
}
