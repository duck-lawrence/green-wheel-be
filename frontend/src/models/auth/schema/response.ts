export type TokenRes = {
    accessToken: string
}

export type LoginGoogleRes = {
    needSetPassword: boolean
    accessToken?: string
    firstName?: string
    lastName?: string
}
