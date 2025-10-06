"use client"

import React from "react"
import { useTranslation } from "react-i18next"

export default function StaffDashboardPage() {
    const { t } = useTranslation()

    return (
        <div className="rounded-2xl bg-white shadow p-6">
            <h1 className="text-xl font-semibold">
                {/* removed: Staff dashboard */}
                {t("staff.dashboard_title")}
            </h1>
            <p className="text-gray-600 mt-2">
                {/* removed: Select a module from the sidebar to get started. */}
                {t("staff.dashboard_subtitle")}
            </p>
        </div>
    )
}
