"use client"
import React, { useMemo, useState } from "react"
import { motion } from "framer-motion"

// --------- Mock Data (replace with your CMS/API) ---------
const vehicle = {
    name: "VinFast VF 3",
    slug: "vinfast-vf3",
    rating: 4.8,
    reviews: 126,
    pricePerDay: 690000,
    priceUnit: "đ/ngày",
    oldPricePerDay: 790000,
    fuel: "Điện",
    seats: 4,
    transmission: "Tự động",
    rangeKm: 210,
    powerKw: 32,
    torqueNm: 110,
    images: [
        "https://images.unsplash.com/photo-1493236272126-8c562028cd64?q=80&w=1600&auto=format&fit=crop",
        "https://images.unsplash.com/photo-1525609004556-c46c7d6cf023?q=80&w=1600&auto=format&fit=crop",
        "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?q=80&w=1600&auto=format&fit=crop",
        "https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?q=80&w=1600&auto=format&fit=crop"
    ],
    highlights: [
        "Miễn phí 200km/ngày",
        "Giao xe tận nơi trong 30 phút",
        "Hỗ trợ 24/7",
        "Bảo hiểm thân vỏ"
    ],
    policies: [
        { title: "Giấy tờ", text: "CCCD gắn chip/Passport + GPLX B2 trở lên." },
        { title: "Cọc", text: "5.000.000đ hoặc xe máy giấy tờ chính chủ." },
        { title: "Nhiên liệu", text: "Trả xe mức pin như khi nhận." },
        { title: "Phụ phí", text: "Phát sinh giờ: 10%/giờ, quá 5 giờ tính 1 ngày." }
    ],
    specs: [
        { k: "Loại xe", v: "Mini SUV" },
        { k: "Số chỗ", v: "4" },
        { k: "Truyền động", v: "Tự động" },
        { k: "Quãng đường", v: "~210 km/ lần sạc" },
        { k: "Công suất", v: "32 kW" },
        { k: "Mô-men xoắn", v: "110 Nm" },
        { k: "Sạc", v: "AC Type 2" }
    ]
}

// --------- Helpers ---------
const currency = (n: number) => new Intl.NumberFormat("vi-VN").format(n)

