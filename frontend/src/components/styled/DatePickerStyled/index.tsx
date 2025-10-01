"use client"
import { cn, DatePicker, DatePickerProps } from "@heroui/react"
import React from "react"
export function DatePickerStyled(props: DatePickerProps) {
    return (
        <DatePicker
            color="primary"
            label="Birth date"
            {...props}
            className={cn(" font-medium text-base", props.className)}
        />
    )
}
