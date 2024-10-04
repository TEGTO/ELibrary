export const pathes =
{
    manager: 'manager',
    manager_bookstock: 'bookstock',
    manager_bookstock_details: 'bookstock/:id',
    manager_books: 'books',
    manager_genres: 'genres',
    manager_authors: 'authors',
    manager_publishers: 'publishers',
    manager_orders: 'orders',
    manager_orders_details: 'orders/:id',
    manager_statistics: 'statistics',

    client: "",
    client_products: "",
    client_productInfo: ":id",
    client_order: "order",
    client_order_makeOrder: "make",
    client_order_addInformation: "add-contact-info",
    client_order_history: "history",
};

export const redirectPathes =
{
    manager_bookstock: `${pathes.manager}/${pathes.manager_bookstock}`,
    manager_books: `${pathes.manager}/${pathes.manager_books}`,
    manager_genres: `${pathes.manager}/${pathes.manager_genres}`,
    manager_authors: `${pathes.manager}/${pathes.manager_authors}`,
    manager_publishers: `${pathes.manager}/${pathes.manager_publishers}`,
    manager_orders: `${pathes.manager}/${pathes.manager_orders}`,

    client_products: `${pathes.client}/${pathes.client_products}`,
    client_productInfo: `${pathes.client}`,
    client_makeOrder: `${pathes.client}/${pathes.client_order}/${pathes.client_order_makeOrder}`,
    client_addInformation: `${pathes.client}/${pathes.client_order}/${pathes.client_order_addInformation}`,
    client_order_history: `${pathes.client}/${pathes.client_order}/${pathes.client_order_history}`
};