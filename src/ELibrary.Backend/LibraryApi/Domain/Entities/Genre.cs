using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Domain.Entities
{
    public class Genre : BaseEntity
    {
        [MaxLength(256)]
        public string Name { get; set; } = default!;

        public override void Copy(BaseEntity other)
        {
            if (other is Genre otherGenre)
            {
                this.Name = otherGenre.Name;
            }
        }
    }
}