"use client"
import { cn, NavbarProps, Navbar } from "@heroui/react"
import React from "react"
import "./index.css"

export function NavbarStyled(props: NavbarProps) {
    return (
        <Navbar
            {...props}
            className={cn("backdrop-blur-none navbar-no-saturate text-base", props.className)}
        />
    )
}
