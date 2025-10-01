"use client"
import { cn, DatePicker, DatePickerProps } from "@heroui/react"
import React from "react"
export function DatePickerStyled(props: DatePickerProps) {
    return (
        <DatePicker
            showMonthAndYearPickers
            variant="bordered"
            {...props}
            className={cn("font-medium text-base", props.className)}
        />
    )
}
