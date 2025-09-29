"use client"
import React from "react"
import { useRegisDiscloresureSingleton } from "@/hooks"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import { ModalStyled } from "@/components/styled"

import { useTranslation } from "react-i18next"

import { RegisForm } from "@/components/shared"

export function RegisModal() {
    const { t } = useTranslation()
    const { isOpen, onOpenChange } = useRegisDiscloresureSingleton() //onclose

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent className="w-full max-w-140">
                <ModalHeader className="flex justify-center items-center text-2xl front-bold mt">
                    Regis
                </ModalHeader>
                <ModalBody>
                    <RegisForm />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
