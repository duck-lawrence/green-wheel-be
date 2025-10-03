"use client"
import React from "react"
import { ModalBody, ModalContent } from "@heroui/react"
import { useSetPasswordDiscloresureSingleton } from "@/hooks"
import { ModalStyled } from "@/components/"
import { SetPasswordForm } from "./SetPasswordForm"

export function SetPasswordModal() {
    const { isOpen, onOpenChange, onClose } = useSetPasswordDiscloresureSingleton()

    return (
        <ModalStyled isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent className="max-w-150 w-full p-14">
                <ModalBody>
                    <SetPasswordForm onSuccess={onClose} />
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
