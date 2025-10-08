"use client"

import React from "react"
import { useTranslation } from "react-i18next"

export default function StaffReportsPage() {
    const { t } = useTranslation()

    return (
        <div className="rounded-2xl bg-white shadow-sm px-6 py-8 text-center space-y-3">
            <h1 className="text-xl font-semibold text-gray-800">{t("staff.sidebar_reports")}</h1>
            <p className="text-sm text-gray-500">{t("staff.dashboard_subtitle")}</p>
        </div>
    )
}
