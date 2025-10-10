import { SupportRequestStatus, SupportRequestType } from "@/constants/enum"

export type CreateSupportReq = {
    title: string
    description: string
    type: SupportRequestType
}

export type UpdateSupportReq = {
    reply: string
    status: SupportRequestStatus
}
