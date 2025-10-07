import { TrendCardStyled } from "@/components/styled"
import React from "react"

type TrendCardProps = {
    title: string
    value: string
    change: string
    changeType: "positive" | "neutral" | "negative" // đổi màu changeType
    trendType: "up" | "neutral" | "down" // đổi dấu mũi tên changeType
    trendChipPosition?: "top" | "bottom" // đổi vị trí changeType
    trendChipVariant?: "flat" | "light" // bg changeType
}
const data: TrendCardProps[] = [
    {
        title: "Total Revenue",
        value: "$228,451",
        change: "33%",
        changeType: "neutral",
        trendType: "neutral",
        trendChipPosition: "top",
        trendChipVariant: "flat"
    },
    {
        title: "Total Expenses",
        value: "$71,887",
        change: "13.0%",
        changeType: "negative",
        trendType: "up"
    },
    {
        title: "Total Profit",
        value: "$156,540",
        change: "0.0%",
        changeType: "neutral",
        trendType: "neutral"
    },
    {
        title: "New Customers",
        value: "1,234",
        change: "1.0%",
        changeType: "positive",
        trendType: "up"
    }
]

export function KpiStat() {
    return (
        <dl className="grid w-full grid-cols-1 gap-5 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
            {data.map((props, index) => (
                <TrendCardStyled key={index} {...props} />
            ))}
        </dl>
    )
}
