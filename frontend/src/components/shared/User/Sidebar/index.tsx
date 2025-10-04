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
            <Tabs
                color="primary"
                variant="bordered"
                aria-label="Options"
                placement="start"
                selectedKey={pathname}
                isVertical={true}
                size="lg"
                radius="none"
                className={"font-medium text-base w-50 rounded-2xl bg-[#ffffff] overflow-hidden"}
                classNames={{
                    tabList: "p-0 w-full",
                    tab: "p-0"
                }}
                items={tabs}
            >
                {(item) => (
                    <Tab
                        key={item.href}
                        title={
                            <Link href={item.href} className="block">
                                {item.label}
                            </Link>
                        }
                        className="block text-xl gap-6 w-full"
                    />
                )}
            </Tabs>
        </div>
    )
}
