"use client"
import React from "react"
import { Card, CardBody, CardFooter, Image } from "@heroui/react"
import { BatteryChargingIcon, Couch, SuitcaseIcon, Users } from "@phosphor-icons/react"

// cắt chuỗi để chỉnh format cho đẹp =)
function splitTitle(title: string) {
    const parts = title.split(" ")
    const brand = parts[0] || ""
    const model = parts.slice(1).join(" ") || ""
    return { brand, model }
}
//{ data }: CardItempProps
export default function CardList() {
    // Call Store
    // const selectCars = useCarStore((state) => state.selectCars)

    return (
        <div className="gap-8 grid grid-cols-2 sm:grid-cols-3 ">
            {selectCars.map((item) => {
                const { brand, model } = splitTitle(item.title)

                return (
                    <Card
                        className="hover:shadow-xl hover:scale-[1.02] transition-transform duration-300 ease-in-out"
                        key={item.id}
                        isPressable
                        shadow="sm"
                        onPress={() => console.log("item pressed")}
                    >
                        <CardBody className="overflow-visible ">
                            <Image
                                alt={item.title}
                                className="w-full object-cover h-[200px] shadow-lg"
                                radius="lg"
                                shadow="sm"
                                src={item.img}
                                width="100%"
                            />
                        </CardBody>

                        <CardFooter className="flex flex-col text-small justify-between">
                            <div className="flex justify-between items-center w-full">
                                <div className="ml-6">
                                    <b className="text-2xl">{brand}</b> <br />
                                    <b className="text-2xl">{model}</b>
                                </div>

                                <div className="mr-5">
                                    <span className="text-2xl font-semibold text-green-600 whitespace-nowrap">
                                        {item.price.toLocaleString()}
                                    </span>
                                    <br />
                                    <span className="text-black">VNĐ/Ngày</span>
                                </div>
                            </div>

                            {/* Info */}
                            <div className="grid grid-cols-2 ">
                                <div className="flex gap-2 mb-4">
                                    <Couch className="flex h-6 w-6" />
                                    <span>{item.couch}</span>
                                </div>

                                <div className="flex gap-2">
                                    <BatteryChargingIcon className="h-6 w-6" />
                                    <span>{item.km}km (NEDC)</span>
                                </div>

                                <div className="flex gap-2">
                                    <Users className="h-6 w-6" />
                                    <span>{item.seats} chỗ</span>
                                </div>

                                <div className="flex gap-0">
                                    <SuitcaseIcon className="h-6 w-6" />
                                    <span>Dung tích cốp {item.trunk ?? "-"}L</span>
                                </div>
                            </div>
                        </CardFooter>
                    </Card>
                )
            })}
        </div>
    )
}
