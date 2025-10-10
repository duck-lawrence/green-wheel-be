import { DepositStatus } from "@/constants/enum"

export type DepositViewRes = {
    id: string
    amount: number
    refundedAt?: string
    status: DepositStatus
}
