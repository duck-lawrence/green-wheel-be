"use client"
import { ButtonStyled, Carousel } from "@/components"
import Link from "next/link"
import React, { useEffect } from "react"
import { slides } from "../../public/cars"
import { useTranslation } from "react-i18next"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"

export default function HomePage() {
    const { t } = useTranslation()
    const { setActiveMenuKey } = useNavbarItemStore()

    useEffect(() => {
        setActiveMenuKey("home")
    }, [setActiveMenuKey])

    return (
        <div>
            <div className="mt-30 min-h-[80vh]">
                <div className="flex flex-col items-center justify-center gap-4">
                    <div className="font-bold text-2xl ">Green Rides. Brighter Future.</div>
                    <div className="text-gray-500">{t("home.description")}</div>
                    <ButtonStyled
                        as={Link}
                        href="/self-drive"
                        className="text-black h-13 transition-all duration-500
                                            hover:bg-primary hover:text-white hover:border-black"
                        variant="bordered"
                    >
                        {t("home.view_details")}
                    </ButtonStyled>
                </div>

                {/* Carousel */}
                <Carousel slides={slides} />
            </div>
            <Carousel slides={slides} />
            <Carousel slides={slides} />
            <Carousel slides={slides} />
            <Carousel slides={slides} />
            <Carousel slides={slides} />
        </div>
    )
}
