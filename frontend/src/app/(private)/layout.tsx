"use client"

import { useTokenStore } from "@/hooks"
import { useRouter } from "next/navigation"
import React, { useEffect } from "react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export default function PrivateLayout({ children }: { children: React.ReactNode }) {
    const router = useRouter()
    const { t } = useTranslation()
    const isLogined = useTokenStore((s) => !!s.accessToken)

    useEffect(() => {
        if (!isLogined) {
            toast.dismiss()
            router.replace("/")
            toast.error(t("login.please_login"))
        }
    }, [isLogined, router, t])

    return <>{children}</>
}
