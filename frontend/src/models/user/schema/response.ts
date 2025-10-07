import { RoleViewRes } from "@/models/role/response"
import { StationViewRes } from "@/models/station/schema/response"

export type UserProfileViewRes = {
    email?: string
    firstName: string
    lastName: string
    sex?: number
    dateOfBirth?: string
    avatarUrl?: string
    phone?: string
    licenseUrl?: string
    citizenUrl?: string
    role?: RoleViewRes
    station?: StationViewRes
}
