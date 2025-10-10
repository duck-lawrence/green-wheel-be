import {
    InvoiceType,
    InvoiceStatus,
    PaymentMethod,
    DepositStatus,
    InvoiceItemType
} from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"

export const mockInvoices: InvoiceViewRes[] = [
    {
        id: "INV_START_001",
        type: InvoiceType.HandoverPayment,
        subtotal: 10000000,
        tax: 1000000,
        total: 11000000,
        payAmount: 11000000,
        paymentMentod: PaymentMethod.MomoWallet,
        notes: "Thanh toán khi nhận xe gồm tiền thuê + tiền cọc + thuế VAT.",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-01T09:00:00Z",
        items: [
            {
                id: "ITEM_1",
                quantity: 1,
                unitPrice: 8000000,
                subTotal: 8000000,
                type: InvoiceItemType.BaseRental
            },
            {
                id: "ITEM_2",
                quantity: 1,
                unitPrice: 2000000,
                subTotal: 2000000,
                type: InvoiceItemType.Other
            },
            {
                id: "ITEM_3",
                quantity: 1,
                unitPrice: 2000000,
                subTotal: 2000000,
                type: InvoiceItemType.Cleaning
            }
        ]
    },
    {
        id: "INV_END_001",
        type: InvoiceType.ReturnPayment,
        subtotal: 450000,
        tax: 0,
        total: 450000,
        payAmount: 450000,
        paymentMentod: PaymentMethod.Cash,
        notes: "Thanh toán khi trả xe gồm phí vệ sinh và trễ giờ.",
        status: InvoiceStatus.Cancelled,
        paidAt: "2025-10-08T17:00:00Z",
        items: [
            {
                id: "ITEM_3",
                quantity: 1,
                unitPrice: 300000,
                subTotal: 300000,
                type: InvoiceItemType.Cleaning
            },
            {
                id: "ITEM_4",
                quantity: 1,
                unitPrice: 150000,
                subTotal: 150000,
                type: InvoiceItemType.LateReturn
            }
        ]
    },
    {
        id: "INV_REFUND_001",
        type: InvoiceType.RefundPayment,
        subtotal: 2000000,
        tax: 0,
        total: 2000000,
        payAmount: 1800000,
        paymentMentod: PaymentMethod.Cash,
        notes: "Hoàn tiền cọc, trừ phạt nguội 200.000đ.",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-09T09:00:00Z",
        items: [
            {
                id: "ITEM_5",
                quantity: 1,
                unitPrice: 2000000,
                subTotal: 2000000,
                type: InvoiceItemType.Other
            },
            {
                id: "ITEM_6",
                quantity: 1,
                unitPrice: -200000,
                subTotal: -200000,
                type: InvoiceItemType.Penalty
            }
        ],
        deposit: {
            id: "DEP_001",
            amount: 2000000,
            refundedAt: "2025-10-09T09:00:00Z",
            status: DepositStatus.Refunded
        }
    },
    {
        id: "INV_SUPPORT_001",
        type: InvoiceType.OtherPayment,
        subtotal: 1500000,
        tax: 0,
        total: 1500000,
        payAmount: 1500000,
        paymentMentod: PaymentMethod.MomoWallet,
        notes: "Chi phí hỗ trợ sửa chữa xe bị thủng lốp.",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-05T10:30:00Z",
        items: [
            {
                id: "ITEM_7",
                quantity: 1,
                unitPrice: 1500000,
                subTotal: 1500000,
                type: InvoiceItemType.Damage
            }
        ]
    }
]
