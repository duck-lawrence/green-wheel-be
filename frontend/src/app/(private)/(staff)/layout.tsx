"use client"

import { StaffSidebar, STAFF_MENU } from "@/components"
import { ROLE_STAFF } from "@/constants/constants"
import { useGetMe } from "@/hooks"
import { Spinner } from "@heroui/react"
import { usePathname, useRouter } from "next/navigation"
import React, { useEffect, useMemo } from "react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export default function StaffLayout({ children }: { children: React.ReactNode }) {
    const router = useRouter()
    const pathname = usePathname()
    const { t } = useTranslation()
    const { data: user, isLoading, isError } = useGetMe({ enabled: true })

    const isStaff = useMemo(() => {
        return user?.role?.name === ROLE_STAFF
    }, [user])

    const selectedPath = useMemo(() => {
        if (!pathname) return STAFF_MENU[0]?.path ?? "/dashboard"

        const matched = STAFF_MENU.reduce<string | undefined>((current, item) => {
            if (pathname === item.path || pathname.startsWith(`${item.path}/`)) {
                if (!current || item.path.length > current.length) {
                    return item.path
                }
            }
            return current
        }, undefined)

        return matched ?? STAFF_MENU[0]?.path ?? pathname
    }, [pathname])

    useEffect(() => {
        if (isLoading) return
        if (isError || !isStaff) {
            toast.dismiss()
            toast.error(t("user.unauthorized"))
            router.replace("/")
        }
    }, [isError, isLoading, isStaff, router, t])

    if (isLoading) return <Spinner />
    if (!isStaff) return <Spinner />

    return (
        <div className="flex w-full max-w-6xl flex-col gap-6 md:flex-row">
            <StaffSidebar selectedPath={selectedPath} />
            <div className="flex-1">{children}</div>
        </div>
    )
}
