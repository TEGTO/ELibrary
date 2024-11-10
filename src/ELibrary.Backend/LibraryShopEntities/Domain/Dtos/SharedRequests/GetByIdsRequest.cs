namespace LibraryShopEntities.Domain.Dtos.SharedRequests
{
    public class GetByIdsRequest
    {
        public List<int> Ids { get; set; }

        public override string ToString()
        {
            return string.Join(", ", Ids);
        }
    }
}
