import { InvoiceItemType, InvoiceStatus, InvoiceType, PaymentMethod } from "@/constants/enum"
import { VehicleChecklistItemViewRes } from "@/models/checklist/schema/response"
import { DepositViewRes } from "@/models/deposit/schema/response"

export type InvoiceViewRes = {
    id: string
    subtotal: number
    tax: number
    total: number
    payAmount: number
    paymentMentod?: PaymentMethod
    notes: string
    status: InvoiceStatus
    type: InvoiceType
    paidAt?: string
    invoiceItems: InvoiceItemViewRes[]
    deposit?: DepositViewRes
}

export type InvoiceItemViewRes = {
    id: string
    quantity: number
    unitPrice: number
    subTotal: number
    type: InvoiceItemType
    checkListItem?: VehicleChecklistItemViewRes
}
