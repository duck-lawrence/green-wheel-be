"use client"

import React, { useEffect, useMemo } from "react"
import { useAuth } from "@/hooks/queries/useAuth"
import { useRouter } from "next/navigation"

type MaybeRoleDetail = { name?: string | null } | null | undefined

// helper: collapse various backend role shapes into lowercase string for comparison
function normalizeRole(role?: unknown, roleDetail?: MaybeRoleDetail) {
    if (typeof role === "string" && role.trim().length > 0) {
        const normalized = role.trim().toLowerCase()
        return normalized.replace(/^"+|"+$/g, "").replace(/^'+|'+$/g, "")
    }
    if (roleDetail && typeof roleDetail === "object" && typeof roleDetail.name === "string") {
        return roleDetail.name.trim().toLowerCase()
    }
    return undefined
}

export default function RoleGuard({ children }: { children: React.ReactNode }) {
    const { data, isLoading, isFetching } = useAuth()
    const router = useRouter()

    const isStaff = useMemo(() => {
        return normalizeRole(data?.role, data?.roleDetail) === "staff"
    }, [data?.role, data?.roleDetail])

    const isCheckingAuth = isLoading || isFetching

    if (process.env.NODE_ENV !== "production") {
        console.log("[RoleGuard] state", { data, isLoading, isFetching, isStaff })
    }

    useEffect(() => {
        if (!isCheckingAuth) {
            console.debug("RoleGuard auth state", { rawProfile: data, isStaff })
        }
    }, [isCheckingAuth, data, isStaff])

    useEffect(() => {
        if (isCheckingAuth) return
        if (!isStaff) {
            router.replace("/")
        }
    }, [isCheckingAuth, isStaff, router])

    if (isCheckingAuth) return null
    if (!isStaff) return null

    return <>{children}</>
}

