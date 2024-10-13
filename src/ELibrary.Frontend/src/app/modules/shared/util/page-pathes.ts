export const pathes =
{
    admin: "admin",
    admin_userTable: "users",
    admin_userPage: "users/:id",

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

export function getManagerBookStockPath() {
    return `${pathes.manager}/${pathes.manager_bookstock}`;
}
export function getManagerBookStockDetailsPath(id: number) {
    return `${pathes.manager}/${pathes.manager_bookstock}/${id}`;
}
export function getManagerBooksPath() {
    return `${pathes.manager}/${pathes.manager_books}`;
}
export function getManagerGenresPath() {
    return `${pathes.manager}/${pathes.manager_genres}`;
}
export function getManagerAuthorsPath() {
    return `${pathes.manager}/${pathes.manager_authors}`;
}
export function getManagerPublishersPath() {
    return `${pathes.manager}/${pathes.manager_publishers}`;
}
export function getManagerOrdersPath() {
    return `${pathes.manager}/${pathes.manager_orders}`;
}
export function getManagerOrderDetailsPath(id: number) {
    return `${pathes.manager}/${pathes.manager_orders}/${id}`;
}

export function getProductsPath() {
    return `${pathes.client_products}`;
}
export function getProductInfoPath(id: number) {
    return `${id}`;
}

export function getClientMakeOrderPath() {
    return `${pathes.client}/${pathes.client_order}/${pathes.client_order_makeOrder}`;
}
export function getClientOrderHistoryPath() {
    return `${pathes.client}/${pathes.client_order}/${pathes.client_order_history}`;
}
export function getClientAddInformationPath() {
    return `${pathes.client}/${pathes.client_order}/${pathes.client_order_addInformation}`;
}

export function getAdminUerTable() {
    return `${pathes.admin}/${pathes.admin_userTable}`;
}
export function getAdminUserPagePath(id: string) {
    return `${pathes.admin}/${pathes.admin_userTable}/${id}`;
}