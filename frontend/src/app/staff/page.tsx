"use client"

import React from "react"
import { useTranslation } from "react-i18next"
import { KeyRound, Car, ReceiptText } from "lucide-react"
import { CardStyled } from "@/components/styled/CardStyled"

const METRICS_CARDS = [
    { key: "pickup_today", color: "sage", icon: KeyRound, title: "Pickup Today", value: "18" },
    { key: "total_car", color: "cream", icon: Car, title: "Total Car", value: "124" },
    { key: "total_contract", color: "coral", icon: ReceiptText, title: "Total Contract", value: "2,457" }
] as const

export default function StaffDashboardPage() {
    const { t } = useTranslation()

    return (
        <div className="rounded-2xl bg-white shadow p-6">
            <div className="mb-6">
                <h1 className="text-xl font-semibold text-gray-900">{t("staff.dashboard_title")}</h1>
                {/* <p className="text-gray-600 mt-2">{t("staff.dashboard_subtitle")}</p> */}
            </div>
            <div className="grid gap-5 md:grid-cols-3 ">
                {METRICS_CARDS.map((card) => {
                    const Icon = card.icon
                    return (
                        <CardStyled key={card.key} color={card.color} className="rounded-2xl flex flex-col gap-3 border-2">
                            <div className="h-10 w-10 rounded-lg bg-white/90 flex items-center justify-center shadow-sm">
                                <Icon className="h-5 w-5 text-gray-800" strokeWidth={1.75} />
                            </div>
                            <div>
                                <p className="text-3xl font-semibold tracking-tight text-black">{card.value}</p>
                                <p className="text-black/80 text-base font-medium">{card.title}</p>
                            </div>
                        </CardStyled>
                    )
                })}
            </div>
        </div>
    )
}
