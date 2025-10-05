"use client"
import React, { useEffect, useMemo, useState } from "react"
import { motion } from "framer-motion"
import { BreadCrumbsStyled, ButtonStyled } from "@/components/styled"
import { Field } from "@/components/styled/FieldStyled"
import { GasPump, UsersFour, SteeringWheel, RoadHorizon } from "@phosphor-icons/react"
import Link from "next/link"
import { currency } from "@/utils/helpers/currentcy"
import { MathDate } from "@/utils/helpers/mathDate"
import { vehicleData } from "@/data/vehicleData"
import { useParams } from "next/navigation"
import Vehicle from "@/models/vehicle/vehicle"
import { useToken } from "@/hooks"

export default function DetailPage() {
    const isLoggedIn = useToken((s) => !!s.accessToken)
    const { id } = useParams()
    const [vehicle, setVehicle] = useState<Vehicle | null>(null)

    // handle render picture
    const [active, setActive] = useState(0)

    // handle count date
    const [dates, setDates] = useState({ start: "2025-10-02", end: "2025-10-06" })
    const totalDays = useMemo(() => {
        return MathDate(dates)
    }, [dates])

    useEffect(() => {
        const vehicleID = Array.isArray(id) ? id[0] : id
        const car = vehicleData.find((v) => v.id === vehicleID)
        if (car) setVehicle(car)
    }, [id])

    if (!vehicle) return <p>Loading....</p>

    // display item similar
    const similarVehicles = vehicleData
        .filter((v) => v.id !== id)
        .sort(() => Math.random() - 0.5)
        .slice(0, 3)

    const total = totalDays * vehicle.costPerDay + vehicle.depositFee

    function mapSpecs(vehicle: any) {
        return [
            { key: "Số chỗ", value: vehicle.seatingCapacity },
            { key: "Công suất", value: vehicle.motorPower + " kW" },
            { key: "Dung lượng pin", value: vehicle.batteryCapacity + " kWh" },
            { key: "Eco Range", value: vehicle.ecoRangeKm + " km" },
            { key: "Sport Range", value: vehicle.sportRangeKm + " km" },
            { key: "Hộp số", value: vehicle.transmission }
        ]
    }

    const basePolocies = (deposite: number) => [
        { title: "Giấy tờ", text: "CCCD gắn chip + GPLX B1 trở lên." },
        { title: "Cọc", text: `${deposite}đ hoặc xe máy giấy tờ chính chủ. ` },
        {
            title: "Phụ phí",
            text: "Phát sinh giờ: quá 45 phút tính phụ phí 10%/giờ, quá 5 giờ tính 1 ngày."
        },
        {
            title: "Hình thức thanh toán",
            text: "Trả trước. Thời hạn thanh toán: đặt cọc giữ xe thanh toán 100% khi kí hợp đồng và nhận xe"
        }
    ]

    return (
        <div className="min-h-dvh bg-neutral-50 text-neutral-900 mt-20 rounded">
            {/* Breadcrumb */}
            <div className="p-4">
                <BreadCrumbsStyled
                    items={[
                        { label: "Home", href: "/" },
                        { label: "Vehicle Rental", href: "/vehicle-rental" },
                        { label: "Detail", href: "/detail" }
                    ]}
                />
            </div>

            {/* Header */}
            <header className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pb-4">
                <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-3">
                    <div>
                        <h1 className="text-3xl/tight sm:text-4xl font-bold tracking-tight">
                            {vehicle.name}
                        </h1>
                        <p className="mt-1 text-neutral-600">{vehicle.description}</p>
                        {/* <p className="mt-1 text-sm text-neutral-500">
                            Hãng xe: <span className="font-medium">{vehicle.brand_name}</span> •
                            Phân khúc: <span className="font-medium">{vehicle.segment_name}</span>
                        </p> */}
                        <p className="mt-1 text-sm text-neutral-500">
                            Số lượng xe còn:{" "}
                            <span className="font-medium text-emerald-600">
                                {vehicle.availableVehicleCount}
                            </span>
                        </p>
                    </div>
                    {/*  font-extrabold*/}
                    <div className="text-right">
                        <p className="text-2xl sm:text-3xl font-semibold text-emerald-600">
                            {currency(vehicle.costPerDay)}
                            <span className="text-base font-normal text-neutral-500">
                                {" "}
                                VND/Ngày
                            </span>
                        </p>
                    </div>
                </div>
            </header>

            {/* Main content */}
            <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pb-24 grid grid-cols-1 lg:grid-cols-12 gap-6">
                {/* Gallery */}
                <section className="lg:col-span-8">
                    {/* Picture */}
                    <div className="grid grid-rows-4 gap-3">
                        <div className="row-span-3 aspect-[16/10] overflow-hidden rounded-2xl bg-neutral-200">
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

                        <div className="row-span-1 grid grid-cols-4 gap-3">
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

                    {/* Thông số */}
                    <div className="mt-8 rounded-2xl bg-white p-6 shadow-sm">
                        <h2 className="text-xl font-semibold mb-4">Thông số</h2>
                        <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-4">
                            {mapSpecs(vehicle).map(({ key, value }) => (
                                <div key={key} className="rounded-xl border border-neutral-200 p-4">
                                    <p className="text-xs uppercase tracking-wide text-neutral-500">
                                        {key}
                                    </p>
                                    <p className="mt-1 font-medium">{value}</p>
                                </div>
                            ))}
                        </div>
                    </div>

                    {/*================ Policies =======================*/}
                    <section className="mt-8 rounded-2xl bg-white p-6 shadow-sm">
                        <div className="grid gap-4 md:grid-cols-2">
                            {basePolocies(vehicle.depositFee).map((p) => (
                                <div key={p.title} className="rounded-2xl bg-white p-5 shadow-sm">
                                    <h3 className="font-semibold">{p.title}</h3>
                                    <p className="mt-1 text-sm text-neutral-600">{p.text}</p>
                                </div>
                            ))}
                        </div>
                    </section>
                </section>
                {/* ========================================================= */}
                {/* Booking Card (sticky on desktop) */}
                <aside className="lg:col-span-4 lg:sticky lg:top-10 space-y-6 ">
                    <div className="rounded-2xl bg-white p-5 shadow-sm border border-neutral-100">
                        <h2 className="text-lg font-semibold">Thông tin xe</h2>
                        <div className="mt-4 grid gap-4">
                            <div className="grid grid-cols-2 gap-3">
                                <Field
                                    label="Nhiên liệu"
                                    value="Điện"
                                    icon={<GasPump size={18} weight="duotone" />}
                                />
                                <Field
                                    label="Số chỗ"
                                    value={`${vehicle.seatingCapacity}`}
                                    icon={<UsersFour size={18} />}
                                />
                                <Field
                                    label="Hộp số"
                                    value="Tự động"
                                    icon={<SteeringWheel size={18} />}
                                />
                                <Field
                                    label="Quãng đường"
                                    value={`~${vehicle.ecoRangeKm} km`}
                                    icon={<RoadHorizon size={18} />}
                                />
                            </div>

                            {/* Đơn tạm tính */}
                            <div className="rounded-xl bg-neutral-50 p-4">
                                <div className="flex items-center justify-between text-sm">
                                    <span>Đơn giá</span>
                                    <span className="font-medium">
                                        {currency(vehicle.costPerDay)}
                                    </span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>Số ngày</span>
                                    <span className="font-medium">{totalDays}</span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>Tiền cọc</span>
                                    <span className="font-medium">
                                        {currency(vehicle.depositFee)}
                                    </span>
                                </div>

                                <div className="mt-3 h-px bg-neutral-200" />
                                <div className="mt-3 flex items-center justify-between text-base font-semibold">
                                    <span>Tạm tính</span>
                                    <span className="text-emerald-700">{currency(total)} đ</span>
                                </div>
                            </div>

                            <ButtonStyled
                                isDisabled={isLoggedIn}
                                className="w-full rounded-xl bg-emerald-600 px-4 py-3 font-semibold text-white shadow-sm hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                            >
                                <Link href={"/"}>Yêu cầu đặt xe</Link>
                            </ButtonStyled>
                        </div>
                    </div>
                </aside>
            </main>

            <section className="w-300 ml-10 flex flex-col pb-10">
                <div className="flex items-end gap-250">
                    <h2 className="text-2xl font-semibold">Xe tương tự</h2>
                    <Link
                        href="/vehicles"
                        className="text-sm text-emerald-700 hover:underline font-semibold"
                    >
                        Xem tất cả
                    </Link>
                </div>

                <div className="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    {similarVehicles.map((i) => (
                        <Link
                            key={i.id}
                            href={`/vehicle-rental/detail/${i.id}`}
                            className="group block rounded-2xl bg-white p-5 shadow-md hover:shadow-lg transition transform hover:scale-[1.02]"
                        >
                            <div className="aspect-[16/10] w-full overflow-hidden rounded-xl bg-neutral-200">
                                <img
                                    className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
                                    src={i.images[0]}
                                    alt={i.name}
                                />
                            </div>

                            <div className="mt-4 flex items-center justify-between">
                                <div className="min-w-0">
                                    <p className="font-semibold text-lg truncate">{i.name}</p>
                                    <p className="text-sm text-neutral-500">
                                        Điện • {i.seatingCapacity} chỗ • Tự động
                                    </p>
                                </div>
                                <p className="text-emerald-600 font-semibold text-base whitespace-nowrap">
                                    {currency(i.costPerDay)}{" "}
                                    <span className="text-sm text-neutral-500">VND/Ngày</span>
                                </p>
                            </div>
                        </Link>
                    ))}
                </div>
            </section>
        </div>
    )
}
