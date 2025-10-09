"use client"

import { ROLE_STAFF } from "@/constants/constants"
import { useGetMe } from "@/hooks"
import { Spinner } from "@heroui/react"
import { useRouter } from "next/navigation"
import React, { useEffect, useMemo } from "react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export default function StaffLayout({ children }: { children: React.ReactNode }) {
    const router = useRouter()
    const { t } = useTranslation()
    const { data: user, isLoading, isError } = useGetMe({ enabled: true })

    const isStaff = useMemo(() => {
        return user?.role?.name === ROLE_STAFF
    }, [user])

    useEffect(() => {
        if (isLoading) return
        if (isError || !isStaff) {
            toast.dismiss()
            toast.error(t("user.unauthorized"))
            router.replace("/")
        }
    }, [isError, isLoading, isStaff, router, t])

    if (isLoading) return <Spinner />

    return <>{children}</>
}
