"use client"
import React from "react"
import { InputStyled, TextareaStyled } from "@/components"
import { Money, WarningCircle, ArrowUDownLeft } from "@phosphor-icons/react"

export default function InvoiceDepositRefundForm() {
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Số tiền cọc ban đầu"
                placeholder="2.000.000 VND"
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Phạt nguội (nếu có)"
                placeholder="0 VND"
                startContent={<WarningCircle size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Tổng sau khi trừ phạt"
                placeholder="2.000.000 VND"
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Số tiền hoàn lại khách hàng"
                placeholder="2.000.000 VND"
                startContent={
                    <ArrowUDownLeft size={22} className="text-primary" weight="duotone" />
                }
                variant="bordered"
            />
            <TextareaStyled
                label="Ghi chú"
                placeholder="Hoàn tiền cọc, không có phạt nguội."
                variant="bordered"
                className="sm:col-span-2"
            />
        </div>
    )
}
