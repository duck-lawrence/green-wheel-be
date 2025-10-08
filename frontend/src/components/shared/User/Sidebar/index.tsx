"use client"
import React, { useMemo, useCallback } from "react"
import { Tabs, Tab } from "@heroui/react"
import { usePathname, useRouter } from "next/navigation"
import i18n from "@/lib/i18n"
import { useProfileStore } from "@/hooks"

export type SidebarItem = {
    key: string
    label: string
    href?: string
    onSelect?: () => void | Promise<void>
}

const DEFAULT_TABS: SidebarItem[] = [
    { key: "/profile", label: i18n.t("user.my_profile"), href: "/profile" },
    { key: "/profile/rental-contracts", label: i18n.t("user.rental_contracts"), href: "/profile/rental-contracts" },
    { key: "/profile/change-password", label: i18n.t("auth.change_password"), href: "/profile/change-password" }
]

export type AccountSidebarProps = {
    items?: SidebarItem[]
    selectedKey?: string
}

export default function AccountSidebar({ items, selectedKey }: AccountSidebarProps = {}) {
    const pathname = usePathname()
    const router = useRouter()
    const user = useProfileStore((s) => s.user)

    const normalizedRole = useMemo(() => {
        const roleName =
            typeof user?.role === "object" && user?.role !== null
                ? user.role.name
                : typeof user?.role === "string"
                ? user.role
                : undefined
        if (typeof roleName !== "string") return undefined
        const trimmed = roleName.trim()
        return trimmed.length > 0 ? trimmed.toLowerCase() : undefined
    }, [user?.role])

    const tabs = useMemo(() => {
        if (items) return items
        if (normalizedRole && normalizedRole !== "customer") {
            return DEFAULT_TABS.filter((tab) => tab.key !== "/profile/rental-contracts")
        }
        return DEFAULT_TABS
    }, [items, normalizedRole])

    const resolvedSelectedKey = useMemo(() => {
        const key = selectedKey ?? pathname
        return tabs.find((tab) => tab.key === key) ? key : tabs[0]?.key
    }, [selectedKey, pathname, tabs])

    const handleSelection = useCallback(
        async (key: string | number) => {
            const stringKey = String(key)
            const target = tabs.find((tab) => tab.key === stringKey)
            if (!target) return

            if (target.onSelect) {
                await target.onSelect()
                return
            }

            if (target.href && target.href !== pathname) {
                router.push(target.href)
            }
        },
        [tabs, pathname, router]
    )

    return (
        <div className="flex flex-col pr-4">
            <Tabs
                color="primary"
                variant="bordered"
                aria-label="Options"
                placement="start"
                selectedKey={resolvedSelectedKey}
                isVertical
                size="lg"
                radius="none"
                className="font-medium text-base w-50 rounded-2xl bg-[#ffffff] overflow-hidden"
                classNames={{
                    tabList: "p-0 w-full",
                    tab: "p-0"
                }}
                items={tabs}
                onSelectionChange={handleSelection}
            >
                {(item) => (
                    <Tab
                        key={item.key}
                        title={<span className="block text-center text-xl font-medium w-full">{item.label}</span>}
                        className="block text-xl gap-6 w-full"
                    />
                )}
            </Tabs>
        </div>
    )
}
