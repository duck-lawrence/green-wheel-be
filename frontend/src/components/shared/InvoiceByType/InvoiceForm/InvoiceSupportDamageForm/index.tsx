"use client"
import React from "react"
import { InputStyled, TextareaStyled } from "@/components"
import { Wrench, CalendarBlank, Money, ClipboardText } from "@phosphor-icons/react"

export default function InvoiceSupportDamageForm() {
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Nguyên nhân sự cố"
                placeholder="Hư bánh xe / Lỗi động cơ / Tai nạn nhẹ..."
                startContent={<Wrench size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
                className="sm:col-span-2"
            />
            {/* <InputStyled
                label="Ngày phát sinh"
                type="date"
                startContent={<CalendarBlank size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            /> */}
            {/* <TextareaStyled
                label="Mô tả chi tiết"
                placeholder="Xe bị thủng lốp do cán đinh tại Km 25, cần cứu hộ..."
                variant="bordered"
                className="sm:col-span-2"
            /> */}
            <InputStyled
                label="Chi phí sửa chữa / cứu hộ"
                placeholder="1.500.000 VND"
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
                className="sm:col-span-2"
            />
            <TextareaStyled
                label="Ghi chú"
                placeholder="Chi phí do khách hàng chịu (ngoài hợp đồng)."
                variant="bordered"
                className="sm:col-span-2"
            />
        </div>
    )
}
