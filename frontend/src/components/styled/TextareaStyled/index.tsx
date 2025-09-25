"use client"
import React from "react"
import { cn, Textarea, TextAreaProps } from "@heroui/react"

export function TextareaStyled(props: TextAreaProps) {
    return (
        <Textarea variant="bordered" color="primary" {...props} className={cn(props.className)} />
    )
}
