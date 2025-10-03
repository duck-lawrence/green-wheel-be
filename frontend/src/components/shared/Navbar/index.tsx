"use client"
import React, { useEffect, useState } from "react"
import "./index.css"
import { NavbarBrand, NavbarContent, NavbarItem, Link } from "@heroui/react"
import NextLink from "next/link"
import { useTranslation } from "react-i18next"
import { ButtonStyled, NavbarStyled, LanguageSwitcher } from "@/components/"
import { useLoginDiscloresureSingleton, useToken } from "@/hooks"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
import { ProfileDropdown } from "./ProfileDropdown"

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
    const { t } = useTranslation()
    // handle navbar
    type NavbarState = "default" | "top" | "middle"
    const [scrollState, setScroledState] = useState<NavbarState>("default")
    const [isHiddenNavbar, setIsHiddenNavbar] = useState(false)
    const [lastScrollY, setLastScrollY] = useState(0)
    const activeMenuKey = useNavbarItemStore((s) => s.activeMenuKey)
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)

    // handle when login
    const isLoggedIn = useToken((s) => !!s.accessToken)
    const { onOpen: onOpenLogin } = useLoginDiscloresureSingleton()

    // handle navbar animation
    const baseClasses = `
        bg-transparent
        transition-all duration-400 ease-in-out
        mt-3 
        fixed left-0 w-full z-50 h-xl
        mx-auto max-w-6xl
        data-[visible=false]:mt-0
        rounded-3xl
        justify-between
        ${isHiddenNavbar ? "-translate-y-full opacity-0" : "translate-y-0 opacity-100"}
        ${
            scrollState === "top" || scrollState === "middle"
                ? "text-white rounded-3xl bg-[#080808]/60 backdrop-blur-md mx-auto max-w-3xl scale-95"
                : "backdrop-blur-none"
        }
    `
    const itemClasses = [
        "flex",
        "relative",
        "h-full",
        "data-[active=true]:after:content-['']",
        "data-[active=true]:after:absolute",
        "data-[active=true]:after:bottom-0",
        "data-[active=true]:after:left-0",
        "data-[active=true]:after:right-0",
        "data-[active=true]:after:h-[3px]",
        "data-[active=true]:after:w-full",
        "data-[active=true]:after:rounded-[2px]",
        "data-[active=true]:after:bg-primary"
    ]
    const menus = [
        { key: "home", label: t("navbar.home") },
        { key: "vehicle-rental", label: t("navbar.vehicle_rental") },
        { key: "about", label: t("navbar.about_us") },
        { key: "contact", label: t("navbar.contact") }
    ]

    // useEffect
    // handle navbar scroll
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
                item: [itemClasses]
            }}
        >
            {/* start content */}
            <NavbarBrand>
                <NextLink href={"/"} className="flex items-center">
                    <AcmeLogo />
                    <p className="font-bold text-inherit">ACME</p>
                </NextLink>
            </NavbarBrand>
            {/* middle content */}
            <NavbarContent className="hidden sm:flex gap-4 justify-center">
                {menus.map((menu) => (
                    <NavbarItem
                        key={menu.key}
                        onPress={() => setActiveMenuKey(menu.key)}
                        isActive={activeMenuKey == menu.key}
                        as={Link}
                        href={menu.key === "home" ? "/" : "/" + menu.key}
                        className={
                            scrollState === "top" || scrollState === "middle"
                                ? "text-white"
                                : "text-inherit"
                        }
                    >
                        <div className="text-center px-3 min-w-full">{menu.label}</div>
                    </NavbarItem>
                ))}
            </NavbarContent>
            {/* end content */}
            <NavbarContent justify="end">
                <LanguageSwitcher
                    isChangeTextColor={scrollState === "top" || scrollState === "middle"}
                />
                <NavbarItem className="flex items-center">
                    {isLoggedIn ? (
                        <ProfileDropdown />
                    ) : (
                        <ButtonStyled
                            onPress={onOpenLogin}
                            color="primary"
                            variant="solid"
                            className="rounded-3xl opacity-97"
                        >
                            {t("login.login")}
                        </ButtonStyled>
                    )}
                </NavbarItem>
            </NavbarContent>
        </NavbarStyled>
    )
}
