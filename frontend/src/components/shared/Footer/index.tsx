"use client"
import { LogoStyle } from "@/components/styled"
import { FacebookLogoIcon, InstagramLogoIcon, TiktokLogoIcon } from "@phosphor-icons/react"
import Link from "next/link"
import React from "react"
import { useTranslation } from "react-i18next"

export function Footer() {
    const { t } = useTranslation()
    return (
        <footer className="bg-black text-white border-t mt-10">
            <div className="max-w-7xl mx-auto px-6 py-10 grid grid-cols-1 md:grid-cols-4 gap-8">
                {/* Logo + contact */}
                <div className="space-y-4">
                    <div className="flex items-center gap-2 mb-4">
                        <Link href={"/"}>
                            <LogoStyle />
                            <p className="font-semibold text-xl text-primary">Green Wheel</p>
                        </Link>
                    </div>

                    <p className="text-sm">{t("footer.green_wheel_company_name")}</p>
                    <p className="mt-2">
                        Địa chỉ: Văn phòng 02, Tầng 08, Tòa nhà Pearl Plaza, Số 561A Điện Biên Phủ,
                        Phường 25, Quận Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam.
                    </p>
                </div>

                {/* giới thiệu*/}
                <div className="ml-6">
                    <h1 className="font-semibold mb-3 text-xl">{t("footer.introduction")}</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <Link href="#">{t("footer.about_us")}</Link>
                        </li>
                        <li>
                            <Link href="#">{t("footer.services")}</Link>
                        </li>
                    </ul>
                </div>

                {/* Chính sách */}
                <div className="">
                    <h1 className="font-semibold mb-3 text-xl">{t("footer.policy")}</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <Link href="#">{t("footer.policy_and_regulations")}</Link>
                        </li>
                        <li>
                            <Link href="#">{t("footer.operating_regulations")}</Link>
                        </li>
                        <li>
                            <Link href="#">{t("footer.information_security")}</Link>
                        </li>
                        <li>
                            <Link href="#">{t("footer.dispute_resolution")}</Link>
                        </li>
                    </ul>
                </div>

                {/* Đối tác */}
                <div>
                    <h1 className="font-semibold mb-3 text-xl">{t("footer.contact")}</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <Link href="#">{t("footer.hotline")}: 0797 123 432</Link>
                        </li>
                        <li>
                            <Link href="#">Email: greenwhell.work@gmail.com</Link>
                        </li>
                        {/* Social icons */}
                        <div className="flex gap-3 mt-4 text-2xl">
                            <Link href="#" className="bg-primary rounded-2xl p-1 text-black ">
                                <FacebookLogoIcon />
                            </Link>
                            <Link href="#" className="bg-primary rounded-2xl p-1 text-black  ">
                                <TiktokLogoIcon />
                            </Link>
                            <Link href="#" className="bg-primary rounded-2xl p-1 text-black ">
                                <InstagramLogoIcon />
                            </Link>
                        </div>
                    </ul>
                </div>
            </div>

            {/* Company info */}
            <div className="border-t py-6 px-6 text-sm text-white max-w-7xl mx-auto">
                <p>{t("footer.copyright_notice")}</p>
            </div>
        </footer>
    )
}
