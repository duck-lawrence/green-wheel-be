import { VehicleStatus } from "@/constants/enum"

export type CreateVehicleReq = {
    licensePlate: string
    modelId: string
    stationId: string
}

export type UpdateVehicleReq = {
    licensePlate?: string
    status?: VehicleStatus
    modelId?: string
    stationId?: string
}

export type CreateVehicleModelReq = {
    name: string
    description: string
    costPerDay: number
    depositFee: number
    seatingCapacity: number
    numberOfAirbags: number
    motorPower: number
    batteryCapacity: number
    ecoRangeKm: number
    sportRangeKm: number
    brandId: string
    segmentId: string
}

export type UpdateVehicleModelReq = {
    name?: string
    description?: string
    costPerDay?: number
    depositFee?: number
    seatingCapacity?: number
    numberOfAirbags?: number
    motorPower?: number
    batteryCapacity?: number
    ecoRangeKm?: number
    sportRangeKm?: number
    brandId?: string
    segmentId?: string
}

export type DeleteModelImagesReq = {
    imageIds: string[]
}

export type VehicleFilterReq = {
    stationId: string
    startDate: string
    endDate: string
    segmentId?: string
}
