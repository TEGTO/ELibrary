namespace ShopApi.Features.AdvisorFeature.Domain.Dtos
{
    public class AdvisorQueryRequest
    {
        public string Query { get; set; }

        public override string ToString()
        {
            return Query;
        }
    }
}
