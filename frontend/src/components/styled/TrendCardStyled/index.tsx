"use client"
import { Chip, cn } from "@heroui/react"
import React from "react"
import { CardStyled } from "../CardStyled"
import { Icon } from "@iconify/react"

// đưa cái này vô models
type TrendCardProps = {
    title: string
    value: string
    change: string
    changeType: "positive" | "neutral" | "negative" // đổi màu changeType
    trendType: "up" | "neutral" | "down" // đổi dấu mũi tên changeType
    trendChipPosition?: "top" | "bottom" // đổi vị trí changeType
    trendChipVariant?: "flat" | "light" // bg changeType
}

export function TrendCardStyled({
    title,
    value,
    change,
    changeType,
    trendType,
    trendChipPosition = "top",
    trendChipVariant = "flat"
}: TrendCardProps) {
    return (
        <CardStyled className="dark:border-default-100 border border-transparent">
            <div className="flex p-4">
                <div className="flex flex-col gap-y-2">
                    <dt className="text-small text-default-500 font-medium">{title}</dt>
                    <dd className="text-default-700 text-2xl font-semibold">{value}</dd>
                </div>
                <Chip
                    className={cn("absolute right-4", {
                        "top-4": trendChipPosition === "top",
                        "bottom-4": trendChipPosition === "bottom"
                    })}
                    classNames={{
                        content: "font-medium text-[0.65rem]"
                    }}
                    color={
                        changeType === "positive"
                            ? "success"
                            : changeType === "neutral"
                            ? "warning"
                            : "danger"
                    }
                    radius="sm"
                    size="sm"
                    startContent={
                        trendType === "up" ? (
                            <Icon height={12} icon={"solar:arrow-right-up-linear"} width={12} />
                        ) : trendType === "neutral" ? (
                            <Icon height={12} icon={"solar:arrow-right-linear"} width={12} />
                        ) : (
                            <Icon height={12} icon={"solar:arrow-right-down-linear"} width={12} />
                        )
                    }
                    variant={trendChipVariant}
                >
                    {change}
                </Chip>
            </div>
        </CardStyled>
    )
}
