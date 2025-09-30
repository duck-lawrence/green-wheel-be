"use client"
import React from "react"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import { useTranslation } from "react-i18next"
import { ModalStyled, RegisterForm } from "@/components"
import { useRegisterDiscloresureSingleton } from "@/hooks"

export function RegisterModal() {
    const { t } = useTranslation()
    const { isOpen, onOpenChange } = useRegisterDiscloresureSingleton() //onclose

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent className="w-full max-w-140">
                <ModalHeader className="flex justify-center items-center text-2xl front-bold mt">
                    {t("login.register")}
                </ModalHeader>
                <ModalBody>
                    <RegisterForm />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
