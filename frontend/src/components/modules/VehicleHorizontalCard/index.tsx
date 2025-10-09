"use client"

import React from "react"
import { Button, Chip, Image, cn } from "@heroui/react"
import { SteeringWheel, UsersThree } from "@phosphor-icons/react"
import { useTranslation } from "react-i18next"
import { CardStyled } from "@/components"
import { formatCurrency } from "@/utils/helpers/currentcy"
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"

type VehicleHorizontalCardProps = {
    vehicleModel: VehicleModelViewRes
    onSelect?: (vehicleModel: VehicleModelViewRes) => void
    className?: string
    readOnly?: boolean
}

type SpecItem = {
    icon: React.ReactNode
    label: string
    value: string
    className?: string
}

export function VehicleHorizontalCard({ vehicleModel, onSelect, className, readOnly }: VehicleHorizontalCardProps) {
    const { t } = useTranslation()
    const {
        name,
        brand,
        segment,
        imageUrl,
        imageUrls,
        seatingCapacity,
        availableVehicleCount,
        costPerDay
    } = vehicleModel

    const brandName = brand?.name ?? t("vehicle_model.brand_unknown", "Unknown brand")
    const segmentName = segment?.name ?? t("vehicle_model.segment_unknown", "Unknown segment")
    const isAvailable = availableVehicleCount > 0
    const cover = imageUrl ?? imageUrls?.[0] ?? "/images/vehicle-placeholder.png"
    const unitLabel = t("vehicle_model.unit", "Unit")

    const typeSpec: SpecItem = {
        icon: (
            <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-slate-100 text-slate-500">
                <SteeringWheel className="h-6 w-6" weight="duotone" />
            </div>
        ),
        label: t("vehicle_model.type_car_label", "Type car"),
        value: segmentName,
        className: "lg:items-start lg:text-left"
    }

    const capacitySpec: SpecItem = {
        icon: (
            <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-slate-100 text-slate-500">
                <UsersThree className="h-6 w-6" weight="duotone" />
            </div>
        ),
        label: t("vehicle_model.capacity_label", "Capacity"),
        value: `${seatingCapacity} ${t("vehicle_model.seats", "seats")}`,
        className: "lg:items-start lg:text-left"
    }

    return (
        <CardStyled
            shadow="sm"
            className={cn(
                "space-y-0 bg-white p-6 sm:p-8",
                "hover:shadow-xl interactive-scale",
                className
            )}
        >
            <div className="flex w-full flex-col gap-6 lg:flex-row lg:items-center lg:gap-6 lg:flex-nowrap">
                <div className="flex justify-center lg:w-[160px]">
                    <div className="relative h-24 w-36 overflow-hidden rounded-2xl bg-slate-100 sm:h-28 sm:w-44">
                        <Image
                            alt={`${brandName} ${name}`}
                            className="h-full w-full object-cover"
                            radius="none"
                            shadow="none"
                            src={cover}
                            loading="lazy"
                            removeWrapper
                        />
                    </div>
                </div>

                <div className="flex flex-1 flex-col items-center gap-3 text-center lg:min-w-[200px] lg:items-start lg:text-left">
                    <div className="space-y-1">
                        <span className="text-sm text-slate-500">{brandName}</span>
                        <h3 className="text-2xl font-semibold text-slate-900">{name}</h3>
                    </div>

                    <div className="flex flex-wrap items-center justify-center gap-0 lg:justify-start">
                        <Chip
                            size="sm"
                            variant="solid"
                            color={isAvailable ? "success" : "default"}
                            className={cn(
                                "px-3 py-1 text-xs font-semibold uppercase tracking-wide",
                                !isAvailable && "bg-slate-200 text-slate-500"
                            )}
                        >
                            {isAvailable
                                ? t("vehicle_model.available", "Available")
                                : t("vehicle_model.unavailable", "Unavailable")}
                        </Chip>

                        <Chip size="sm" variant="flat" className="bg-slate-100 px-3 py-1 text-xs font-medium text-slate-600">
                            {availableVehicleCount} {unitLabel}
                        </Chip>
                    </div>
                </div>

                <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:gap-3">
                    <SpecColumn {...typeSpec} />
                    <SpecColumn {...capacitySpec} />
                    <div className="flex min-w-[120px] flex-col items-center gap-2 text-center lg:items-start lg:text-left">
                        <p className="text-xs font-semibold uppercase tracking-wide text-slate-400">
                            {t("vehicle_model.price_label", "Price")}
                        </p>
                        <p className="text-2xl font-bold text-slate-900">
                            {formatCurrency(costPerDay)}
                            <span className="ml-1 text-sm font-medium text-slate-500">
                                /{t("vehicle_model.vnd_per_day", "days")}
                            </span>
                        </p>
                    </div>
                </div>

                {!readOnly && (
                    <Button
                        color="danger"
                        className="h-12 px-6 font-semibold self-center lg:ml-6 lg:self-auto"
                        isDisabled={!isAvailable}
                        onPress={() => onSelect?.(vehicleModel)}
                    >
                        {t("vehicle_model.select", "Select")}
                    </Button>
                )}
            </div>
        </CardStyled>
    )
}

export default VehicleHorizontalCard

function SpecColumn({ icon, label, value, className }: SpecItem) {
    return (
        <div className={cn("min-w-[120px] flex flex-col items-center gap-2 text-center", className)}>
            {icon}
            <p className="text-xs font-semibold uppercase tracking-wide text-slate-400">{label}</p>
            <p className="text-sm font-semibold text-slate-900">{value}</p>
        </div>
    )
}
