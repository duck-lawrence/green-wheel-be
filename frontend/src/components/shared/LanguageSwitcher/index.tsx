"use client"
import { ButtonStyled } from "@/components/styled"
import React from "react"
import { useTranslation } from "react-i18next"

export function LanguageSwitcher() {
    const { i18n } = useTranslation()

    const switchLang = (lang: string) => {
        if (i18n.language !== lang) {
            i18n.changeLanguage(lang)
        }
    }

    return (
        <div className="w-full flex justify-end">
            {i18n.language !== "en" && (
                <ButtonStyled onPress={() => switchLang("en")}>VI</ButtonStyled>
            )}
            {i18n.language !== "vi" && (
                <ButtonStyled onPress={() => switchLang("vi")}>EN</ButtonStyled>
            )}
        </div>
    )
}
