"use client"

import React, { useRef } from "react"
import { ButtonStyled } from "@/components/styled"
import { useTranslation } from "react-i18next"

export function AvatarUploadButton({ onFileSelect }: { onFileSelect: (file: File) => void }) {
    const { t } = useTranslation()
    const fileInputRef = useRef<HTMLInputElement>(null)

    const handleClick = () => fileInputRef.current?.click()
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0]
        if (file) onFileSelect(file)
    }

    return (
        <div className="w-fit">
            {/* real input */}
            <input
                ref={fileInputRef}
                type="file"
                accept="image/*"
                onChange={handleChange}
                className="hidden"
            />

            {/* fake input */}
            <ButtonStyled className="block w-fit bg-transparent" onPress={handleClick}>
                {t("user.upload_avatar")}
            </ButtonStyled>
        </div>
    )
}
