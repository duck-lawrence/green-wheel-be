export default interface VehicleModel {
    id: string
    createdAt: string
    updatedAt: string
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
    deletedAt?: string
}
