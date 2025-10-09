"use client"

import React from "react"
import { motion } from "framer-motion"
import { AccordionStyled, InputStyled, TextareaStyled } from "@/components"
import {
    Car,
    IdentificationBadge,
    ClipboardText,
    ArrowsLeftRight,
    FileText,
    Invoice
} from "@phosphor-icons/react"
import InvoiceRentalStartForm from "@/components/shared/InvoiceByType/InvoiceForm/InvoiceRentalStartForm"
import InvoiceRentalEndForm from "@/components/shared/InvoiceByType/InvoiceForm/InvoiceRentalEndForm"
import InvoiceDepositRefundForm from "@/components/shared/InvoiceByType/InvoiceForm/InvoiceDepositRefundForm"
import InvoiceSupportDamageForm from "@/components/shared/InvoiceByType/InvoiceForm/InvoiceSupportDamageForm"
import { InvoiceStatus } from "@/constants/enum"
import { DatePicker } from "@heroui/react"

export default function RentalContractPage() {
    const invoiceAccordion = [
        {
            key: "1",
            ariaLabel: "Thanh toán khi nhận xe",
            title: "Hóa đơn thanh toán khi nhận xe",
            status: InvoiceStatus.Paid,
            content: <InvoiceRentalStartForm />
        },
        {
            key: "2",
            ariaLabel: "Thanh toán khi trả xe",
            title: "Hóa đơn thanh toán khi trả xe",
            status: InvoiceStatus.Pending,
            content: <InvoiceRentalEndForm />
        },
        {
            key: "3",
            ariaLabel: "Hoàn trả tiền cọc",
            title: "Hóa đơn hoàn trả tiền cọc",
            status: InvoiceStatus.Cancelled,
            content: <InvoiceDepositRefundForm />
        },
        {
            key: "4",
            ariaLabel: "Chi phí hỗ trợ / hư hỏng",
            title: "Hóa đơn hỗ trợ phát sinh khi xe gặp sự cố",
            status: InvoiceStatus.Cancelled,
            content: <InvoiceSupportDamageForm />
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
                <Section title="Thông tin hợp đồng thuê xe">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <InputStyled
                            isReadOnly
                            label="Mã hợp đồng"
                            placeholder="INV-2025-0012"
                            startContent={
                                <Invoice size={22} className="text-primary" weight="duotone" />
                            }
                            variant="bordered"
                            // className="sm:col-span-2"
                        />
                        <InputStyled
                            isReadOnly
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

                        <InputStyled
                            isReadOnly
                            label="Tên xe"
                            placeholder="VinFast VF8"
                            startContent={
                                <Car size={22} className="text-primary" weight="duotone" />
                            }
                            variant="bordered"
                        />
                        <InputStyled
                            isReadOnly
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
                            isReadOnly
                            label="Mô tả hợp đồng"
                            placeholder=". . . "
                            variant="bordered"
                            className="sm:col-span-2"
                        />
                    </div>
                </Section>

                {/* Group 2 - Customer Info */}
                {/* <Section title="Thông tin khách hàng">
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
                </Section> */}

                {/* Group 3 - Rental Dates */}
                <Section title="Thời gian thuê">
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                        <DatePicker label="Ngày bắt đầu" isReadOnly />
                        <DatePicker label="Ngày thực tế bắt đầu" isReadOnly />
                        <DatePicker label="Ngày kết thúc" isReadOnly />
                        <DatePicker label="Ngày thực tế kết thúc" isReadOnly />

                        {/* <InputStyled
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
                        /> */}
                    </div>
                </Section>

                {/* Group 4 - Staff Info */}
                <Section title="Nhân viên xử lý & Hóa đơn">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <InputStyled
                            isReadOnly
                            label="Nhân viên bàn giao xe"
                            value="Gia Huy"
                            placeholder="Nhân viên A"
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
                            isReadOnly
                            label="Nhân viên nhận xe"
                            value="Gia Huy"
                            placeholder="Nhân viên B"
                            startContent={
                                <ArrowsLeftRight
                                    size={22}
                                    className="text-primary"
                                    weight="duotone"
                                />
                            }
                            variant="bordered"
                        />
                        {/* <InputStyled
                            label="Mã hóa đơn"
                            placeholder="INV-2025-0012"
                            startContent={
                                <Invoice size={22} className="text-primary" weight="duotone" />
                            }
                            variant="bordered"
                            className="sm:col-span-2"
                        /> */}
                    </div>
                </Section>

                {/* Action */}

                {/* Các section thông tin hợp đồng */}
                {/* ... phần Section của bạn giữ nguyên ... */}

                {/* Invoice Accordion */}
                <Section title="Danh sách hóa đơn thanh toán">
                    <AccordionStyled items={invoiceAccordion} />
                </Section>

                {/* Nút hành động */}
                {/* <div className="mt-12 flex justify-center">
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
                </div> */}
            </motion.div>
        </div>
    )
}

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
