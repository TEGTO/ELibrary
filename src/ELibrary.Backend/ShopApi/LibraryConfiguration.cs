namespace ShopApi
{
    public static class LibraryConfiguration
    {
        public static string LIBRARY_API_GET_BOOKS_BY_IDS_ENDPOINT { get; } = "book/ids";
        public static string LIBRARY_API_RAISE_BOOK_POPULARITY_ENDPOINT { get; } = "book/popularity";
        public static string LIBRARY_API_UPDATE_STOCK_AMOUNT_ENDPOINT { get; } = "book/stockamount";
    }
}
