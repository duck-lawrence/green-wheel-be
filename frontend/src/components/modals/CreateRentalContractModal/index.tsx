import { ModalStyled } from "@/components/"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import React from "react"
import { useTranslation } from "react-i18next"
import { CreateRentalContractForm } from "./CreateRentalContractForm"
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"

export function CreateRentalContractModal({
    isOpen,
    onClose,
    modelViewRes
}: {
    isOpen: boolean
    onClose: () => void
    modelViewRes: VehicleModelViewRes
}) {
    const { t } = useTranslation()

    return (
        <ModalStyled isOpen={isOpen} onClose={onClose}>
            <ModalContent className="min-w-fit px-3 py-2">
                <ModalHeader className=" self-center">{t("car_rental.register_title")}</ModalHeader>
                <ModalBody>
                    <CreateRentalContractForm modelViewRes={modelViewRes} onSuccess={onClose} />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
