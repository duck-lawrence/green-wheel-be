import i18n from "@/lib/i18n"
import {
    Sex,
    DispatchRequestStatus,
    DepositStatus,
    InvoiceStatus,
    RentalContractStatus,
    DamageStatus,
    VehicleStatus,
    InvoiceItemType,
    PaymentMethod,
    SupportRequestStatus,
    SupportRequestType,
    StaffReportStatus,
    StaffReportType,
    LicenseClass,
    OrderStatus
} from "./enum"

export const SexLabels: Record<Sex, string> = {
    [Sex.Male]: i18n.t("enum.male"),
    [Sex.Female]: i18n.t("enum.female")
}

export const DispatchRequestStatusLabels: Record<DispatchRequestStatus, string> = {
    [DispatchRequestStatus.Pending]: i18n.t("enum.pending"),
    [DispatchRequestStatus.Approved]: i18n.t("enum.approved"),
    [DispatchRequestStatus.Rejected]: i18n.t("enum.rejected"),
    [DispatchRequestStatus.Received]: i18n.t("enum.received")
}

export const DepositStatusLabels: Record<DepositStatus, string> = {
    [DepositStatus.Pending]: i18n.t("enum.pending"),
    [DepositStatus.Paid]: i18n.t("enum.paid"),
    [DepositStatus.Refunded]: i18n.t("enum.refunded"),
    [DepositStatus.Forfeited]: i18n.t("enum.forfeited")
}

export const InvoiceStatusLabels: Record<InvoiceStatus, string> = {
    [InvoiceStatus.Pending]: i18n.t("enum.pending"),
    [InvoiceStatus.Paid]: i18n.t("enum.paid"),
    [InvoiceStatus.Cancelled]: i18n.t("enum.cancelled")
}

export const RentalContractStatusLabels: Record<RentalContractStatus, string> = {
    [RentalContractStatus.RequestPending]: i18n.t("enum.request_pending"),
    [RentalContractStatus.ConfirmChangeStationPending]: i18n.t(
        "enum.confirm_change_station_pending"
    ),
    [RentalContractStatus.PaymentPending]: i18n.t("enum.payment_pending"),
    [RentalContractStatus.Active]: i18n.t("enum.active"),
    [RentalContractStatus.Completed]: i18n.t("enum.completed"),
    [RentalContractStatus.Cancelled]: i18n.t("enum.cancelled")
}

export const DamageStatusLabels: Record<DamageStatus, string> = {
    [DamageStatus.Good]: i18n.t("enum.good"),
    [DamageStatus.Minor]: i18n.t("enum.minor"),
    [DamageStatus.Moderate]: i18n.t("enum.moderate"),
    [DamageStatus.Severe]: i18n.t("enum.severe"),
    [DamageStatus.Totaled]: i18n.t("enum.totaled")
}

export const VehicleStatusLabels: Record<VehicleStatus, string> = {
    [VehicleStatus.Available]: i18n.t("enum.available"),
    [VehicleStatus.Unavailable]: i18n.t("enum.unavailable"),
    [VehicleStatus.Rented]: i18n.t("enum.rented"),
    [VehicleStatus.Maintenance]: i18n.t("enum.maintenance")
}

export const InvoiceItemTypeLabels: Record<InvoiceItemType, string> = {
    [InvoiceItemType.BaseRental]: i18n.t("enum.base_rental"),
    [InvoiceItemType.Damage]: i18n.t("enum.damage"),
    [InvoiceItemType.LateReturn]: i18n.t("enum.late_return"),
    [InvoiceItemType.Cleaning]: i18n.t("enum.cleaning"),
    [InvoiceItemType.Penalty]: i18n.t("enum.penalty"),
    [InvoiceItemType.Other]: i18n.t("enum.other")
}

export const PaymentMethodLabels: Record<PaymentMethod, string> = {
    [PaymentMethod.Cash]: i18n.t("enum.cash"),
    [PaymentMethod.MomoWallet]: i18n.t("enum.momo_wallet")
}

export const SupportRequestStatusLabels: Record<SupportRequestStatus, string> = {
    [SupportRequestStatus.Pending]: i18n.t("enum.pending"),
    [SupportRequestStatus.InProgress]: i18n.t("enum.in_progress"),
    [SupportRequestStatus.Resolved]: i18n.t("enum.resolved")
}

export const SupportRequestTypeLabels: Record<SupportRequestType, string> = {
    [SupportRequestType.Technical]: i18n.t("enum.technical"),
    [SupportRequestType.Payment]: i18n.t("enum.payment"),
    [SupportRequestType.Other]: i18n.t("enum.other")
}

export const StaffReportStatusLabels: Record<StaffReportStatus, string> = {
    [StaffReportStatus.Pending]: i18n.t("enum.pending"),
    [StaffReportStatus.InProgress]: i18n.t("enum.in_progress"),
    [StaffReportStatus.Resolved]: i18n.t("enum.resolved")
}

export const StaffReportTypeLabels: Record<StaffReportType, string> = {
    [StaffReportType.Internal]: i18n.t("enum.internal"),
    [StaffReportType.RelatedToSupport]: i18n.t("enum.related_to_support"),
    [StaffReportType.Other]: i18n.t("enum.other")
}

export const LicenseClassLabels: Record<LicenseClass, string> = {
    [LicenseClass.B1]: i18n.t("enum.b1"),
    [LicenseClass.B]: i18n.t("enum.b"),
    [LicenseClass.C1]: i18n.t("enum.c1"),
    [LicenseClass.C]: i18n.t("enum.c"),
    [LicenseClass.D1]: i18n.t("enum.d1"),
    [LicenseClass.D2]: i18n.t("enum.d2"),
    [LicenseClass.D]: i18n.t("enum.d"),
    [LicenseClass.BE]: i18n.t("enum.be"),
    [LicenseClass.C1E]: i18n.t("enum.c1e"),
    [LicenseClass.CE]: i18n.t("enum.ce"),
    [LicenseClass.D1E]: i18n.t("enum.d1e"),
    [LicenseClass.D2E]: i18n.t("enum.d2e"),
    [LicenseClass.DE]: i18n.t("enum.de")
}

export const OrderStatusLabels: Record<OrderStatus, string> = {
    [OrderStatus.All]: i18n.t("enum.all"),
    [OrderStatus.Pending]: i18n.t("enum.pending"),
    [OrderStatus.AwaitingDeposit]: i18n.t("enum.awaiting_deposit"),
    [OrderStatus.Confirmed]: i18n.t("enum.confirmed"),
    [OrderStatus.InUse]: i18n.t("enum.in_use"),
    [OrderStatus.Returned]: i18n.t("enum.returned"),
    [OrderStatus.Overdue]: i18n.t("enum.overdue"),
    [OrderStatus.Completed]: i18n.t("enum.completed"),
    [OrderStatus.Cancelled]: i18n.t("enum.cancelled")
}
