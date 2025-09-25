"use client"

import React from "react"
import { ReactNode, useEffect, useState } from "react"
import i18n from "@/lib/i18n"

export function ClientI18nProvider({ children, locale }: { children: ReactNode; locale: string }) {
    const [ready, setReady] = useState(false)

    useEffect(() => {
        if (i18n.language !== locale) i18n.changeLanguage(locale)
        setReady(true)
    }, [locale])

    // chỉ render children khi i18next đã sẵn sàng
    if (!ready) return null

    return <>{children}</>
}
