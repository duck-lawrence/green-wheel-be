"use client"
import React from "react"
import { motion } from "framer-motion"
import { AccordionStyled, InputStyled, SectionStyled, TextareaStyled } from "@/components"
import {
    Car,
    IdentificationBadge,
    ClipboardText,
    ArrowsLeftRight,
    Invoice
} from "@phosphor-icons/react"
import { DatePicker } from "@heroui/react"
import { mockContracts } from "@/data/mockContracts"
import { InvoiceTypeLabels, RentalContractStatusLabels } from "@/constants/labels"
import { useDay } from "@/hooks"
import { mockInvoices } from "@/data/mockIvoices"
import { renderInvoiceForm } from "@/components/shared/InvoiceForm/renderInvoiceForm"

export default function RentalContractPage() {
    const { toCalenderDateTime } = useDay()

    // render accordion
    const invoiceAccordion = mockInvoices.map((invoice) => ({
        key: invoice.id,
        ariaLabel: invoice.id,
        title: `${InvoiceTypeLabels[invoice.type]}`,
        status: invoice.status,
        content: renderInvoiceForm(invoice.type)
    }))

    // id: "CON001",
    const dataContract = mockContracts.find((v) => v.id === "CON001")!

    // const dataInvoice = mockInvoices.find((v) => v.id === "INV_B001")!

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
                <SectionStyled title="Thông tin hợp đồng thuê xe">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <div className="sm:col-span-2">
                            <p>Ngày thuê: 10/10/2025 ???</p>
                        </div>

                        <InputStyled
                            isReadOnly
                            label="Mã hợp đồng"
                            value={dataContract.id}
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
                            value={RentalContractStatusLabels[dataContract.status]}
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
                            value={dataContract.vehicleId}
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
                            value={dataContract.description}
                            placeholder=". . . "
                            variant="bordered"
                            className="sm:col-span-2"
                        />
                    </div>
                </SectionStyled>

                {/* Group 3 - Rental Dates */}
                <SectionStyled title="Thời gian thuê">
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                        <DatePicker
                            value={toCalenderDateTime(dataContract.startDate)}
                            label="Ngày bắt đầu"
                            isReadOnly
                        />
                        <DatePicker
                            value={toCalenderDateTime(dataContract.actualStartDate)}
                            label="Ngày thực tế bắt đầu"
                            isReadOnly
                        />
                        <DatePicker
                            value={toCalenderDateTime(dataContract.endDate)}
                            label="Ngày kết thúc"
                            isReadOnly
                        />
                        <DatePicker
                            value={toCalenderDateTime(dataContract.actualEndDate)}
                            label="Ngày thực tế kết thúc"
                            isReadOnly
                        />
                    </div>
                </SectionStyled>

                {/* Group 4 - Staff Info */}
                <SectionStyled title="Nhân viên xử lý & Hóa đơn">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <InputStyled
                            isReadOnly
                            label="Nhân viên bàn giao xe"
                            value={dataContract.handoverStaffId}
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
                            value={dataContract.returnStaffId}
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
                    </div>
                </SectionStyled>

                {/* Invoice Accordion */}
                <SectionStyled title="Danh sách hóa đơn thanh toán">
                    <AccordionStyled items={invoiceAccordion} />
                </SectionStyled>
            </motion.div>
        </div>
    )
}
