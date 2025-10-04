"use client"
import { FacebookLogoIcon, TiktokLogoIcon } from "@phosphor-icons/react"
import React from "react"


export default function Footer() {
    return (
        <footer className="bg-gray-700 text-white border-t mt-10 rounded-2xl">
            <div className="max-w-7xl mx-auto px-6 py-10 grid grid-cols-1 md:grid-cols-4 gap-8">
                {/* Logo + contact */}
                <div className="space-y-4">
                    <div className="flex items-center gap-2 mb-4">
                        <img src="/logo.png" alt="Mioto" className="h-8" />
                        <span className="text-xl font-bold">MIOTO</span>
                    </div>
                    <p className="font-semibold">1900 9217</p>
                    <p className="text-sm">Tổng đài hỗ trợ: 7AM - 10PM</p>
                    <p className="mt-2">contact@mioto.vn</p>
                    <p className="text-sm">Gửi mail cho Mioto</p>

                    {/* Social icons */}
                    <div className="flex gap-3 mt-4 text-2xl">
                        <a href="#" className="hover:text-blue-600">
                            <FacebookLogoIcon />
                        </a>
                        <a href="#" className="hover:text-black">
                            <TiktokLogoIcon />
                        </a>
                        <a href="#" className="hover:text-green-600">
                            <TiktokLogoIcon />
                        </a>
                    </div>
                </div>

                {/* Chính sách */}
                <div className="">
                    <h4 className="font-semibold mb-3">Chính Sách</h4>
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

                {/* Tìm hiểu thêm */}
                <div>
                    <h4 className="font-semibold mb-3">Tìm Hiểu Thêm</h4>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <a href="#">Hướng dẫn chung</a>
                        </li>
                        <li>
                            <a href="#">Hướng dẫn đặt xe</a>
                        </li>
                        <li>
                            <a href="#">Hướng dẫn thanh toán</a>
                        </li>
                        <li>
                            <a href="#">Hỏi và trả lời</a>
                        </li>
                        <li>
                            <a href="#">Về Mioto</a>
                        </li>
                        <li>
                            <a href="#">Mioto Blog</a>
                        </li>
                        <li>
                            <a href="#">Tuyển dụng</a>
                        </li>
                    </ul>
                </div>

                {/* Đối tác */}
                <div>
                    <h4 className="font-semibold mb-3">Đối Tác</h4>
                    <ul className="space-y-4 text-sm">
                        <li>
                            <a href="#">Đăng ký chủ xe Mioto</a>
                        </li>
                        <li>
                            <a href="#">Đăng ký GPS MITRACK 4G</a>
                        </li>
                    </ul>
                </div>
            </div>

            {/* Company info */}
            <div className="border-t py-6 px-6 text-sm text-white max-w-7xl mx-auto">
                <p>© Công ty Cổ Phần Mioto Asia</p>
                <div className="flex flex-col md:flex-row md:justify-between mt-2">
                    <p>
                        Số GCNĐKKD: 0317307544 - Ngày cấp: 24-05-22 - Nơi cấp: Sở Kế hoạch và Đầu tư
                        TPHCM
                    </p>
                    <p>
                        Tên TK: CT CP MIOTO ASIA - Số TK: 102-989-1989 - Ngân hàng Vietcombank - CN
                        Tân Định
                    </p>
                </div>
                <p className="mt-2">
                    Địa chỉ: Văn phòng 02, Tầng 08, Tòa nhà Pearl Plaza, Số 561A Điện Biên Phủ,
                    Phường 25, Quận Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam.
                </p>
            </div>
        </footer>
    )
}
