"use client"
import React from "react"
import { Card, CardProps, cn } from "@heroui/react"

type CardColor = "primary" | "secondary" | "danger" | "sage" | "cream" | "coral"

type CardStyledProps = CardProps & {
    color?: CardColor
}

const COLOR_MAP = {
    primary: "bg-[#00A63E]",
    secondary: "bg-[#f4f4f4]",
    danger: "bg-[#FF6B6B]",
    sage: "bg-[#BBDEA4]",
    cream: "bg-[#F0EBD6]",
    coral: "bg-[#E58E61]"
} satisfies Record<CardColor, string>

export function CardStyled({ color, className, ...rest }: CardStyledProps) {
    return (
        <Card {...rest} className={cn("p-4 space-y-2 text-black", color && COLOR_MAP[color], className)} />
    )
}
