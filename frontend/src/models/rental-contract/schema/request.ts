export type CreateRentalContractReq = {
    customerId?: string
    modelId: string
    startDate: string
    endDate: string
    stationId: string
}

export type HandoverContractReq = {
    isSignedByStaff: boolean
    isSignedByCustomer: boolean
    amount?: number
}
