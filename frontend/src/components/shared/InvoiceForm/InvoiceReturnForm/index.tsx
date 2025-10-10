"use client"
import React from "react"
import { ButtonStyled, InputStyled, TextareaStyled } from "@/components"
import { Money, Broom, Clock, Wrench } from "@phosphor-icons/react"
import { useDisclosure } from "@heroui/react"
import SeeDetailDamageModal from "@/components/modals/SeeDetailDamageModel"
import { mockInvoices } from "@/data/mockIvoices"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { formatCurrency } from "@/utils/helpers/currency"
import { InvoiceItemType } from "@/constants/enum"

export default function InvoiceReturnForm({ invoice }: { invoice: InvoiceViewRes }) {
    const { isOpen, onOpen, onOpenChange } = useDisclosure()
    const clean = formatCurrency(
        invoice.items.find((item) => item.type === InvoiceItemType.Cleaning)?.subTotal ?? 0
    )
    const lateReturn = formatCurrency(
        invoice.items.find((item) => item.type === InvoiceItemType.LateReturn)?.subTotal ?? 0
    )
    const totalDamage = invoice.items
        .filter((i) => i.type === InvoiceItemType.Damage)
        .reduce((sum, i) => sum + i.subTotal, 0)

    const total = clean + lateReturn + totalDamage

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Phí vệ sinh"
                placeholder="300.000 VND"
                value={clean}
                startContent={<Broom size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Phí trễ giờ"
                placeholder="150.000 VND"
                value={lateReturn}
                startContent={<Clock size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Phí hư hỏng"
                placeholder="150.000 VND"
                value={formatCurrency(totalDamage)}
                startContent={<Wrench size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <div className="mt-1 flex justify-center">
                <ButtonStyled
                    onPress={onOpen}
                    size="lg"
                    color="primary"
                    className="px-8 py-3 font-semibold text-white rounded-xl 
              bg-gradient-to-r from-primary to-teal-400 
              hover:from-teal-500 hover:to-green-400 
              shadow-md transition-all duration-300"
                >
                    Xem chi tiết hư hỏng
                </ButtonStyled>
            </div>

            <InputStyled
                label="Tổng cộng"
                placeholder="450.000 VND"
                value={total}
                startContent={<Money size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
                className="sm:col-span-2"
            />
            <TextareaStyled
                label="Ghi chú"
                placeholder="Thanh toán khi trả xe, gồm phí vệ sinh + trễ + hư hỏng."
                variant="bordered"
                className="sm:col-span-2"
            />

            <SeeDetailDamageModal
                isOpen={isOpen}
                onOpenChange={onOpenChange}
                itemDamage={mockInvoices[0]}
            />
        </div>
    )
}
