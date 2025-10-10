import DetailDamage from "@/components/shared/InvoiceForm/InvoiceReturnForm/DetailDamage"
import { ModalStyled } from "@/components/styled"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import React from "react"

export default function SeeDetailDamageModal({
    isOpen,
    onOpenChange,
    itemDamage
}: {
    isOpen: boolean
    onOpenChange: (isOpen: boolean) => void
    itemDamage: InvoiceViewRes
}) {
    return (
        <ModalStyled
            isOpen={isOpen}
            onOpenChange={onOpenChange}
            // isKeyboardDismissDisabled
            // placement="center"
            // backdrop="blur"
        >
            <ModalContent>
                <ModalHeader className="flex flex-col gap-1">Chi tiết hư hỏng</ModalHeader>
                <ModalBody>
                    <DetailDamage data={itemDamage} />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
