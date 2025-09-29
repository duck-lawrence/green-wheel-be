"use client"
import { ButtonStyled } from "@/components"
import { LoginModal } from "@/components/modals/LoginModal"
import Carousel from "@/components/shared/Carousel"
import Navbar from "@/components/shared/Navbar"
import UserIconStyled from "@/components/styled/UserIconStyled"
import { useLoginDiscloresureSingleton } from "@/hooks"
import Link from "next/link"
import React from "react"
import { slides } from "../../../public/cars"
import { useTranslation } from "react-i18next"

export default function page() {
    const { t } = useTranslation()
    return (
        <>
            <Navbar />
            <div>
                <div className="mt-30">
                    <div className="relative">
                        <div className="absolute top-0 left-164 flex flex-col items-center justify-center gap-4">
                            <div className="font-bold text-2xl ">Green Rides. Brighter Future.</div>
                            <div className="text-gray-500">{t("home.description")}</div>
                            <ButtonStyled
                                as={Link}
                                href="/self-drive"
                                className="text-black h-13 transition-all duration-500
                                            hover:bg-primary hover:text-white hover:border-black"
                                variant="bordered"
                            >
                                {t("home.viewDetails")}
                            </ButtonStyled>
                        </div>

                        {/* Carousel */}
                        <Carousel slides={slides} />
                        <Carousel slides={slides} />
                    </div>
                </div>
            </div>
            <div>
                <LoginModal />
            </div>
        </>
    )
}
