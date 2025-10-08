"use client"
import React, { useCallback, useEffect, useMemo, useState } from "react"
import { motion } from "framer-motion"
import { BreadCrumbsStyled, ButtonStyled, FieldStyled } from "@/components"
import { GasPump, UsersFour, SteeringWheel, RoadHorizon } from "@phosphor-icons/react"
import { formatCurrency } from "@/utils/helpers/currentcy"
import { getDatesDiff } from "@/utils/helpers/mathDate"
import { useParams, useRouter } from "next/navigation"
import { useBookingFilterStore, useGetVehicleModelById, useGetMe } from "@/hooks"
import { useTranslation } from "react-i18next"
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"
import { Spinner } from "@heroui/react"
import { ROLE_CUSTOMER } from "@/constants/constants"
import toast from "react-hot-toast"

export default function VehicleDetailPage() {
    const { id } = useParams()
    const modelId = id?.toString()
    const router = useRouter()

    const { t } = useTranslation()
    const { data: user } = useGetMe()
    const isCustomer = useMemo(() => {
        return user?.role?.name === ROLE_CUSTOMER
    }, [user])

    const [vehicle, setVehicle] = useState<VehicleModelViewRes | null>(null)
    // handle render picture
    const [active, setActive] = useState(0)

    const stationId = useBookingFilterStore((s) => s.stationId)
    const startDate = useBookingFilterStore((s) => s.startDate)
    const endDate = useBookingFilterStore((s) => s.endDate)

    const {
        data: model,
        isLoading: isModelLoading,
        isError: isModelError
    } = useGetVehicleModelById({
        modelId: modelId || "",
        query: {
            stationId: stationId || "",
            startDate: startDate || "",
            endDate: endDate || ""
        }
    })

    const { totalDays, total } = useMemo(() => {
        if (!vehicle) return { totalDays: 0, total: 0 }

        const totalDays = getDatesDiff({ startDate, endDate })
        return {
            totalDays,
            total: totalDays * vehicle.costPerDay + vehicle.depositFee
        }
    }, [endDate, startDate, vehicle])

    useEffect(() => {
        if (model) setVehicle(model)
    }, [model])

    // display item similar
    // const similarVehicles = vehicleModels
    //     .filter((v) => v.id !== id)
    //     .sort(() => Math.random() - 0.5)
    //     .slice(0, 3)

    const handleClickBooking = useCallback(() => {
        if (!user?.phone) {
            toast.error(t("user.enter_phone_to_booking"))
        } else {
            router.replace("/rental-contracts")
        }
    }, [router, t, user?.phone])

    function mapSpecs(vehicle: VehicleModelViewRes) {
        return [
            { key: "Số chỗ", value: vehicle.seatingCapacity },
            { key: "Công suất", value: vehicle.motorPower + " kW" },
            { key: "Dung lượng pin", value: vehicle.batteryCapacity + " kWh" },
            { key: "Eco Range", value: vehicle.ecoRangeKm + " km" },
            { key: "Sport Range", value: vehicle.sportRangeKm + " km" },
            { key: "Hộp số", value: vehicle.numberOfAirbags }
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

    if (!vehicle || isModelLoading || isModelError) return <Spinner />

    return (
        <div className="min-h-dvh bg-neutral-50 text-neutral-900 rounded">
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
                            {t("vehicle_model.remaining_vehicle_count")} &nbsp;
                            <span className="font-medium text-emerald-600">
                                {vehicle.availableVehicleCount}
                            </span>
                        </p>
                    </div>
                    {/*  font-extrabold*/}
                    <div className="text-right">
                        <p className="text-2xl sm:text-3xl font-semibold text-emerald-600">
                            {formatCurrency(vehicle.costPerDay)} &nbsp;
                            <span className="text-base font-normal text-neutral-500">
                                {t("vehicle_model.vnd_per_day")}
                            </span>
                        </p>
                    </div>
                </div>
            </header>

            {/* Main content */}
            <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pb-24 grid grid-cols-1 lg:grid-cols-12 gap-6">
                {/* Gallery */}
                <section className="lg:col-span-8">
                    {/* Images */}
                    <div className="grid grid-rows-4 gap-3">
                        <div className="row-span-3 aspect-[16/10] overflow-hidden rounded-2xl bg-neutral-200">
                            <motion.img
                                key={active}
                                initial={{ opacity: 0, scale: 1.02 }}
                                animate={{ opacity: 1, scale: 1 }}
                                transition={{ duration: 0.35 }}
                                src={vehicle.imageUrls && vehicle.imageUrls[active]}
                                alt={`${vehicle.name} - ${active + 1}`}
                                className="h-full w-full object-cover"
                            />
                        </div>

                        {/* List sub img */}
                        <div className="row-span-1 grid grid-cols-4 gap-3">
                            {vehicle.imageUrls &&
                                [vehicle.imageUrl, ...vehicle.imageUrls].map((src, idx) => (
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
                {/* lỗi nên chưa làm en chỗ này */}
                <aside className="self-start lg:col-span-4 lg:sticky lg:top-32 space-y-6">
                    <div className="rounded-2xl bg-white p-5 shadow-sm border border-neutral-100">
                        <h2 className="text-lg font-semibold">
                            {t("vehicle_model.vehicle_information")}
                        </h2>
                        <div className="mt-4 grid gap-4">
                            <div className="grid grid-cols-2 gap-3">
                                <FieldStyled
                                    label="Nhiên liệu"
                                    value="Điện"
                                    icon={<GasPump size={18} weight="duotone" />}
                                />
                                <FieldStyled
                                    label="Số chỗ"
                                    value={`${vehicle.seatingCapacity}`}
                                    icon={<UsersFour size={18} />}
                                />
                                <FieldStyled
                                    label="Hộp số"
                                    value="Tự động"
                                    icon={<SteeringWheel size={18} />}
                                />
                                <FieldStyled
                                    label="Quãng đường"
                                    value={`~${vehicle.ecoRangeKm} km`}
                                    icon={<RoadHorizon size={18} />}
                                />
                            </div>

                            {/* Đơn tạm tính */}
                            <div className="rounded-xl bg-neutral-50 p-4">
                                <div className="flex items-center justify-between text-sm">
                                    <span>{t("vehicle_model.unit_price")}</span>
                                    <span className="font-medium">
                                        {formatCurrency(vehicle.costPerDay)}
                                    </span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>{t("vehicle_model.number_of_days")}</span>
                                    <span className="font-medium">{totalDays}</span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>{t("vehicle_model.deposit_fee")}</span>
                                    <span className="font-medium">
                                        {formatCurrency(vehicle.depositFee)}
                                    </span>
                                </div>

                                <div className="mt-3 h-px bg-neutral-200" />
                                <div className="mt-3 flex items-center justify-between text-base font-semibold">
                                    <span>{t("vehicle_model.temporary_total")}</span>
                                    <span className="text-emerald-700">
                                        {formatCurrency(total)} đ
                                    </span>
                                </div>
                            </div>

                            <ButtonStyled
                                isDisabled={!isCustomer}
                                className="w-full rounded-xl bg-emerald-600 px-4 py-3 font-semibold text-white shadow-sm hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                            ></ButtonStyled>
                        </div>
                    </div>
                </aside>
            </main>

            {/* <section className="w-300 ml-10 flex flex-col pb-10">
                <div className="flex items-end gap-250">
                    <h2 className="text-2xl font-semibold">
                        {t("vehicle_model.similar_vehicles")}
                    </h2>
                    <Link
                        href="/vehicle-rental"
                        className="text-sm text-emerald-700 hover:underline font-semibold"
                    >
                        {t("vehicle_model.view_all")}
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
                                    src={i.imageUrls && i.imageUrls[0]}
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
                                    {formatCurrency(i.costPerDay)}{" "}
                                    <span className="text-sm text-neutral-500">VND/Ngày</span>
                                </p>
                            </div>
                        </Link>
                    ))}
                </div>
            </section> */}
        </div>
    )
}
