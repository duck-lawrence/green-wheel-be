"use client"

import React from "react"
import RoleGuard from "@/components/modules/Staff/RoleGuard"
import StaffSidebar from "@/components/modules/Staff/Sidebar"
import { usePathname } from "next/navigation"

export default function StaffLayout({ children }: { children: React.ReactNode }) {
    const pathname = usePathname() ?? "/staff"

    return (
        <RoleGuard>
            <div className="min-h-screen w-full">
                {/* note: global navbar handles top chrome; push content down for visual breathing room */}
                <div className="max-w-6xl mx-auto mt-[50px] flex gap-8 px-6 pb-16">
                    <StaffSidebar selectedPath={pathname} />
                    <main className="flex-1">{children}</main>
                </div>
            </div>
            <div id="portal-modals" />
        </RoleGuard>
    )
}
