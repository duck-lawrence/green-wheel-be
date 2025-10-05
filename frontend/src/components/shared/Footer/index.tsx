"use client"
import { LogoStyle } from "@/components/styled"
import { FacebookLogoIcon, InstagramLogoIcon, TiktokLogoIcon } from "@phosphor-icons/react"
import Link from "next/link"
import React from "react"

export function Footer() {
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

                    <p className="text-sm">CÔNG TY CỔ PHẦN THƯƠNG MẠI VÀ DỊCH VỤ GREEN WHEEL</p>
                    <p className="mt-2">
                        Địa chỉ: Văn phòng 02, Tầng 08, Tòa nhà Pearl Plaza, Số 561A Điện Biên Phủ,
                        Phường 25, Quận Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam.
                    </p>
                </div>

                {/* giới thiệu*/}
                <div className="ml-6">
                    <h1 className="font-semibold mb-3 text-xl">Giới thiệu</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <a href="#">Về chúng tôi</a>
                        </li>
                        <li>
                            <a href="#">Dịch vụ</a>
                        </li>
                        <li>
                            <a href="#">Tin tức</a>
                        </li>
                    </ul>
                </div>

                {/* Chính sách */}
                <div className="">
                    <h1 className="font-semibold mb-3 text-xl">Chính Sách</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <a href="#">Chính sách và quy định</a>
                        </li>
                        <li>
                            <a href="#">Quy chế hoạt động</a>
                        </li>
                        <li>
                            <a href="#">Bảo mật thông tin</a>
                        </li>
                        <li>
                            <a href="#">Giải quyết tranh chấp</a>
                        </li>
                    </ul>
                </div>

                {/* Đối tác */}
                <div>
                    <h1 className="font-semibold mb-3 text-xl">Liên hệ</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <a href="#">Hotline: 0797 123 432</a>
                        </li>
                        <li>
                            <a href="#">Email: greenwhell.work@gmail.com</a>
                        </li>
                        {/* Social icons */}
                        <div className="flex gap-3 mt-4 text-2xl">
                            <a href="#" className="bg-primary rounded-2xl p-1 text-black ">
                                <FacebookLogoIcon />
                            </a>
                            <a href="#" className="bg-primary rounded-2xl p-1 text-black  ">
                                <TiktokLogoIcon />
                            </a>
                            <a href="#" className="bg-primary rounded-2xl p-1 text-black ">
                                <InstagramLogoIcon />
                            </a>
                        </div>
                    </ul>
                </div>
            </div>

            {/* Company info */}
            <div className="border-t py-6 px-6 text-sm text-white max-w-7xl mx-auto">
                <p>© 2025 Green Wheel corp. All rights reserved.</p>
            </div>
        </footer>
    )
}
