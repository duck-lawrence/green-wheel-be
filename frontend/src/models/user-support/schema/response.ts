import { SupportRequestStatus, SupportRequestType } from "@/constants/enum"

export type SupportRes = {
    id: string
    title: string
    description: string
    reply?: string
    status: SupportRequestStatus
    type: SupportRequestType
    createdAt: string
    customerName: string
    staffName?: string
}
