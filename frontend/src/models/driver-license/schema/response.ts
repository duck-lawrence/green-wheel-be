import { LicenseClass, Sex } from "@/constants/enum"

export type DriverLicenseRes = {
    id: string
    number: string
    fullName: string
    class: LicenseClass
    sex: Sex
    dateOfBirth: string
    expiresAt: string
    imageUrl: string
}
