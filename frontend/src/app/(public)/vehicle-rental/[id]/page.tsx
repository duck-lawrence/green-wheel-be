"use client"
import React, { useCallback, useEffect, useMemo, useState } from "react"
import { motion } from "framer-motion"
import { BreadCrumbsStyled, ButtonStyled, FieldStyled } from "@/components"
import { GasPump, UsersFour, SteeringWheel, RoadHorizon } from "@phosphor-icons/react"
import { formatCurrency } from "@/utils/helpers/currency"
import { useParams, useRouter } from "next/navigation"
import { useBookingFilterStore, useGetVehicleModelById, useGetMe, useDay } from "@/hooks"
import { useTranslation } from "react-i18next"
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"
import { Spinner, useDisclosure } from "@heroui/react"
import { ROLE_CUSTOMER } from "@/constants/constants"
import toast from "react-hot-toast"
import { BackendError } from "@/models/common/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { CreateRentalContractModal } from "@/components/modals/CreateRentalContractModal"

export default function VehicleDetailPage() {
    const { id } = useParams()
    const modelId = id?.toString()
    const router = useRouter()

    const { t } = useTranslation()
    const { getDiffDaysCeil } = useDay()
    const { isOpen, onOpen, onClose } = useDisclosure()
    const { data: user } = useGetMe()
    const isCustomer = useMemo(() => {
        return user?.role?.name === ROLE_CUSTOMER
    }, [user])

    // handle render picture
    const [active, setActive] = useState(0)

    const stationId = useBookingFilterStore((s) => s.stationId)
    const startDate = useBookingFilterStore((s) => s.startDate)
    const endDate = useBookingFilterStore((s) => s.endDate)

    const {
        data: model,
        isLoading: isModelLoading,
        error: modelError
    } = useGetVehicleModelById({
        modelId: modelId || "",
        query: {
            stationId: stationId || "",
            startDate: startDate || "",
            endDate: endDate || ""
        }
    })

    // redirect if filter not valid
    useEffect(() => {
        if (!stationId || !startDate || !endDate) {
            toast.error(t("vehicle_filter.require"))
            router.replace("/vehicle-rental")
        }
        if (modelError) {
            const error = modelError as BackendError
            toast.error(translateWithFallback(t, error.detail))
            router.replace("/vehicle-rental")
        }
    }, [endDate, modelError, router, startDate, stationId, t])

    const { totalDays, totalPrice } = useMemo(() => {
        if (!model) return { totalDays: 0, totalPrice: 0 }

        const totalDays = getDiffDaysCeil({ startDate, endDate })
        return {
            totalDays,
            totalPrice: totalDays * model.costPerDay + model.depositFee
        }
    }, [endDate, getDiffDaysCeil, model, startDate])

    // display item similar
    // const similarVehicles = vehicleModels
    //     .filter((v) => v.id !== id)
    //     .sort(() => Math.random() - 0.5)
    //     .slice(0, 3)

    const handleClickBooking = useCallback(() => {
        if (!user?.phone) {
            toast.error(t("user.enter_phone_to_booking"))
        } else {
            onOpen()
        }
    }, [onOpen, t, user?.phone])

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

    const basePolicies = (deposite: number) => [
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

    if (!model || isModelLoading) return <Spinner />

    return (
        <div className="min-h-dvh bg-neutral-50 text-neutral-900 rounded">
            <CreateRentalContractModal
                isOpen={isOpen}
                onClose={onClose}
                modelViewRes={model}
                totalDays={totalDays}
                totalPrice={totalPrice}
            />

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
                            {model.name}
                        </h1>
                        <p className="text-neutral-600">{model.description}</p>
                        {/* <p className="mt-1 text-sm text-neutral-500">
                            Hãng xe: <span className="font-medium">{vehicle.brand_name}</span> •
                            Phân khúc: <span className="font-medium">{vehicle.segment_name}</span>
                        </p> */}
                        <p className="text-lg text-neutral-500">
                            {t("vehicle_model.remaining_vehicle_count")} &nbsp;
                            <span className="font-extrabold text-emerald-600">
                                {model.availableVehicleCount}
                            </span>
                        </p>
                    </div>
                    {/*  font-extrabold*/}
                    <div className="text-right">
                        <p className="text-2xl sm:text-3xl font-semibold text-emerald-600">
                            {formatCurrency(model.costPerDay)} &nbsp;
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
                                src={model.imageUrls && model.imageUrls[active]}
                                alt={`${model.name} - ${active + 1}`}
                                className="h-full w-full object-cover"
                            />
                        </div>

                        {/* List sub img */}
                        <div className="row-span-1 grid grid-cols-4 gap-3">
                            {model.imageUrls &&
                                [model.imageUrl, ...model.imageUrls].map((src, idx) => (
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
                            {mapSpecs(model).map(({ key, value }) => (
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
                            {basePolicies(model.depositFee).map((p) => (
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
                                    value={`${model.seatingCapacity}`}
                                    icon={<UsersFour size={18} />}
                                />
                                <FieldStyled
                                    label="Hộp số"
                                    value="Tự động"
                                    icon={<SteeringWheel size={18} />}
                                />
                                <FieldStyled
                                    label="Quãng đường"
                                    value={`~${model.ecoRangeKm} km`}
                                    icon={<RoadHorizon size={18} />}
                                />
                            </div>

                            {/* Đơn tạm tính */}
                            <div className="rounded-xl bg-neutral-50 p-4">
                                <div className="flex items-center justify-between text-sm">
                                    <span>{t("vehicle_model.unit_price")}</span>
                                    <span className="font-medium">
                                        {formatCurrency(model.costPerDay)}
                                    </span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>{t("vehicle_model.number_of_days")}</span>
                                    <span className="font-medium">{totalDays}</span>
                                </div>
                                <div className="mt-2 flex items-center justify-between text-sm">
                                    <span>{t("vehicle_model.deposit_fee")}</span>
                                    <span className="font-medium">
                                        {formatCurrency(model.depositFee)}
                                    </span>
                                </div>

                                <div className="mt-3 h-px bg-neutral-200" />
                                <div className="mt-3 flex items-center justify-between text-base font-semibold">
                                    <span>{t("vehicle_model.temporary_total")}</span>
                                    <span className="text-emerald-700">
                                        {formatCurrency(totalPrice)}
                                    </span>
                                </div>
                            </div>

                            <ButtonStyled
                                isDisabled={!isCustomer}
                                className="w-full rounded-xl bg-emerald-600 px-4 py-3 font-semibold text-white shadow-sm hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                                onPress={handleClickBooking}
                            >
                                {t("vehicle_model.create_rental_request")}
                            </ButtonStyled>
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
