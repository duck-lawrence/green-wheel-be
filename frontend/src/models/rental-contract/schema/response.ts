import { RentalContractStatus } from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { StationViewRes } from "@/models/station/schema/response"
import { UserProfileViewRes } from "@/models/user/schema/response"
import { VehicleViewRes } from "@/models/vehicle/schema/response"

export type RentalContractViewRes = {
    id: string
    description?: string
    notes?: string
    startDate: string
    actualStartDate?: string
    endDate: string
    actualEndDate?: string
    status: RentalContractStatus
    vehicle: VehicleViewRes
    customerId: string
    station: StationViewRes
    handoverStaffId?: string
    returnStaffId?: string
    invoices: InvoiceViewRes[]
}

export type RentalContractForStaffViewRes = {
    id: string
    createdAt: string
    updatedAt: string
    description?: string
    notes?: string
    startDate: string
    actualStartDate?: string
    endDate: string
    actualEndDate?: string
    status: RentalContractStatus
    isSignedByStaff: boolean
    isSignedByCustomer: boolean

    vehicleId?: string
    customer: UserProfileViewRes
    handoverStaffId?: string
    returnStaffId?: string
    invoices: InvoiceViewRes[]
}
