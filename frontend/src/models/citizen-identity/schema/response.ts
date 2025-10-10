import { Sex } from "@/constants/enum"

export type CitizenIdentityRes = {
    id: string
    number: string
    fullName: string
    nationality: string
    sex: Sex
    dateOfBirth: string
    expiresAt: string
    imageUrl: string
}
