"use client"

import React, { useCallback, useEffect } from "react"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { DropdownTrigger, DropdownMenu, DropdownItem, User, Spinner } from "@heroui/react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { DropdownStyle } from "@/components"
import { useGetMe, useLogout, useProfileStore, useTokenStore } from "@/hooks"
import { BackendError } from "@/models/common/response"
import Link from "next/link"
import { DEFAULT_AVATAR_URL } from "@/constants/constants"

type MaybeRoleDetail = { name?: string | null } | null | undefined

function normalizeRole(role?: unknown, roleDetail?: MaybeRoleDetail) {
    if (typeof role === "string" && role.trim().length > 0) {
        const normalized = role.trim().toLowerCase()
        return normalized.replace(/^"+|"+$/g, "").replace(/^'+|'+$/g, "")
    }

    if (role && typeof role === "object") {
        const roleName = (role as { name?: string | null }).name
        if (typeof roleName === "string" && roleName.trim().length > 0) {
            return roleName.trim().toLowerCase()
        }
    }

    if (roleDetail && typeof roleDetail === "object" && typeof roleDetail.name === "string") {
        return roleDetail.name.trim().toLowerCase()
    }

    return undefined
}

type DropdownLinkItem = {
    key: string
    href: string
    label: string
}

export function ProfileDropdown() {
    const { t } = useTranslation()
    const logoutMutation = useLogout({ onSuccess: () => window.location.replace("/") })
    const user = useProfileStore((s) => s.user)
    const setUser = useProfileStore((s) => s.setUser)
    const isLoggedIn = useTokenStore((s) => !!s.accessToken)
    const isStaff = normalizeRole(user?.role, user?.roleDetail) === "staff"
    const dropdownItems: DropdownLinkItem[] = isStaff
        ? [
              {
                  key: "staff_management",
                  href: "/staff",
                  label: t<string>("navbar.staff_management")
              },
              {
                  key: "staff_profile",
                  href: "/staff/profile",
                  label: t<string>("navbar.staff_profile")
              },
              {
                  key: "staff_contracts",
                  href: "/staff/contracts",
                  label: t<string>("navbar.staff_contracts")
              }
          ]
        : [
              {
                  key: "profile",
                  href: "/profile",
                  label: t<string>("user.profile")
              },
              {
                  key: "rental_contracts",
                  href: "/profile/rental-contracts",
                  label: t<string>("user.rental_contracts")
              }
          ]

    const {
        data: userRes,
        isLoading: isGetMeLoading,
        error: getMeError,
        isError: isGetMeError
    } = useGetMe({ enabled: isLoggedIn })

    const handleLogout = useCallback(async () => {
        await logoutMutation.mutateAsync()
    }, [logoutMutation])

    // handle get me success
    useEffect(() => {
        if (userRes) {
            setUser(userRes)
        }
    }, [userRes, setUser])

    // handle get me error
    useEffect(() => {
        if (isGetMeError && getMeError) {
            const error = getMeError as BackendError
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    }, [isGetMeError, getMeError, t])

    if (isGetMeLoading || isGetMeError) return <Spinner />

    return (
        <div className="gap-4 flex items-center">
            <DropdownStyle>
                <DropdownTrigger>
                    <User
                        as="button"
                        avatarProps={{
                            isBordered: true,
                            src: user?.avatarUrl || DEFAULT_AVATAR_URL
                        }}
                        className="transition-transform"
                        name={user?.firstName.trim() || ""}
                        classNames={{
                            name: "text-[16px] font-bold"
                        }}
                    />
                </DropdownTrigger>
                <DropdownMenu aria-label="User Actions" variant="flat">
                    {dropdownItems.map((item) => (
                        <DropdownItem key={item.key} as={Link} href={item.href} className="block">
                            {item.label}
                        </DropdownItem>
                    ))}
                    <DropdownItem
                        key="logout"
                        textValue={t("navbar.logout")}
                        color="danger"
                        onPress={handleLogout}
                    >
                        {t("navbar.logout")}
                    </DropdownItem>
                </DropdownMenu>
            </DropdownStyle>
        </div>
    )
}
