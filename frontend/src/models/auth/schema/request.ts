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

export type UserChangePasswordReq = {
    oldPassword: string
    password: string
    confirmPassword: string
}

export type UserSetPasswordReq = {
    firstName: string
    lastName: string
    password: string
    confirmPassword: string
}
