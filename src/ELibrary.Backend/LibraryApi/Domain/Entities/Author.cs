using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Domain.Entities
{
    public class Author : BaseEntity
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; } = default!;

        public DateTime DateOfBirth { get; set; }

        public override void Copy(BaseEntity other)
        {
            if (other is Author otherAuthor)
            {
                this.Name = otherAuthor.Name;
                this.LastName = otherAuthor.LastName;
                this.DateOfBirth = otherAuthor.DateOfBirth;
            }
        }
    }
}