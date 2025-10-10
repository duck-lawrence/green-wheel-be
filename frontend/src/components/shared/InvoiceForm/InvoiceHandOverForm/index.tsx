"use client"
import React from "react"
import { InputStyled, TextareaStyled } from "@/components"
import { Money, ClipboardText, Receipt, Percent } from "@phosphor-icons/react"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { formatCurrency } from "@/utils/helpers/currency"
export default function InvoiceHandOvertForm({ invoice }: { invoice: InvoiceViewRes }) {
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Tiền cọc"
                placeholder="2.000.000 VND"
                value={
                    invoice.deposit?.amount != null
                        ? formatCurrency(invoice.deposit.amount)
                        : undefined
                }
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Tiền thuê xe"
                placeholder="8.000.000 VND"
                value={formatCurrency(invoice.subtotal)}
                startContent={<Receipt size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Thuế VAT (10%)"
                placeholder="800.000 VND"
                value={formatCurrency(invoice.tax)}
                startContent={<Percent size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Tổng cộng"
                placeholder="10.800.000 VND"
                value={formatCurrency(invoice.total)}
                startContent={<ClipboardText size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <TextareaStyled
                label="Ghi chú"
                placeholder="Thanh toán khi nhận xe (cọc + tiền thuê + thuế)."
                value={invoice.notes}
                variant="bordered"
                className="sm:col-span-2"
            />
        </div>
    )
}
