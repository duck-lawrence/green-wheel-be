"use client"
import React from "react"
import { Accordion, AccordionItem } from "@heroui/react"

export function AccordionStyled({
    items
}: {
    items: { key: string; ariaLabel: string; title: React.ReactNode; content: React.ReactNode }[]
}) {
    return (
        <Accordion variant="splitted">
            {items.map((val) => {
                return (
                    <AccordionItem key={val.key} aria-label={val.ariaLabel} title={val.title}>
                        {val.content}
                    </AccordionItem>
                )
            })}
        </Accordion>
    )
}
