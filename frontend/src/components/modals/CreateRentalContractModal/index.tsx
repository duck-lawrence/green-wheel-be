import { ModalStyled } from "@/components/"
import { ModalBody, ModalContent } from "@heroui/react"
import React, { useCallback } from "react"
import { CreateRentalContractForm } from "./CreateRentalContractForm"
import { VehicleModelViewRes } from "@/models/vehicle/schema/response"
import { useRouter } from "next/navigation"

export function CreateRentalContractModal({
    isOpen,
    onClose,
    totalDays,
    totalPrice,
    modelViewRes
}: {
    isOpen: boolean
    onClose: () => void
    totalDays: number
    totalPrice: number
    modelViewRes: VehicleModelViewRes
}) {
    const router = useRouter()
    const handleSuccess = useCallback(() => {
        router.push("/vehicle-rental")
        onClose()
    }, [onClose, router])

    return (
        <ModalStyled isOpen={isOpen} onClose={onClose} isKeyboardDismissDisabled>
            <ModalContent className="min-w-fit px-3 py-2">
                {/* <ModalHeader className=" self-center">{t("car_rental.register_title")}</ModalHeader> */}
                <ModalBody>
                    <CreateRentalContractForm
                        onSuccess={handleSuccess}
                        totalDays={totalDays}
                        totalPrice={totalPrice}
                        modelViewRes={modelViewRes}
                    />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
