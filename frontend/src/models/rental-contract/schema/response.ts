import { RentalContractStatus } from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"

export type RentalContractViewRes = {
    id: string
    description?: string
    notes?: string
    startDate: string
    actualStartDate?: string
    endDate: string
    actualEndDate?: string
    status: RentalContractStatus
    vehicleId: string
    customerId: string
    handoverStaffId: string
    returnStaffId: string
    invoices: InvoiceViewRes[]
}
