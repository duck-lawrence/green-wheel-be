"use client"

import { ROLE_CUSTOMER } from "@/constants/constants"
import { useGetMeFromCache } from "@/hooks"
import { Spinner } from "@heroui/react"
import { useRouter } from "next/navigation"
import React, { useEffect, useMemo } from "react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export default function CustomerLayout({ children }: { children: React.ReactNode }) {
    const router = useRouter()
    const { t } = useTranslation()
    const user = useGetMeFromCache()

    const isCustomer = useMemo(() => {
        return user?.role?.name === ROLE_CUSTOMER
    }, [user])

    useEffect(() => {
        if (!isCustomer) {
            toast.dismiss()
            toast.error(t("user.unauthorized"))
            router.replace("/")
        }
    }, [isCustomer, router, t])

    if (!isCustomer) return <Spinner />

    return <>{children}</>
}
