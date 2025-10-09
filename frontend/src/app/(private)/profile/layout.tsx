import { ProfileSidebar } from "@/components/shared/SidebarGroup/ProfileSidebar"
import React from "react"

export default function ProfileLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="w-[1200] flex h-full justify-center">
            <ProfileSidebar className="w-64 min-w-[16rem]" />
            <div className="flex-1 px-9 py-6 shadow-2xs rounded-2xl bg-white">{children}</div>
        </div>
    )
}
