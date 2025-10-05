"use client"
import React from "react"
import { Card, CardBody, CardFooter, Image } from "@heroui/react"
import { BatteryChargingIcon, Couch, SuitcaseIcon, Users } from "@phosphor-icons/react"
import { useRouter } from "next/navigation"
import VehicleModel from "@/models/vehicle/vehicle"
import { useTranslation } from "react-i18next"

// cắt chuỗi để chỉnh format cho đẹp =)
function splitTitle(title: string) {
    const parts = title.split(" ")
    const brand = parts[0] || ""
    const model = parts.slice(1).join(" ") || ""
    return { brand, model }
}

// className="gap-8 grid grid-cols-2 sm:grid-cols-3 "
export default function CardVehicalStyled({ car }: { car: VehicleModel }) {
    const { t } = useTranslation()
    const router = useRouter()

    const { brand, model } = splitTitle(car.name)
    return (
        <div>
            <Card
                isPressable
                // key={car.id}
                onPress={() => {
                    router.prefetch(`/vehicle-rental/detail/${car.id}`)
                    router.push(`/vehicle-rental/detail/${car.id}`)
                }}
                className="hover:shadow-xl hover:scale-[1.02] transition-transform duration-300 ease-in-out"
                shadow="sm"
            >
                <CardBody className="overflow-visible ">
                    <Image
                        alt={car.name}
                        className="w-full object-cover h-[200px] shadow-lg"
                        radius="lg"
                        shadow="sm"
                        src={car.images[0]}
                        width="100%"
                    />
                    {car.quantity === 0 && (
                        <span className="absolute top-3 right-3 bg-red-500 text-white px-3 py-1 text-xs rounded-xl z-10">
                            {t("vehicle_model.out_of_stock")}
                        </span>
                    )}
                </CardBody>

                <CardFooter className="flex flex-col text-small justify-between">
                    <div className="flex justify-between items-center w-full">
                        <div className="ml-6">
                            <b className="text-2xl">{brand}</b> <br />
                            <b className="text-2xl">{model}</b>
                        </div>

                        <div className="mr-5">
                            <span className="text-2xl font-semibold text-green-600 whitespace-nowrap">
                                {car.costPerDay.toLocaleString()}
                            </span>
                            <br />
                            <span className="text-black">{t("vehicle_model.vnd_per_day")}</span>
                        </div>
                    </div>

                    {/* segmentName */}
                    <div className="grid grid-cols-2 ">
                        <div className="flex gap-2 mb-4">
                            <Couch className="flex h-6 w-6" />
                            <span>{car.segment}</span>
                        </div>

                        <div className="flex gap-2">
                            <BatteryChargingIcon className="h-6 w-6" />
                            <span>{car.ecoRangeKm}Km</span>
                        </div>

                        <div className="flex gap-2">
                            <Users className="h-6 w-6" />
                            <span>
                                {car.seatingCapacity} {t("vehicle_model.seats")}
                            </span>
                        </div>

                        <div className="flex gap-0">
                            <SuitcaseIcon className="h-6 w-6" />
                            <span>
                                {t("vehicle_model.battery_capacity")} {car.batteryCapacity ?? "-"}
                            </span>
                        </div>
                    </div>
                </CardFooter>
            </Card>
        </div>
    )
}
