import { InvoiceType } from "@/constants/enum"

export const InvoiceTypeStyle: Record<InvoiceType, { color: string; icon: string }> = {
    [InvoiceType.BookingPayment]: { color: "text-blue-500", icon: "💰" },
    [InvoiceType.ReturnPayment]: { color: "text-amber-500", icon: "🧾" },
    [InvoiceType.DepositRefund]: { color: "text-green-500", icon: "💸" },
    [InvoiceType.DamageSupport]: { color: "text-red-500", icon: "🛠️" }
}
