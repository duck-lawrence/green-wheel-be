"use client"
import React from "react"
import { useToken } from "@/hooks"
import { useEffect } from "react"
import { Spinner } from "@heroui/react"

export function ClientHydration({ children }: { children: React.ReactNode }) {
    const hydrate = useToken((s) => s.hydrate)
    const isHydrated = useToken((s) => s.isHydrated)

    useEffect(() => {
        hydrate()
    }, [hydrate])

    if (!isHydrated) return <Spinner />
    return <>{children}</>
}
