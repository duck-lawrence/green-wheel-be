"use client"
import React from "react"
import { InputStyled, TextareaStyled } from "@/components"
import { Money, CalendarBlank, ClipboardText } from "@phosphor-icons/react"

export default function InvoiceForm({ typeLabel, code }: { typeLabel: string; code: string }) {
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Mã hóa đơn"
                placeholder={code}
                startContent={<ClipboardText size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Loại hóa đơn"
                placeholder={typeLabel}
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Ngày tạo"
                type="date"
                startContent={<CalendarBlank size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Số tiền"
                placeholder="2.500.000 VND"
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <TextareaStyled
                label="Ghi chú"
                placeholder="Khách thanh toán đợt đầu..."
                variant="bordered"
                className="sm:col-span-2"
            />
        </div>
    )
}
