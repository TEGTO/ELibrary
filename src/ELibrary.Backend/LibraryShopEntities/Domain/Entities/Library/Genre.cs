namespace LibraryShopEntities.Domain.Entities.Library
{
    public class Genre : BaseLibraryEntity
    {
        public override void Copy(BaseLibraryEntity other)
        {
            if (other is Genre otherGenre)
            {
                Name = otherGenre.Name;
            }
        }
    }
}