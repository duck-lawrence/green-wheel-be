"use client"
import React from "react"
import RowSteps from "@/components/modules/RowSteps"
import { useTranslation } from "react-i18next"

interface RowStepProps {
    n: number // chỉ nhận giá trị step từ cha
}
export function RowStep({ n }: RowStepProps) {
    const { t } = useTranslation()

    const steps = [
        { title: t("email.label") },
        { title: t("auth.otp") },
        { title: t("login.register") }
    ]

    return (
        <div className="flex justify-center">
            <RowSteps currentStep={n} steps={steps} />
        </div>
    )
}
