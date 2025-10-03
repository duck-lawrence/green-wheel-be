"use client"
import React from "react"
import { Button, ButtonProps, cn } from "@heroui/react"

export function ButtonStyled(props: ButtonProps) {
    return (
        <Button
            color="secondary"
            {...props}
            className={cn("font-medium text-base", props.className)}
        />
    )
}
