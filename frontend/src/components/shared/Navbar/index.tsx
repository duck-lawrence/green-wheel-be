/* eslint-disable indent */
"use client"
import React, { useCallback, useEffect, useState } from "react"
import { NavbarBrand, NavbarContent, NavbarItem, Link, Spinner } from "@heroui/react"
import { useTranslation } from "react-i18next"
import { ButtonStyled, NavbarStyled, ProfileDropdown, LanguageSwitcher } from "@/components/"
import { useLoginDiscloresureSingleton, useProfileStore, useToken } from "@/hooks"
import { useLogout } from "@/hooks/queries/useAuth"
import { useGetMe } from "@/hooks/queries/useProfile"

export const AcmeLogo = () => {
    return (
        <svg fill="none" height="36" viewBox="0 0 32 32" width="36">
            <path
                clipRule="evenodd"
                d="M17.6482 10.1305L15.8785 7.02583L7.02979 22.5499H10.5278L17.6482 10.1305ZM19.8798 14.0457L18.11 17.1983L19.394 19.4511H16.8453L15.1056 22.5499H24.7272L19.8798 14.0457Z"
                fill="currentColor"
                fillRule="evenodd"
            />
        </svg>
    )
}

export function Navbar() {
    type NavbarState = "default" | "top" | "middle"
    const { t } = useTranslation()
    /*xử lí navbar */
    const [scrollState, setScroledState] = useState<NavbarState>("default")
    const [isHiddenNavbar, setIsHiddenNavbar] = useState(false)
    const [lastScrollY, setLastScrollY] = useState(0)
    // state lưu menu đang chọn
    const [activeMenu, setActiveMenu] = useState("")
    // xứ lí khi login thì hiện icon user
    const isLoggedIn = useToken((s) => !!s.accessToken)
    const { onOpen: onOpenLogin } = useLoginDiscloresureSingleton()
    const user = useProfileStore((s) => s.user)
    const { isLoading: isGetMeLoading } = useGetMe({ enabled: isLoggedIn })
    const logoutMutation = useLogout({ onSuccess: undefined })

    // handle navbar animation
    const baseClasses = `
        transition-all duration-400 ease-in-out
        mt-3 
        fixed left-0 w-full z-50
        mx-auto max-w-7xl
        data-[visible=false]:mt-0
        rounded-3xl
        ${isHiddenNavbar ? "-translate-y-full opacity-0" : "translate-y-0 opacity-100"}
        ${
            scrollState === "top" || scrollState === "middle"
                ? "rounded-3xl bg-[#4A9782] opacity-97 justify-between mx-auto max-w-3xl scale-95"
                : "max-w-7xl scale-100"
        }
    `
    const itemClasses = [
        "flex",
        "relative",
        "h-full",
        "items-center",
        "data-[active=true]:after:content-['']",
        "data-[active=true]:after:absolute",
        "data-[active=true]:after:bottom-0",
        "data-[active=true]:after:left-0",
        "data-[active=true]:after:right-0",
        "data-[active=true]:after:h-[2px]",
        "data-[active=true]:after:rounded-[2px]",
        "data-[active=true]:after:bg-primary"
    ]
    const menus = [
        { key: "home", label: t("navbar.home") },
        { key: "self-drive", label: t("navbar.self_drive") },
        { key: "about", label: t("navbar.about_us") },
        { key: "contact", label: t("navbar.contact") }
    ]

    // func
    const handleLogout = useCallback(async () => {
        await logoutMutation.mutateAsync()
    }, [logoutMutation])

    useEffect(() => {
        const handleScroll = () => {
            const y = window.scrollY
            let nextScrollState: NavbarState
            // let nextHidden: boolean

            if (y >= 0 && y < 10) {
                nextScrollState = "default"
                // nextHidden = false
            } else if (y >= 10 && y < 600) {
                nextScrollState = "top"
                // nextHidden = false
            } else {
                nextScrollState = "middle"
                // nextHidden = true
            }

            setScroledState((prev) => (prev === nextScrollState ? prev : nextScrollState))
            // setIsHiddenNavbar((prev) =>
            //     prev === nextHidden ? prev : nextHidden
            // )

            if (y < 600) {
                setIsHiddenNavbar(false)
            } else {
                if (y > lastScrollY) {
                    // scroll xuống
                    setIsHiddenNavbar(true)
                } else {
                    // scroll lên
                    setIsHiddenNavbar(false)
                }
            }
            setLastScrollY(y)
        }

        window.addEventListener("scroll", handleScroll, { passive: true })

        return () => window.removeEventListener("scroll", handleScroll)
    }, [lastScrollY])

    return (
        <NavbarStyled
            data-visible={!isHiddenNavbar}
            classNames={{
                base: [baseClasses],
                item: [
                    // dấu gạch chân dưới mục được chọn
                    itemClasses
                    //
                ]
            }}
        >
            <NavbarBrand>
                <AcmeLogo />
                <p className="font-bold text-inherit">ACME</p>
            </NavbarBrand>
            <NavbarContent className="hidden sm:flex gap-4" justify="center">
                {menus.map((menu) => (
                    <NavbarItem
                        key={menu.key}
                        onClick={() => setActiveMenu(menu.key)}
                        isActive={activeMenu == menu.key}
                        as={Link}
                        href={"/" + menu.key}
                        className="text-black"
                    >
                        {menu.label}
                    </NavbarItem>
                ))}
            </NavbarContent>
            {/* className="absolute right-30" */}
            <NavbarContent justify="end">
                {/* <div className={clsx("absolute", isLogin ? "right-[120px]" : "right-[88px]")}>
                    <LanguageSwitcher />
                </div> */}
                <LanguageSwitcher />
                <NavbarItem>
                    {isLoggedIn ? (
                        isGetMeLoading ? (
                            <Spinner />
                        ) : (
                            <ProfileDropdown
                                name={`${user?.lastName} ${user?.firstName}`}
                                img={user?.avatarUrl}
                                onLogout={handleLogout}
                            />
                        )
                    ) : (
                        <ButtonStyled
                            onPress={onOpenLogin}
                            // as={Link}
                            variant="solid"
                            className="rounded-3xl opacity-97 text-black"
                        >
                            {t("login.login")}
                        </ButtonStyled>
                    )}
                </NavbarItem>
            </NavbarContent>
        </NavbarStyled>
    )
}
