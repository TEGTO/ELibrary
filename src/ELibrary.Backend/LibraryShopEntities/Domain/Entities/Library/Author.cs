using System.ComponentModel.DataAnnotations;

namespace LibraryShopEntities.Domain.Entities.Library
{
    public class Author : BaseLibraryEntity
    {
        [Required]
        [MaxLength(256)]
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }

        public override void Copy(BaseLibraryEntity other)
        {
            if (other is Author otherAuthor)
            {
                Name = otherAuthor.Name;
                LastName = otherAuthor.LastName;
                DateOfBirth = otherAuthor.DateOfBirth;
            }
        }
    }
}