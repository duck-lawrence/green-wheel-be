"use client"
import React from "react"
import { Card, CardBody, CardFooter, Image } from "@heroui/react"
import { BatteryChargingIcon, Couch, SuitcaseIcon, Users } from "@phosphor-icons/react"
import { useRouter } from "next/navigation"
import VehicleModel from "@/models/vehicle/vehicle"
import { useTranslation } from "react-i18next"
import { currency } from "@/utils/helpers/currentcy"

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
                        className="max-w-[370px] w-full object-cover h-[280px] shadow-lg"
                        radius="lg"
                        shadow="sm"
                        src={car.images[0]}
                        width="100%"
                    />
                    {car.quantity === 0 && (
                        <span className="absolute top-3 right-3 bg-red-500 text-white px-3 py-1 text-xs rounded z-10">
                            {t("vehicle_model.out_of_stock")}
                        </span>
                    )}
                </CardBody>

                <CardFooter className="flex flex-col text-small justify-between">
                    {/* <div className="flex flex-col  w-full"> */}
                    <div className="flex justify-between items-center text-2xl mb-2">
                        {/* <b className="text-2xl">{brand}</b> <br />
                            <b className="text-2xl">{model}</b> */}
                        <b>{car.name}</b>
                    </div>
                    {/* </div> */}
                    <hr className=" text-gray-300 border-1 w-full m-1" />

                    <div className=" flex items-center justify-center mt-2 mb-2    p-2 ">
                        <span className="text-2xl font-bold text-green-600 whitespace-nowrap">
                            {currency(car.costPerDay)} &nbsp;
                        </span>

                        <span className="text-black">{"   " + t("vehicle_model.vnd_per_day")}</span>
                    </div>
                    {/* <hr className="bg-gray-300 border w-full m-1" /> */}
                    {/* <hr className=" text-gray-300 border-1 w-full m-1" /> */}
                    {/* segmentName */}
                    <div className="grid grid-cols-2 gap-2 mt-2 mr-0 max-w-60 w-full">
                        <div className="flex gap-2 ">
                            <Couch className="flex w-6 h-6" />
                            <span>{car.segment}</span>
                        </div>

                        <div className="flex gap-2 justify-end">
                            <BatteryChargingIcon className="h-6 w-6" />
                            <span>{car.ecoRangeKm} Km</span>
                        </div>

                        <div className="flex gap-2">
                            <Users className="h-6 w-6" />
                            <span>
                                {car.seatingCapacity} {t("vehicle_model.seats")}
                            </span>
                        </div>

                        <div className="flex gap-2 justify-end">
                            <SuitcaseIcon className="h-6 w-6" />
                            <span>
                                {car.numberOfAirbags} {t("vehicle_model.airbag")}
                            </span>
                        </div>
                    </div>
                </CardFooter>
            </Card>
        </div>
    )
}
