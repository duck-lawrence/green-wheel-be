export enum Sex {
    Male = 0,
    Female = 1
}
// Có thể map ngược DispatchRequestStatus[1] → "Approved".
export enum DispatchRequestStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Received = 3
}

export enum DepositStatus {
    Pending = 0,
    Paid = 1,
    Refunded = 2,
    Forfeited = 3
}

export enum InvoiceStatus {
    Pending = 0,
    Paid = 1,
    Cancelled = 2
}

export enum InvoiceType {
    Handover = 0,
    Return = 1,
    Refund = 2,
    Other = 3,
    Reservation = 4
}

export enum RentalContractStatus {
    RequestPending = 0,
    PaymentPending = 1,
    Active = 2,
    Completed = 3,
    Cancelled = 4,
    ConfirmChangeStationPending = 5
}

export enum DamageStatus {
    Good = 0,
    Minor = 1,
    Moderate = 2,
    Severe = 3,
    Totaled = 4
}

export enum VehicleStatus {
    Available = 0,
    Unavailable = 1,
    Rented = 2,
    Maintenance = 3,
    MissingNoReason = 4,
    LateReturn = 5
}

export enum InvoiceItemType {
    BaseRental = 0,
    Damage = 1,
    LateReturn = 2,
    Cleaning = 3,
    Penalty = 4,
    Other = 5
}

export enum PaymentMethod {
    Cash = 0,
    MomoWallet = 1
}

export enum TicketType {
    CustomerSupport = 0,
    StaffReport = 1
}

export enum TicketStatus {
    Pending = 0,
    Resolve = 1,
    EscalatedToAdmin = 2
}

export enum LicenseClass {
    B1 = 0,
    B = 1,
    C1 = 2,
    C = 3,
    D1 = 4,
    D2 = 5,
    D = 6,
    BE = 7,
    C1E = 8,
    CE = 9,
    D1E = 10,
    D2E = 11,
    DE = 12
}

export enum OrderStatus {
    All = 0,
    Pending = 1,
    AwaitingDeposit = 2,
    Confirmed = 3,
    InUse = 4,
    Returned = 5,
    Overdue = 6,
    Completed = 7,
    Cancelled = 8
}
