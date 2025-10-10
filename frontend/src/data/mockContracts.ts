import {
    RentalContractStatus,
    InvoiceStatus,
    DepositStatus,
    PaymentMethod,
    InvoiceItemType
} from "@/constants/enum"
import { RentalContractViewRes } from "@/models/rental-contract/schema/response"

export const mockContracts: RentalContractViewRes[] = [
    {
        id: "CON001",
        description: "Hợp đồng thuê xe VF8 cho khách Nguyễn Văn A",
        notes: "Khách thuê trong 7 ngày, nhận xe tại chi nhánh Quận 7",
        startDate: "2025-10-01",
        actualStartDate: "2025-10-01",
        endDate: "2025-10-08",
        actualEndDate: "2025-10-08",
        status: RentalContractStatus.Completed,
        vehicleId: "VH001",
        customerId: "CUS001",
        handoverStaffId: "STF001",
        returnStaffId: "STF002",
        invoices: [
            {
                id: "INV_START_001",
                subtotal: 10000000,
                tax: 1000000,
                total: 11000000,
                payAmount: 11000000,
                paymentMentod: PaymentMethod.MomoWallet,
                notes: "Thanh toán khi nhận xe (cọc + tiền thuê + VAT)",
                status: InvoiceStatus.Paid,
                paidAt: "2025-10-01T08:00:00Z",
                items: [
                    {
                        id: "IT001",
                        quantity: 1,
                        unitPrice: 8000000,
                        subTotal: 8000000,
                        type: InvoiceItemType.BaseRental
                    },
                    {
                        id: "IT002",
                        quantity: 1,
                        unitPrice: 2000000,
                        subTotal: 2000000,
                        type: InvoiceItemType.Other
                    } // tiền cọc
                ]
            },
            {
                id: "INV_END_001",
                subtotal: 450000,
                tax: 0,
                total: 450000,
                payAmount: 450000,
                paymentMentod: PaymentMethod.Cash,
                notes: "Thanh toán khi trả xe (vệ sinh + trễ + hư hỏng)",
                status: InvoiceStatus.Paid,
                paidAt: "2025-10-08T17:00:00Z",
                items: [
                    {
                        id: "IT003",
                        quantity: 1,
                        unitPrice: 300000,
                        subTotal: 300000,
                        type: InvoiceItemType.Cleaning
                    },
                    {
                        id: "IT004",
                        quantity: 1,
                        unitPrice: 150000,
                        subTotal: 150000,
                        type: InvoiceItemType.LateReturn
                    }
                ]
            },
            {
                id: "INV_REFUND_001",
                subtotal: 2000000,
                tax: 0,
                total: 2000000,
                payAmount: 1800000, // do có phạt nguội 200k
                paymentMentod: PaymentMethod.Cash,
                notes: "Hoàn tiền cọc (trừ phạt nguội 200k)",
                status: InvoiceStatus.Paid,
                paidAt: "2025-10-09T09:00:00Z",
                items: [
                    {
                        id: "IT005",
                        quantity: 1,
                        unitPrice: 2000000,
                        subTotal: 2000000,
                        type: InvoiceItemType.Other
                    },
                    {
                        id: "IT006",
                        quantity: 1,
                        unitPrice: -200000,
                        subTotal: -200000,
                        type: InvoiceItemType.Penalty
                    }
                ],
                deposit: {
                    id: "DEP001",
                    amount: 2000000,
                    refundedAt: "2025-10-09T09:00:00Z",
                    status: DepositStatus.Refunded
                }
            },
            {
                id: "INV_SUPPORT_001",
                subtotal: 1500000,
                tax: 0,
                total: 1500000,
                payAmount: 1500000,
                paymentMentod: PaymentMethod.MomoWallet,
                notes: "Chi phí hỗ trợ sửa chữa xe khi gặp sự cố trên đường.",
                status: InvoiceStatus.Paid,
                paidAt: "2025-10-05T10:30:00Z",
                items: [
                    {
                        id: "IT007",
                        quantity: 1,
                        unitPrice: 1500000,
                        subTotal: 1500000,
                        type: InvoiceItemType.Damage
                    }
                ]
            }
        ]
    }
]
