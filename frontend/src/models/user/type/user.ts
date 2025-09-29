import { Sex } from "@/constants/enum"

export type User = {
    id: string
    createdAt: string
    updatedAt: string
    firstName: string
    lastName: string
    email?: string
    password?: string
    phone?: string
    isGoogleLinked: boolean
    sex?: Sex
    dateOfBirth?: string
    avatarUrl?: string
    avatarPublicId?: string
    deletedAt?: string
    roleId: string
}
