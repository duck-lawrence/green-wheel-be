"use client"

import React, { useCallback, useEffect } from "react"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { DropdownTrigger, DropdownMenu, DropdownItem, User, Spinner } from "@heroui/react"
import Link from "next/link"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { DropdownStyle } from "@/components"
import { useGetMe, useLogout, useProfileStore, useToken } from "@/hooks"
import { BackendError } from "@/models/common/response"

export function ProfileDropdown() {
    const defaultAvatarUrl = "/images/avtFallback.jpg"
    const { t } = useTranslation()
    const logoutMutation = useLogout({ onSuccess: undefined })
    const user = useProfileStore((s) => s.user)
    const setUser = useProfileStore((s) => s.setUser)
    const isLoggedIn = useToken((s) => !!s.accessToken)

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

    if (isGetMeLoading) return <Spinner />

    return (
        <div className="gap-4">
            <DropdownStyle>
                <DropdownTrigger>
                    <User
                        as="button"
                        avatarProps={{
                            isBordered: true,
                            src: user?.avatarUrl || defaultAvatarUrl
                        }}
                        className="transition-transform"
                        name={user?.firstName.trim() || ""}
                        classNames={{
                            name: "text-[16px] font-bold"
                        }}
                    />
                </DropdownTrigger>
                <DropdownMenu aria-label="User Actions" variant="flat">
                    <DropdownItem key="profile" textValue={t("user.profile")}>
                        <Link href="/profile">{t("user.profile")}</Link>
                    </DropdownItem>
                    <DropdownItem key="team_settings" textValue={t("user.booking_history")}>
                        <Link href="/#">{t("user.booking_history")}</Link>
                    </DropdownItem>
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
