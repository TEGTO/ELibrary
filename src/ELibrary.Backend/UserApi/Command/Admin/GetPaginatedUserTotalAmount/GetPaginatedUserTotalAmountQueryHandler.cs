using MediatR;
using UserApi.Services;

namespace UserApi.Command.Admin.GetPaginatedUserTotalAmount
{
    public class GetPaginatedUserTotalAmountQueryHandler : IRequestHandler<GetPaginatedUserTotalAmountQuery, int>
    {
        private readonly IUserService userService;

        public GetPaginatedUserTotalAmountQueryHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<int> Handle(GetPaginatedUserTotalAmountQuery request, CancellationToken cancellationToken)
        {
            return await userService.GetUserTotalAmountAsync(request.Filter, cancellationToken);
        }
    }
}
