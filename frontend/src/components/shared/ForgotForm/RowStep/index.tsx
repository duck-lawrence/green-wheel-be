"use client"
import React from "react"
import RowSteps from "./RowStepStyled"

interface RowStepProps {
    n: number // chỉ nhận giá trị step từ cha
}
export function RowStep({ n }: RowStepProps) {
    const steps = [{ title: "Email" }, { title: "OTP" }, { title: "Password" }]

    return (
        <div className="flex flex-col gap-4">
            <RowSteps currentStep={n} steps={steps} />
        </div>
    )
}
