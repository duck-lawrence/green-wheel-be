"use client"
import { cn, NavbarProps, Navbar } from "@heroui/react"
import React from "react"

export default function NavbarStyled(props: NavbarProps) {
    return <Navbar color="primary" {...props} className={cn("text-base", props.className)} />
}
