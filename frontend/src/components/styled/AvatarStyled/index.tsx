"use client"
import { Avatar } from "@heroui/react"
import React from "react"

type AvatarStyledProps = {
    props?: object
    img: string
    name: string
}

export function AvaterStyled({ img, name, props }: AvatarStyledProps) {
    return (
        <div className="flex gap-4 items-center">
            <div>
                <Avatar
                    isBordered
                    color="default"
                    className="w-30 h-30 text-large"
                    src={img}
                    {...props}
                    // className={cn("font-medium text-base", props.className)}
                />
            </div>
            <span className="text-3xl">{name}</span>
        </div>
    )
}
