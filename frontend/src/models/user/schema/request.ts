import { Sex, SupportRequestStatus, SupportRequestType } from "@/constants/enum"

export type CreateUserReq = {
    email?: string
    phone: string
    firstName: string
    lastName: string
    dateOfBirth: string
}

export type UserUpdateReq = {
    firstName?: string
    lastName?: string
    phone?: string
    sex?: Sex
    dateOfBirth?: string
}

export type CreateSupportReq = {
    title: string
    description: string
    type: SupportRequestType
}

export type UpdateSupportReq = {
    reply: string
    status: SupportRequestStatus
}
