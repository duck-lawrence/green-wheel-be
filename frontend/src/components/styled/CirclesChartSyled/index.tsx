"use client"

import type { ButtonProps } from "@heroui/react"
import React, { useEffect } from "react"
import { ResponsiveContainer, PieChart, Pie, Tooltip, Cell } from "recharts"
import { Select, SelectItem, cn } from "@heroui/react"
import { CardStyled } from "../CardStyled"

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

const formatTotal = (total: number) => {
    return total >= 1000 ? `${(total / 1000).toFixed(1)}K` : total
}

export function CircleChartStyled({
    title,
    color,
    categories,
    chartData,
    onChange
}: CircleChartProps) {
    const selectDefault = "per-month"
    useEffect(
        () => onChange?.(selectDefault),
        // eslint-disable-next-line react-hooks/exhaustive-deps
        []
    )

    const handleSelectChange = (keys: any) => {
        const selected = Array.from(keys)[0] as string
        onChange?.(selected)
    }
    return (
        <CardStyled
            className={cn("dark:border-default-100 min-h-[240px] border border-transparent")}
            // {...props}
        >
            <div className="flex flex-col gap-y-2 p-4 pb-0">
                <div className="flex items-center justify-between gap-x-2">
                    <dt>
                        <h3 className="text-small text-default-500 font-medium">{title}</h3>
                    </dt>
                    <div className="flex items-center justify-end gap-x-2">
                        <Select
                            aria-label="Time Range"
                            classNames={{
                                trigger: "min-w-[100px] min-h-7 h-7",
                                value: "text-tiny text-default-500!",
                                selectorIcon: "text-default-500",
                                popoverContent: "min-w-[120px]"
                            }}
                            defaultSelectedKeys={["per-month"]}
                            listboxProps={{
                                itemClasses: {
                                    title: "text-tiny"
                                }
                            }}
                            placeholder="Per Day"
                            size="sm"
                            // onChange={handleSelectChange}
                            onSelectionChange={handleSelectChange}
                        >
                            <SelectItem key="per-week">Per Week</SelectItem>
                            <SelectItem key="per-month">Per Month</SelectItem>
                        </Select>
                    </div>
                </div>
            </div>
            <div className="flex h-full flex-wrap items-center justify-center gap-x-2 lg:flex-nowrap">
                <ResponsiveContainer
                    className="[&_.recharts-surface]:outline-hidden"
                    height={200}
                    width="100%"
                >
                    <PieChart accessibilityLayer margin={{ top: 0, right: 0, left: 0, bottom: 0 }}>
                        <Tooltip
                            content={({ label, payload }) => (
                                <div className="rounded-medium bg-background text-tiny shadow-small flex h-8 min-w-[120px] items-center gap-x-2 px-1">
                                    <span className="text-foreground font-medium">{label}</span>
                                    {payload?.map((p, index) => {
                                        const name = p.name
                                        const value = p.value
                                        const category =
                                            categories.find((c) => c.toLowerCase() === name) ?? name

                                        return (
                                            <div
                                                key={`${index}-${name}`}
                                                className="flex w-full items-center gap-x-2"
                                            >
                                                <div
                                                    className="h-2 w-2 flex-none rounded-full"
                                                    style={{
                                                        backgroundColor: `hsl(var(--heroui-${color}-${
                                                            (index + 1) * 200
                                                        }))`
                                                    }}
                                                />
                                                <div className="text-default-700 flex w-full items-center justify-between gap-x-2 pr-1 text-xs">
                                                    <span className="text-default-500">
                                                        {category}
                                                    </span>
                                                    <span className="text-default-700 font-mono font-medium">
                                                        {formatTotal(value as number)}
                                                    </span>
                                                </div>
                                            </div>
                                        )
                                    })}
                                </div>
                            )}
                            cursor={false}
                        />
                        <Pie
                            animationDuration={1000}
                            animationEasing="ease"
                            data={chartData}
                            dataKey="value"
                            innerRadius="68%"
                            nameKey="name"
                            paddingAngle={-20}
                            strokeWidth={0}
                        >
                            {chartData.map((_, index) => (
                                <Cell
                                    key={`cell-${index}`}
                                    fill={`hsl(var(--heroui-${color}-${(index + 1) * 200}))`}
                                />
                            ))}
                        </Pie>
                    </PieChart>
                </ResponsiveContainer>

                <div className="text-tiny text-default-500 flex w-full flex-col justify-center gap-4 p-4 lg:p-0">
                    {categories.map((category, index) => (
                        <div key={index} className="flex items-center gap-2">
                            <span
                                className="h-2 w-2 rounded-full"
                                style={{
                                    backgroundColor: `hsl(var(--heroui-${color}-${
                                        (index + 1) * 200
                                    }))`
                                }}
                            />
                            <span className="capitalize">{category}</span>
                        </div>
                    ))}
                </div>
            </div>
        </CardStyled>
    )
}
