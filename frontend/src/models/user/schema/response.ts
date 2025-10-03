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
}

export type LoginGoogleRes = {
    needSetPassword: boolean
    accessToken?: string
    firstName: string
    lastName: string
}
