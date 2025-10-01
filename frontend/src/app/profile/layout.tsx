import AccountSidebar from "@/components/shared/User/Sidebar"
import React from "react"

export default function UserLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="w-[1200] flex h-full justify-center mt-20">
            <AccountSidebar className="h-40 w-50 shadow-2xs rounded-2xl bg-white" />
            <div className="flex-1 p-2 shadow-2xs rounded-2xl bg-white">{children}</div>
        </div>
    )
}
