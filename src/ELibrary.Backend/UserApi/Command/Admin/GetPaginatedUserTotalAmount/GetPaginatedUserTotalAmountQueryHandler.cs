using MediatR;
using UserApi.Services;

namespace UserApi.Command.Admin.GetPaginatedUserTotalAmount
{
    public class GetPaginatedUserTotalAmountQueryHandler : IRequestHandler<GetPaginatedUserTotalAmountQuery, int>
    {
        private readonly IAuthService authService;

        public GetPaginatedUserTotalAmountQueryHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<int> Handle(GetPaginatedUserTotalAmountQuery request, CancellationToken cancellationToken)
        {
            return await authService.GetUserTotalAmountAsync(request.Filter, cancellationToken);
        }
    }
}
