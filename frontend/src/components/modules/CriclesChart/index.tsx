"use client"
import React, { useCallback, useEffect, useState } from "react"
import type { ButtonProps } from "@heroui/react"
import { CircleChartStyled } from "@/components/styled"

type ChartData = {
    name: string
    [key: string]: string | number
}
type CircleChartProps = {
    className?: string
    title: string
    color: ButtonProps["color"]
    categories: string[]
    chartData: ChartData[]
    onChange?: (value: string) => void
}
const data: CircleChartProps[] = [
    {
        title: "Traffic Sources",
        categories: ["Search", "Direct", "Social", "Referral"],
        color: "warning",
        chartData: [
            { name: "Search", value: 600 },
            { name: "Direct", value: 300 },
            { name: "Social", value: 300 },
            { name: "Referral", value: 200 }
        ]
    },
    {
        title: "Device Usage",
        categories: ["Mobile", "Desktop", "Tablet", "Smart TV"],
        color: "success",
        chartData: [
            { name: "Mobile", value: 450 },
            { name: "Desktop", value: 300 },
            { name: "Tablet", value: 250 },
            { name: "Smart TV", value: 200 }
        ]
    },
    {
        title: "Browser Usage",
        categories: ["Chrome", "Safari", "Firefox", "Edge"],
        color: "danger",
        chartData: [
            { name: "Chrome", value: 350 },
            { name: "Safari", value: 280 },
            { name: "Firefox", value: 220 },
            { name: "Edge", value: 150 }
        ]
    }
]

export function CriclesChart() {
    const [selectedRanges, setSelectedRanges] = useState<Record<string, string>>({})

    const handleChartChange = useCallback((title: string, value: string) => {
        setSelectedRanges((prev) => ({ ...prev, [title]: value }))
        // console.log(selectedRanges)
    }, [])

    const handleSubmit = useCallback(() => {
        console.log(selectedRanges)
    }, [selectedRanges])

    useEffect(() => {
        handleSubmit()
    }, [handleSubmit])

    return (
        <dl className="grid w-full grid-cols-1 gap-5 sm:grid-cols-2 md:grid-cols-3">
            {data.map((item, index) => (
                <CircleChartStyled
                    key={index}
                    {...item}
                    onChange={(value) => handleChartChange(item.title, value)}
                />
            ))}
        </dl>
    )
}
