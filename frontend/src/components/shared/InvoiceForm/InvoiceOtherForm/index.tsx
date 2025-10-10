"use client"
import React from "react"
import { InputStyled, TextareaStyled } from "@/components"
import { Wrench, Money } from "@phosphor-icons/react"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { getOtherNotes } from "@/utils/helpers/getDamageNotes"
import { formatCurrency } from "@/utils/helpers/currency"
import { InvoiceItemType } from "@/constants/enum"

export default function InvoiceOtherForm({ invoice }: { invoice: InvoiceViewRes }) {
    const totalOther = invoice.items
        .filter((item) => item.type === InvoiceItemType.Other)
        .reduce((sum, item) => sum + item.subTotal, 0)

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Nguyên nhân sự cố"
                value={getOtherNotes(invoice)}
                placeholder="Hư bánh xe / Lỗi động cơ / Tai nạn nhẹ..."
                startContent={<Wrench size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
                className="sm:col-span-2"
            />
            <InputStyled
                label="Chi phí sửa chữa / cứu hộ"
                placeholder="1.500.000 VND"
                value={formatCurrency(totalOther)}
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
