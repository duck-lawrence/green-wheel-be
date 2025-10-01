"use client"
import React from "react"
import { Tabs, Tab, TabsProps, cn } from "@heroui/react"
import Link from "next/link"
import { usePathname } from "next/navigation"
import i18n from "@/lib/i18n"

const tabs = [
    { key: "profile", label: i18n.t("user.my_profile"), href: "/profile" },
    { key: "orders", label: i18n.t("user.booking_history"), href: "/profile/my-orders" },
    // { key: "legal", label: t("user.legal"), href: "/user/legal" },
    {
        key: "changePassword",
        label: i18n.t("user.change_password"),
        href: "/profile/change-password"
    }
]

export default function AccountSidebar(props: TabsProps) {
    const pathname = usePathname()

    return (
        <div className="flex flex-col pr-4">
            <div className="flex w-full flex-col justify-start items-start">
                <Tabs
                    color="primary"
                    variant="light"
                    aria-label="Options"
                    placement="start"
                    selectedKey={pathname}
                    {...props}
                    className={cn("font-medium text-base", props.className)}
                >
                    {tabs.map((tab) => (
                        <Tab
                            key={tab.href}
                            title={<Link href={tab.href}>{tab.label}</Link>}
                            className={cn(
                                "text-xl gap-6 flex w-49"
                                // tab.key === "changePassword" ? "w-49" : "w-49"
                            )}
                        />
                    ))}
                </Tabs>
            </div>
        </div>
    )
}
