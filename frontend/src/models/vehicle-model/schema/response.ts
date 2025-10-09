import { BrandViewRes } from "@/models/brand/schema/response"
import { VehicleSegmentViewRes } from "@/models/vehicle-segment/schema/response"

export type VehicleModelViewRes = {
    id: string
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
    brand: BrandViewRes
    segment: VehicleSegmentViewRes
    imageUrl?: string
    imageUrls?: string[]
    availableVehicleCount: number
}

export type VehicleComponentViewRes = {
    id: string
    name: string
    description: string
}
