"use client"

import React from "react"
import { useTranslation } from "react-i18next"
import { KeyRound, Car, ReceiptText } from "lucide-react"
import { CardStyled } from "@/components/styled/CardStyled"

const METRICS_CARDS = [
    { key: "pickup_today", title: "Pickup Today", value: 18 },
    { key: "total_car", title: "Total Car", value: 124 },
    { key: "total_contract", title: "Total Contract", value: 2457 }
] as const

type MetricCard = (typeof METRICS_CARDS)[number]

type IconType = typeof KeyRound

const METRIC_ICON_MAP: Record<MetricCard["key"], IconType> = {
    pickup_today: KeyRound,
    total_car: Car,
    total_contract: ReceiptText
}

export default function StaffDashboardPage() {
    const { t } = useTranslation()

    return (
        <div className="rounded-2xl bg-white shadow p-6">
            <div className="mb-6">
                <h1 className="text-3xl mb-3 px-4 font-bold text-gray-900">{t("staff.dashboard_title")}</h1>
            </div>
            {/* <div className="grid gap-5 md:grid-cols-3">
                {METRICS_CARDS.map((card) => {
                    const Icon = METRIC_ICON_MAP[card.key]

                    return (
                        <CardStyled
                            key={card.key}
                            className="rounded-2xl border border-slate-300/80 bg-white p-6 shadow-lg shadow-slate-200/80"
                        >
                            <div className="flex items-center gap-4">
                                <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-primary shadow-sm">
                                    <Icon className="h-5 w-5 text-secondary" strokeWidth={2.75} />
                                </div>
                                <div className="flex flex-1 flex-col items-end">
                                    <p className="text-sm font-medium text-slate-500">{card.title}</p>
                                    <p className="text-3xl font-semibold tracking-tight text-slate-900">{card.value}</p>
                                </div>
                            </div>
                        </CardStyled>
                    )
                })}
            </div> */}
        </div>
    )
}
