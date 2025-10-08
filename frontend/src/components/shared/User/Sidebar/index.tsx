"use client"

import React, { useEffect, useMemo, useCallback, useState } from "react"
import { usePathname, useRouter } from "next/navigation"
import { LayoutGroup, motion } from "framer-motion"
import clsx from "clsx"
import i18n from "@/lib/i18n"
import { useGetMe } from "@/hooks"

export type SidebarItem = {
    key: string
    label: string
    href?: string
    onSelect?: () => void | Promise<void>
}

const DEFAULT_TABS: SidebarItem[] = [
    { key: "/profile", label: i18n.t("user.my_profile"), href: "/profile" },
    {
        key: "/profile/rental-contracts",
        label: i18n.t("user.rental_contracts"),
        href: "/profile/rental-contracts"
    },
    {
        key: "/profile/change-password",
        label: i18n.t("auth.change_password"),
        href: "/profile/change-password"
    }
]

export type AccountSidebarProps = {
    items?: SidebarItem[]
    selectedKey?: string
    widthClass?: string
}

export default function AccountSidebar({
    items,
    selectedKey,
    widthClass = "w-50"
}: AccountSidebarProps = {}) {
    const pathname = usePathname()
    const router = useRouter()
    const { data: user } = useGetMe()
    const [pendingKey, setPendingKey] = useState<string | null>(null)

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
        const candidate = selectedKey ?? pathname
        if (candidate && tabs.some((tab) => tab.key === candidate)) {
            return candidate
        }
        return tabs[0]?.key
    }, [selectedKey, pathname, tabs])

    const [activeKey, setActiveKey] = useState<string | undefined>(resolvedSelectedKey)

    useEffect(() => {
        if (selectedKey !== undefined) {
            console.debug("AccountSidebar:selectedKey", selectedKey, "pathname", pathname)
        }
        setActiveKey(resolvedSelectedKey)
    }, [resolvedSelectedKey])

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
        [tabs, pathname, router, resolvedSelectedKey]
    )

    return (
        <LayoutGroup id="account-sidebar">
            <div className="flex flex-col pr-4">
                <div
                    className={clsx(
                        "relative flex flex-col gap-2 rounded-2xl bg-white p-3 shadow-lg shadow-slate-200/70",
                        widthClass
                    )}
                >
                    {tabs.map((item) => {
                        const isActive = activeKey === item.key
                        return (
                            <motion.button
                                key={item.key}
                                type="button"
                                onClick={() => {
                                    void handleSelection(item.key)
                                }}
                                className={clsx(
                                    "relative w-full overflow-hidden rounded-xl px-4 py-3 text-xl font-medium",
                                    "flex items-center justify-center whitespace-nowrap transition-colors duration-150"
                                )}
                                whileHover={{ scale: 1.02 }}
                                whileTap={{ scale: 0.98 }}
                            >
                                {isActive && (
                                    <motion.span
                                        layoutId="account-sidebar-active"
                                        className="absolute inset-0 rounded-xl bg-primary"
                                        transition={{ type: "spring", stiffness: 350, damping: 30 }}
                                    />
                                )}
                                <motion.span
                                    className="relative w-full text-center text-xl font-medium whitespace-nowrap"
                                    animate={{
                                        color: isActive ? "#ffffff" : "#475569",
                                        scale: isActive ? 1 : 0.98
                                    }}
                                    transition={{ type: "spring", stiffness: 300, damping: 22 }}
                                >
                                    {item.label}
                                </motion.span>
                            </motion.button>
                        )
                    })}
                </div>
            </div>
        </LayoutGroup>
    )
}
