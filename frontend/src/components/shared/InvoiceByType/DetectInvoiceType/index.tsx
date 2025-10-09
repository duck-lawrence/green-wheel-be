import { InvoiceItemType, InvoiceType } from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"

export function DetectInvoiceType(invoice: InvoiceViewRes): InvoiceType {
    if (invoice.deposit && invoice.items.some((val) => val.type === InvoiceItemType.BaseRental)) {
        return InvoiceType.BookingPayment
    }
    if (
        invoice.items.some((i) =>
            [InvoiceItemType.Cleaning, InvoiceItemType.Damage, InvoiceItemType.LateReturn].includes(
                i.type
            )
        )
    ) {
        return InvoiceType.ReturnPayment
    }

    if (invoice.deposit && invoice.items.some((i) => i.type === InvoiceItemType.Penalty)) {
        return InvoiceType.DepositRefund
    }

    if (
        invoice.items.some((i) =>
            [InvoiceItemType.Damage, InvoiceItemType.Other].includes(i.type)
        ) &&
        !invoice.deposit
    ) {
        return InvoiceType.DamageSupport
    }

    // fallback nếu không xác định được
    return InvoiceType.BookingPayment
}
