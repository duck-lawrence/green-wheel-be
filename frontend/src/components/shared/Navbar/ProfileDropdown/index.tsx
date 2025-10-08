"use client"

import React, { useCallback, useEffect } from "react"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { DropdownTrigger, DropdownMenu, DropdownItem, User, Spinner } from "@heroui/react"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { DropdownStyled } from "@/components"
import { useGetMe, useLogout, useTokenStore } from "@/hooks"
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
    href?: string
    label: string
    color?: "danger"
}

export function ProfileDropdown() {
    const { t } = useTranslation()
    const logoutMutation = useLogout({ onSuccess: () => window.location.replace("/") })
    const isLoggedIn = useTokenStore((s) => !!s.accessToken)

    const {
        data: user,
        isLoading: isGetMeLoading,
        error: getMeError,
        isError: isGetMeError
    } = useGetMe({ enabled: isLoggedIn })

    const roleDetail = (user as Partial<{ roleDetail?: MaybeRoleDetail }> | null | undefined)
        ?.roleDetail
    const isStaff = normalizeRole(user?.role, roleDetail) === "staff"

    const baseItems: DropdownLinkItem[] = isStaff
        ? [
              {
                  key: "staff_management",
                  href: "/dashboard",
                  label: t("navbar.staff_management") as string
              },
              {
                  key: "staff_profile",
                  href: "/profile",
                  label: t("navbar.staff_profile") as string
              },
              {
                  key: "staff_contracts",
                  href: "/dashboard/contracts",
                  label: t("navbar.staff_contracts") as string
              }
          ]
        : [
              { key: "profile", href: "/profile", label: t("user.profile") as string },
              {
                  key: "rental_contracts",
                  href: "/profile/rental-contracts",
                  label: t("user.rental_contracts") as string
              }
          ]

    const dropdownItems: DropdownLinkItem[] = [
        ...baseItems,
        { key: "logout", label: t("navbar.logout") as string, color: "danger" }
    ]

    const handleLogout = useCallback(async () => {
        await logoutMutation.mutateAsync()
    }, [logoutMutation])

    // handle get me error
    useEffect(() => {
        if (isGetMeError && getMeError) {
            const error = getMeError as BackendError
            toast.error(translateWithFallback(t, error.detail))
        }
    }, [isGetMeError, getMeError, t])

    if (isGetMeLoading || isGetMeError) return <Spinner />

    return (
        <div className="gap-4 flex items-center">
            <DropdownStyled>
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
                    {dropdownItems.map((item) =>
                        item.href ? (
                            <DropdownItem
                                key={item.key}
                                as={Link}
                                href={item.href}
                                className="block"
                            >
                                {item.label}
                            </DropdownItem>
                        ) : (
                            <DropdownItem
                                key={item.key}
                                textValue={item.label}
                                color={item.color}
                                onPress={handleLogout}
                            >
                                {item.label}
                            </DropdownItem>
                        )
                    )}
                </DropdownMenu>
            </DropdownStyled>
        </div>
    )
}
