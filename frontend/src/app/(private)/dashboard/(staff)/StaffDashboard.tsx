"use client"

import React from "react"
import { useTranslation } from "react-i18next"

export default function StaffDashboard() {
    const { t } = useTranslation()

    return (
        <div className="rounded-2xl bg-white p-6 shadow">
            <div className="mb-6">
                <h1 className="mb-3 px-4 text-3xl font-bold text-gray-900">
                    {t("staff.dashboard_title")}
                </h1>
                <p className="px-4 text-base text-slate-500">{t("staff.dashboard_subtitle")}</p>
            </div>
        </div>
    )
}
