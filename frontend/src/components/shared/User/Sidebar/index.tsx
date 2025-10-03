"use client"
import React from "react"
import { Tabs, Tab } from "@heroui/react"
import Link from "next/link"
import { usePathname } from "next/navigation"
import i18n from "@/lib/i18n"

const tabs = [
    { key: "profile", label: i18n.t("user.my_profile"), href: "/profile" },
    { key: "orders", label: i18n.t("user.booking_history"), href: "/profile/my-orders" },
    // { key: "legal", label: t("user.legal"), href: "/user/legal" },
    {
        key: "changePassword",
        label: i18n.t("auth.change_password"),
        href: "/profile/change-password"
    }
]

export default function AccountSidebar() {
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
                    className={
                        "font-medium text-base h-40 w-50 shadow-2xs bg-white rounded-2xl overflow-hidden"
                    }
                    classNames={{
                        tabList: "p-0 w-full"
                    }}
                >
                    {tabs.map((tab) => (
                        <Tab
                            key={tab.href}
                            title={
                                <Link className={"rounded-none"} href={tab.href}>
                                    {tab.label}
                                </Link>
                            }
                            className={"text-xl gap-6 flex rounded-none w-full"}
                        />
                    ))}
                </Tabs>
            </div>
        </div>
    )
}
