export const currency = (n: number) => new Intl.NumberFormat("vi-VN").format(n)
// export const currency = (n: number) =>
//     new Intl.NumberFormat("en-US", {
//         minimumFractionDigits: 2,
//         maximumFractionDigits: 2
//     }).format(n) + " VND / Ngày"
