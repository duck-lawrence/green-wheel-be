"use client"
import React from "react"
import { Tabs, Tab, TabsProps, cn } from "@heroui/react"
import Link from "next/link"
import { usePathname } from "next/navigation"

const tabs = [
    { key: "profile", label: "Tài khoản của tôi", href: "/user/profile" },
    { key: "orders", label: "Đơn hàng của tôi", href: "/user/my-orders" },
    // { key: "legal", label: "Điều khoản và pháp lý", href: "/user/legal" },
    { key: "changePassword", label: "Đổi mật khẩu", href: "/user/change-password" }
]

export default function AccountSidebar(props: TabsProps) {
    const pathname = usePathname()

    return (
        <div className="flex flex-col pr-4">
            <div className="flex w-full flex-col">
                <Tabs
                    color="primary"
                    variant="underlined"
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
                                "text-xl gap-6 flex ",
                                tab.key === "changePassword" ? "w-38" : "w-49"
                            )}
                        />
                    ))}
                </Tabs>
            </div>
        </div>
    )
}
