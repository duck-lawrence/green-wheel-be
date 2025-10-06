// export type UserProfileViewRes = {
//     email: string
//     firstName: string
//     lastName: string
//     sex?: number
//     dateOfBirth?: string
//     avatarUrl?: string
//     phone?: string
//     licenseUrl?: string
//     citizenUrl?: string
// }

export type UserProfileViewRes = {
    email: string
    firstName: string
    lastName: string
    sex?: number
    dateOfBirth?: string
    avatarUrl?: string
    phone?: string
    licenseUrl?: string
    citizenUrl?: string
    role?: string
    roleId?: string
    roleDetail?: {
        id: string
        name: string
        description?: string
    }
    stationId?: string
}
