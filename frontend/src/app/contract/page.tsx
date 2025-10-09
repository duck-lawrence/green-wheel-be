"use client"

import React from "react"
import { motion } from "framer-motion"
import {
    AccordionStyled,
    AccoredionSyled,
    ButtonStyled,
    InputStyled,
    TextareaStyled
} from "@/components"
import {
    Car,
    IdentificationBadge,
    User,
    CalendarBlank,
    ClipboardText,
    ArrowsLeftRight,
    FileText,
    Invoice
} from "@phosphor-icons/react"

export default function RentalContractPage() {
    const accordion = [
        {
            key: "1",
            ariaLabel: "Accordion 1",
            title: "Accordion 1",
            value: "content 1 "
        },
        {
            key: "2",
            ariaLabel: "Accordion 2",
            title: "Accordion 2",
            value: "content 2 "
        },
        {
            key: "3",
            ariaLabel: "Accordion3",
            title: "Accordion 3",
            value: "content 3 "
        },
        {
            key: "4",
            ariaLabel: "Accordion 4",
            title: "Accordion 4",
            value: "content 4 "
        }
    ]
    return (
        <div className="min-h-screen flex items-center justify-center dark:bg-gray-950 py-16 px-4">
            <motion.div
                initial={{ opacity: 0, y: 40 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5 }}
                className="w-full max-w-6xl bg-white dark:bg-gray-900 shadow-xl rounded-2xl border border-gray-200 dark:border-gray-800 p-8 md:p-12"
            >
                {/* Header */}
                <div className="text-center space-y-3 mb-12">
                    <h1 className="text-4xl font-bold text-primary">Hợp đồng thuê xe</h1>
                    <p className="text-gray-600 dark:text-gray-400 text-lg">
                        Thông tin chi tiết về hợp đồng thuê xe điện của khách hàng.
                    </p>
                </div>

                {/* Group 1 - Vehicle Info */}
                <Section title="Thông tin xe">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <InputStyled
                            label="Tên xe"
                            placeholder="VinFast VF8"
                            startContent={
                                <Car size={22} className="text-primary" weight="duotone" />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Biển số"
                            placeholder="51H-123.45"
                            startContent={
                                <IdentificationBadge
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        <TextareaStyled
                            label="Mô tả xe"
                            placeholder="Xe mới, màu trắng, giao tại chi nhánh Quận 7..."
                            variant="bordered"
                            className="sm:col-span-2"
                        />
                    </div>
                </Section>

                {/* Group 2 - Customer Info */}
                <Section title="Thông tin khách hàng">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <InputStyled
                            label="Tên khách hàng"
                            placeholder="Nguyễn Văn A"
                            startContent={
                                <User size={22} className="text-primary" weight="duotone" />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Trạng thái hợp đồng"
                            placeholder="Đang hoạt động / Hoàn thành / Hủy"
                            startContent={
                                <ClipboardText
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                    </div>
                </Section>

                {/* Group 3 - Rental Dates */}
                <Section title="Thời gian thuê">
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                        <InputStyled
                            label="Ngày bắt đầu"
                            type="date"
                            startContent={
                                <CalendarBlank
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Ngày thực tế bắt đầu"
                            type="date"
                            startContent={
                                <CalendarBlank
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Ngày kết thúc"
                            type="date"
                            startContent={
                                <CalendarBlank
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Ngày thực tế kết thúc"
                            type="date"
                            startContent={
                                <CalendarBlank
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                    </div>
                </Section>

                {/* Group 4 - Staff Info */}
                <Section title="Nhân viên xử lý & Hóa đơn">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <InputStyled
                            label="Nhân viên bàn giao xe"
                            placeholder="Trần Văn B"
                            startContent={
                                <ArrowsLeftRight
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Nhân viên nhận xe"
                            placeholder="Lê Thị C"
                            startContent={
                                <ArrowsLeftRight
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            label="Mã hóa đơn"
                            placeholder="INV-2025-0012"
                            startContent={
                                <Invoice size={22} className="text-primary" weight="duotone" />
                            }
                            variant="bordered"
                            className="sm:col-span-2"
                        />
                    </div>
                </Section>

                {/* Action */}
                <div className="mt-12 flex justify-center">
                    <ButtonStyled
                        size="lg"
                        color="primary"
                        className="px-12 py-3 font-semibold text-white rounded-xl 
                       bg-gradient-to-r from-primary to-teal-400 
                       hover:from-teal-500 hover:to-green-400 
                       shadow-md transition-all duration-300"
                    >
                        Xác nhận hợp đồng
                    </ButtonStyled>
                </div>
            </motion.div>
            <AccordionStyled items={accordion} />
        </div>
    )
}

/* ===== Subcomponent: Section Wrapper ===== */
function Section({ title, children }: { title: string; children: React.ReactNode }) {
    return (
        <div className="mb-10">
            <h3 className="text-xl font-semibold text-gray-800 dark:text-white mb-4 flex items-center gap-2">
                <FileText size={20} className="text-primary" /> {title}
            </h3>
            <div>{children}</div>
        </div>
    )
}
