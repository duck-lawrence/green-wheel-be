"use client"
import React from "react"
import { InputStyled, TextareaStyled } from "@/components"
import { Money, WarningCircle, ArrowUDownLeft } from "@phosphor-icons/react"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { formatCurrency } from "@/utils/helpers/currency"
import { InvoiceItemType } from "@/constants/enum"
import { getDamageNotes } from "@/utils/helpers/getDamageNotes"

export default function InvoiceRefundForm({ invoice }: { invoice: InvoiceViewRes }) {
    const deposit = invoice.deposit?.amount ?? 0
    const penalty =
        invoice.items.find((item) => item.type === InvoiceItemType.Penalty)?.subTotal ?? 0

    const total = deposit - penalty
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Số tiền cọc ban đầu"
                value={formatCurrency(invoice.deposit?.amount ?? 0)}
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Phạt nguội (nếu có)"
                value={formatCurrency(penalty)}
                startContent={<WarningCircle size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Tổng sau khi trừ phạt"
                value={formatCurrency(total)}
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Số tiền hoàn lại khách hàng"
                value={formatCurrency(total)}
                startContent={
                    <ArrowUDownLeft size={22} className="text-primary" weight="duotone" />
                }
                variant="bordered"
            />
            <TextareaStyled
                label="Ghi chú"
                placeholder="Hoàn tiền cọc, không có phạt nguội."
                value={getDamageNotes(invoice)}
                variant="bordered"
                className="sm:col-span-2"
            />
        </div>
    )
}
