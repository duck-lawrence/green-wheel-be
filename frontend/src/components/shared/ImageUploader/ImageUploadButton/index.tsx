"use client"
import React, { useRef } from "react"
import { ButtonStyled } from "@/components/styled"
import { cn } from "@heroui/react"

type ImageUploadButtonProps = {
    onFileSelect: (file: File) => void
    label: string
    accept?: string
    className?: string
}

export function ImageUploadButton({
    onFileSelect,
    label,
    accept = "image/*",
    className = ""
}: ImageUploadButtonProps) {
    const fileRef = useRef<HTMLInputElement>(null)

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0]
        if (file) onFileSelect(file)
    }

    return (
        <div className={cn("w-fit", className)}>
            <input
                ref={fileRef}
                type="file"
                accept={accept}
                onChange={handleChange}
                className="hidden"
            />
            <ButtonStyled
                className="block w-fit bg-transparent"
                onPress={() => fileRef.current?.click()}
            >
                {label}
            </ButtonStyled>
        </div>
    )
}
