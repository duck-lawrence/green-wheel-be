"use client"
import { DropdownStyle } from "@/components"
import { DropdownTrigger, DropdownMenu, DropdownItem, User } from "@heroui/react"
import Link from "next/link"
import React from "react"
import { useTranslation } from "react-i18next"

type ProfileDropdownProps = {
    // props: object
    name: string
    img?: string
    onLogout: () => void
}

export function ProfileDropdown({ name, img, onLogout }: ProfileDropdownProps) {
    const { t } = useTranslation()
    const defaultAvatarUrl = "/images/avtFallback.jpg"

    return (
        <div className="flex items-center gap-4">
            <DropdownStyle>
                <DropdownTrigger>
                    <User
                        as="button"
                        avatarProps={{
                            isBordered: true,
                            src: img || defaultAvatarUrl
                        }}
                        className="transition-transform"
                        name={name}
                        classNames={{
                            name: "text-[16px] font-bold" // chỉnh style cho name
                            // description: "text-sm text-gray-400" // nếu bạn có description
                        }}
                    />
                </DropdownTrigger>
                <DropdownMenu aria-label="User Actions" variant="flat">
                    <DropdownItem key="profile" textValue={t("user.profile")}>
                        <Link href="/#">{t("user.profile")}</Link>
                    </DropdownItem>
                    <DropdownItem key="team_settings" textValue={t("user.booking_history")}>
                        <Link href="/#">{t("user.booking_history")}</Link>
                    </DropdownItem>
                    <DropdownItem
                        key="logout"
                        textValue={t("navbar.logout")}
                        color="danger"
                        onPress={onLogout}
                    >
                        {t("navbar.logout")}
                    </DropdownItem>
                </DropdownMenu>
            </DropdownStyle>
        </div>
    )
}
