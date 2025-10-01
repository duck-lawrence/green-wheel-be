import { Sex } from "@/constants/enum"

export type UserRegisterCompleteReq = {
    password: string
    confirmPassword: string
    firstName: string
    lastName: string
    dateOfBirth: string
    phone: string
    sex: Sex
}
