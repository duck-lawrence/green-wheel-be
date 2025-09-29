"use client"
import React from "react"
import { ModalBody, ModalContent } from "@heroui/react"
import { useLoginDiscloresureSingleton } from "@/hooks"
import { ModalStyled, LoginForm } from "@/components/"

export function LoginModal() {
    const { isOpen, onOpenChange, onClose } = useLoginDiscloresureSingleton()

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent className="max-w-150 w-full p-14">
                {/* <ModalHeader className="flex flex-col gap-1">{t("login.login")}</ModalHeader> */}
                <ModalBody>
                    <LoginForm onSuccess={onClose} />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
