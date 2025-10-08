import { CreateRentalContractForm, CreateRentalContractFormProps, ModalStyled } from "@/components/"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import React from "react"
import { useTranslation } from "react-i18next"

export function CreateRentalContractModal({
    isOpen,
    onClose,
    props
}: {
    isOpen: boolean
    onClose: () => void
    props: CreateRentalContractFormProps
}) {
    const { t } = useTranslation()

    return (
        <ModalStyled isOpen={isOpen} onClose={onClose}>
            <ModalContent className="min-w-fit px-3 py-2">
                <ModalHeader className=" self-center">{t("car_rental.register_title")}</ModalHeader>
                <ModalBody>
                    <CreateRentalContractForm {...props} />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
