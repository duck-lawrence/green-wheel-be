export const formatCurrency = (n: number) => new Intl.NumberFormat("vi-VN").format(n)
// export const formatCurrency = (n: number) =>
//     new Intl.NumberFormat("en-US", {
//         minimumFractionDigits: 2,
//         maximumFractionDigits: 2
//     }).format(n) + " VND / Ngày"
