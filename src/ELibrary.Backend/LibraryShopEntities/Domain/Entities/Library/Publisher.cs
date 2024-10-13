namespace LibraryShopEntities.Domain.Entities.Library
{
    public class Publisher : BaseLibraryEntity
    {
        public override void Copy(BaseLibraryEntity other)
        {
            if (other is Publisher otherPublisher)
            {
                Name = otherPublisher.Name;
            }
        }
    }
}
