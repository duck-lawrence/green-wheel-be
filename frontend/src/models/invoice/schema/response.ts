import { DepositStatus, InvoiceItemType, InvoiceStatus, PaymentMethod } from "@/constants/enum"
import { VehicleChecklistItemViewRes } from "@/models/checklist/schema/response"

export type InvoiceViewRes = {
    id: string
    subtotal: number
    tax: number
    total: number
    payAmount: number
    paymentMentod?: PaymentMethod
    notes: string
    status: InvoiceStatus
    paidAt?: string
    items: InvoiceItemViewRes[]
    deposit?: DepositViewRes
}

export type DepositViewRes = {
    id: string
    amount: number
    refundedAt?: string
    status: DepositStatus
}

export type InvoiceItemViewRes = {
    id: string
    quantity: number
    unitPrice: number
    subTotal: number
    type: InvoiceItemType
    checkListItem?: VehicleChecklistItemViewRes
}
