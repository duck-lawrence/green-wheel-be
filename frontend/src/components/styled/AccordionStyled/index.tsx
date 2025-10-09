"use client"
import React from "react"
import { Accordion, AccordionItem } from "@heroui/react"

export function AccordionStyled({
    items
}: {
    items: { key: string; ariaLabel: string; title: string; value: string }[]
}) {
    return (
        <Accordion variant="splitted">
            {items.map((val) => {
                return (
                    <AccordionItem key={val.key} aria-label={val.ariaLabel} title={val.title}>
                        {val.value}
                    </AccordionItem>
                )
            })}
        </Accordion>
    )
}
