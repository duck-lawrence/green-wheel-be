"use client"

import React from "react"
import { useTranslation } from "react-i18next"

export default function StaffTestPage() {
    const { t } = useTranslation()

    return (
        <div className="rounded-2xl bg-white px-6 py-8 shadow-sm">
            <h1 className="text-2xl font-semibold text-gray-900">{t("staff.test_title", { defaultValue: "Test Page" })}</h1>
            <p className="mt-3 text-sm text-gray-600">
                {t("staff.test_description", {
                    defaultValue: "This is a placeholder route for staff testing features."
                })}
            </p>
        </div>
    )
}
