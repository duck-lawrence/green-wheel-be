import { RentalContractStatus } from "@/constants/enum"
import { RentalContractViewRes } from "@/models/rental-contract/schema/response"
import { mockInvoices } from "./mockIvoices"

// 🔸 Giả lập 3 hợp đồng thuê xe điện
export const mockContracts: RentalContractViewRes[] = [
    {
        id: "CON001",
        description: "Thuê xe VF8 Eco 2 ngày",
        startDate: "2025-10-05T09:00:00Z",
        endDate: "2025-10-07T09:00:00Z",
        status: RentalContractStatus.Completed,
        vehicleId: "VEH001",
        customerId: "CUS001",
        handoverStaffId: "STF001",
        returnStaffId: "STF002",
        notes: "Khách thuê đầy đủ, có phạt nguội nhỏ",
        invoices: [
            mockInvoices.find((x) => x.id === "INV_B001")!, // Nhận xe
            mockInvoices.find((x) => x.id === "INV_R001")!, // Trả xe
            mockInvoices.find((x) => x.id === "INV_D001")! // Hoàn cọc
        ]
    },
    {
        id: "CON002",
        description: "Thuê xe VF3 trong 1 ngày",
        startDate: "2025-10-02T09:00:00Z",
        endDate: "2025-10-03T09:00:00Z",
        status: RentalContractStatus.Completed,
        vehicleId: "VEH002",
        customerId: "CUS002",
        handoverStaffId: "STF001",
        returnStaffId: "STF002",
        notes: "Xe bị trầy nhẹ, hoàn cọc đủ",
        invoices: [
            mockInvoices.find((x) => x.id === "INV_B002")!, // Nhận xe
            mockInvoices.find((x) => x.id === "INV_R002")!, // Trả xe có damage
            mockInvoices.find((x) => x.id === "INV_D002")! // Hoàn cọc đầy đủ
        ]
    },
    {
        id: "CON003",
        description: "Thuê xe VF6 - đang hoạt động",
        startDate: "2025-10-07T08:00:00Z",
        endDate: "2025-10-10T08:00:00Z",
        status: RentalContractStatus.Active,
        vehicleId: "VEH003",
        customerId: "CUS003",
        handoverStaffId: "STF004",
        returnStaffId: "STF004",
        notes: "Khách đang thuê, có 1 hóa đơn sửa chữa giữa chuyến",
        invoices: [
            mockInvoices.find((x) => x.id === "INV_B003")!, // Nhận xe - chờ thanh toán
            mockInvoices.find((x) => x.id === "INV_S001")!, // Hóa đơn hỗ trợ hư hỏng
            mockInvoices.find((x) => x.id === "INV_S002")! // Thay pin
        ]
    }
]
