"use client"

import React, { useEffect } from "react"
import { Spinner } from "@heroui/react"
import toast from "react-hot-toast"
import { useRouter } from "next/navigation"
import { useTranslation } from "react-i18next"
import { ROLE_ADMIN, ROLE_STAFF } from "@/constants/constants"
import { useGetMe } from "@/hooks"
import AdminDashboard from "./(admin)/AdminDashboard"
import StaffDashboard from "./(staff)/StaffDashboard"

export default function DashboardPage() {
    const router = useRouter()
    const { t } = useTranslation()
    const { data: user, isLoading, isError } = useGetMe({ enabled: true })

    const roleName = user?.role?.name
    const isAdmin = roleName === ROLE_ADMIN
    const isStaff = roleName === ROLE_STAFF

    useEffect(() => {
        if (isLoading) return
        if (isError || (!isAdmin && !isStaff)) {
            toast.dismiss()
            toast.error(t("user.unauthorized"))
            router.replace("/")
        }
    }, [isAdmin, isStaff, isError, isLoading, router, t])

    if (isLoading) {
        return (
            <div className="flex w-full justify-center py-10">
                <Spinner />
            </div>
        )
    }

    if (!isAdmin && !isStaff) {
        return null
    }

    if (isAdmin) return <AdminDashboard />

    return <StaffDashboard />
}
