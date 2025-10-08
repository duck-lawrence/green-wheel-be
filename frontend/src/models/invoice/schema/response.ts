import { InvoiceItemType, InvoiceStatus, PaymentMethod } from "@/constants/enum"

export type InvoiceViewRes = {
    id: string
    invoiceItems: InvoiceItemViewRes[]
    deposit: DepositViewRes
    subtotal: number
    tax: number
    total: number
    payAmount: number
    paymentMentod?: PaymentMethod
    notes: string
    status: InvoiceStatus
    paidAt?: string
    checkListId?: string
}

export type DepositViewRes = {
    id: string
    description?: string
    amount: number
    refundedAt?: string
    status: number
}

export type InvoiceItemViewRes = {
    id: string
    quantity: number
    unitPrice: number
    notes: string
    subTotal: number
    type: InvoiceItemType
    checkListItemId?: string
}
