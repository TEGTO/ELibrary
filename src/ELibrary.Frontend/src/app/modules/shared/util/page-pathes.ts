export const pathes =
{
    manager: 'manager',
    manager_books: 'books',
    manager_genres: 'genres',
    manager_authors: 'authors',
    manager_publishers: 'publishers',

    client: "",
    client_products: "",
    client_productInfo: ":id",
    client_order: "order",
    client_order_makeOrder: "make",
    client_order_addInformation: "add-contact-info",
};

export const redirectPathes =
{
    manager_books: `${pathes.manager}/${pathes.manager_books}`,
    manager_genres: `${pathes.manager}/${pathes.manager_genres}`,
    managerAuthors: `${pathes.manager}/${pathes.manager_authors}`,
    managerPublishers: `${pathes.manager}/${pathes.manager_publishers}`,

    client_products: `${pathes.client}/${pathes.client_products}`,
    client_productInfo: `${pathes.client}`,
    client_makeOrder: `${pathes.client}/${pathes.client_order}/${pathes.client_order_makeOrder}`,
    client_addInformation: `${pathes.client}/${pathes.client_order}/${pathes.client_order_addInformation}`,
};