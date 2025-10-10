// "use client"
// import React from "react"
// import { Accordion, AccordionItem } from "@heroui/react"

// export function AccordionStyled({
//     items
// }: {
//     items: { key: string; ariaLabel: string; title: React.ReactNode; content: React.ReactNode }[]
// }) {
//     return (
//         <Accordion variant="splitted">
//             {items.map((val) => {
//                 return (
//                     <AccordionItem key={val.key} aria-label={val.ariaLabel} title={val.title}>
//                         {val.content}
//                     </AccordionItem>
//                 )
//             })}
//         </Accordion>
//     )
// }

"use client"
import React from "react"
import { Accordion, AccordionItem, Chip } from "@heroui/react"
import { InvoiceStatus } from "@/constants/enum"
import { InvoiceStatusLabels } from "@/constants/labels"
import { ButtonStyled } from "../ButtonStyled"

export function AccordionStyled({
    items
}: {
    items: {
        key: string
        ariaLabel: string
        title: React.ReactNode
        status: InvoiceStatus
        content: React.ReactNode
    }[]
}) {
    const renderStatusChip = (status: InvoiceStatus) => {
        switch (status) {
            case InvoiceStatus.Paid:
                return (
                    <Chip color="success" variant="flat" className="font-medium">
                        {InvoiceStatusLabels[status]}
                    </Chip>
                )
            case InvoiceStatus.Pending:
                return (
                    <Chip color="warning" variant="flat" className="font-medium">
                        {InvoiceStatusLabels[status]}
                    </Chip>
                )
            case InvoiceStatus.Cancelled:
                return (
                    <Chip color="danger" variant="flat" className="font-medium">
                        {InvoiceStatusLabels[status]}
                    </Chip>
                )
            default:
                return null
        }
    }

    return (
        <Accordion variant="splitted" className="w-full">
            {items.map((val) => (
                <AccordionItem
                    key={val.key}
                    aria-label={val.ariaLabel}
                    title={
                        <div className="flex justify-between items-center w-full">
                            <span className="font-semibold text-base">{val.title}</span>
                            {renderStatusChip(val.status)}
                        </div>
                    }
                >
                    {val.content}
                    <div className="flex justify-end items-center gap-2 p-2">
                        <div className="mt-0 flex justify-center">
                            <ButtonStyled
                                size="lg"
                                color="primary"
                                className="px-12 py-3 font-semibold text-white rounded-xl 
                                           bg-gradient-to-r from-primary to-teal-400 
                                           hover:from-teal-500 hover:to-green-400 
                                           shadow-md transition-all duration-300"
                            >
                                Payment
                            </ButtonStyled>
                        </div>
                        <div className="mt-0 flex justify-center">
                            <ButtonStyled
                                size="lg"
                                color="primary"
                                className="px-8 py-3 font-semibold text-gray-400 rounded-xl 
                                           bg-white border-2 border-gray-400 hover:border-red-700
                                           hover:bg-red-700 hover:text-black
                                           shadow-md transition-all duration-300"
                            >
                                Cancle
                            </ButtonStyled>
                        </div>
                    </div>
                </AccordionItem>
            ))}
        </Accordion>
    )
}
