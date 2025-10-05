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
                            <Link href="#">Về chúng tôi</Link>
                        </li>
                        <li>
                            <Link href="#">Dịch vụ</Link>
                        </li>
                        <li>
                            <Link href="#">Tin tức</Link>
                        </li>
                    </ul>
                </div>

                {/* Chính sách */}
                <div className="">
                    <h1 className="font-semibold mb-3 text-xl">Chính Sách</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <Link href="#">Chính sách và quy định</Link>
                        </li>
                        <li>
                            <Link href="#">Quy chế hoạt động</Link>
                        </li>
                        <li>
                            <Link href="#">Bảo mật thông tin</Link>
                        </li>
                        <li>
                            <Link href="#">Giải quyết tranh chấp</Link>
                        </li>
                    </ul>
                </div>

                {/* Đối tác */}
                <div>
                    <h1 className="font-semibold mb-3 text-xl">Liên hệ</h1>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <Link href="#">Hotline: 0797 123 432</Link>
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
                <p>© 2025 Green Wheel corp. All rights reserved.</p>
            </div>
        </footer>
    )
}
