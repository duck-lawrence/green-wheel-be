"use client"
import React from "react"
import { ModalBody, ModalContent } from "@heroui/react"
import { useChangePasswordDiscloresureSingleton } from "@/hooks"
import { ModalStyled } from "@/components/"

export function ChangePasswordModal() {
    const { isOpen, onOpenChange, onClose } = useChangePasswordDiscloresureSingleton()

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent className="max-w-150 w-full p-14">
                <ModalBody>{/* <LoginForm onSuccess={onClose} /> */}</ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
