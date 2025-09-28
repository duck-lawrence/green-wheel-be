"use client"
import React from "react"
import { useLoginDiscloresureSingleton } from "@/hooks"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import { ModalStyled } from "@/components/styled"

import { useTranslation } from "react-i18next"
import Login from "@/components/shared/LoginForm"

export function LoginModal() {
    const { t } = useTranslation()
    const { isOpen, onOpenChange } = useLoginDiscloresureSingleton() //onclose

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent>
                <ModalHeader className="flex flex-col gap-1">Login</ModalHeader>
                <ModalBody>
                    <Login />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
