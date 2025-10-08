"use client"

import { ROLE_STAFF } from "@/constants/constants"
import { useGetMeFromCache } from "@/hooks"
import { Spinner } from "@heroui/react"
import { useRouter } from "next/navigation"
import React, { useEffect, useMemo } from "react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export default function StaffLayout({ children }: { children: React.ReactNode }) {
    const router = useRouter()
    const { t } = useTranslation()
    const user = useGetMeFromCache()

    const isStaff = useMemo(() => {
        return user?.role?.name === ROLE_STAFF
    }, [user])

    useEffect(() => {
        if (!isStaff) {
            toast.dismiss()
            toast.error(t("user.unauthorized"))
            router.replace("/")
        }
    }, [isStaff, router, t])

    if (!isStaff) return <Spinner />

    return <>{children}</>
}
