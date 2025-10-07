import { Sex } from "@/constants/enum"

export type UserUpdateReq = {
    firstName?: string
    lastName?: string
    phone?: string
    sex?: Sex
    dateOfBirth?: string
}
