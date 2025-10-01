"use client"
import React from "react"
import { cn, Modal, ModalProps } from "@heroui/react"

export function ModalStyled(props: ModalProps) {
    return (
        <Modal
            placement="center"
            // scrollBehavior="inside"
            color="secondary"
            isDismissable={false}
            size="lg"
            {...props}
            className={cn(props.className)}
        />
    )
}
