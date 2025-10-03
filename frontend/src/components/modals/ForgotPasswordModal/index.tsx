"use client"
import React from "react"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import { useForgotPasswordDiscloresureSingleton } from "@/hooks"
import { ModalStyled } from "@/components/"
import { ForgotForm } from "./ForgotForm"
import { useTranslation } from "react-i18next"

export function ForgotPasswordModal() {
    const { t } = useTranslation()
    const { isOpen, onOpenChange, onClose } = useForgotPasswordDiscloresureSingleton()

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent className="w-full max-w-140">
                <ModalHeader className="flex justify-center items-center text-2xl front-bold mt">
                    {t("auth.reset_password")}
                </ModalHeader>
                <ModalBody>
                    <ForgotForm onSuccess={onClose} />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