// --------- UI ---------
export default function VF3RentalPage() {
    const [active, setActive] = useState(0)
    const [dates, setDates] = useState({ pick: "", drop: "" })
    const totalDays = useMemo(() => {
        const { pick, drop } = dates
        if (!pick || !drop) return 0
        const a = new Date(pick)
        const b = new Date(drop)
        const diff = Math.ceil((+b - +a) / (1000 * 60 * 60 * 24))
        return Math.max(0, diff)
    }, [dates])

    const total = totalDays * vehicle.pricePerDay

    return (
        <div className="min-h-dvh bg-neutral-50 text-neutral-900">
            {/* Breadcrumb */}
            <nav className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-4 text-sm text-neutral-500">
                <ol className="flex flex-wrap items-center gap-2">
                    <li>
                        <a href="/" className="hover:text-neutral-800">
                            Trang chủ
                        </a>
                    </li>
                    <li>/</li>
                    <li>
                        <a href="/thue-xe-tu-lai" className="hover:text-neutral-800">
                            Thuê xe tự lái
                        </a>
                    </li>
                    <li>/</li>
                    <li className="text-neutral-800 font-medium">{vehicle.name}</li>
                </ol>
            </nav>

            {/* Header */}
            <header className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pb-4">
                <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-3">
                    <div>
                        <h1 className="text-3xl/tight sm:text-4xl font-bold tracking-tight">
                            {vehicle.name}
                        </h1>
                        <div className="mt-2 flex items-center gap-3 text-sm">
                            <div className="flex items-center">
                                {Array.from({ length: 5 }).map((_, i) => (
                                    <svg
                                        key={i}
                                        viewBox="0 0 24 24"
                                        className={`h-5 w-5 ${
                                            i < Math.round(vehicle.rating)
                                                ? "fill-yellow-400"
                                                : "fill-neutral-200"
                                        }`}
                                    >
                                        <path d="M12 17.27 18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z" />
                                    </svg>
                                ))}
                            </div>
                            <span className="text-neutral-600">
                                {vehicle.rating} ({vehicle.reviews} đánh giá)
                            </span>
                        </div>
                    </div>
                    <div className="text-right">
                        <p className="text-sm line-through text-neutral-400">
                            {currency(vehicle.oldPricePerDay)} {vehicle.priceUnit}
                        </p>
                        <p className="text-2xl sm:text-3xl font-extrabold text-emerald-600">
                            {currency(vehicle.pricePerDay)}{" "}
                            <span className="text-base font-normal text-neutral-500">
                                {vehicle.priceUnit}
                            </span>
                        </p>
                    </div>
                </div>
            </header>

            {/* Main content */}
            <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pb-24 grid grid-cols-1 lg:grid-cols-12 gap-6">
                {/* Gallery */}
                <section className="lg:col-span-8">
                    <div className="grid grid-cols-4 gap-3">
                        <div className="col-span-4 md:col-span-3 aspect-[16/10] overflow-hidden rounded-2xl bg-neutral-200">
                            <motion.img
                                key={active}
                                initial={{ opacity: 0, scale: 1.02 }}
                                animate={{ opacity: 1, scale: 1 }}
                                transition={{ duration: 0.35 }}
                                src={vehicle.images[active]}
                                alt={`${vehicle.name} - ${active + 1}`}
                                className="h-full w-full object-cover"
                            />
                        </div>
                        <div className="col-span-4 md:col-span-1 grid grid-rows-3 gap-3">
                            {vehicle.images.map((src, idx) => (
                                <button
                                    key={src}
                                    onClick={() => setActive(idx)}
                                    className={`group relative aspect-[4/3] overflow-hidden rounded-2xl outline-none ring-2 ring-transparent focus:ring-emerald-500 ${
                                        active === idx ? "ring-emerald-500" : ""
                                    }`}
                                >
                                    <img
                                        src={src}
                                        alt={`thumb ${idx + 1}`}
                                        className="h-full w-full object-cover group-hover:scale-[1.02] transition"
                                    />
                                </button>
                            ))}
                        </div>
                    </div>

                    {/* Highlights */}
                    <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-3">
                        {vehicle.highlights.map((h) => (
                            <div
                                key={h}
                                className="flex items-center gap-3 rounded-xl bg-white p-4 shadow-sm"
                            >
                                <span className="inline-flex h-8 w-8 items-center justify-center rounded-full bg-emerald-50">
                                    ✅
                                </span>
                                <p className="text-sm text-neutral-700">{h}</p>
                            </div>
                        ))}
                    </div>

                    {/* Specs */}
                    <div className="mt-8 rounded-2xl bg-white p-6 shadow-sm">
                        <h2 className="text-xl font-semibold mb-4">Thông số</h2>
                        <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-4">
                            {vehicle.specs.map(({ k, v }) => (
                                <div key={k} className="rounded-xl border border-neutral-200 p-4">
                                    <p className="text-xs uppercase tracking-wide text-neutral-500">
                                        {k}
                                    </p>
                                    <p className="mt-1 font-medium">{v}</p>
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Tabs */}
                    <section className="mt-8">
                        <div className="flex flex-wrap gap-2">
                            <TabButton>Chi tiết</TabButton>
                            <TabButton>Chính sách</TabButton>
                            <TabButton>FAQ</TabButton>
                            <TabButton>Đánh giá ({vehicle.reviews})</TabButton>
                        </div>
                        <div className="mt-4 grid gap-4 md:grid-cols-2">
                            {vehicle.policies.map((p) => (
                                <div key={p.title} className="rounded-2xl bg-white p-5 shadow-sm">
                                    <h3 className="font-semibold">{p.title}</h3>
                                    <p className="mt-1 text-sm text-neutral-600">{p.text}</p>
                                </div>
                            ))}
                        </div>
                    </section>

                    {/* Similar vehicles */}
                    <section className="mt-10">
                        <div className="flex items-center justify-between">
                            <h2 className="text-xl font-semibold">Xe tương tự</h2>
                            <a href="#" className="text-sm text-emerald-700 hover:underline">
                                Xem tất cả
                            </a>
                        </div>
                        <div className="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                            {[1, 2, 3].map((i) => (
                                <a
                                    key={i}
                                    href="#"
                                    className="group rounded-2xl bg-white p-3 shadow-sm hover:shadow-md transition"
                                >
                                    <div className="aspect-[16/10] overflow-hidden rounded-xl bg-neutral-200">
                                        <img
                                            className="h-full w-full object-cover group-hover:scale-105 transition"
                                            src={`https://picsum.photos/seed/vf3-${i}/800/500`}
                                            alt="car"
                                        />
                                    </div>
                                    <div className="mt-3 flex items-center justify-between">
                                        <div>
                                            <p className="font-medium">VinFast VF 5</p>
                                            <p className="text-sm text-neutral-500">
                                                Điện • 5 chỗ • Tự động
                                            </p>
                                        </div>
                                        <p className="text-emerald-600 font-semibold">
                                            {currency(790000)}{" "}
                                            <span className="text-xs text-neutral-500">đ/ngày</span>
                                        </p>
                                    </div>
                                </a>
                            ))}
                        </div>
                    </section>
                </section>

                {/* Booking Card (sticky on desktop) */}
                <aside className="lg:col-span-4">
                    <div className="lg:sticky lg:top-6 rounded-2xl bg-white p-5 shadow-sm border border-neutral-100">
                        <h2 className="text-lg font-semibold">Đặt xe nhanh</h2>
                        <div className="mt-4 grid gap-4">
                            <div>
                                <label className="text-sm text-neutral-600">Ngày nhận</label>
                                <input
                                    type="date"
                                    className="mt-1 w-full rounded-xl border border-neutral-300 bg-white px-3 py-2 outline-none focus:ring-2 focus:ring-emerald-500"
                                    value={dates.pick}
                                    onChange={(e) =>
                                        setDates((p) => ({ ...p, pick: e.target.value }))
                                    }
                                />
                            </div>
                            <div>
                                <label className="text-sm text-neutral-600">Ngày trả</label>
                                <input
                                    type="date"
                                    className="mt-1 w-full rounded-xl border border-neutral-300 bg-white px-3 py-2 outline-none focus:ring-2 focus:ring-emerald-500"
                                    value={dates.drop}
                                    onChange={(e) =>
                                        setDates((p) => ({ ...p, drop: e.target.value }))
                                    }
                                />
                            </div>
                            <div className="grid grid-cols-2 gap-3">
                                <Field label="Nhiên liệu" value={vehicle.fuel} />
                                <Field label="Số chỗ" value={String(vehicle.seats)} />
                                <Field label="Hộp số" value={vehicle.transmission} />
                                <Field label="Quãng đường" value={`~${vehicle.rangeKm} km`} />
                            </div>

                            <div className="rounded-xl bg-neutral-50 p-4">
                                <div className="flex items-center justify-between text-sm">
                                    <span>Đơn giá</span>
                                    <span className="font-medium">
                                        {currency(vehicle.pricePerDay)} {vehicle.priceUnit}
                                    </span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>Số ngày</span>
                                    <span className="font-medium">{totalDays}</span>
                                </div>
                                <div className="mt-3 h-px bg-neutral-200" />
                                <div className="mt-3 flex items-center justify-between text-base font-semibold">
                                    <span>Tạm tính</span>
                                    <span className="text-emerald-700">{currency(total)} đ</span>
                                </div>
                            </div>

                            <button
                                className="w-full rounded-xl bg-emerald-600 px-4 py-3 font-semibold text-white shadow-sm hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                                onClick={() => alert("Gửi yêu cầu đặt xe! (hook API tại đây)")}
                            >
                                Yêu cầu đặt xe
                            </button>
                            <p className="text-center text-xs text-neutral-500">
                                Không cần thanh toán trước
                            </p>
                        </div>
                    </div>

                    {/* Policy quicklist */}
                    <div className="mt-6 rounded-2xl bg-white p-5 shadow-sm">
                        <h3 className="font-semibold mb-3">Chính sách nhanh</h3>
                        <ul className="space-y-2 text-sm text-neutral-700">
                            {vehicle.policies.map((p) => (
                                <li key={p.title} className="flex items-start gap-2">
                                    <span className="mt-0.5">•</span>
                                    <div>
                                        <p className="font-medium">{p.title}</p>
                                        <p className="text-neutral-600">{p.text}</p>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    </div>
                </aside>
            </main>

            {/* Sticky mobile CTA */}
            <div className="lg:hidden fixed inset-x-0 bottom-0 z-50 border-t border-neutral-200 bg-white/95 backdrop-blur supports-[backdrop-filter]:bg-white/80">
                <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-3 flex items-center justify-between">
                    <div>
                        <div className="text-xs text-neutral-500">Giá từ</div>
                        <div className="text-lg font-bold text-emerald-700">
                            {currency(vehicle.pricePerDay)} đ/ngày
                        </div>
                    </div>
                    <button
                        onClick={() => {
                            const el = document.querySelector("aside")
                            el?.scrollIntoView({ behavior: "smooth" })
                        }}
                        className="rounded-xl bg-emerald-600 px-4 py-2 font-semibold text-white shadow-sm"
                    >
                        Đặt xe
                    </button>
                </div>
            </div>

            <footer className="mt-14 border-t border-neutral-200 bg-white">
                <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-8 text-sm text-neutral-500">
                    © {new Date().getFullYear()} GreenFuture — Thuê xe điện dễ dàng ⚡
                </div>
            </footer>
        </div>
    )
}

function Field({ label, value }: { label: string; value: string }) {
    return (
        <div className="rounded-xl border border-neutral-200 p-3">
            <p className="text-xs uppercase tracking-wide text-neutral-500">{label}</p>
            <p className="mt-1 font-medium">{value}</p>
        </div>
    )
}

function TabButton({ children }: { children: React.ReactNode }) {
    const [active, setActive] = useState(false)
    return (
        <button
            onClick={() => setActive((p) => !p)}
            className={`relative rounded-full px-3 py-1.5 text-sm transition ${
                active ? "bg-emerald-600 text-white" : "bg-white text-neutral-700 shadow-sm"
            }`}
        >
            {children}
        </button>
    )
}
