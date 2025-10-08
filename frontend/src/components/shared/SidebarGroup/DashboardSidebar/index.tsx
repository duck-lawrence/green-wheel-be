import { buildTabs, Sidebar, SidebarItem } from "@/components"
import { useGetMe } from "@/hooks"
import React from "react"
import { useTranslation } from "react-i18next"

export function DashboardSidebar({ className }: { className?: string }) {
    const { data: user } = useGetMe()
    const { t } = useTranslation()

    const defaultTabs: SidebarItem[] = [
        { key: "/dashboard", label: t("staff.sidebar_dashboard"), href: "/dashboard" }
    ]

    const staffTabs: SidebarItem[] = [
        {
            key: "/dashboard/handovers",
            label: t("staff.sidebar_handovers"),
            href: "/dashboard/handovers"
        },
        {
            key: "/dashboard/contracts",
            label: t("staff.sidebar_contracts"),
            href: "/dashboard/contracts"
        },
        {
            key: "/dashboard/reports",
            label: t("staff.sidebar_reports"),
            href: "/dashboard/reports"
        },
        {
            key: "/dashboard/test",
            label: t("staff.sidebar_test"),
            href: "/dashboard/test"
        }
    ]

    const adminTabs: SidebarItem[] = []

    const tabs = buildTabs({
        roleName: user?.role?.name,
        defaultTabs,
        staffTabs,
        adminTabs
    })

    return <Sidebar tabs={tabs} className={className} />
}
