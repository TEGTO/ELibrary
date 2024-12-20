﻿using LibraryShopEntities.Domain.Dtos.Library;

namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class CartBookResponse
    {
        public string? Id { get; set; }
        public int BookAmount { get; set; }
        public int BookId { get; set; }
        public BookResponse? Book { get; set; }
    }
}
