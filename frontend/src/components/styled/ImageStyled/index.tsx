import React from "react"
import Image from "next/image"
import { cn } from "@heroui/react"

export function ImageStyled({
    src,
    alt,
    className,
    width,
    height
}: {
    src: string
    alt: string
    className?: string
    width?: number
    height?: number
}) {
    return (
        <Image
            src={src}
            alt={alt}
            className={cn("object-cover rounded-xl", className)}
            width={width || 500}
            height={height || 500}
        />
    )
}
