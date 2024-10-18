export const environment =
{
  production: true,
  ecryptionSecretKey: "encryption-secret-key",
  userApi: 'https://localhost:7130',
  libraryApi: 'https://localhost:7131',
  shopApi: 'https://localhost:7132',
  maxOrderAmount: 99,
  bookCoverPlaceholder: '../../../../../assets/book_cover_placeholder.png',
  botProfilePicture: '../../../../../assets/bot_avatar.png',
  minOrderTime: new Date(2023, 11, 1, 6, 30),
  maxOrderTime: new Date(2023, 11, 1, 21, 0),
};