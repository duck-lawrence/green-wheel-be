import { DamageStatus } from "@/constants/enum"

export type CreateVehicleChecklistReq = {
    contractId?: string
    vehicleId?: string
}

export type UpdateVehicleChecklistReq = {
    isSignedByStaff: boolean
    isSignedByCustomer: boolean
    vehicleChecklistId: string
    description?: string
    checklistItems: UpdateChecklistItemReq[]
}

export type UpdateChecklistItemReq = {
    id: string
    status: DamageStatus
    notes?: string
}
