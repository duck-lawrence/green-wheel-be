"use client"
import RowSteps from "@/components/modules/RowSteps"
import React from "react"
import { useTranslation } from "react-i18next"

interface RowStepProps {
    n: number // chỉ nhận giá trị step từ cha
}
export function RowStep({ n }: RowStepProps) {
    const { t } = useTranslation()

    const steps = [
        { title: t("auth.email") },
        { title: t("auth.otp") },
        { title: t("auth.password") }
    ]

    return (
        <div className="flex justify-center">
            <RowSteps currentStep={n} steps={steps} />
        </div>
    )
}
