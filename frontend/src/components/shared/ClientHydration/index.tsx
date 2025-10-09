"use client"
import React from "react"
import { useTokenStore } from "@/hooks"
import { useEffect } from "react"
import { Spinner } from "@heroui/react"

export function ClientHydration({ children }: { children: React.ReactNode }) {
    const hydrate = useTokenStore((s) => s.hydrate)
    const isHydrated = useTokenStore((s) => s.isHydrated)

    useEffect(() => {
        hydrate()
    }, [hydrate])

    if (!isHydrated)
        return (
            <div className="text-center">
                <Spinner />
            </div>
        )
    return <>{children}</>
}
