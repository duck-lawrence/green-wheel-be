import { Brand } from "@/models/brand/brand"
import VehicleSegment from "@/models/vehicle-segment/vehicle-segment"

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
    brand: Brand
    segment: VehicleSegment
    imageUrl?: string
    imageUrls?: string[]
    availableVehicleCount: number
}
