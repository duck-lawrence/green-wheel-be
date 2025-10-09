import { DepositStatus, InvoiceItemType, InvoiceStatus, PaymentMethod } from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"

// 👆 Nếu DepositStatus/InvoiceItemType ở file khác, sửa lại import tương ứng

// 10 HOÁ ĐƠN MẪU
export const mockInvoices: InvoiceViewRes[] = [
    // 1) BookingPayment — thanh toán khi nhận xe (có cọc + base rental + VAT)
    {
        id: "INV_B001",
        subtotal: 2000000,
        tax: 200000,
        total: 2200000,
        payAmount: 2200000,
        paymentMentod: PaymentMethod.MomoWallet,
        notes: "Nhận xe VF8 — thanh toán tiền thuê + VAT",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-05T09:00:00Z",
        checkListId: "CHK001",
        items: [
            {
                id: "IT001-1",
                quantity: 2,
                unitPrice: 1000000,
                notes: "Thuê VF8 2 ngày",
                subTotal: 2000000,
                type: InvoiceItemType.BaseRental,
                checkListItem: {
                    id: "CLI001-BASE",
                    notes: "Kiểm tra ngoại thất trước khi giao",
                    status: 0,
                    imageUrl: "/images/checks/vf8-exterior.jpg",
                    component: { id: "COMP-EXT", name: "Exterior" } as unknown as any
                }
            }
        ],
        deposit: {
            id: "DEP001",
            description: "Tiền cọc xe VF8",
            amount: 3000000,
            status: DepositStatus.Paid
        }
    },

    // 2) BookingPayment — nhận VF3
    {
        id: "INV_B002",
        subtotal: 900000,
        tax: 90000,
        total: 990000,
        payAmount: 990000,
        paymentMentod: PaymentMethod.Cash,
        notes: "Nhận xe VF3 — thuê 1 ngày",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-02T13:30:00Z",
        checkListId: "CHK002",
        items: [
            {
                id: "IT002-1",
                quantity: 1,
                unitPrice: 900000,
                notes: "Thuê VF3 1 ngày",
                subTotal: 900000,
                type: InvoiceItemType.BaseRental,
                checkListItem: {
                    id: "CLI002-BASE",
                    status: 0,
                    component: { id: "COMP-ODO", name: "Odometer" } as unknown as any
                }
            }
        ],
        deposit: {
            id: "DEP002",
            description: "Tiền cọc xe VF3",
            amount: 1500000,
            status: DepositStatus.Paid
        }
    },

    // 3) ReturnPayment — trả xe VF8 (vệ sinh + trễ hạn)
    {
        id: "INV_R001",
        subtotal: 700000,
        tax: 70000,
        total: 770000,
        payAmount: 770000,
        paymentMentod: PaymentMethod.Cash,
        notes: "Trả VF8 — phát sinh vệ sinh + trễ hạn",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-07T18:00:00Z",
        checkListId: "CHK001",
        items: [
            {
                id: "IT003-1",
                quantity: 1,
                unitPrice: 300000,
                notes: "Phí vệ sinh nội thất",
                subTotal: 300000,
                type: InvoiceItemType.Cleaning,
                checkListItem: {
                    id: "CLI001-CLEAN",
                    notes: "Ghế trước bám bụi",
                    status: 1,
                    imageUrl: "/images/checks/seat-dust.jpg",
                    component: { id: "COMP-INT", name: "Interior" } as unknown as any
                }
            },
            {
                id: "IT003-2",
                quantity: 1,
                unitPrice: 400000,
                notes: "Trễ 2 tiếng",
                subTotal: 400000,
                type: InvoiceItemType.LateReturn,
                checkListItem: {
                    id: "CLI001-LATE",
                    status: 2,
                    component: { id: "COMP-RETURN", name: "Return Time" } as unknown as any
                }
            }
        ]
    },

    // 4) ReturnPayment — trả VF3 (damage + cleaning)
    {
        id: "INV_R002",
        subtotal: 600000,
        tax: 60000,
        total: 660000,
        payAmount: 660000,
        paymentMentod: PaymentMethod.MomoWallet,
        notes: "Trả VF3 — trầy sơn nhẹ + vệ sinh",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-08T10:00:00Z",
        checkListId: "CHK002",
        items: [
            {
                id: "IT004-1",
                quantity: 1,
                unitPrice: 400000,
                notes: "Trầy sơn hông trái",
                subTotal: 400000,
                type: InvoiceItemType.Damage,
                checkListItem: {
                    id: "CLI002-DMG",
                    notes: "Trầy 5cm khu vực cửa sau",
                    status: 2,
                    imageUrl: "/images/checks/left-door-scratch.jpg",
                    component: { id: "COMP-DOOR-L", name: "Left Door" } as unknown as any
                }
            },
            {
                id: "IT004-2",
                quantity: 1,
                unitPrice: 200000,
                notes: "Vệ sinh cơ bản",
                subTotal: 200000,
                type: InvoiceItemType.Cleaning,
                checkListItem: {
                    id: "CLI002-CLEAN",
                    status: 1,
                    component: { id: "COMP-FLOOR", name: "Floor Mat" } as unknown as any
                }
            }
        ]
    },

    // 5) DepositRefund — hoàn cọc có phạt nguội
    {
        id: "INV_D001",
        subtotal: 3000000,
        tax: 0,
        total: 3000000,
        payAmount: 2700000,
        paymentMentod: PaymentMethod.MomoWallet,
        notes: "Hoàn cọc VF8 — trừ phạt nguội",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-10T09:00:00Z",
        checkListId: "CHK001",
        items: [
            {
                id: "IT005-1",
                quantity: 1,
                unitPrice: 300000,
                notes: "Phạt nguội: vượt tốc",
                subTotal: 300000,
                type: InvoiceItemType.Penalty,
                checkListItem: {
                    id: "CLI001-PEN",
                    status: 2,
                    component: { id: "COMP-FINE", name: "Traffic Fine" } as unknown as any
                }
            }
        ],
        deposit: {
            id: "DEP001",
            description: "Hoàn lại cọc VF8",
            amount: 3000000,
            refundedAt: "2025-10-10T09:00:00Z",
            status: DepositStatus.Refunded
        }
    },

    // 6) DepositRefund — hoàn cọc đủ
    {
        id: "INV_D002",
        subtotal: 1500000,
        tax: 0,
        total: 1500000,
        payAmount: 1500000,
        paymentMentod: PaymentMethod.Cash,
        notes: "Hoàn cọc VF3 — không phát sinh",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-09T14:00:00Z",
        checkListId: "CHK002",
        items: [],
        deposit: {
            id: "DEP002",
            description: "Hoàn lại cọc VF3",
            amount: 1500000,
            refundedAt: "2025-10-09T14:00:00Z",
            status: DepositStatus.Refunded
        }
    },

    // 7) DamageSupport — giữa chuyến (thay gương + cứu hộ)
    {
        id: "INV_S001",
        subtotal: 850000,
        tax: 85000,
        total: 935000,
        payAmount: 935000,
        paymentMentod: PaymentMethod.MomoWallet,
        notes: "Giữa chuyến VF6 — thay gương + xe kéo",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-06T15:00:00Z",
        checkListId: "CHK003",
        items: [
            {
                id: "IT007-1",
                quantity: 1,
                unitPrice: 500000,
                notes: "Thay gương bị vỡ",
                subTotal: 500000,
                type: InvoiceItemType.Damage,
                checkListItem: {
                    id: "CLI003-MIRROR",
                    notes: "Gương phải vỡ",
                    status: 2,
                    imageUrl: "/images/checks/mirror-broken.jpg",
                    component: { id: "COMP-MIR-R", name: "Right Mirror" } as unknown as any
                }
            },
            {
                id: "IT007-2",
                quantity: 1,
                unitPrice: 350000,
                notes: "Cứu hộ kéo về trạm",
                subTotal: 350000,
                type: InvoiceItemType.Other,
                checkListItem: {
                    id: "CLI003-TOW",
                    status: 1,
                    component: { id: "COMP-TOW", name: "Towing Service" } as unknown as any
                }
            }
        ]
    },

    // 8) DamageSupport — giữa chuyến (pin lỗi + công sửa)
    {
        id: "INV_S002",
        subtotal: 1000000,
        tax: 100000,
        total: 1100000,
        payAmount: 1100000,
        paymentMentod: PaymentMethod.Cash,
        notes: "Giữa chuyến VF5 — pin lỗi, công sửa",
        status: InvoiceStatus.Paid,
        paidAt: "2025-10-07T11:30:00Z",
        checkListId: "CHK004",
        items: [
            {
                id: "IT008-1",
                quantity: 1,
                unitPrice: 700000,
                notes: "Thay cell pin lỗi",
                subTotal: 700000,
                type: InvoiceItemType.Damage,
                checkListItem: {
                    id: "CLI004-BATT",
                    status: 2,
                    component: { id: "COMP-BATT", name: "Battery Pack" } as unknown as any
                }
            },
            {
                id: "IT008-2",
                quantity: 1,
                unitPrice: 300000,
                notes: "Công thay + kiểm tra an toàn",
                subTotal: 300000,
                type: InvoiceItemType.Other,
                checkListItem: {
                    id: "CLI004-LABOR",
                    status: 1,
                    component: { id: "COMP-LABOR", name: "Labor" } as unknown as any
                }
            }
        ]
    },

    // 9) BookingPayment — pending (chưa thanh toán, cọc Pending)
    {
        id: "INV_B003",
        subtotal: 1200000,
        tax: 120000,
        total: 1320000,
        payAmount: 0,
        paymentMentod: undefined,
        notes: "Đặt VF6 — chờ thanh toán",
        status: InvoiceStatus.Pending,
        checkListId: "CHK005",
        items: [
            {
                id: "IT009-1",
                quantity: 2,
                unitPrice: 600000,
                notes: "VF6 — 2 ngày",
                subTotal: 1200000,
                type: InvoiceItemType.BaseRental,
                checkListItem: {
                    id: "CLI005-BASE",
                    status: 0,
                    component: { id: "COMP-ODO", name: "Odometer" } as unknown as any
                }
            }
        ],
        deposit: {
            id: "DEP003",
            description: "Cọc VF6 chưa thanh toán",
            amount: 2000000,
            status: DepositStatus.Pending
        }
    },

    // 10) ReturnPayment — bị huỷ
    {
        id: "INV_R003",
        subtotal: 0,
        tax: 0,
        total: 0,
        payAmount: 0,
        paymentMentod: undefined,
        notes: "Hủy hóa đơn trả xe do lỗi hệ thống",
        status: InvoiceStatus.Cancelled,
        checkListId: "CHK006",
        items: []
    }
]
