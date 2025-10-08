"use client"

import React, { useMemo } from "react"
import AccountSidebar from "@/components/shared/User/Sidebar"
import { STAFF_MENU } from "@/components/modules/Staff/sidebar.menu"
import { useTranslation } from "react-i18next"
import { useLogout } from "@/hooks/queries/useAuth"
import { useRouter } from "next/navigation"

interface StaffSidebarProps {
    selectedPath: string
}

export default function StaffSidebar({ selectedPath }: StaffSidebarProps) {
    const { t } = useTranslation()
    const router = useRouter()
    const { mutateAsync: triggerLogout } = useLogout({
        onSuccess: () => router.replace("/")
    })

    const items = useMemo(
        () =>
            STAFF_MENU.map((item) => {
                const label = t(item.translationKey ?? "", { defaultValue: item.label })
                return {
                    key: item.path,
                    label,
                    href: item.path
                }
            }),
        [triggerLogout, t]
    )

    return <AccountSidebar items={items} selectedKey={selectedPath} />
}
