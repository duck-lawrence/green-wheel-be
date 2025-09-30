"use client"

import { cn, Dropdown, DropdownProps } from "@heroui/react"
import React from "react"

type DropdownStyleProps = DropdownProps & {
    className?: string
    children: React.ReactNode
}

export function DropdownStyle({ className, children, ...props }: DropdownStyleProps) {
    return (
        <Dropdown
            placement="bottom-start"
            {...props}
            classNames={{
                base: cn(className)
            }}
        >
            {children}
        </Dropdown>
    )
}
