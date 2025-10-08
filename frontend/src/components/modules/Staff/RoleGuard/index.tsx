"use client"

import React, { useEffect, useMemo } from "react"
import { useRouter } from "next/navigation"
import { useProfileStore, useTokenStore } from "@/hooks"
import { ROLE_ADMIN, ROLE_STAFF } from "@/constants/constants"
import { useGetMe } from "@/hooks/queries/useProfile"

type MaybeRole = { name?: string | null } | string | null | undefined

function normalizeRole(role: MaybeRole) {
    if (typeof role === "string" && role.trim().length > 0) {
        return role.trim().toLowerCase()
    }
    if (role && typeof role === "object" && "name" in role) {
        const roleName = role.name
        if (typeof roleName === "string" && roleName.trim().length > 0) {
            return roleName.trim().toLowerCase()
        }
    }
    return undefined
}
//Các trang dành cho staff  tự /chuyển “/” khi reload lại trang
//vì RoleGuard lập tức redirect mỗi khi useProfileStore vẫn chưa có role.
//Ở lần load đầu, store chỉ hydrate (khôi phục dữ liệu) sau khi đã redirect,
// nên guard đánh nhầm một  staff hợp lệ là không có quyền. Gây bất lợi cho trải nghiệm
export default function RoleGuard({ children }: { children: React.ReactNode }) {
    // const { data, isLoading, isFetching } = useAuth()
    // const user = useProfileStore((s) => s.user)
    // const user = useGetMeFromCache()
    const router = useRouter()
    const user = useProfileStore((s) => s.user)
    const setUser = useProfileStore((s) => s.setUser)
    const removeUser = useProfileStore((s) => s.removeUser)
    const accessToken = useTokenStore((s) => s.accessToken)
    const hydrateTokenStore = useTokenStore((s) => s.hydrate)
    const isTokenHydrated = useTokenStore((s) => s.isHydrated)
    //FIX: Guard hiện đợi hydrate token và hoàn tất gọi API /users/me rồi mới xác định role.
    //  Khi dữ liệu trả về, nó cập nhật profile store,
    // và chỉ redirect nếu không có token, request thất bại, hoặc role không phải staff-admin.
    useEffect(() => {
        if (!isTokenHydrated) {
            hydrateTokenStore()
        }
    }, [hydrateTokenStore, isTokenHydrated])

    const {
        data: profile,
        isLoading,
        isFetching,
        isError
    } = useGetMe({ enabled: Boolean(accessToken) })

    useEffect(() => {
        if (profile) {
            setUser(profile)
        }
    }, [profile, setUser])

    const normalizedRole = useMemo(
        () => normalizeRole(profile?.role ?? user?.role),
        [profile?.role, user?.role]
    )
    const isStaff = useMemo(() => {
        if (!normalizedRole) return false
        return (
            normalizedRole === ROLE_STAFF.toLowerCase() ||
            normalizedRole === ROLE_ADMIN.toLowerCase()
        )
    }, [normalizedRole])
    //OUTCOME: Guard đợi hoàn tất kiểm tra auth rồi mới render children hoặc redirect.
    //  Nếu không có token, request thất bại, hoặc role không phải staff-admin, nó redirect về "/".
    //  Ngược lại, nó render children (cho phép truy cập trang staff).
    // Điều này ngăn việc render nhấp nháy nội dung không đúng trước khi redirect.
    // Lưu ý: Trong thời gian chờ kiểm tra auth, guard trả về null (không render gì cả).
    const isCheckingAuth = useMemo(() => {
        if (!isTokenHydrated) return true
        if (!accessToken) return false
        return isLoading || isFetching
    }, [accessToken, isFetching, isLoading, isTokenHydrated])

    useEffect(() => {
        if (isCheckingAuth) return
        if (!accessToken) {
            removeUser()
            router.replace("/")
            return
        }
        if (isError || !isStaff) {
            router.replace("/")
        }
    }, [accessToken, isCheckingAuth, isError, isStaff, removeUser, router])

    if (isCheckingAuth) return null
    if (!isStaff) return null

    return <>{children}</>
}
