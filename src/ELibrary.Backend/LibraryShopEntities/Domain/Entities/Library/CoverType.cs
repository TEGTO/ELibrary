namespace LibraryShopEntities.Domain.Entities.Library
{
    public class CoverType : BaseLibraryEntity
    {
        public override void Copy(BaseLibraryEntity other)
        {
            if (other is CoverType otherCoverType)
            {
                Name = otherCoverType.Name;
            }
        }
    }
}
