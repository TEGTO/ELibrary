export const environment =
{
    production: false,
    ecryptionSecretKey: 'encryption-secret-key',
    userApi: '/api',
    libraryApi: '/api',
    shopApi: '/api',
    maxOrderAmount: 99,
    bookCoverPlaceholder: '../../../../../assets/book_cover_placeholder.png',
    botProfilePicture: '../../../../../assets/bot_avatar.png',
    bookIcon: '../../../../../assets/open_book.png',
    botChatDebouncingTimeInMilliseconds: 3000,
    minOrderTime: new Date(2023, 11, 1, 6, 30),
    maxOrderTime: new Date(2023, 11, 1, 21, 0),
};
