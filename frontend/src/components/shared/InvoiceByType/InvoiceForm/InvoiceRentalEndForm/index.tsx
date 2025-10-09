"use client"
import React from "react"
import { ButtonStyled, InputStyled, TextareaStyled } from "@/components"
import { Money, Broom, Clock, Wrench } from "@phosphor-icons/react"
import { useDisclosure } from "@heroui/react"
import SeeDetailDamageModal from "@/components/modals/SeeDetailDamageModel"
import { mockInvoices } from "@/data/mockIvoices"

export default function InvoiceRentalEndForm() {
    const { isOpen, onOpen, onOpenChange } = useDisclosure()
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <InputStyled
                label="Phí vệ sinh"
                placeholder="300.000 VND"
                startContent={<Broom size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Phí trễ giờ"
                placeholder="150.000 VND"
                startContent={<Clock size={22} className="text-primary" weight="duotone" />}
                variant="bordered"
            />
            <InputStyled
                label="Phí hư hỏng"
                placeholder="150.000 VND"
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

            {/* <TextareaStyled
                label="Các hạng mục hư hỏng"
                placeholder="- Trầy cản trước\n- Rách ghế da\n- Gãy gương chiếu hậu"
                variant="bordered"
                className="sm:col-span-2"
            /> */}
            <InputStyled
                label="Tổng cộng"
                placeholder="450.000 VND"
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
