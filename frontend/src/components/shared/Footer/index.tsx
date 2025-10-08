"use client"
import React from "react"
import Link from "next/link"
import { useTranslation } from "react-i18next"
import {
    FacebookLogo,
    InstagramLogo,
    YoutubeLogo,
    EnvelopeSimple,
    Phone,
    MapPin,
    IconProps
} from "@phosphor-icons/react"
import { LogoStyle } from "@/components/styled"

export function Footer() {
    const { t } = useTranslation()

    return (
        <footer className="bg-neutral-950 text-gray-200 border-t border-gray-800 w-full">
            {/* CONTENT GRID */}
            <div className="max-w-7xl mx-auto px-6 py-12 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-10">
                {/* Logo + About */}
                <div className="space-y-4">
                    <Link href="/" className="flex items-center gap-2">
                        <LogoStyle />
                        <p className="font-semibold text-2xl text-primary">Green Wheel</p>
                    </Link>

                    <p className="text-sm leading-relaxed text-gray-400 max-w-sm">
                        {t("footer.green_wheel_company_name") ||
                            "Công ty cổ phần thương mại và dịch vụ Green Wheel. Hướng tới tương lai xanh và bền vững."}
                    </p>

                    <div className="flex items-center gap-2 text-sm text-gray-400">
                        <MapPin size={18} weight="fill" className="text-primary" />
                        <span>TP Hồ Chí Minh, Việt Nam</span>
                    </div>
                </div>

                {/* Introduction */}
                <div>
                    <h3 className="font-semibold mb-4 text-white text-lg">
                        {t("footer.introduction")}
                    </h3>
                    <ul className="space-y-3 text-sm">
                        <li>
                            <Link href="/about" className="hover:text-primary duration-200">
                                {t("footer.about_us")}
                            </Link>
                        </li>
                        <li>
                            <Link href="/services" className="hover:text-primary duration-200">
                                {t("footer.services")}
                            </Link>
                        </li>
                    </ul>
                </div>

                {/* Policy */}
                <div>
                    <h3 className="font-semibold mb-4 text-white text-lg">{t("footer.policy")}</h3>
                    <ul className="space-y-3 text-sm">
                        <li>
                            <Link href="#" className="hover:text-primary duration-200">
                                {t("footer.policy_and_regulations")}
                            </Link>
                        </li>
                        <li>
                            <Link href="#" className="hover:text-primary duration-200">
                                {t("footer.operating_regulations")}
                            </Link>
                        </li>
                        <li>
                            <Link href="#" className="hover:text-primary duration-200">
                                {t("footer.information_security")}
                            </Link>
                        </li>
                        <li>
                            <Link href="#" className="hover:text-primary duration-200">
                                {t("footer.dispute_resolution")}
                            </Link>
                        </li>
                    </ul>
                </div>

                {/* Contact */}
                <div>
                    <h3 className="font-semibold mb-4 text-white text-lg">{t("footer.contact")}</h3>
                    <ul className="space-y-3 text-sm">
                        <li className="flex items-center gap-2">
                            <Phone size={18} weight="fill" className="text-primary" />
                            <span>0797 123 432</span>
                        </li>
                        <li className="flex items-center gap-2">
                            <EnvelopeSimple size={18} weight="fill" className="text-primary" />
                            <span>greenwheel.work@gmail.com</span>
                        </li>
                    </ul>

                    {/* Social icons */}
                    <div className="flex gap-3 mt-5">
                        <SocialIcon href="#" Icon={FacebookLogo} />
                        <SocialIcon href="#" Icon={YoutubeLogo} />
                        <SocialIcon href="#" Icon={InstagramLogo} />
                    </div>
                </div>
            </div>

            {/* COPYRIGHT */}
            <div className="border-t border-gray-800 mt-8 py-5">
                <p className="text-center text-gray-400 text-sm">
                    © 2025 Green Wheel Corp. All rights reserved.
                </p>
            </div>
        </footer>
    )
}

/* === Social icon subcomponent === */
function SocialIcon({
    href,
    Icon
}: {
    href: string
    Icon: React.ForwardRefExoticComponent<IconProps & React.RefAttributes<SVGSVGElement>>
}) {
    return (
        <Link
            href={href}
            className="p-2 rounded-xl bg-gradient-to-r from-primary to-teal-400 
                 hover:from-teal-500 hover:to-green-400 text-black
                 transition-all duration-300 hover:scale-110"
        >
            <Icon size={22} weight="fill" />
        </Link>
    )
}
