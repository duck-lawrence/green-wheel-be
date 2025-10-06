import AccountSidebar from "@/components/shared/User/Sidebar"
import React from "react"

export default function UserLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="w-[1200] flex h-full justify-center">
            <AccountSidebar />
            <div className="flex-1 px-9 py-6 shadow-2xs rounded-2xl bg-white">{children}</div>
        </div>
    )
}
