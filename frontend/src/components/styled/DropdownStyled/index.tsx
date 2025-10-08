"use client"

import { Dropdown, DropdownProps } from "@heroui/react"
import React from "react"

type DropdownStyleProps = DropdownProps & {
    children: React.ReactNode
}

export function DropdownStyled({ children, ...props }: DropdownStyleProps) {
    return (
        <Dropdown placement="bottom-start" {...props}>
            {children}
        </Dropdown>
    )
}
